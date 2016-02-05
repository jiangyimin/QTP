using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.Infra;

namespace QTP.MDWrapper
{
    public class MDFactory
    {
        public static IMdAdapter Create(EventServer es)
        {
            MdAdapter adapter = new MdAdapter(es);

            // LTS 实时行情
            // Read config file
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection("LTS");

            adapter.InitLTSMDAdapter(nv["MdFrontAddress"], nv["BrokerID"], nv["UserID"], nv["MdPassword"]);

            // GM 行情
            nv = (NameValueCollection)ConfigurationManager.GetSection("GM");
            adapter.InitGMMDAdapter(nv["UserID"], nv["Password"]);

            return adapter;       
        }
    }
}
