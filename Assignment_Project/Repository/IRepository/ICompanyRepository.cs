using Assignment_Project.Models;

namespace Assignment_Project.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> UpdateAsync(Company entity);

    }
}
