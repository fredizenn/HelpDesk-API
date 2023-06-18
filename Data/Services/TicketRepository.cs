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

        public async Task CreateTicket(Ticket ticket)
        {
            await CreateAsync(ticket);
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets(bool trackChanges)
            => await FindAllAsync(trackChanges).Result
            .Include(t => t.Faculty)
            .Include(t => t.Department)
            .OrderByDescending(c => c.Id)
            .ToListAsync();

        public async Task<Ticket?> GetTicket(long ticketId, bool trackChanges)
            => await FindByConditionAsync(c => c.Id.Equals(ticketId), trackChanges).Result
             .Include(t => t.Faculty)
             .Include(t => t.Department)
             .SingleOrDefaultAsync();

        public async Task<Ticket?> GetTicketByCode(string code, bool trackChanges) => 
                 await FindByConditionAsync(t => t.Code.Equals(code), trackChanges).Result
                .Include(t => t.Faculty)
                .Include(t => t.Department)
                .SingleOrDefaultAsync();

        public async Task DeleteTicket(Ticket ticket) => await RemoveAsync(ticket);
    }
    
}
