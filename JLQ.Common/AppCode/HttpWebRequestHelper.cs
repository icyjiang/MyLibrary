using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.IO.Compression;

namespace JLQ.Common
{
    //仅作实例.每个网站Request的内容可能不一样，压缩的方式不一样，中文的编码方式不一样...
    class HttpWebRequestHelper
    {
        const int MaxTryTimes = 3;

        /// <summary>
        /// 获取指定网页的验证码
        /// </summary>
        /// <param name="url">验证码地址</param>
        /// <returns></returns>
        public static Bitmap GetBitmap(string requestUrl, string referer, string method, CookieContainer cookieContainer)
        {
            Bitmap bitMap = null;
            int tryTime = 0;
        Again:
            try
            {
                HttpWebRequest request = GetNewRequest(requestUrl, referer, method, cookieContainer);
                //忽略证书
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                using (var webresponse = (HttpWebResponse)request.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                {
                    Image img = Image.FromStream(stream);
                    bitMap = new Bitmap(img);
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
                if (tryTime < MaxTryTimes) { tryTime++; goto Again; }
            }
            return bitMap;
        }

        /// <summary>
        /// 向指定地址提交数据
        /// </summary>
        /// <param name="loginUrl">目标地址</param>
        /// <param name="refer">refer</param>
        /// <param name="method">提交方法</param>
        /// <param name="postData">提交数据</param>
        /// <returns></returns>
        public static string SendDataToServer(string requestUrl, string referer, string method, string postData, CookieContainer cookieContainer)
        {
            string responseFromServer = "";
            //byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            int tryTime = 0;
        Again:
            try
            {
                ServicePointManager.Expect100Continue = false;//基于apache服务器,IIS发布的则不需要 
                HttpWebRequest request = GetNewRequest(requestUrl, referer, method, cookieContainer); //创建对url的请求  
                //post开始  
                if (request.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.ContentLength = byteArray.Length;
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        // 请求数据放入请求流  
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;//忽略证书

                /*System.Web.SessionState.HttpSessionState mys=Syste HttpContext.Current.Session; 
                但是有种情况就是用到了AJAX的话在返回的方法中调用就会有问题所以要有小的改动

                [AjaxMethod]
                [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]*/
                using (var webresponse = (HttpWebResponse)request.GetResponse())
                {
                    if (webresponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = webresponse.GetResponseStream())
                        {
                            Stream responseStream = null;
                            //解压缩
                            if (webresponse.ContentEncoding.ToLower().Contains("gzip"))
                                responseStream = new GZipStream(stream, CompressionMode.Decompress);
                            else if (webresponse.ContentEncoding.ToLower().Contains("deflate"))
                                responseStream = new DeflateStream(stream, CompressionMode.Decompress);
                            else responseStream = stream;
                            using (var sr = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                            {
                                responseFromServer = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
                if (tryTime < MaxTryTimes) { tryTime++; goto Again; }
            }
            return responseFromServer;
        }
        /// <summary>
        /// 具有重定向的httpwebrequest向指定地址提交数据
        /// </summary>
        /// <param name="requestUrl">目标地址</param>
        /// <param name="referer">refer</param>
        /// <param name="method">提交方法</param>
        /// <param name="postData">提交数据</param>
        /// <returns></returns>
        public static string RedirectSendDataToServer(string requestUrl, string referer, string method, string postData, CookieContainer cookieContainer)
        {
            string responseFromServer = "";
            HttpWebResponse httpWebResponse;
            //byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            int tryTime = 0;
        Again:
            try
            {
                ServicePointManager.Expect100Continue = false;//基于apache服务器,IIS发布的则不需要  
                HttpWebRequest request = GetNewRequest(requestUrl, referer, method, cookieContainer); //创建对url的请求  

                //post开始  
                if (request.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.ContentLength = byteArray.Length;
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        // 请求数据放入请求流  
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;//忽略证书
                //返回html  
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                while (httpWebResponse.StatusCode == HttpStatusCode.Found)//如果提交的方式为POST 那么重定向使用GET
                {
                    //httpWebResponse.Close();
                    request = GetNewRequest(httpWebResponse.Headers["Location"], referer, null, cookieContainer);
                    httpWebResponse = (HttpWebResponse)request.GetResponse();
                }

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        Stream responseStream = null;
                        //解压缩
                        if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                            responseStream = new GZipStream(stream, CompressionMode.Decompress);
                        else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
                            responseStream = new DeflateStream(stream, CompressionMode.Decompress);
                        else responseStream = stream;
                        using (var sr = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                        {
                            responseFromServer = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch { if (tryTime < MaxTryTimes) { tryTime++; goto Again; } }
            return responseFromServer;
        }

        static HttpWebRequest GetNewRequest(string requestUrl, string referer, string method, CookieContainer cookieContainer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Referer = referer;
            request.CookieContainer = cookieContainer;
            request.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            //httpWebRequest.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/QVOD, application/QVOD, application/xaml+xml, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-xpsdocument, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            request.Headers["Accept-Language"] = "zh-cn";
            request.Headers["Accept-Encoding"] = "gzip, deflate";//压缩
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
            //httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = true;
            request.Headers["Cache-Control"] = "no-cache";
            //httpWebRequest.AllowAutoRedirect = false;
            //协议方式  
            if (!string.IsNullOrWhiteSpace(method)) request.Method = method;
            request.Timeout = 20000;
            return request;
        }
    }
}
