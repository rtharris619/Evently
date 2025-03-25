﻿using Evently.Modules.Events.Domain.Abstractions;
using MediatR;

namespace Evently.Modules.Events.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

public interface IBaseCommand;
