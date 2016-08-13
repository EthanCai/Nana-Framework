using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Nana.Framework.Utility
{
    public class NetHelper
    {
        /// <summary>
        /// 通过Post访问指定的URL
        /// </summary>
        /// <param name="strUrl">地址</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <param name="strParm">参数</param>
        /// <returns>返回的内容</returns>
        public static string PostModel(string strUrl, int timeout, string strParm)
        {
            Encoding encode = System.Text.Encoding.Default;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(strUrl);

            if (!string.IsNullOrEmpty(strParm))
            {
                byte[] arrB = encode.GetBytes(strParm);
                myReq.ContentLength = arrB.Length;
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";

                Stream outStream = myReq.GetRequestStream();
                outStream.Write(arrB, 0, arrB.Length);
                outStream.Close();
            }

            myReq.Timeout = timeout;

            //接收HTTP做出的响应
            WebResponse myResp = myReq.GetResponse();
            Stream ReceiveStream = myResp.GetResponseStream();

            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            return readStream.ReadToEnd();

        }
    }
}
