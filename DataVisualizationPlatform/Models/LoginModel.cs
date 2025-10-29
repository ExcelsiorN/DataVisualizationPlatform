using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizationPlatform.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ValidationResult Validate()
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrWhiteSpace(Username))
            {
                result.IsValid = false;
                result.UsernameError = "请输入用户名";
            }
            else if (Username.Length < 3)
            {
                result.IsValid = false;
                result.UsernameError = "用户名至少3个字符";
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                result.IsValid = false;
                result.PasswordError = "请输入密码";
            }
            else if (Password.Length < 6)
            {
                result.IsValid = false;
                result.PasswordError = "密码至少6个字符";
            }

            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string UsernameError { get; set; }
        public string PasswordError { get; set; }
        public string GeneralError { get; set; }
    }
}
