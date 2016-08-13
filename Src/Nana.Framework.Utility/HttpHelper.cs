using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// HTTP请求相关的封装工具类
    /// </summary>
    public class HttpHelper
    {

        /// <summary>
        /// 根据指定的编码格式返回HttpRequest请求的参数集合，用户在服务端获得客户端请求的HttpRequest请求参数，
        /// Get方式通过QuqueryString获取，Post方式则通过流获取，返回的结构为NameValueCollection集合；
        /// </summary>
        /// <param name="request">请求的字符串</param>
        /// <param name="encode">编码模式</param>
        /// <returns>NameValueCollection参数集合</returns>
        public static NameValueCollection GetRequestParameters(HttpRequest request, string encode)
        {
            Encoding destEncode = null;
            if (!String.IsNullOrEmpty(encode))
            {
                try
                {
                    destEncode = Encoding.GetEncoding(encode);
                }
                catch { }
            }

            return GetRequestParameters(request, destEncode);
        }

        /// <summary>
        /// 根据指定的编码格式返回HttpRequest请求的参数集合，用户在服务端获得客户端请求的HttpRequest请求参数，
        /// Get方式通过QuqueryString获取，Post方式则通过流获取，返回的结构为NameValueCollection集合；
        /// </summary>
        /// <param name="request">请求的字符串</param>
        /// <param name="encode">编码模式</param>
        /// <returns>NameValueCollection参数集合</returns>
        public static NameValueCollection GetRequestParameters(HttpRequest request, Encoding encode)
        {
            NameValueCollection nv = null;
            if (request.HttpMethod == "POST")
            {
                if (null != encode)
                {
                    Stream resStream = request.InputStream;
                    byte[] filecontent = new byte[resStream.Length];
                    resStream.Read(filecontent, 0, filecontent.Length);
                    string postquery = Encoding.Default.GetString(filecontent);
                    nv = HttpUtility.ParseQueryString(postquery, encode);
                }
                else
                    nv = request.Form;
            }
            else
            {
                if (null != encode)
                {
                    nv = System.Web.HttpUtility.ParseQueryString(request.Url.Query, encode);
                }
                else
                {
                    nv = request.QueryString;
                }
            }
            return nv;
        }


        /// <summary>
        /// 发送一个POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paras"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static byte[] SendPostRequest(string url, string paras, string encodetype)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";

            Encoding encode = Encoding.Default;
            if (!String.IsNullOrEmpty(encodetype))
            {
                try
                {
                    encode = Encoding.GetEncoding(encodetype);
                }
                catch { }
            }

            byte[] data = encode.GetBytes(paras.ToString());
            Stream reqstream = req.GetRequestStream();

            reqstream.Write(data, 0, data.Length);
            reqstream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream resst = res.GetResponseStream();
            byte[] result = new byte[8092];
            resst.Read(result, 0, result.Length);
            resst.Close();
            res.Close();
            return result;
        }

        /// <summary>
        /// 发送一个GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] SendGetRequest(string url, string encodetype)
        {
            Encoding encode = Encoding.Default;
            if (!String.IsNullOrEmpty(encodetype))
            {
                try
                {
                    encode = Encoding.GetEncoding(encodetype);
                }
                catch { }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.MaximumAutomaticRedirections = 3;
            req.Timeout = 5000;

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            Stream resst = res.GetResponseStream();
            byte[] result = new byte[8096];
            resst.Read(result, 0, result.Length);
            resst.Close();
            res.Close();
            return result;
        }


        /// <summary>
        /// 使用指定编码格式发送一个POST请求，并通过约定的编码格式获取返回的数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="parameters">请求的参数集合</param>
        /// <param name="reqencode">请求的编码格式</param>
        /// <param name="resencode">接收的编码格式</param>
        /// <returns></returns>
        public static string SendPostRequest(string url, NameValueCollection parameters, Encoding reqencode, Encoding resencode)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";

            StringBuilder parassb = new StringBuilder();
            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    if (parassb.Length > 0)
                        parassb.Append("&");
                    parassb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key, reqencode), HttpUtility.UrlEncode(parameters[key], reqencode));
                }
            }
            byte[] data = reqencode.GetBytes(parassb.ToString());
            Stream reqstream = req.GetRequestStream();

            reqstream.Write(data, 0, data.Length);
            reqstream.Close();

            string result = String.Empty;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), resencode))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }


        /// <summary>
        /// 通过客户端跳转发起Post请求并且重定向
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="context"></param>
        public static void PostAndRedirect(string url, NameValueCollection parameters, HttpContext context)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("<form name=redirpostform action='{0}' method='post'>", url);
            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    script.AppendFormat("<input type='hidden' name='{0}' value='{1}'>",
                        key, parameters[key]);
                }
            }
            script.Append("</form>");
            script.Append("<script language='javascript'>redirpostform.submit();</script>");
            context.Response.Write(script);
            context.Response.End();
        }

        /// <summary>
        /// 发送一个GET请求
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="parameters">请求的参数集合</param>
        /// <param name="reqencode">请求的编码格式</param>
        /// <param name="resencode">接收的编码格式</param>
        /// <returns></returns>
        public static string SendGetRequest(string baseurl, NameValueCollection parameters, Encoding reqencode, Encoding resencode)
        {
            StringBuilder parassb = new StringBuilder();
            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    if (parassb.Length > 0)
                        parassb.Append("&");
                    parassb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key, reqencode), HttpUtility.UrlEncode(parameters[key], reqencode));
                }
            }
            if (parassb.Length > 0)
            {
                baseurl += "?" + parassb;
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(baseurl);
            req.Method = "GET";
            req.MaximumAutomaticRedirections = 3;
            req.Timeout = 5000;
            req.KeepAlive = false;

            string result = String.Empty;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), resencode))
            {
                result = reader.ReadToEnd();
            }

            response.Close();
            
            return result;

        }

        public static string BuilderGetRequestUrl(string baseurl, NameValueCollection parameters, Encoding reqencode)
        {
            StringBuilder parassb = new StringBuilder();
            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    if (parassb.Length > 0)
                        parassb.Append("&");
                    parassb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key, reqencode), HttpUtility.UrlEncode(parameters[key], reqencode));
                }
            }

            if (parassb.Length > 0)
            {
                baseurl += "?" + parassb;
            }
            return baseurl;
        }

        public static string BuilderGetRequestUrlNoEncode(string baseurl, NameValueCollection parameters)
        {
            StringBuilder parassb = new StringBuilder();
            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    if (parassb.Length > 0)
                        parassb.Append("&");
                    parassb.AppendFormat("{0}={1}", key, parameters[key]);
                }
            }

            if (parassb.Length > 0)
            {
                baseurl += "?" + parassb;
            }
            return baseurl;
        }
        /// <summary>
        /// 转换输入字符串的编码格式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="srcEncoding"></param>
        /// <param name="desEncoding"></param>
        /// <returns></returns>
        public static string EncodeString(string input, Encoding srcEncoding, Encoding desEncoding)
        {
            if (srcEncoding == null || desEncoding == null)
            {
                throw new Exception("需要提供相应的encoding");
            }
            if (srcEncoding == desEncoding)
            {
                return input;
            }
            else
            {
                return desEncoding.GetString(Encoding.Convert(srcEncoding, desEncoding, srcEncoding.GetBytes(input)));
            }
        }

        /// <summary>
        /// 转换输入字符串的编码格式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="srcEncoding"></param>
        /// <param name="desEncoding"></param>
        /// <returns></returns>
        public static string EncodeString(string input, Encoding desEncoding)
        {
            if (desEncoding == null)
            {
                throw new Exception("需要提供相应的encoding");
            }
            if (Encoding.Default == desEncoding)
            {
                return input;
            }
            else
            {
                return desEncoding.GetString(Encoding.Convert(Encoding.Default, desEncoding, Encoding.Default.GetBytes(input)));
            }
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostJsonDataToUrl(string data, string url)
        {
            string sRequestEncoding = "utf-8";
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostJsonDataToUrl(bytesToPost, url);
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostJsonDataToUrl(byte[] data, string url)
        {
            #region 创建httpWebRequest对象

            string sUserAgent =
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            string sContentType =
                "application/json;charset=utf-8";
            string sResponseEncoding = "utf-8";

            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
                    string.Format("Invalid url string: {0}", url)
                    );
            }
            #endregion

            #region 填充httpWebRequest的基本信息
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "POST";
            #endregion

            #region 填充要post的内容
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                Console.WriteLine(
                    string.Format("POST操作发生异常：{0}", e.Message)
                    );
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion

            return stringResponse;
        }

    }
}
