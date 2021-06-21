using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using EmailSender.Responses;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace EmailSender.Controllers
{
    // WS URL --> https://emailsenderws.azurewebsites.net/api/Email/Send
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private MessageResponse messageResponse;
        private IConfiguration configuration;

        public EmailController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// La peticion de tipo POST que envia un objeto en forma de mensaje
        /// al servidor de Gmail.
        /// </summary>
        /// <param name="message"> Message - El objeto en forma de mensaje que es enviado 
        /// por la peticion. </param>
        /// <returns> MessageResponse - El objeto en forma de respuesta que 
        /// crea la peticion POST al enviar
        /// el mensaje. </returns>
        [HttpPost("[action]")]
        public async Task<MessageResponse> Send(Message message)
        {
            try
            {
                string host = configuration.GetValue<string>("Smtp:host");
                int port = configuration.GetValue<int>("Smtp:port");
                string user = configuration.GetValue<string>("Smtp:user");
                string password = configuration.GetValue<string>("Smtp:password");

                var smtpClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(user, password),
                    EnableSsl = true
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(message.EmailFrom, message.From),
                    Subject = message.Subject,
                    Body = $"<h3>Mensaje enviado de {message.From}</h3>" +
                            $"<p>{message.Body}</p>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(new MailAddress(user, "Nicolas Juncos"));

                await smtpClient.SendMailAsync(mailMessage);

                messageResponse = new MessageResponse
                {
                    StatusCode = 200,
                    ResponseMessage = "Tu mensaje ha sido enviado, muchas gracias!",
                    Body = message
                };
            }
            catch (Exception e)
            {
                messageResponse = new MessageResponse
                {
                    StatusCode = 400,
                    ResponseMessage = e.Message,
                    Body = null
                };
            }

            return await Task.FromResult(messageResponse);    
        }
    }
}
