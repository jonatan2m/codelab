namespace Gatherly.Application.Gatherings.GetGatheringById;

public sealed record AttendeeResponse(Guid MemberId, DateTime CreatedOnUtc);
