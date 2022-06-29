using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mailService.Settings
{
    public class MailSettings
    {
        public string Mail { get; set; }
  
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        
        public string Credentials { get; set; }
    }
}