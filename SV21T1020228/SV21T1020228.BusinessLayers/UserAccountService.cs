using SV21T1020228.DataLayers;
using SV21T1020228.DataLayers.SQL_Server;
using SV21T1020228.DomainModels;
using System.Data;

namespace SV21T1020228.BusinessLayers
{
    public enum UserTypes
    {
        Employee,
        Customer
    }

    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccount;
        private static readonly IUserAccountDAL customerAccount;

        static UserAccountService()
        {
            employeeAccount = new EmployeeAccountDAL(Configuration.ConnectionString);
            customerAccount = new CustomerAccountDAL(Configuration.ConnectionString);
        }
        
        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccount.Authorize(username, password);
            }
            else
            {
                return customerAccount.Authorize(username, password);
            }
        }

        public static bool ChangePassword(UserTypes userTypes, string username, string oldPassword, string newPassword)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccount.ChangePassword(username, oldPassword, newPassword);
            }
            else
            {
                return customerAccount.ChangePassword(username, oldPassword, newPassword);
            }
        }

        public static int Create(UserTypes userTypes, string fullName, string username, string password)
        {
            if (userTypes == UserTypes.Customer)
            {
                return customerAccount.Create(fullName, username, password);
            }
            else
            {
                return employeeAccount.Create(fullName, username, password);
            }
        }
    }
}
