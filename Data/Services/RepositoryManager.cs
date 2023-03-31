using AutoMapper;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HD_Backend.Data.Services
{
    public class RepositoryManager : IRepositoryManager
    {
        private HelpDeskDbContext _dbContext;
        private ITicketRepository _ticketRepository;
        private IDepartmentRepository _departmentRepository;
        private IFacultyRepository _facultyRepository;
        private IUserAuthenticationRepository _userAuthenticationRepository;
        private UserManager<User> _userManager;
        private IMapper _mapper;
        private IConfiguration _config;

        public RepositoryManager(HelpDeskDbContext helpDeskDbContext, UserManager<User> userManager, IMapper mapper, IConfiguration config)
        {
            _dbContext = helpDeskDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }

        public ITicketRepository Ticket
        {
            get
            {
                if (_ticketRepository is null)
                    _ticketRepository = new TicketRepository(_dbContext);
                return _ticketRepository;
            }
        }

        public IDepartmentRepository Department
        {
            get
            {
                if (_departmentRepository is null)
                    _departmentRepository = new DepartmentRepository(_dbContext);
                return _departmentRepository;
            }
        }

        public IFacultyRepository Faculty
        {
            get
            {
                if (_facultyRepository is null)
                    _facultyRepository = new FacultyRepository(_dbContext);
                return _facultyRepository;
            }
        }


        public IUserAuthenticationRepository UserAuthentication
        {
            get
            {
                if (_userAuthenticationRepository is null)
                    _userAuthenticationRepository = new UserAuthenticationRepository(_userManager, _config, _mapper);
                return _userAuthenticationRepository;
            }
        }

        public Task SaveAsync() => _dbContext.SaveChangesAsync();
    }
}
