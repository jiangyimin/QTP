using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.DBAccess
{
    public class TLogin
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public TLogin() { }
        public TLogin(string n, string p)
        {
            UserName = n;
            Password = p;
        }
    }
}
