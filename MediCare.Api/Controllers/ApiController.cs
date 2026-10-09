using ErrorOr;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MediCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    /// <summary>UserId đọc từ claim "sub" (hoặc NameIdentifier) của JWT hiện tại.</summary>
    protected Guid? CurrentUserId
    {
        get
        {
            var sub = User.FindFirstValue("sub")
                      ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    /// <summary>Trả về kết quả thành công, hoặc ProblemDetails nếu có lỗi.</summary>
    protected IActionResult Ok<T>(ErrorOr<T> result) =>
        result.Match(value => Ok(value), errors => Problem(errors));

    protected IActionResult Problem(IReadOnlyList<Error> errors)
    {
        if (errors.Count == 0)
            return Problem();

        // Toàn bộ lỗi là Validation -> trả về dạng ValidationProblemDetails
        if (errors.All(e => e.Type == ErrorType.Validation))
            return ValidationProblem(errors);

        var error = errors[0];
        var statusCode = GetStatusCode(error.Type);

        // Không lộ chi tiết lỗi nội bộ ra ngoài
        var detail = statusCode >= StatusCodes.Status500InternalServerError
            ? "Đã xảy ra lỗi không mong muốn."
            : error.Description;

        var problem = Problem(statusCode: statusCode, title: GetTitle(statusCode), detail: detail);

        if (problem is ObjectResult { Value: ProblemDetails pd })
            pd.Extensions["code"] = error.Code;

        return problem;
    }

    private IActionResult ValidationProblem(IReadOnlyList<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
            modelState.AddModelError(error.Code, error.Description);

        return ValidationProblem(modelState);
    }

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ when (int)type is >= 400 and <= 599 => (int)type,
        _ => StatusCodes.Status500InternalServerError,
    };

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status401Unauthorized => "Chưa xác thực.",
        StatusCodes.Status403Forbidden => "Không có quyền truy cập.",
        StatusCodes.Status404NotFound => "Không tìm thấy.",
        StatusCodes.Status409Conflict => "Xung đột dữ liệu.",
        >= 500 => "Lỗi hệ thống.",
        _ => "Yêu cầu không hợp lệ.",
    };
}