using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nana.Framework.Utility
{
    public class ConvertHelper
    {
        #region Safe convert

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(string s)
        {
            return ToTimeSpan(s, TimeSpan.Zero);
        }

        public static TimeSpan ToTimeSpan(object obj)
        {
            return ToTimeSpan(obj, TimeSpan.Zero);
        }

        public static TimeSpan ToTimeSpan(object obj, TimeSpan defaultValue)
        {
            if (obj != null)
                return ToTimeSpan(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static TimeSpan ToTimeSpan(string s, TimeSpan defaultValue)
        {
            TimeSpan result;
            bool success = TimeSpan.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            if (obj != null) return obj.ToString();

            return string.Empty;
        }

        public static string ToString(string s)
        {
            return ToString(s, string.Empty);
        }

        public static string ToString(string s, string defaultString)
        {
            if (s == null) return defaultString;

            return s.ToString();
        }

        public static string ToString(object s, string defaultString)
        {
            if (s == null) return defaultString;

            return s.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(string s, double defaultValue)
        {
            double result;
            bool success = double.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static double ToDouble(string s)
        {
            return ToDouble(s, 0);
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, 0);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            if (obj != null)
                return ToDouble(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToSingle(string s, float defaultValue)
        {
            float result;
            bool success = float.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static float ToSingle(string s)
        {
            return ToSingle(s, 0);
        }

        public static float ToSingle(object obj)
        {
            return ToSingle(obj, 0);
        }

        public static float ToSingle(object obj, float defaultValue)
        {
            if (obj != null)
                return ToSingle(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string s, decimal defaultValue)
        {
            decimal result;
            bool success = decimal.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static decimal ToDecimal(string s)
        {
            return ToDecimal(s, 0);
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0);
        }

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            if (obj != null)
                return ToDecimal(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBoolean(string s)
        {
            return ToBoolean(s, false);
        }

        public static bool ToBoolean(object obj)
        {
            return ToBoolean(obj, false);
        }

        public static bool ToBoolean(object obj, bool defaultValue)
        {
            if (obj != null)
                return ToBoolean(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static bool ToBoolean(string s, bool defaultValue)
        {
            //修复1被转换为false的BUG
            if (s == "1")
                return true;

            bool result;
            bool success = bool.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static char ToChar(string s, char defaultValue)
        {
            char result;
            bool success = char.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static char ToChar(string s)
        {
            return ToChar(s, '\0');
        }

        public static char ToChar(object obj)
        {
            return ToChar(obj, '\0');
        }

        public static char ToChar(object obj, char defaultValue)
        {
            if (obj != null)
                return ToChar(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte ToByte(string s)
        {
            return ToByte(s, 0);
        }

        public static byte ToByte(object obj)
        {
            return ToByte(obj, 0);
        }

        public static byte ToByte(object obj, byte defaultValue)
        {
            if (obj != null)
                return ToByte(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static byte ToByte(string s, byte defaultValue)
        {
            byte result;
            bool success = byte.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static sbyte ToSByte(string s)
        {
            return ToSByte(s, 0);
        }

        public static sbyte ToSByte(object obj)
        {
            return ToSByte(obj, 0);
        }

        public static sbyte ToSByte(object obj, sbyte defaultValue)
        {
            if (obj != null)
                return ToSByte(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static sbyte ToSByte(string s, sbyte defaultValue)
        {
            sbyte result;
            bool success = sbyte.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ToInt16(string s)
        {
            return ToInt16(s, 0);
        }

        public static short ToInt16(object obj)
        {
            return ToInt16(obj, 0);
        }

        public static short ToInt16(object obj, short defaultValue)
        {
            if (obj != null)
                return ToInt16(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static short ToInt16(string s, short defaultValue)
        {
            short result;
            bool success = short.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ushort ToUInt16(string s)
        {
            return ToUInt16(s, 0);
        }

        public static ushort ToUInt16(object obj)
        {
            return ToUInt16(obj, 0);
        }

        public static ushort ToUInt16(object obj, ushort defaultValue)
        {
            if (obj != null)
                return ToUInt16(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static ushort ToUInt16(string s, ushort defaultValue)
        {
            ushort result;
            bool success = ushort.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(string s, int defaultValue)
        {
            int result;
            bool success = int.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static int ToInt32(string s)
        {
            return ToInt32(s, 0);
        }

        public static int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            if (obj != null)
                return ToInt32(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static uint ToUInt32(string s, uint defaultValue)
        {
            uint result;
            bool success = uint.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static uint ToUInt32(string s)
        {
            return ToUInt32(s, 0);
        }

        public static uint ToUInt32(object obj)
        {
            return ToUInt32(obj, 0);
        }

        public static uint ToUInt32(object obj, uint defaultValue)
        {
            if (obj != null)
                return ToUInt32(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToInt64(string s, long defaultValue)
        {
            long result;
            bool success = long.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static long ToInt64(string s)
        {
            return ToInt64(s, 0);
        }

        public static long ToInt64(object obj)
        {
            return ToInt64(obj, 0);
        }

        public static long ToInt64(object obj, long defaultValue)
        {
            if (obj != null)
                return ToInt64(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToUInt64(string s, ulong defaultValue)
        {
            ulong result;
            bool success = ulong.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static ulong ToUInt64(string s)
        {
            return ToUInt64(s, 0);
        }

        public static ulong ToUInt64(object obj)
        {
            return ToUInt64(obj, 0);
        }

        public static ulong ToUInt64(object obj, ulong defaultValue)
        {
            if (obj != null)
                return ToUInt64(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string s, DateTime defaultValue)
        {
            DateTime result;
            bool success = DateTime.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public static DateTime ToDateTime(string s)
        {
            return ToDateTime(s, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            if (obj != null)
                return ToDateTime(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeUsingFormat(string s, string format, DateTime defaultValue)
        {
            DateTime result = defaultValue;

            try
            {
                result = DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static DateTime ToDateTimeUsingFormat(object obj, string format, DateTime defaultValue)
        {
            if (obj != null)
                return ToDateTimeUsingFormat(obj.ToString(), format, defaultValue);

            return defaultValue;
        }

        public static DateTime ToDateTimeUsingFormat(string s, string format)
        {
            return ToDateTimeUsingFormat(s, format, DateTime.MinValue);
        }

        public static DateTime ToDateTimeUsingFormat(object obj, string format)
        {
            return ToDateTimeUsingFormat(obj, format, DateTime.MinValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(string text, T defaultValue)
        {
            Type type = typeof (T);

            if (!type.IsEnum)
            {
                throw new ArgumentException("type parameter is not an Enum type");
            }

            if (Enum.IsDefined(type, text))
            {
                return (T)Enum.Parse(type, text, false);
            }

            return defaultValue;
        }

        public static T ToEnum<T>(object obj, T defaultValue)
        {
            if (obj != null)
                return ToEnum<T>(obj.ToString(), defaultValue);

            return defaultValue;
        }

        public static T ToEnum<T>(int index)
        {
            return (T)Enum.ToObject(typeof(T), index);
        }

        #endregion
    }
}
