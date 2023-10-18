using Assignment_Project.Models;
using System.Linq.Expressions;

namespace Assignment_Project.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> UpdateAsync(User entity);
    }
}
