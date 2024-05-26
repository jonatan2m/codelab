using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Members.CreateMember;

public sealed record CreateMemberCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand<Guid>;
