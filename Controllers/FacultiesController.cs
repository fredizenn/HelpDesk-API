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

        //create faculty
        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateFaculty([FromBody] CreateFacultyDto faculty)
        {
            // Action method to create a new faculty
            // It accepts a CreateFacultyDto object in the request body

            // Check if a faculty with the provided code already exists
            var fac = await _repository.Faculty.GetFacultyByCode(faculty.Code, trackChanges: false);
            if (fac != null)
            {
                // If a faculty with the same code exists, return a BadRequest response
                return (BadRequest($"Faculty with code: {fac.Code} already exists"));
            }
            else
            {
                // If the faculty code is unique, create a new Faculty entity
                var facultyData = _mapper.Map<Faculty>(faculty);

                // Add the new faculty entity to the repository
                await _repository.Faculty.CreateFaculty(facultyData);

                // Save the changes to the database
                await _repository.SaveAsync();

                // Map the created faculty entity to a FacultyDto object
                var facultyReturn = _mapper.Map<FacultyDto>(facultyData);

                // Return a Created response with the created faculty's location and data
                return Created($"api/Faculties/{facultyReturn.Id}", _mapper.Map<FacultyDto>(facultyData));
            }
        }


        //get all faculties
        [HttpGet]
        public async Task<IActionResult> GetFaculties()
        {
            try
            {
                // Action method to retrieve all faculties
                // It does not require any parameters

                // Retrieve all faculties from the repository
                var faculties = await _repository.Faculty.GetAllFaculties(trackChanges: false);

                // Map the faculties to an IEnumerable<FacultyDto> using AutoMapper
                var facultiesDto = _mapper.Map<IEnumerable<FacultyDto>>(faculties);

                // Return an Ok response with the list of faculty DTOs
                return Ok(facultiesDto);
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and return a StatusCode 500 (Internal Server Error) response
                _logger.LogError($"Something went wrong in the {nameof(GetFaculties)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        //get faculty by code
        [HttpGet("{code}")]
        public async Task<IActionResult> GetFacultyByCode(string code)
        {
            try
            {
                // Action method to retrieve a faculty by its code
                // Takes a string parameter 'code' representing the faculty code

                // Retrieve the faculty from the repository based on the code
                var faculty = await _repository.Faculty.GetFacultyByCode(code, trackChanges: false);

                // If the faculty is not found, return a NotFound response
                if (faculty == null)
                    return NotFound();

                // Map the faculty to a FacultyDto using AutoMapper
                var facultyDto = _mapper.Map<FacultyDto>(faculty);

                // Return an Ok response with the faculty DTO
                return Ok(facultyDto);
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and return a StatusCode 500 (Internal Server Error) response
                _logger.LogError($"Something went wrong in the {nameof(GetFacultyByCode)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        //update a faculty
        [HttpPut("{facultyId}")]
        [ServiceFilter(typeof(ValidateFaculty))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateFaculty(long facultyId, [FromBody] UpdateFacultyDto faculty)
        {
            // Action method to update a faculty by its ID
            // Takes a long parameter 'facultyId' representing the faculty ID
            // Takes a JSON payload 'faculty' containing the updated faculty data

            // Retrieve the faculty from the HttpContext items based on the facultyId
            var facultyData = HttpContext.Items["faculty"] as Faculty;

            // Map the updated faculty data to the faculty entity using AutoMapper
            _mapper.Map(faculty, facultyData);

            // Save the changes to the repository
            await _repository.SaveAsync();

            // Return an Ok response with the updated faculty data
            return Ok(facultyData);
        }


        //delete a faculty
        [HttpDelete("{facultyId}")]
        [ServiceFilter(typeof(ValidateFaculty))]
        public async Task<IActionResult> DeleteFaculty(long facultyId)
        {
            // Action method to delete a faculty by its ID
            // Takes a long parameter 'facultyId' representing the faculty ID

            // Retrieve the faculty from the repository based on the facultyId
            var faculty = await _repository.Faculty.GetFacultyById(facultyId, trackChanges: false);

            if (faculty != null)
            {
                // If the faculty exists, delete it from the repository
                await _repository.Faculty.DeleteFaculty(faculty);

                // Save the changes to the repository
                await _repository.SaveAsync();

                // Return an Ok response with a boolean value indicating successful deletion
                return Ok(true);
            }
            else
            {
                // If the faculty doesn't exist, return a NotFound response
                return NotFound();
            }
        }
    }
}
