using System.Security.AccessControl;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repositories;

internal sealed class GatheringRepository : IGatheringRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GatheringRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<List<Gathering>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByNameSpecification(name))
            .ToListAsync(cancellationToken);

    public async Task<Gathering?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdSplitSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Gathering?> GetByIdWithCreatorAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdWithCreatorSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Gathering?> GetByIdWithInvitationsAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Gathering>()
            .Include(g => g.Invitations)
            .Where(gathering => gathering.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    private IQueryable<Gathering> ApplySpecification(
        Specification<Gathering> specification)
    {
        return SpecificationEvaluator.GetQuery(
            _dbContext.Set<Gathering>(),
            specification);
    }

    public void Add(Gathering gathering) =>
        _dbContext.Set<Gathering>().Add(gathering);

    public void Remove(Gathering gathering) =>
        _dbContext.Set<Gathering>().Remove(gathering);
}
