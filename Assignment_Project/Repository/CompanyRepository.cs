using Assignment_Project.Data;
using Assignment_Project.Models;
using Assignment_Project.Repository.IRepository;

namespace Assignment_Project.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public async Task<Company> UpdateAsync(Company entity)
        {
            _db.Companies.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
