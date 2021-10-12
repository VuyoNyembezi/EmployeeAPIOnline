using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.EmployeesBL
{
    public interface IEmployeeBL
    {
        Task<IEnumerable<EmployeeModel>> GetEmployees();
        Task<IEnumerable<EmployeeModel>> GetEmployee(int EmployeeID);
        Task<UpdateEmployeeModel> UpdateEmployee(UpdateEmployeeModel employee);
        Task<Employee> TermianteEmployee(Employee employee);
        Task<AddEmployeeModel> InsertEmployee(AddEmployeeModel employee);
        Task<IEnumerable<EmployeeModel>> TerminatedEmployees();
        Task<IEnumerable<EmployeeModel>> GetTerminatedByID(int EmployeeID);

        Task<IEnumerable<GenderModel>> GetGender();
        Task<IEnumerable<NationalityModel>> GetNationality();
    }
}
