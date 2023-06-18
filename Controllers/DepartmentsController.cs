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
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto department)
        {
            var dept = await _repository.Department.GetDepartmentByCode(department.Code, trackChanges: false);
            if (dept != null)
            {
                return BadRequest($"Department with code {dept.Code} already exists");
            }
            else
            {
                var departmentData = _mapper.Map<Department>(department);
                await _repository.Department.CreateDepartment(departmentData);
                await _repository.SaveAsync();
                var departmentReturn = _mapper.Map<DepartmentDto>(departmentData);
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

        [HttpGet("{code}")]
        public async Task<IActionResult> GetDepartmentByCode(string code)
        {
            try
            {
                var dept = await _repository.Department.GetDepartmentByCode(code, trackChanges: true);
                if (dept == null) return NotFound();

                var departmentDto = _mapper.Map<DepartmentDto>(dept);
                return Ok(departmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetDepartmentByCode)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPut("{departmentId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDepartment))]

        public async Task<IActionResult> UpdateDepartment(long departmentId, [FromBody] UpdateDepartmentDto department)
        {
            var departmentData = HttpContext.Items["department"] as Department;
            _mapper.Map(department, departmentData);
            await _repository.SaveAsync();
            return Ok(departmentData);
        }

        [HttpDelete("{departmentId}")]
        [ServiceFilter(typeof(ValidateDepartment))]

        public async Task<ActionResult> DeleteDepartment(long departmentId)
        {
            var department = await _repository.Department.GetDepartment(departmentId, trackChanges: false);
            if (department != null)
            {
                await _repository.Department.DeleteDepartment(department);
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
