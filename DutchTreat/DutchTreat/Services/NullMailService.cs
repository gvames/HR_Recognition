using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> logger;

        //in constructorul clasei se injecteaza logger-ul
        public NullMailService(ILogger<NullMailService> logger)
        {
            this.logger = logger;
        }

        public void SendMessage(string to, string subject, string body)
        {
            // Lof the message
            //pentru a folosi logger-ul trebuie injectat in aceasta clasa
            logger.LogInformation($"To: {to} Subbject; {subject} Body: {body}");
        }
    }
}
