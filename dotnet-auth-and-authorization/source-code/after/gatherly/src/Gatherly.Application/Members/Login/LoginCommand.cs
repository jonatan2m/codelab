using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Members.Login;
public record LoginCommand(string Email) : ICommand<string>;
