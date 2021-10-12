using BussinessLayer.AdministrationBL;
using BussinessLayer.SystemLogging;
using BussinessLayer.TokenServices;
using DataAccessLayer.Administration;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPIOnline.Controllers
{
    [Route("api/login")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class LoginController : ControllerBase
    {
        private IUserBL _loginbl;
        private ILogsManager _logger;
        private readonly ITokenService _tokenService;
        public LoginController(IUserBL loginBL, ILogsManager logger,ITokenService tokenService)
        {
            _loginbl = loginBL;
            _logger = logger;
            _tokenService = tokenService;
        }

        [Route("InsertUser")]
        [HttpPost]
        public async Task<Response> RegisterUser(Register register)
        {
            var returnobj = new Response
            {
                ReturnId = 0,
                Status = "Failed",
                Message = "Sign up process failed"

            };
            try
            {
                if (register != null)
                {
                   
                    var result = await _loginbl.CreateProfile(register);
                    if (result >= 1)
                    {
                        _logger.Infor($"new user Added to the Sytem administration Rights name : {register.AdminName }, Email : { register.Email}");
                        returnobj.ReturnId = result;
                        returnobj.Status = "Success";
                        returnobj.Message = "user added successful";
                    }
                    return returnobj;
                }
                return returnobj;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return returnobj;
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<Response> LoginUser(Login login)
        {
            var returnobj = new Response
            {
                ReturnId = 0,
                Status = "Invalid",
                Message = "Please check Input",
                Role = null
            };
            try
            {
                if (login != null)
                {
                    
                    var results = await _loginbl.Adminlogin(login);
                    if (results != null)
                    {
                        var token = _tokenService.GenerateJwtToken(login.Email);
                        if (token != null )
                        {
                         _logger.Infor($"Admin/User with email = {login.Email} Accessed the Sytem");
                        returnobj.Status = "Success";
                        returnobj.Message = "Log in successfully";
                        returnobj.Role = results.RoleID;
                            returnobj.Token = token;
                        }
                        return returnobj;
                        
                    }
                    return returnobj;
                }
                return returnobj;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }
        [Route("Reset")]
        [HttpPut]
        [Authorize()]
        public async Task<ActionResult<Response>> ResetPasword(AdminPasswordResetModel passwordreset)
        {
            try
            {
                var result = await _loginbl.AdminPasswordReset(passwordreset);
                if (result != null)
                {
                    if (passwordreset.Email != null)
                    {
                        StatusCode(StatusCodes.Status200OK, $"record with Id = {passwordreset.Email} password Updated");
                    }
                    _logger.Infor($"User's passsword with Email = {passwordreset.Email} is reseted ");
                    return new Response { Status = "Success", Message = "Password reset successful", ReturnId=1 };
                }
                return new Response { Status = "Failed", Message = "process failed please check  input",ReturnId=0 };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Accessing data from the database");
            }
        }
        [Route("Forgottenpassword")]
        [HttpPut]
        public async Task<ActionResult<Response>> ForgortPassword(ForgotPasswordModel newpassword)
        {
            try
            {
                var result = await _loginbl.ForgotPassword(newpassword);
                if (result != null)
                {
                    _logger.Infor($"User's passsword with Email = {newpassword.Email}  Changed ");
                    return new Response { Status = "Success", Message = "Password successfully changed" };
                }
                return new Response { Status = "Failed", Message = "Process failed please Chack your input" };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Data");
            }
        }
        [Route("UserResetPass")]
        [HttpPut]
        public async Task<ActionResult<Response>> generateResetKey(PassKeyModel forgot)
        {
            try
            {
                var result = await _loginbl.Passkey(forgot);
                if (result != null)
                {
                    _logger.Infor($"User's with Email = {forgot.Email} is reset key sent ");
                    return new Response { Status = "Success", Message = "reset link sent please check your email to get the reset key" };
                }
                return new Response { Status = "Failed", Message = "process failed please check input " };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving data from the Database");
            }
        }
        [Route ("ChangeRole")]
        [HttpPut]
        [Authorize()]
        public async Task<ActionResult<Response>> ChangeRole(ChangeUserRoleModel users)
        {
            try
            {
                var result = await _loginbl.ChangeRole(users);
                if (result != null)
                {
                    _logger.Infor($"User with ID = {users.Id} role is changed to {users.FkRoleId}");
                    return new Response { Status = "Success", Message = "Role Changed successful" };
                }
                return new Response { Status = "Failed", Message = "Changing role proccess failed" };
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Accessing data from the database");
            }
        }
        [Route("Roles")]
        [HttpGet]
        public async Task<ActionResult> GetRoles()
        {
            try
            {
                return Ok(await _loginbl.GetRoles());
            }
            catch (Exception EX)
            {
                _logger.Error(EX.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [Route("Departments")]
        [HttpGet]
        public async Task<ActionResult> GetDepartment()
        {
            try
            {
                return Ok(await _loginbl.GetDepartments());
            }
            catch (Exception EX)
            {
                _logger.Error(EX.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [Route("Cities")]
        [HttpGet]
        public async Task<ActionResult> GetCity()
        {
            try
            {
                return Ok(await _loginbl.GetCities());
            }
            catch (Exception EX)
            {
                _logger.Error(EX.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                return Ok(await _loginbl.GetUsers());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpGet("Users/{Id}")]
        [Authorize()]
        public async Task<ActionResult<Object>> GetUser(int Id)
        {
            try
            {
                var result = Ok(await _loginbl.Get_User_By_Id(Id));
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
      
    }
}
