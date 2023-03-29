using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartments(bool trackChanges);

        Task<Department?> GetDepartment(long departmentId, bool trackChanges);

        Task<Department?> GetDepartmentByCode(string code, bool trackChanges);

        Task CreateDepartment (Department department);

        Task DeleteDepartment(Department department);
    }
}
