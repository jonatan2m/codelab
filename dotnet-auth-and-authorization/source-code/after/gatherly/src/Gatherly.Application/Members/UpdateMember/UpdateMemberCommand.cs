using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Members.UpdateMember;

public sealed record UpdateMemberCommand(Guid MemberId, string FirstName, string LastName) : ICommand;
