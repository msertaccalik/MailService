using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using mailService.Models;
using mailService.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mailService.Services;
using Microsoft.AspNetCore.HttpOverrides;
using MimeKit.Text;

namespace mailService.Services
{
    public class MailService : IMailService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MailService));
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                Console.WriteLine("requester: "+mailRequest.System);
                log.Info("requester: " + mailRequest.System);
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\NewTemplate.html";
                StreamReader str = new StreamReader(FilePath);
            
                string MailText = str.ReadToEnd();
                MailText = MailText.Replace("[DATA]", mailRequest.Body);
                //MailText = MailText.Replace("[PREHEADER]", mailRequest.Body);
                str.Close();
                string smtpUserName = "";
                string smtpUserPassword = "";
                var email = new MimeMessage();
            
                log.Info("Mail Addresses: " + mailRequest.ToEmail);
                string[] mailAddress = (mailRequest.ToEmail).Split(new [] {",",";"}, StringSplitOptions.RemoveEmptyEntries);
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

                foreach (var curr_add in mailAddress)
                { 
               
                    email.To.Add(MailboxAddress.Parse(curr_add));
                }
            
                Console.WriteLine(email.To.ToString());
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
            
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
            
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                Console.WriteLine(email.Body);
                smtp.ServerCertificateValidationCallback = (s,c,h,e) => true;
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, false);
           
                if (mailRequest.SenderPassword == null && mailRequest.Sender == null)
                {
                
                    smtp.Authenticate("HAOS\\"+_mailSettings.Credentials, _mailSettings.Password);
                    log.Info("authenticated with " + "HAOS\\"+_mailSettings.Credentials +" " + _mailSettings.Password );
                    Console.WriteLine("authenticated with " + "HAOS\\"+_mailSettings.Credentials +" " + _mailSettings.Password );
                }
                else
                {
                    smtp.Authenticate(mailRequest.Sender, mailRequest.SenderPassword);
                    log.Info("authenticate with " + mailRequest.Sender +" " + mailRequest.SenderPassword);
                    Console.WriteLine("authenticate with " + mailRequest.Sender +" " + mailRequest.SenderPassword);
                }
          
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                Console.WriteLine("mail sent: " + email.To.ToString());
                log.Info("mail sent: " +email.To.ToString() );
            
            }
            
            
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error("mail issue "+e );
                throw;
            }
            
            
        }

        public async Task UploadFile(MailRequest uploadFile)
        {
            //
        }

      
        
        
    }
}
