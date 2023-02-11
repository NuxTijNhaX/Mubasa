using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Web.Functionality.EmailSender
{
    public class MailGun
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
