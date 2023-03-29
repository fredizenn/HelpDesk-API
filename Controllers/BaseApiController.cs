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

        public BaseApiController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
    }
}
