using Microsoft.AspNetCore.Identity;

namespace Business
{

    public class AccountPasswordHasher : PasswordHasher<Account>
    {
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public override string HashPassword(Account user, string password)
        {
            return password;
        }

        public override PasswordVerificationResult VerifyHashedPassword(Account user, string hashedPassword, string providedPassword)
        {
            // 此处实现自己的加密验证路基
            return PasswordVerificationResult.Success;
        }
    }

}
