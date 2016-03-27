using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.Plugin
{
    public abstract class WebTrade
    {
        #region static Factory
        public static WebTrade CreatWebTD(string name)
        {
            if (name == "华泰")
                return new HTWebTrade();

            return null;
        }
        #endregion

        #region member

        // status
        private bool isLoginOK;

        // helper
        protected HttpHelper httpHelper;

        // Parameters Dictionary
        protected Dictionary<string, string> loginParameters;
        protected Dictionary<string, string> cashParameters;
        protected Dictionary<string, string> posParameters;
        protected Dictionary<string, string> buyParameters;
        protected Dictionary<string, string> sellParameters;

        #endregion

        #region Properties

        // service connection
        public delegate void ConnectStatusChangedCallback(bool status);
        public event ConnectStatusChangedCallback ConnectStatusChanged;

        public bool IsLoginOK
        {
            get { return isLoginOK; }
            set { isLoginOK = value; }
        }

        #endregion

        public WebTrade()
        {
            httpHelper = new HttpHelper("Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
        }

        public void Init(string parameters)
        {
            ParseParameters(loginParameters, parameters);
            Login();
        }

        public void Close()
        {
            Logout();
        }

        public void FireConnectStatusChanged()
        {
            if (ConnectStatusChanged != null)
                ConnectStatusChanged(isLoginOK);
        }

        #region abstract define

        public abstract void Login();
        public abstract void Logout();

        public abstract GMSDK.Cash GetCash();
        public abstract List<GMSDK.Position> GetPositions();

        public abstract int Buy(string exchange, string sec_id, double price, double volume);
        public abstract int Sell(string exchange, string sec_id, double price, double volume);


        #endregion

        #region utils

        private void ParseParameters(Dictionary<string, string> dict, string pars)
        {
            string[] flds = pars.Split(',');

            foreach (string fld in flds)
            {
                string[] pair = fld.Split('=');
                dict[pair[0]] = pair[1];
            }
        }

        #endregion
    }
}
