using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
