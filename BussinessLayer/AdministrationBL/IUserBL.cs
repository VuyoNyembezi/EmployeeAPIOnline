using DataAccessLayer.Administration;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.AdministrationBL
{
    public interface IUserBL
    {
        Task<int> CreateProfile(Register register);
        Task<UserModel> Adminlogin(Login login);
        Task<AdminPasswordResetModel> AdminPasswordReset(AdminPasswordResetModel login);
        Task<ForgotPasswordModel> ForgotPassword(ForgotPasswordModel User_Forgotten_Password);
        Task<IEnumerable<UsersModel>> GetUsers();
        Task<IEnumerable<UsersModel>> Get_User_By_Id(int Id);
        Task<User> RemoveUser(User objRemove);
        Task<IEnumerable<DepartmentModel>> GetDepartments();
        Task<IEnumerable<CityModel>> GetCities();
        Task<IEnumerable<RolesModel>> GetRoles();
        Task<ChangeUserRoleModel> ChangeRole(ChangeUserRoleModel users);


        Task<PassKeyModel> Passkey(PassKeyModel forgotpass);
    }
}
