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

        // post parameters

        #endregion


        public HTWebTrade()
        {
            loginParameters = new Dictionary<string, string>()
            {
                {"accountType", "1"},
                {"userType", "jy"},
            };

            cashParameters = new Dictionary<string, string>()
            {
                {"cssweb_type", "GET_FUNDS"},
                {"function_id", "405" },
                {"identity_type", ""},
                {"money_type", "" }
            };
        }

        #region abstract 
        public override void Login()
        {
            // go_login_page
            httpHelper.CreateGetHttpResponse(login_pageUrl);

            // RecognizeCode
            string verifycode = null;
            int i = 0;
            while (true)
            {
                verifycode = GetRecognizeCode();
                if (verifycode.Length == 4)
                    break;
                i++;
                Thread.Sleep(200);
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

        public override void GetCash()
        {
           // System.Web.HttpUtility
        }

        #endregion

        #region private utils


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
