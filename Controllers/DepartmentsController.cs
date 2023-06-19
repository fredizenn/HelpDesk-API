using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Interfaces;
using HD_Backend.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD_Backend.Controllers
{
    [Route("api/Departments")]
    public class DepartmentsController : BaseApiController
    {
        public DepartmentsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext) : base(repository, logger, mapper, dbContext)
        {
            // Constructor for the DepartmentsController class
            // It accepts four parameters: repository, logger, mapper, and dbContext

            // Call the constructor of the base class using the 'base' keyword
            // Pass in the provided parameters to initialize the base class

            // The base class is being initialized with the provided dependencies,
            // which will be used for data access, logging, mapping, and the database context.
            // This allows the DepartmentsController to inherit the functionality and dependencies
            // from its base class, enabling it to perform CRUD operations on departments.
        }


        //create a department
        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto department)
        {
            // Check if a department with the same code already exists
            var dept = await _repository.Department.GetDepartmentByCode(department.Code, trackChanges: false);
            if (dept != null)
            {
                // Return a BadRequest response indicating that a department with the same code already exists
                return BadRequest($"Department with code {dept.Code} already exists");
            }
            else
            {
                // Map the CreateDepartmentDto to a Department entity
                var departmentData = _mapper.Map<Department>(department);

                // Create the department in the repository
                await _repository.Department.CreateDepartment(departmentData);

                // Save changes to the database
                await _repository.SaveAsync();

                // Map the created department back to a DepartmentDto
                var departmentReturn = _mapper.Map<DepartmentDto>(departmentData);

                // Return a Created response with the created department as the body
                return Created($"api/Departments/{departmentReturn.Id}", _mapper.Map<DepartmentDto>(departmentData));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _repository.Department.GetAllDepartments(trackChanges: false);
                var departmentsDto = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Ok(departmentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetDepartments)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        //get department by department code
        [HttpGet("{code}")]
        public async Task<IActionResult> GetDepartmentByCode(string code)
        {
            try
            {
                // Retrieve the department from the repository based on the provided code
                var dept = await _repository.Department.GetDepartmentByCode(code, trackChanges: true);

                // If the department is not found, return a NotFound response
                if (dept == null)
                    return NotFound();

                // Map the department to a DepartmentDto
                var departmentDto = _mapper.Map<DepartmentDto>(dept);

                // Return the DepartmentDto in an Ok response
                return Ok(departmentDto);
            }
            catch (Exception ex)
            {
                // Log the error and return an Internal Server Error response
                _logger.LogError($"Something went wrong in the {nameof(GetDepartmentByCode)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }


        //update department
        [HttpPut("{departmentId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDepartment))]

        public async Task<IActionResult> UpdateDepartment(long departmentId, [FromBody] UpdateDepartmentDto department)
        {
            // Retrieve the department from the HttpContext items
            var departmentData = HttpContext.Items["department"] as Department;

            // Map the properties of the UpdateDepartmentDto to the department entity
            _mapper.Map(department, departmentData);

            // Save the changes to the repository
            await _repository.SaveAsync();

            // Return the updated department in an Ok response
            return Ok(departmentData);
        }

        [HttpDelete("{departmentId}")]
        [ServiceFilter(typeof(ValidateDepartment))]
        public async Task<ActionResult> DeleteDepartment(long departmentId)
        {
            // Retrieve the department from the repository based on the departmentId
            var department = await _repository.Department.GetDepartment(departmentId, trackChanges: false);

            // If the department exists, delete it from the repository and save the changes
            if (department != null)
            {
                await _repository.Department.DeleteDepartment(department);
                await _repository.SaveAsync();

                // Return true in an Ok response to indicate successful deletion
                return Ok(true);
            }
            else
            {
                // If the department is not found, return a NotFound response
                return NotFound();
            }
        }



    }

}
