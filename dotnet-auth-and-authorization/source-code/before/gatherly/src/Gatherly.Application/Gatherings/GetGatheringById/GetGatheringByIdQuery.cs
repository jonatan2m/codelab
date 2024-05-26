using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Gatherings.GetGatheringById;

public sealed record GetGatheringByIdQuery(Guid GatheringId) : IQuery<GatheringResponse>;
