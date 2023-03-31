using HD_Backend.Data.Entities;
using HD_Backend.Data.GenericRepository.cs.Service;
using HD_Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HD_Backend.Data.Services
{
    public class FacultyRepository : RepositoryBase<Faculty>, IFacultyRepository
    {
        public FacultyRepository(HelpDeskDbContext helpDeskDbContext) : base(helpDeskDbContext)
        {
        }

        public async Task CreateFaculty(Faculty faculty)
        {
            await CreateAsync(faculty);
        }

        public async Task<Faculty?> GetFacultyById(long facultyId, bool trackChanges)
            => await FindByConditionAsync(d => d.Id.Equals(facultyId), trackChanges).Result.SingleOrDefaultAsync();

       public async Task<Faculty?> GetFacultyByCode(string code, bool trackChanges)
         => await FindByConditionAsync(d => d.Code.Equals(code), trackChanges).Result
          .Include(f => f.Department)
          .SingleOrDefaultAsync();


        public async Task DeleteFaculty(Faculty faculty)
            => await RemoveAsync(faculty);


        public async Task<IEnumerable<Faculty>> GetAllFaculties(bool trackChanges)
            => await FindAllAsync(trackChanges).Result
            .Include(f => f.Department)
            .OrderBy(d => d.Name).ToListAsync();
    }
}
