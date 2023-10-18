using Assignment_Project.Data;
using Assignment_Project.Models;
using Assignment_Project.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Assignment_Project.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db) 
        {
            this._db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> user = dbSet;

            if (!tracked)
            {
                user = user.AsNoTracking();
            }
            if (filter != null)
            {
                user = user.Where(filter);
            }
            return await user.FirstOrDefaultAsync();

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> user = dbSet;

            if (filter != null)
            {
                user = user.Where(filter);
            }
            return await user.ToListAsync();

        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetUserWithCompaniesAsync(int userId)
        {
            return await _db.Users
                .Include(u => u.Companies)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<Company> GetCompanyWithUserAsync(int companyId)
        {
            return await _db.Companies
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }
    }
}
