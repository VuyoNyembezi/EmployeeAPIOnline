using BussinessLayer.EmployeesBL;
using BussinessLayer.SystemLogging;
using DataAccessLayer.Helper;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmployeeAPIOnline.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeBL _employeeBL;
        private ILogsManager _logger;
        public EmployeeController(IEmployeeBL employeeBL, ILogsManager logger)
        {
            _employeeBL = employeeBL;
            _logger = logger;
        }
        [HttpGet]
        [Authorize()]
        public async Task<ActionResult<Object>> Get()
        {
            try
            {
                _logger.Infor("Employees data accessed");
                return Ok(await _employeeBL.GetEmployees());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database server");
            }
        }
        [HttpGet("{EmployeeID}")]
        [Authorize()]
        public async Task<ActionResult<Object>> Get(int EmployeeID)
        {
            try
            {
                var result = await _employeeBL.GetEmployee(EmployeeID);
                if (result == null)
                {
                    _logger.Infor($"No records were found whith ID ={EmployeeID}");
                    return NotFound();
                }
                _logger.Infor($"Employee with ID ={EmployeeID}, Data was accessed");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [Route("Terminated")]
        [HttpGet]
        [Authorize()]
        public async Task<ActionResult<Object>> GetTerminatedEmployees()
        {
            try
            {
                return Ok(await _employeeBL.TerminatedEmployees());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Data from database server");
            }
        }
        [HttpGet("Terminated/{EmployeeID}")]
        [Authorize()]
        public async Task<ActionResult<Object>> GetTerminatedById(int EmployeeID)
        {
            try
            {
                var result = Ok(await _employeeBL.GetTerminatedByID(EmployeeID));
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the server");
            }
        }

      [Route("UpdateEmployee")]
      [HttpPut]
        [Authorize()]
        public async Task<ActionResult<Responder>> UpdateEmployee(UpdateEmployeeModel employee)
        {
            try
            {
                var result = await _employeeBL.UpdateEmployee(employee);
                if (result != null)
                {
                    _logger.Infor($"Employee Details of ID = {employee.EmployeeId} were updated ");
                    return new Responder { Message="Employee Successfully Updated", Status="Success", ReturnId =1};
                }
                return new Responder { Message = " Updated proccess Failed", Status = "Failed", ReturnId = -1 };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Accessing data from the database");
            }
        }
        [Route("TerminateEmployee")]
        [HttpDelete]
        [Authorize()]

        public async Task<ActionResult<Responder>> TerminateEmployee(Employee employee)
        {
            try
            {
                var result = await _employeeBL.TermianteEmployee(employee);
                if (result != null)
                {
                    _logger.Infor($"Employee with ID {employee.EmployeeId} is Terminated");
                    return new Responder { Message = "Employee successfully Terminated ", Status = "Success", ReturnId = 1 };
                  
                }
                return new Responder { Message = "Process Failed", Status = "Failed", ReturnId = -1 };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Accessing the Database ");
            }
        }
        [Route("AddEmployee")]
        [HttpPost]

        [Authorize()]
        public async Task<ActionResult<Responder>> InsertEmployee(AddEmployeeModel employee)
        {
            try
            {
                var result = await _employeeBL.InsertEmployee(employee);
                if (result != null)
                {
                    _logger.Infor($"new record succeffully added  {employee.FirstName}  {employee.Surname}");
                    return new Responder {Message="new employee record added", Status="Success", ReturnId=1};
                }
                return new Responder { Message = "no new record added process failed", Status = "Failed", ReturnId = -1 };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error, try again or contact support");
            }
        }
        [Route("Gender")]
        [HttpGet]
        public async Task<ActionResult> GetGenders()
        {
            try
            {
                return Ok(await _employeeBL.GetGender());
            }
            catch (Exception EX)
            {
                _logger.Error(EX.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [Route("Nationality")]
        [HttpGet]
        public async Task<ActionResult> GetDepartment()
        {
            try
            {
                return Ok(await _employeeBL.GetNationality());
            }
            catch (Exception EX)
            {
                _logger.Error(EX.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
