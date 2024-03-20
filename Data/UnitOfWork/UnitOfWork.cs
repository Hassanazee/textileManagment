using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using textileManagment.Domain.Context;

namespace KametiManager.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork( ApplicationDbContext context , IHttpContextAccessor contextAccessor)
    {
        Context = context;
        _httpContextAccessor = contextAccessor;

    }
    public ApplicationDbContext Context { get; }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction? transaction)
    {
        if (transaction != null)
        {
            await transaction.CommitAsync();
        }
    }

    public async Task RollBackTransactionAsync(IDbContextTransaction? transaction)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public T GetRepository<T>() where T : class
    {
        var result = Activator.CreateInstance(typeof(T), Context, _httpContextAccessor);
        return result as T ?? throw new InvalidOperationException("This Error shouldn't Arise!");
    }
}