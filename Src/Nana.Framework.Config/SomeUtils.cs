using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public class SomeUtils
    {
        /// <summary>
        /// 获得绝对路径
        /// </summary>
        /// <param name="relativePath">SetupInformation.ApplicationBase的相对路径</param>
        /// <returns></returns>
        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, relativePath);
        }

    }
}
