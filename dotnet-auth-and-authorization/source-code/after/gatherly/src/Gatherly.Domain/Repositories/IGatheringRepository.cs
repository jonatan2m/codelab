using Gatherly.Domain.Entities;

namespace Gatherly.Domain.Repositories;

public interface IGatheringRepository
{
    Task<List<Gathering>> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Gathering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Gathering?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Gathering gathering);

    void Remove(Gathering gathering);
}
