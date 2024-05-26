using Gatherly.Domain.Enums;

namespace Gatherly.Application.Gatherings.GetGatheringById;

public sealed record InvitationResponse(Guid InvitationId, InvitationStatus Status);
