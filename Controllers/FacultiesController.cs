using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Interfaces;
using HD_Backend.Filters.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesController : BaseApiController
    {
        public FacultiesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext) : base(repository, logger, mapper, dbContext)
        {

        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> CreateFaculty([FromBody] CreateFacultyDto faculty)
        {
            var fac = await _repository.Faculty.GetFacultyByCode(faculty.Code, trackChanges: false);
            if (fac != null)
            {
                return (BadRequest($"Faculty with code: {fac.Code} already exists"));
            }
            else
            {
                var facultyData = _mapper.Map<Faculty>(faculty);
                await _repository.Faculty.CreateFaculty(facultyData);
                await _repository.SaveAsync();
                var facultyReturn = _mapper.Map<FacultyDto>(facultyData);
                return Created($"api/Faculties/{facultyReturn.Id}", _mapper.Map<FacultyDto>(facultyData));
            }
        }


        [HttpGet]

        public async Task<IActionResult> GetFaculties()
        {
            try
            {
                var faculties = await _repository.Faculty.GetAllFaculties(trackChanges: false);
                var facultiesDto = _mapper.Map<IEnumerable<FacultyDto>>(faculties);
                return Ok(facultiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFaculties)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetFacultyByCode(string code)
        {
            try
            {
                var faculty = await _repository.Faculty.GetFacultyByCode(code, trackChanges: false);
                if (faculty == null) return NotFound();

                var facultyDto = _mapper.Map<FacultyDto>(faculty);
                return Ok(facultyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetFacultyByCode)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{facultyId}")]
        [ServiceFilter(typeof(ValidateFaculty))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateFaculty(long facultyId, [FromBody] UpdateFacultyDto faculty)
        {
            var facultyData = HttpContext.Items["faculty"] as Faculty;
            _mapper.Map(faculty, facultyData);
            await _repository.SaveAsync();
            return Ok(facultyData);
        }

        [HttpDelete("{facultyId}")]
        [ServiceFilter(typeof(ValidateFaculty))]

        public async Task<IActionResult> DeleteFaculty(long facultyId)
        {
            var faculty = await _repository.Faculty.GetFacultyById(facultyId, trackChanges: false);
            if(faculty != null)
            {
                await _repository.Faculty.DeleteFaculty(faculty);
                await _repository.SaveAsync();
                return Ok(true);
            }
            else
            {
                return NotFound();
            }
        }

      
    }
}
