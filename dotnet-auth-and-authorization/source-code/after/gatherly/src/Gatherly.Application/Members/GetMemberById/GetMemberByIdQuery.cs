using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Members.GetMemberById;

public sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<MemberResponse>;