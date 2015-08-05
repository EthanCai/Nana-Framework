using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nana.Framework.Config
{
    public class RemoteConfigSource
    {
        /// <summary>
        /// 远程配置服务的访问地址
        /// </summary>
        /// <value>
        /// The remote configuration source URL.
        /// </value>
        public string RemoteConfigSourceURL { get; set; }

        /// <summary>
        /// 远程配置本地缓存目录
        /// </summary>
        /// <value>
        /// The remote configuration cache directory.
        /// </value>
        public string RemoteConfigCacheDirectory { get; set; }

        public RemoteConfigSource()
        {
            this.RemoteConfigCacheDirectory = GetDefaultRemoteConfigCacheDirectory();
        }

        private string GetDefaultRemoteConfigCacheDirectory()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"..\confcache\");
        }
    }
}
