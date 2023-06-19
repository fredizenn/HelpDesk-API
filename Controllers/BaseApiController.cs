using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD_Backend.Controllers
{

    public class BaseApiController : ControllerBase
    {
        protected readonly IRepositoryManager _repository;
        protected readonly ILoggerManager _logger;
        protected readonly IMapper _mapper;
        protected readonly HelpDeskDbContext _dbContext;

        public BaseApiController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext)
        {
            // Constructor for the base API controller

            // Assign the repository instance passed as a parameter to the _repository field
            _repository = repository;

            // Assign the logger instance passed as a parameter to the _logger field
            _logger = logger;

            // Assign the mapper instance passed as a parameter to the _mapper field
            _mapper = mapper;

            // Assign the dbContext instance passed as a parameter to the _dbContext field
            _dbContext = dbContext;
        }

    }
}
