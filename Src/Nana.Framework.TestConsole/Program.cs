using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nana.Common.Config;
using Nana.Framework.Cache;
using Nana.Framework.Cache.Redis;
using Nana.Framework.Configuration;
using Nana.Framework.Log;
using Nana.Framework.Utility;

namespace Nana.Framework.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_RedisClient_1();
            
            Console.ReadLine();
        }

        private static void Test_Configuration_1()
        {
            //DaoConfig daoConfig = ConfigFactory.GetConfig<DaoConfig>();

            //Console.WriteLine(daoConfig.FileName + " " + daoConfig.FullPath);

            LogHelperConfigUnit cu = ConfigFactory.GetConfig<LogHelperConfigUnit>();

            Console.WriteLine(cu.DefaultLoggerType);
            Console.WriteLine(cu.MsgTypeLoggerTypeMapping.Count);
        }

        private static void Test_Log_1()
        {
            LogHelper.Log(new CommonLogMessage() { LogID = Guid.NewGuid().ToString("N"), ClientIP = "127.0.0.1" }, EnumLogLevel.Error);
        }

        private static void Test_Cache_1()
        {
            SessionCache cache = new SessionCache();

            for (int i = 0; i < 1000; i++)
            {
                cache.Set("ebooking.elong.com_" + i, new Dictionary<string, string>()
                {
                    {"Username", "chengyang.cai" + i},
                    {"HotelID", "00987654" + i}
                });

                var dic = (Dictionary<string, string>)cache.Get("ebooking.elong.com_" + i);
                Console.WriteLine(dic["Username"]);
                Console.WriteLine(dic["HotelID"]);

                cache.Remove("ebooking.elong.com_" + i);
            }
        }

        private static void Test_RedisClient_1()
        {
            RedisClient client = new RedisClient("abc", EnumReadMode.ReadMaster, 
                new List<string>() { "127.0.0.1:6379" }, 
                null, 200, 1000);

            client.Set("abc", new { a = "a", b="b" });

            client.Set("abc1", "abc1");
        }
    }

    public class SessionCache : BaseCache
    {
        public override object GetDataFromSource(object key)
        {
            return null;
        }
    }
}
