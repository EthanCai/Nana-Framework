using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Nana.Framework.Common
{
    public class SerializeHelper
    {

        #region binary serialization

        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="obj">待保存的对象</param>
        /// <returns>byte数组</returns>
        public static byte[] SerializeObjectToBinaryBytes(object obj)
        {
            //将对象序列化到MemoryStream中
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //从MemoryStream中获取获取byte数组
            return ms.ToArray();
        }

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>对象</returns>
        public static object DeserializeFromBinaryBytes(byte[] bytes)
        {
            object result = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                result = formatter.Deserialize(ms);
            }
            return result;
        }

        #endregion

        #region XML serializiation

        /// <summary>
        /// 使用标准的XmlSerializer，将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>XML字符串</returns>
        public static string SerializeObjectToXml<T>(T obj)
        {
            return SerializeObjectToXml(typeof (T), obj);
        }

        public static string SerializeObjectToXml(Type type, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);  //使用UTF-8编码
            serializer.Serialize(sw, obj);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// 使用标准的XmlSerializer，将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>对象实例</returns>
        public static T DeserializeFromXml<T>(string xmlString)
        {
            return (T) DeserializeFromXml(typeof (T), xmlString);
        }

        public static object DeserializeFromXml(Type type, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            StringReader stringReader = new StringReader(xmlString);
            object obj = serializer.Deserialize(stringReader);

            return obj;
        }

        #endregion

        #region JSON serialization

        /// <summary>
        /// 使用Json.NET，将对象序列化成JSON格式字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeObjectToJson<T>(T obj)
        {
            return SerializeObjectToJson(obj);
        }

        public static string SerializeObjectToJson(object obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            return str;
        }

        /// <summary>
        /// 使用Json.NET，将JSON格式字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>对象实例</returns>
        public static T DerializeJsonToObject<T>(string json)
        {
            return (T)DerializeJsonToObject(typeof(T), json);
        }

        public static object DerializeJsonToObject(Type type, string json)
        {
            object obj = JsonConvert.DeserializeObject(json, type);
            return obj;
        }

        #endregion
    }
}
