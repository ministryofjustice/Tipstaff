using System.Net.Mail;
using System;

namespace Tipstaff
{
    public class email
    {
        public static void SendResetEmail(System.Web.Security.MembershipUser user)
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();

            email.From = new MailAddress("noreply@example.com");
            email.To.Add(new MailAddress(user.Email));

            email.Subject = "Password Reset";
            email.IsBodyHtml = true;
            string link = "http://www.example.com/Account/ResetPassword/?username=" + user.UserName + "&reset=" + HashResetParams(user.UserName, user.ProviderUserKey.ToString());
            email.Body = "<p>" + user.UserName + " please click the following link to reset your password: <a href='" + link + "'>" + link + "</a></p>";
            email.Body += "<p>If you did not request a password reset you do not need to take any action.</p>";

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(email);
        }

        //Method to hash parameters to generate the Reset URL
        public static string HashResetParams(string username, string guid)
        {

            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));

            return HashParams;
        }
    }

}