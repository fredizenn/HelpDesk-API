using HD_Backend.Data.Entities;
using HD_Backend.Data.GenericRepository.cs.Service;
using HD_Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HD_Backend.Data.Services
{
    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(HelpDeskDbContext helpDeskDbContext) : base (helpDeskDbContext)
        {

        }

        public async Task CreateTicket(Ticket ticket, long facultyId, long departmentId)
        {
            ticket.DepartmentId = departmentId;
            ticket.FacultyId = facultyId;
            await CreateAsync(ticket);
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets(bool trackChanges)
            => await FindAllAsync(trackChanges).Result.OrderBy(c => c.Id).ToListAsync();

        public async Task<Ticket?> GetTicket(long ticketId, long departmentId, long facultyId, bool trackChanges)
            => await FindByConditionAsync(c => c.Id.Equals(ticketId), trackChanges).Result.SingleOrDefaultAsync();

        public async Task DeleteTicket(Ticket ticket) => await RemoveAsync(ticket);
    }
    
}
