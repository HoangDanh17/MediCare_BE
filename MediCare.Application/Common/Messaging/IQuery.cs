using ErrorOr;
using MediatR;

namespace MediCare.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>> { }
