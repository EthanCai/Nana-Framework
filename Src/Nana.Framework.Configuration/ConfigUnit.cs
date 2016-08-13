using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Nana.Framework.Utility;

namespace Nana.Framework.Configuration
{
    public abstract class ConfigUnit : MarshalByRefObject
    {
        #region 属性

        /// <summary>
        /// 需要每个子类确定各自对应配置文件位置及名称
        /// </summary>
        [XmlIgnore]
        public virtual string FileName
        {
            get
            {
                return this.GetType().Name + ".config";
            }
        }

        /// <summary>
        /// 配置文件的路径
        /// </summary>
        [XmlIgnore]
        public string FullPath
        {
            get
            {
                if (!Config.ConfigFileFullPathDic.ContainsKey(FileName.ToLower()))
                {
                    throw new Exception(
                        string.Format("'{0}'下（包括子文件夹）不存在'{1}'", Config.ConfigFileFolder, FileName));
                }
                
                return Config.ConfigFileFullPathDic[FileName.ToLower()];
            }
        }

        /// <summary>
        /// 配置最后一次加载的时间；
        /// </summary>
        [XmlIgnore]
        public DateTime LastLoadTime { get; set; }

        #endregion

        #region 更新配置

        /// <summary>
        /// Reload方法没有返回一个新的ConfigUnit对象，原因是对于基础组件来说，
        /// 上层业务组件可能依赖基础对象，基础对象的管理必须要严格，不能随意抛弃，创建基础对象
        /// </summary>
        public abstract void Reload();

        #endregion
    }
}
