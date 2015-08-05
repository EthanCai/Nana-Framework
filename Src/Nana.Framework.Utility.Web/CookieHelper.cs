using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Nana.Framework.Utility.Web
{
    public class CookieHelper
    {
        #region set cookie

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <returns></returns>
        public static bool SetCookie(string cookieName, string cookieValue)
        {
            return CookieHelper.SetCookie(cookieName, null, cookieValue, null, null);
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">Value</param>
        /// <param name="domain">域</param>
        /// <returns>是否成功 </returns>
        public static bool SetCookie(string cookieName, string cookieValue, string domain)
        {
            return CookieHelper.SetCookie(cookieName, null, cookieValue, null, domain);
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="day">过期天数</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string cookieValue, int days)
        {
            return CookieHelper.SetCookie(cookieName, cookieValue, DateTime.Now.AddDays(days));
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="expireTime">过期日期</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string cookieValue, DateTime expireTime)
        {
            return CookieHelper.SetCookie(cookieName, null, cookieValue, expireTime, null);
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="subKeyName">Cookies子键的名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="day">过期天数</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string subKeyName, string cookieValue, int days)
        {
            return CookieHelper.SetCookie(cookieName, subKeyName, cookieValue, DateTime.Now.AddDays(days), null);
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="expireTime">日期</param>
        /// <param name="domain">域</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(
            string cookieName, string subKeyName, string cookieValue, DateTime? expireTime, string domain)
        {
            //创建Cookie对象
            if (string.IsNullOrEmpty(cookieName)) return false;

            HttpCookie objCookie;
            //先判断是否有Cookies对象。如果有则需要先获取。
            if (System.Web.HttpContext.Current.Response.Cookies[cookieName] == null)
            {
                objCookie = new HttpCookie(cookieName);
            }
            else
            {
                objCookie = System.Web.HttpContext.Current.Response.Cookies[cookieName];
            }

            //设定cookie 过期时间.
            if (expireTime.HasValue)
            {
                objCookie.Expires = expireTime.Value;
            }
            
            //设置domain
            if (!string.IsNullOrEmpty(domain))
            {
                objCookie.Domain = domain;
            }

            //写入Cookie value  
            if (!String.IsNullOrEmpty(subKeyName))
            {
                objCookie[subKeyName] = HttpUtility.UrlEncode(cookieValue.Trim());
            }
            else
            {
                objCookie.Value = HttpUtility.UrlEncode(cookieValue.Trim());
            }

            //写入Response.Cookies
            if (System.Web.HttpContext.Current.Response.Cookies[cookieName] == null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(objCookie);
            }
            else
            {
                System.Web.HttpContext.Current.Response.Cookies.Set(objCookie);
            }

            return true;
        }

        #endregion

        #region get cookie

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="cookieName">Cookie键名</param>
        /// <returns>Cookies值</returns>
        public static string GetCookie(string cookieName)
        {
            return CookieHelper.GetCookie(cookieName, null);
        }

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="cookieName">Cookie键名</param>
        /// <param name="subKeyName">子键名</param>
        /// <returns>Cookies值</returns>
        public static string GetCookie(string cookieName, string subKeyName)
        {
            string result = string.Empty;

            try
            {
                if (HttpContext.Current.Request.Cookies[cookieName] != null)
                {
                    if (!String.IsNullOrEmpty(subKeyName))
                    {
                        result = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookieName][subKeyName]);
                    }
                    else
                    {
                        if (HttpContext.Current.Request.Cookies[cookieName] != null)
                            result = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookieName].Value.Trim());
                        else
                            result = string.Empty;
                    }
                }
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }

        #endregion
    }
}
