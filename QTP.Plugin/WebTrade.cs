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
        public static WebTrade CreatWebTD(string name, Dictionary<string, string> parameters)
        {
            if (name == "华泰")
                return new HTWebTrade(parameters);

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

        // service exception
        public delegate void ExceptionCallback(string message);
        public event ExceptionCallback OnException;

        public bool IsLoginOK
        {
            get { return isLoginOK; }
            set { isLoginOK = value; }
        }

        #endregion

        public WebTrade(Dictionary<string, string> parameters)
        {
            httpHelper = new HttpHelper("Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");

            loginParameters = new Dictionary<string, string>();
            foreach (string key in parameters.Keys)
            {
                loginParameters[key] = parameters[key];
            }
        }

        public void LoginWeb()
        {
            try
            {
                Login();
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("LoginWeb: "+ex.Message);
            }
        }

        public void LogoutWeb()
        {
            try
            {
                Logout();
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("LogoutWeb: "+ex.Message);
            }

        }
        public GMSDK.Cash GetCashWeb()
        {
            GMSDK.Cash cash = null;
            try
            {
                cash = GetCash();
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("GetCashWeb: " + ex.Message);
            }
            return cash;
        }

        public List<GMSDK.Position> GetPositionsWeb()
        {
            List<GMSDK.Position> lst = null;
            try
            {
                lst = GetPositions();
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("GetPositionsWeb: " + ex.Message);
            }
            return lst;
        }

        public int BuyWeb(string exchange, string sec_id, double price, double volume)
        {
            int ret = 0;
            try
            {
                ret = Buy(exchange, sec_id, price, volume);
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("BuyWeb: " + ex.Message);
            }
            return ret;
        }

        public int SellWeb(string exchange, string sec_id, double price, double volume)
        {
            int ret = 0;
            try
            {
                ret = Sell(exchange, sec_id, price, volume);
            }
            catch (Exception ex)
            {
                if (OnException != null) OnException("SellWeb: " + ex.Message);
            }
            return ret;
        }

        public void FireConnectStatusChanged()
        {
            if (ConnectStatusChanged != null)
                ConnectStatusChanged(isLoginOK);
        }

        #region abstract define


        protected abstract void Login();
        protected abstract void Logout();

        protected abstract GMSDK.Cash GetCash();
        protected abstract List<GMSDK.Position> GetPositions();

        protected abstract int Buy(string exchange, string sec_id, double price, double volume);
        protected abstract int Sell(string exchange, string sec_id, double price, double volume);


        #endregion

    }
}
