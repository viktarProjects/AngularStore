using AngularStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Interfaces
{
    public interface IMessageService
    {
        Task<bool> SendMessage(MailData mailData);
    }
}
