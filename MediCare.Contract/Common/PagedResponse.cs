namespace MediCare.Contract.Common;

public sealed record PagedResponse<T>(int Page, int PageSize, int TotalCount, List<T> Items)
{
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
