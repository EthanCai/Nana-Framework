using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Common
{
    public class GuidHelper
    {
        /// <summary>
        /// 判断指定的GUID是否在指定的百分比范围内
        /// 用于控制日志记录数量
        /// </summary>
        /// <param name="g">GUID</param>
        /// <param name="percent">百分比</param>
        /// <returns></returns>
        public static bool InSample(Guid g, decimal percent)
        {
            byte[] buffer = g.ToByteArray();
            decimal val = Math.Abs(((int)buffer[0]) | ((int)buffer[1] << 8) | ((int)buffer[2] << 16) | ((int)buffer[3] << 24));

            decimal r = val % 100;
            if (r <= percent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
