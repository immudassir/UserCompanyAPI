using Assignment_Project.Models;
using System.Linq.Expressions;

namespace Assignment_Project.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<User> GetUserWithCompaniesAsync(int userId);
        Task<Company> GetCompanyWithUserAsync(int companyId);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
