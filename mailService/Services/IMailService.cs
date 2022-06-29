using mailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mailService
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task UploadFile(MailRequest uploadFile);

    }
}