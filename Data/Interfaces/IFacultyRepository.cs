using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAllFaculties(bool trackChanges);

        Task<Faculty?> GetFacultyById(long facultyId, bool trackChanges);

        Task<Faculty?> GetFacultyByCode(string code, bool trackChanges);

        Task CreateFaculty(Faculty faculty);

        Task DeleteFaculty(Faculty faculty);

    }
}
