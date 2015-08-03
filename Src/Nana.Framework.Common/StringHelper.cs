using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nana.Framework.Common
{
    public class StringHelper
    {

        /// <summary>
        /// 截断字符串（取前len个字符）
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string SubString(string s, int len)
        {
            if (s.Length <= len)
            {
                return s;
            }
            else
            {
                return s.Substring(0, len);
            }
        }

        /// <summary>
        /// 转换数字为中文数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string Number2CHS(int number)
        {
            return Number2CHS(number.ToString());
        }

        /// <summary>
        /// 把数字字符串转换为中文数字字符串
        /// </summary>
        /// <param name="numberstr"></param>
        /// <returns></returns>
        public static string Number2CHS(string numberstr)
        {
            string[] nums = new string[]{
                "零","一","二","三","四","五","六","七","八","九"
            };
            int zero = '0';
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < numberstr.Length; i++)
            {
                if (Char.IsDigit(numberstr[i]))
                {
                    result.Append(nums[numberstr[i] - zero]);
                }
                else
                {
                    throw new ArithmeticException("输入的数字字符串中含有非数字字符：" + numberstr[i].ToString());
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 把中文数字字符串转换为数字字符串
        /// </summary>
        /// <param name="chsString"></param>
        /// <returns></returns>
        public static string CHS2NumberString(string chsString)
        {
            string numstr = "零一二三四五六七八九";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < chsString.Length; i++)
            {
                int pos = numstr.IndexOf(chsString[i]);
                if (pos == -1)
                {
                    result.Append(chsString[i]);
                }
                else
                {
                    result.Append(pos);
                }
            }
            return result.ToString();
        }


        /// <summary>
        /// 剔除script脚本
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterScript(string content)
        {
            string regexstr = @"<script[^>]*>([\s\S](?!<script))*?</script>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 剔除style脚本
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterStyle(string content)
        {
            string regexstr = @"<style[^>]*>([\s\S](?!<style))*?</style>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 剔除HTML标签
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string RemoveHtmlTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }

        /// <summary>
        /// byte数组转为16进制字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ByteToHex(byte[] byteArray)
        {
            StringBuilder outString = new StringBuilder();

            foreach (Byte b in byteArray)
                outString.Append(b.ToString("X2"));

            return outString.ToString();
        }


        /// <summary>
        /// 16进制字符串转为byte数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }

    }
}
