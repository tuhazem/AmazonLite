using Amazon.Application.Interfaces;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AmazonDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AmazonDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
