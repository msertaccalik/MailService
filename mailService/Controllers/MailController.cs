using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mailService.Models;
using mailService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;

namespace mailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MailController));
        private readonly IMailService mailService;
    
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        
        
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromBody]MailRequest request)
        {
            try
            {
                log.Info("send request");
                Console.WriteLine("Request");
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        
        
        [HttpGet("info")]
        public async Task<IActionResult> Get()
        {
        
            try
            {
            
                
                return Ok("Mail API Running...");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}