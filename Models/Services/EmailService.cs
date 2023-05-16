using ISpan.Project_02_DessertStory.Customer.Models.ApiKey;
using ISpan.Project_02_DessertStory.Customer.Models.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System;

namespace ISpan.Project_02_DessertStory.Customer.Models.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;

        /// <summary>
        /// 寄送Email
        /// </summary>
        /// <param name="gmailApiConfig"></param>
        public EmailService(IOptions<GmailApiConfig> gmailApiConfig)
        {
            _senderEmail = "ispan.sweetshop@gmail.com"; // 寄件人的電子郵件地址

            var gmailApiConfigValue = gmailApiConfig.Value;

            _smtpClient = new SmtpClient("smtp.gmail.com", 587)//建立一個新的 SmtpClient 物件，並設定郵件伺服器的主機名稱為 "smtp.gmail.com"，port為 587
            {
                Credentials = new NetworkCredential(gmailApiConfigValue.ClientId, gmailApiConfigValue.ClientSecret),
                //設定 SmtpClient 的認證資訊，使用 gmailApiConfigValue 物件中的 ClientId 和 ClientSecret 屬性作為使用者名稱和密碼。
                EnableSsl = true
                //啟用 SSL 加密連線，以保護電子郵件的傳輸安全性
            };
        }

        /// <summary>
        /// 發送驗證信
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="authenticationUrl"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task SendAuthenticationEmailAsync(string recipientEmail, string authenticationUrl,string username)
        {
            var mailMessage = new MailMessage(_senderEmail, recipientEmail)
            {
                Subject = "[甜點物語]請驗證您的電子信箱",
                Body = $"<p>{username} 您好，</p><p>請點擊以下連結來驗證您的電子信箱。</p><p>驗證連結 :{authenticationUrl}</p>",
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}
