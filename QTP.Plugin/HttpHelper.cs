using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace QTP.Plugin
{
    public class HttpHelper
    {
        private string userAgent;

        public CookieContainer m_Cookie;


        public HttpHelper(string userAgent)
        {
            this.userAgent = userAgent;
            this.m_Cookie = new CookieContainer();
        }

        /// <summary>
        /// get response and get cookie to m_Cookie
        /// </summary>
        public HttpWebResponse GetHttpResponseCookie(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.UserAgent = userAgent;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            CookieCollection cc = response.Cookies;

            //string cookieheader = response.Cookies..CookieContainer.GetCookieHeader(new Uri(url));
            //m_Cookie.SetCookies(new Uri(url), cookieheader);

            return response;
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public HttpWebResponse CreateGetHttpResponse(string url, bool setCookie = false)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = m_Cookie;

            request.UserAgent = userAgent;
            request.Method = "GET";

            if (setCookie)
            {
                string cookieheader = request.CookieContainer.GetCookieHeader(new Uri(url));
                m_Cookie.SetCookies(new Uri(url), cookieheader);
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public static Stream GetResponseStream(HttpWebResponse response)
        {
            return response.GetResponseStream();
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse response)
        {
            using (Stream s = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            request.CookieContainer = m_Cookie;

            request.UserAgent = userAgent;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
    }
}
