using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace mailService.Models
{
    public class MailRequest
    {
      
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
      
       public List<IFormFile>? Attachments { get; set; }
    
        public string? System { get; set; }
        
        public string? Sender { get; set; }
        
        public string? SenderPassword { get; set; }
    }
}