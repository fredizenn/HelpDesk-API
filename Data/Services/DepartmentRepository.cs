using HD_Backend.Data.Entities;
using HD_Backend.Data.GenericRepository.cs.Service;
using HD_Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HD_Backend.Data.Services
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(HelpDeskDbContext helpDeskDbContext) : base(helpDeskDbContext)
        {
        }

        public async Task CreateDepartment(Department department)
        {
            await CreateAsync(department);
        }

        public async Task<Department?> GetDepartment(long departmentId, bool trackChanges) 
            => await FindByConditionAsync(d => d.Id.Equals(departmentId), trackChanges).Result.SingleOrDefaultAsync();

        public async Task<Department?> GetDepartmentByCode(string code, bool trackChanges)
            => await FindByConditionAsync(d => d.Code.Equals(code), trackChanges).Result.SingleOrDefaultAsync();

        public async Task DeleteDepartment(Department department)
            => await RemoveAsync(department);


        public async Task<IEnumerable<Department>> GetAllDepartments(bool trackChanges)
            => await FindAllAsync(trackChanges).Result.OrderBy(d => d.Name).ToListAsync();
        }
}
