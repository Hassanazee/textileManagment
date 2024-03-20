using Microsoft.EntityFrameworkCore.Storage;
using textileManagment.Domain.Context;

public interface IUnitOfWork : IDisposable
{
    Task SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction? transaction);
    Task RollBackTransactionAsync(IDbContextTransaction? transaction);
    ApplicationDbContext Context { get; }
    T GetRepository<T>() where T : class;
}