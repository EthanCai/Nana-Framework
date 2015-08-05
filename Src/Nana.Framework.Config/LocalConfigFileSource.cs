using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nana.Framework.Config
{
    public class LocalConfigFileSource
    {
        public const string XML_Config_Format = "xml";
        public const string JSON_Config_Format = "json";
        public const string YAML_Config_Format = "yaml";

        /// <summary>
        /// 本地配置文件保存的目录
        /// </summary>
        /// <value>
        /// The configuration directory.
        /// </value>
        public string LocalConfigDirectory { get; set; }

        private string _localConfigFormat = null;

        /// <summary>
        /// 本地配置文件的格式
        /// </summary>
        /// <value>
        /// xml, json, yaml
        /// </value>
        public string LocalConfigFormat
        {
            get { return this._localConfigFormat; }
            set
            {
                if (value != XML_Config_Format
                    && value != JSON_Config_Format
                    && value != YAML_Config_Format)
                {
                    string message = string.Format("不支持LocalConfigFormat为{0}的配置文件", LocalConfigFormat);
                    throw new InitConfigException(message);
                }
                this._localConfigFormat = value;
            }
        }

        public string LocalConfigFileExt
        {
            get
            {
                switch (LocalConfigFormat)
                {
                    case XML_Config_Format:
                        return ".config";
                        break;
                    case JSON_Config_Format:
                        return ".json";
                        break;
                    case YAML_Config_Format:
                        return ".yaml";
                        break;
                    default:
                        string message = string.Format("不支持LocalConfigFormat为{0}的配置文件", LocalConfigFormat);
                        throw new InitConfigException(message);
                        break;
                }
            }
        }

        public LocalConfigFileSource()
        {
            this.LocalConfigFormat = GetDefaultLocalConfigFormat();
            this.LocalConfigDirectory = GetDefaultLocalConfigDirectory();
        }

        private string GetDefaultLocalConfigFormat()
        {
            return XML_Config_Format;
        }

        private string GetDefaultLocalConfigDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                 HttpContext.Current != null ? @".\App_Data\conf\" : @"..\conf\");
        }
    }
}
