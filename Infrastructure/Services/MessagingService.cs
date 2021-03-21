using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace Infrastructure.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IConfiguration _config;
        public MessagingService(IConfiguration config)
        {
            _config = config;
        }
        
        public void SendMessage(string msg) 
        {
            string accountSid =  _config["TWILIO_ACCOUNT_SID"];
            string authToken = _config["TWILIO_AUTH_TOKEN"];

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: msg,
                from: new Twilio.Types.PhoneNumber("+18439000731"),
                to: new Twilio.Types.PhoneNumber("+40737807858")
            );

            Console.WriteLine(message.Sid);
        }


        
    }
}
