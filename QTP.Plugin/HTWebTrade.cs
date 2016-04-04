using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Management;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Web;

namespace QTP.Plugin
{
    public class HTWebTrade : WebTrade
    {
        #region JSON object

        // TradeInfo
        [DataContract]
        private class Account
        {
            [DataMember(Order = 0)]
            public string exchange_type { get; set; }
            [DataMember(Order = 1)]
            public string exchange_name { get; set; }
            [DataMember(Order = 2)]
            public string holder_status { get; set; }
            [DataMember(Order = 3)]
            public string stock_account { get; set; }
            [DataMember(Order = 4)]
            public string fund_account { get; set; }
            [DataMember(Order = 5)]
            public string main_flag { get; set; }
            [DataMember(Order = 6)]
            public string holder_rights { get; set; }
        }

        [DataContract]
        private class TradeInfo
        {
            [DataMember(Order = 0)]
            public List<Account> item { get; set; }

            [DataMember(Order = 1)]
            public string cssweb_type { get; set; }

            [DataMember(Order = 2)]
            public string fund_account { get; set; }

            [DataMember(Order = 3)]
            public string client_risklevel_name { get; set; }

            [DataMember(Order = 4)]
            public string uid { get; set; }

            [DataMember(Order = 6)]
            public string branch_no { get; set; }

            [DataMember(Order = 7)]
            public string trdpwd { get; set; }

            [DataMember(Order = 8)]
            public string op_station { get; set; }

            [DataMember(Order = 9)]
            public string client_name { get; set; }

            [DataMember(Order = 10)]
            public string cssweb_code { get; set; }

            [DataMember(Order = 11)]
            public string client_rights { get; set; }

            [DataMember(Order = 12)]
            public string account_content { get; set; }

            [DataMember(Order = 13)]
            public string last_op_station { get; set; }
        }

        // Get Cash
        [DataContract]
        private class HTCash
        {
            [DataMember(Order = 0)]
            public string money_type { get; set; }
            [DataMember(Order = 1)]
            public string money_name { get; set; }
            [DataMember(Order = 2)]
            public double current_balance { get; set; }
            [DataMember(Order = 3)]
            public double enable_balance { get; set; }
            [DataMember(Order = 4)]
            public double fetch_balance { get; set; }
            [DataMember(Order = 5)]
            public double market_value { get; set; }
            [DataMember(Order = 6)]
            public double asset_balance { get; set; }
            [DataMember(Order = 7)]
            public string cssweb_test { get; set; }
        }

        [DataContract]
        private class CashResponse
        {
            [DataMember(Order = 0)]
            public string cssweb_code { get; set; }

            [DataMember(Order = 1)]
            public string cssweb_type { get; set; }

            [DataMember(Order = 3)]
            public List<HTCash> item { get; set; }
        }

        // Get Positions
        [DataContract]
        private class HTPosition
        {
            [DataMember(Order = 0)]
            public string exchange_type { get; set; }
            [DataMember(Order = 1)]
            public string exchange_name { get; set; }
            [DataMember(Order = 2)]
            public string stock_account { get; set; }
            [DataMember(Order = 3)]
            public string stock_code { get; set; }
            [DataMember(Order = 4)]
            public string stock_name { get; set; }
            [DataMember(Order = 5)]
            public double current_amount { get; set; }
            [DataMember(Order = 6)]
            public double enable_amount { get; set; }
            [DataMember(Order = 7)]
            public double last_price { get; set; }
            [DataMember(Order = 8)]
            public double cost_price { get; set; }
            [DataMember(Order = 9)]
            public double keep_cost_price { get; set; }
            [DataMember(Order = 10)]
            public double income_balance { get; set; }
            [DataMember(Order = 11)]
            public string hand_flag { get; set; }
            [DataMember(Order = 12)]
            public double market_value { get; set; }
            [DataMember(Order = 13)]
            public double av_buy_price { get; set; }
            [DataMember(Order = 14)]
            public double av_income_balance { get; set; }
            [DataMember(Order = 15)]
            public double income_balance_ratio { get; set; }
            [DataMember(Order = 15)]
            public string cssweb_test { get; set; }     // end item flag
        }

        [DataContract]
        private class PositionResponse
        {
            [DataMember(Order = 0)]
            public string cssweb_code { get; set; }

            [DataMember(Order = 1)]
            public string cssweb_type { get; set; }

            [DataMember(Order = 3)]
            public List<HTPosition> item { get; set; }
        }


        // BSResponse
        [DataContract]
        private class BSResponse
        {
            [DataMember(Order = 0)]
            public string cssweb_code { get; set; }

            [DataMember(Order = 1)]
            public string cssweb_type { get; set; }

            [DataMember(Order = 3)]
            public List<HTEntrust> item { get; set; }
        }

        [DataContract]
        private class HTEntrust
        {
            [DataMember(Order = 0)]
            public int entrust_no { get; set; }
            [DataMember(Order = 1)]
            public string cssweb_test { get; set; }
        }

        #endregion

        #region member

        // url
        private string login_pageUrl = "https://service.htsc.com.cn/service/login.jsp";
        private string login_apiUrl = "https://service.htsc.com.cn/service/loginAction.do?method=login";
        private string trade_info_pageUrl = "https://service.htsc.com.cn/service/flashbusiness_new3.jsp?etfCode=";
        private string prefixUrl = "https://tradegw.htsc.com.cn";
        private string verify_code_apiUrl = "https://service.htsc.com.cn/service/pic/verifyCodeImage.jsp";
        private string logout_apiUrl = "https://service.htsc.com.cn/service/login.jsp?logout=yes";

        // json object
        private TradeInfo tradeInfo;
        #endregion


        public HTWebTrade(Dictionary<string, string> parameters) : base(parameters)
        {
            loginParameters["accountType"] = "1";
            loginParameters["userType"] = "jy";

            cashParameters = new Dictionary<string, string>()
            {
                {"cssweb_type", "GET_FUNDS"},
                {"function_id", "405" },
                {"identity_type", ""},
                {"money_type", "" },
            };

            posParameters = new Dictionary<string, string>()
            {
                {"cssweb_type", "GET_STOCK_POSITION"},
                {"function_id", "403"},
                {"exchange_type", ""},
                {"stock_account", ""},
                {"stock_code", ""},
                {"query_direction", ""},
                {"query_mode", "0"},
                {"request_num", "100"},
                {"position_str", ""}
            };

            buyParameters = new Dictionary<string, string>()
            {
                {"cssweb_type", "STOCK_BUY"},
                {"function_id", "302"},
                {"exchange_type", ""},
                {"stock_account", ""},
                {"stock_code", ""},
                {"query_direction", ""},
                {"sort_direction", "0"},
                {"request_num", "100"},
                {"identity_type", ""},
                {"entrust_bs", "1"}
           };

            sellParameters = new Dictionary<string, string>()
            {
                {"cssweb_type", "STOCK_SELL"},
                {"function_id", "302"},
                {"exchange_type", ""},
                {"stock_account", ""},
                {"stock_code", ""},
                {"query_direction", ""},
                {"sort_direction", "0"},
                {"request_num", "100"},
                {"identity_type", ""},
                {"entrust_bs", "2"}
           };
        }

        #region abstract 

        protected override void Login()
        {
            IsLoginOK = false;
            // go_login_page
            httpHelper.CreateGetHttpResponse(login_pageUrl, true);

            // RecognizeCode
            string verifycode = null;
            int i = 0;
            while (true)
            {
                verifycode = GetRecognizeCode();
                if (verifycode.Length == 4)
                    break;
                i++;
                Thread.Sleep(300);
            }

            // set vcode 
            loginParameters["vcode"] = verifycode;

            // post loginForm
            HttpWebResponse response = httpHelper.CreatePostHttpResponse(login_apiUrl, loginParameters);
            string retText = HttpHelper.GetResponseString(response);
            bool ret = retText.Contains("欢迎您");

            if (ret != IsLoginOK)
            {
                IsLoginOK = ret;
                base.FireConnectStatusChanged();
            }

            // get trade info
            if (ret) GetTradeInfo();
        }

        protected override void Logout()
        {
            // go_logout_page
            httpHelper.CreateGetHttpResponse(logout_apiUrl);           
        }

        protected override GMSDK.Cash GetCash()
        {
            if (!IsLoginOK) return null;
            
            MakeupBasicParameters(cashParameters);
            string paramsStr = GetPrefixUrlString(cashParameters);
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(paramsStr);

            string jsonStr = Encoding.Default.GetString(Convert.FromBase64String(HttpHelper.GetResponseString(response)));
            CashResponse cr = JSON.parse<CashResponse>(jsonStr);

            if (cr.cssweb_code == "success")
            {
                HTCash htcash = cr.item[0];
                GMSDK.Cash cash = new GMSDK.Cash();
                cash.nav = htcash.asset_balance;
                cash.available = htcash.enable_balance;
                cash.frozen = htcash.market_value;
                return cash;              
            }
            else
            {
                return null;
            }
        }

        protected override List<GMSDK.Position> GetPositions()
        {
            if (!IsLoginOK) return null;
            // set parameter and get response
            MakeupBasicParameters(posParameters);
            string paramsStr = GetPrefixUrlString(posParameters);
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(paramsStr);
            string jsonStr = Encoding.Default.GetString(Convert.FromBase64String(HttpHelper.GetResponseString(response)));
            PositionResponse pr = JSON.parse<PositionResponse>(jsonStr);

            if (pr.cssweb_code == "success")
            {
                List<GMSDK.Position> lst = new List<GMSDK.Position>();
                foreach (HTPosition htp in pr.item)
                {
                    if (htp.cssweb_test == "0") continue;
                    GMSDK.Position pos = new GMSDK.Position();
                    pos.exchange = htp.exchange_type == "1" ? "SHSE" : "SZSE";
                    pos.sec_id = htp.stock_code;
                    pos.volume = htp.current_amount;
                    pos.available = htp.enable_amount;
                    pos.vwap = htp.cost_price;
                    pos.cost = htp.cost_price * htp.current_amount;
                    pos.fpnl = htp.income_balance;
                    pos.price = htp.last_price;

                    lst.Add(pos);
                }
                return lst;
            }
            else
            {
                return null;
            }
        }

        protected override int Buy(string exchange, string sec_id, double price, double volume)
        {
            // exchage
            int i = exchange == "SHSE" ? 0 : 1;
            buyParameters["exchange_type"] = tradeInfo.item[i].exchange_type;
            buyParameters["stock_account"] = tradeInfo.item[i].stock_account;
            
            // get response string
            buyParameters["stock_code"] = sec_id;
            buyParameters["entrust_prop"] = "0";        // limit price
            buyParameters["entrust_price"] = price.ToString();
            buyParameters["entrust_amount"] = ((int)volume).ToString();
            MakeupBasicParameters(buyParameters);
            string paramsStr = GetPrefixUrlString(buyParameters);
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(paramsStr);
            string jsonStr = Encoding.Default.GetString(Convert.FromBase64String(HttpHelper.GetResponseString(response)));
            BSResponse bs = JSON.parse<BSResponse>(jsonStr);

            if (bs.cssweb_code == "success")
                return bs.item[0].entrust_no;
            else
                return -1;
        }

        protected override int Sell(string exchange, string sec_id, double price, double volume)
        {
            // exchage
            int i = exchange == "SHSE" ? 0 : 1;
            sellParameters["exchange_type"] = tradeInfo.item[i].exchange_type;
            sellParameters["stock_account"] = tradeInfo.item[i].stock_account;

            // get response string
            sellParameters["stock_code"] = sec_id;
            sellParameters["entrust_prop"] = "0";        // limit price
            sellParameters["entrust_price"] = price.ToString();
            sellParameters["entrust_amount"] = ((int)volume).ToString();
            MakeupBasicParameters(sellParameters);
            string paramsStr = GetPrefixUrlString(sellParameters);
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(paramsStr);
            string jsonStr = Encoding.Default.GetString(Convert.FromBase64String(HttpHelper.GetResponseString(response)));
            BSResponse bs = JSON.parse<BSResponse>(jsonStr);

            if (bs.cssweb_code == "success")
                return bs.item[0].entrust_no;
            else
                return -1;
        }

        #endregion

        #region private utils

        private void MakeupBasicParameters(Dictionary<string, string> pars)
        {
            Random random = new Random();

            pars["uid"] = tradeInfo.uid;
            pars["version"] = "1";
            pars["custid"] = loginParameters["userName"];           
            pars["op_branch_no"] = tradeInfo.branch_no;
            pars["branch_no"] = tradeInfo.branch_no;
            pars["op_entrust_way"] = "7";
            //pars["op_station"] = tradeInfo.op_station;
            pars["fund_account"] = loginParameters["userName"];
            pars["password"] = tradeInfo.trdpwd;
            pars["ram"] = random.NextDouble().ToString();
        }

        private string GetPrefixUrlString(Dictionary<string, string> pars)
        {
            // get paramters string
            StringBuilder sb = new StringBuilder();
            foreach (string key in pars.Keys)
            {
                if (sb.Length != 0)
                    sb.Append('&');

                sb.Append(string.Format("{0}={1}", key, pars[key]));
            }

            string base64Params = Convert.ToBase64String(Encoding.Default.GetBytes(sb.ToString()));
            return string.Format("{0}?{1}", prefixUrl, base64Params);
        }

        private string GetRecognizeCode()
        {
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(verify_code_apiUrl);
            using (Stream stream = HttpHelper.GetResponseStream(response))
            {
                HTVerifyCode htVC = new HTVerifyCode((Bitmap)Bitmap.FromStream(stream), 4);
                return htVC.GetCodeString();
            }

        }

        private void GetTradeInfo()
        {
            HttpWebResponse response = httpHelper.CreateGetHttpResponse(trade_info_pageUrl);
            Match match =  Regex.Match(HttpHelper.GetResponseString(response), "var data = \"([/=\\w\\+]+)\"");

            string jsonStr = Encoding.Default.GetString(Convert.FromBase64String(match.Groups[1].Value));

            tradeInfo = JSON.parse<TradeInfo>(jsonStr);  
        }

        #endregion
    }

}
