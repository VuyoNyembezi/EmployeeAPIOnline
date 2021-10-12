using Dapper;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.EmployeesBL
{
    public class EmployeeBL : IEmployeeBL
    {
        private string connectionstring;
        public EmployeeBL(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployees()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<EmployeeModel>(
                "GetAllEmployee",
                null,
                 commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployee(int EmployeeID)
        {
             using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", EmployeeID);

                return await sqlConnection.QueryAsync<EmployeeModel>("GetEmployee",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<EmployeeModel>> GetTerminatedByID(int EmployeeID)
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", EmployeeID);
                return await sqlConnection.QueryAsync<EmployeeModel>("GetTerminatedEmployeeById", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<EmployeeModel>> TerminatedEmployees()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<EmployeeModel>(
                    "TerminatedEmployees",
                    null,
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<UpdateEmployeeModel> UpdateEmployee(UpdateEmployeeModel employee)
        {
            if (employee != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", employee.EmployeeId);
                    parameters.Add("@FirstName", employee.FirstName);
                    parameters.Add("@Surname", employee.Surname);

                    await sqlConnection.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
                return employee;
            }
            return null;
        }
        public async Task<Employee> TermianteEmployee(Employee employee)
        {
            if (employee != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", employee.EmployeeId);

                    await sqlConnection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);
                    return employee;
                }
            }
            return null;
        }
        public async Task<AddEmployeeModel> InsertEmployee(AddEmployeeModel employee)
        {
            if (employee != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();

                    parameters.Add("@FirstName", employee.FirstName);
                    parameters.Add("@Surname", employee.Surname);
                    parameters.Add("@DateOfBirth", employee.DateOfBirth);
                    parameters.Add("@GenderID", employee.FkGenderId);
                    parameters.Add("@NationID", employee.FkNationId);

                    await sqlConnection.ExecuteAsync("InsertEmployee", parameters, commandType: CommandType.StoredProcedure);
                    return employee;
                }
            }
            return null;
        }

        //GET Gender, Nationality##########################################################
        public async Task<IEnumerable<GenderModel>> GetGender()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<GenderModel>("GetGender", null, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<NationalityModel>> GetNationality()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<NationalityModel>("GetNationality", null, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
