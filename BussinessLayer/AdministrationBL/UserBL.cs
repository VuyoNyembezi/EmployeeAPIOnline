using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccessLayer.Administration;
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BussinessLayer.AdministrationBL
{
    public class UserBL :IUserBL
    {
        TestingContext _db = new TestingContext();
        private string connectionstring;

        public UserBL(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateProfile(Register register)
        {

            if (register != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    var KeyNew = BLSecurity.GeneratePassword(10);
                    var password = BLSecurity.EncodePassword(register.Password, KeyNew);
                    register.Password = password;
                    register.VerificationCode = KeyNew;
                    sqlConnection.Open();
                    var parameters = new DynamicParameters();

                    parameters.Add("@Email", register.Email);
                    parameters.Add("@Password", register.Password);
                    parameters.Add("@AdminName", register.AdminName);
                    parameters.Add("@City", register.CityID);
                    parameters.Add("@Department", register.DepartmentID);
                    parameters.Add("@VerificationCode", register.VerificationCode);
                    parameters.Add("@FK_Role", register.RoleID);
                    parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 2147483647);

                    await sqlConnection.ExecuteAsync("AddUser", parameters, commandType: CommandType.StoredProcedure);
                    var ReturnID = parameters.Get<int>("@Id");
                    sqlConnection.Close();
                    return ReturnID;
                }
            }
            return 0;
        }

        public async Task<UserModel> Adminlogin(Login login)
        {
            var getUser = (from s in _db.Users where s.Email == login.Email select s).FirstOrDefault();
            if (getUser != null)
            {
                var hashCode = getUser.VerificationCode;
                var encodingpasswordString = BLSecurity.EncodePassword(login.Password, hashCode);
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    login.Password = encodingpasswordString;
                    sqlConnection.Open();
                    var parameters = new DynamicParameters();

                    parameters.Add("@Email", login.Email);
                    parameters.Add("@Password", encodingpasswordString);

                    var obj = await sqlConnection.QueryAsync<UserModel>("UserLogin", parameters, commandType: CommandType.StoredProcedure);
                    return obj.FirstOrDefault();
                }
            }
            return null;
        }

        public async Task<AdminPasswordResetModel> AdminPasswordReset(AdminPasswordResetModel reset)
        {
            var getUser = (from s in _db.Users where s.Email == reset.Email select s).FirstOrDefault();
            if (reset != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    var KeyNew = BLSecurity.GeneratePassword(10);
                    var password = BLSecurity.EncodePassword(reset.Password, KeyNew);
                    reset.Password = password;
                    reset.VerificationCode = KeyNew;
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", reset.Email);
                    parameters.Add("@Password", reset.Password);
                    parameters.Add("@VerificationCode", reset.VerificationCode);

                    await sqlConnection.ExecuteAsync("PasswordReset", parameters, commandType: CommandType.StoredProcedure);
                }
                return reset;
            }
            return null;
        }
        public async Task<ForgotPasswordModel> ForgotPassword(ForgotPasswordModel User_Forgotten_Password)
        {
            var getUser = (from s in _db.Users where s.Email == User_Forgotten_Password.Email select s).FirstOrDefault();
            if (getUser != null && User_Forgotten_Password.ResetPasswordKey == getUser.ResetPasswordKey)
            {

                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    var keyNew = BLSecurity.GeneratePassword(10);
                    var password = BLSecurity.EncodePassword(User_Forgotten_Password.Password, keyNew);

                    User_Forgotten_Password.Password = password;
                    User_Forgotten_Password.VerificationCode = keyNew;

                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", User_Forgotten_Password.Email);
                    parameters.Add("@Password", User_Forgotten_Password.Password);
                    parameters.Add("@PassKey", User_Forgotten_Password.ResetPasswordKey);
                    parameters.Add("@VerificationCode", User_Forgotten_Password.VerificationCode);

                    await sqlConnection.ExecuteAsync("ForgottenPassword", parameters, commandType: CommandType.StoredProcedure);
                }
                return User_Forgotten_Password;

            }
            return null;
        }





        public async Task<PassKeyModel> Passkey(PassKeyModel forgotpass)
        {
            var getUser = (from s in _db.Users where s.Email == forgotpass.Email select s).FirstOrDefault();
            if (getUser != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    var PassResetKey = BLSecurity.GeneratePassword(10);
                    forgotpass.ResetPasswordKey = PassResetKey;
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", forgotpass.Email);
                    parameters.Add("@ResetKey", forgotpass.ResetPasswordKey);

                    await sqlConnection.ExecuteAsync("PasswordKey", parameters, commandType: CommandType.StoredProcedure);
                }
                return forgotpass;
            }
            return null;
        }




        public async Task<ChangeUserRoleModel> ChangeRole(ChangeUserRoleModel users)
        {
            var getUser = (from s in _db.Users where s.Id == users.Id select s).FirstOrDefault();
            if (getUser != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", users.Id);
                    parameters.Add("@RoleId", users.FkRoleId);

                    await sqlConnection.ExecuteAsync("UpdateSystemUserRole", parameters, commandType: CommandType.StoredProcedure);
                }
                return users;
            }
            return null;
        }

        public async Task<User> RemoveUser(User objRemove)
        {
            var getUser = (from s in _db.Users where s.Id == objRemove.Id select s).FirstOrDefault();
            if (getUser != null)
            {
                using (var sqlConnection = new SqlConnection(connectionstring))
                {
                    await sqlConnection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", objRemove.Id);

                    await sqlConnection.ExecuteAsync("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
                }
                return objRemove;
            }
            return null;
        }

        //Get Departments ,Roles, Cities,Users##############################################
        public async Task<IEnumerable<UsersModel>> GetUsers()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<UsersModel>("SelectAllSystemUsers", null, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<UsersModel>> Get_User_By_Id(int Id)
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);

                return await sqlConnection.QueryAsync<UsersModel>("SelectSystemUserById",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<DepartmentModel>> GetDepartments()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<DepartmentModel>("GetDepartments", null, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<CityModel>> GetCities()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<CityModel>("GetCities", null, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<RolesModel>> GetRoles()
        {
            using (var sqlConnection = new SqlConnection(connectionstring))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<RolesModel>("GetRoles", null, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
