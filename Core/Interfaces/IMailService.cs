using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(string toEmail, string subject, string content);
    }
}
