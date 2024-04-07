using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Service.Contract
{
    public interface IMailingService
    {
        Task SendEmailAsync(string Mailto, string ConfirmationCode);    
    }
}
