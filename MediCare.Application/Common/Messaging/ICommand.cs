using ErrorOr;
using MediatR;

namespace MediCare.Application.Common.Messaging;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>> { }
