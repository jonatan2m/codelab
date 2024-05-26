using MediatR;

namespace Gatherly.Application.Invitations.SendInvitation;

public sealed record SendInvitationCommand(Guid MemberId, Guid GatheringId) : IRequest<Unit>;