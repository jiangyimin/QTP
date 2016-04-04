using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.DBAccess
{
    public class TPool
    {
        public string Name { get; set; }

        public string ManagerName { get; set; }
        public string ManagerInfo { get; set; }

        public Type ManagerType { get; set; }
        public Dictionary<string, string> ManagerParameters { get; set; }

    }
}
