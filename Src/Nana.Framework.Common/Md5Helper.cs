using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Nana.Framework.Common
{
    public class Md5Helper
    {
        /// <summary>
        /// 比较md5签名
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="signstr"></param>
        /// <returns></returns>
        public static bool ValidateMd5Sign(string inputstr, string signstr)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return (signstr.ToLower() == resultCode);
        }

        /// <summary>
        /// 比较md5签名
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="signstr"></param>
        /// <returns></returns>
        public static bool ValidateMd5Sign(string inputstr, string signstr, Encoding encode)
        {
            byte[] data = encode.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return (signstr.ToLower() == resultCode);
        }

        /// <summary>
        /// 获得输入字符串的md5签名，去除“-”，并转为小写格式
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static string GetMd5Sign(string inputstr)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return resultCode;
        }

        /// <summary>
        /// 给字节数组签名（去除“-”，并转为小写格式）
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static string GetMd5Sign(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return resultCode;
        }

        /// <summary>
        /// 获得输入字符串的md5签名，去除“-”，并转为小写格式
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static string GetMd5Sign(string inputstr, Encoding encode)
        {
            byte[] data = encode.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return resultCode;
        }

        /// <summary>
        /// Gets the file MD5 sign.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string GetFileMd5Sign(string filePath)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                }
            }
        }

    }
}
