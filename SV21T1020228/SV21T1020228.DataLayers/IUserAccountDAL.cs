using SV21T1020228.DomainModels;

namespace SV21T1020228.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra đăng nhập
        /// Nếu đúng -> trả về thông tin user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount Authorize(string username, string password);

        bool ChangePassword(string username, string oldPassword, string newPassword);
        int Create(string fullName, string username, string password);
    }
}
