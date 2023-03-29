namespace HD_Backend.Data.Interfaces
{
    public interface IRepositoryManager 
    {
       IDepartmentRepository Department { get; }

       // IFacultyRepository Faculty { get; }

       // ITicketRepository Ticket { get; }

        IUserAuthenticationRepository UserAuthentication { get; }

        Task SaveAsync();
    }
}
