using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.Loader
{
    public class LocalConfigLoader : AbstractConfigLoader
    {
        public LocalConfigLoader(Settings settings) : base(settings)
        {
            
        }

        public override ConfigUnit LoadConfig(string confName, string version)
        {
            Settings.Logger.Log(string.Format(
                "开始读取本地配置文件的配置信息, confName={0}, version={1}", confName, version), 
                EnumLogLevel.Debug);

            string configFileText = this.GetLocalConfigFileText(
                this.Settings.LocalConfigFileSource.LocalConfigDirectory, 
                this.Settings.AppName, confName, version, 
                this.Settings.LocalConfigFileSource.LocalConfigFileExt);
            IConfigReader configReader = this.GetConfigReader(
                this.Settings.LocalConfigFileSource.LocalConfigFormat);
            var result = configReader.Read(configFileText);

            Settings.Logger.Log("完成读取本地配置文件的配置信息", EnumLogLevel.Debug);
            
            return result;
        }

        private string GetLocalConfigFileText(string localConfigDir, 
            string appName, string confName, string version, string configFileExt)
        {
            string fileName = string.Format("{0}_{1}_{2}{3}", appName, confName, version, configFileExt);
            string filePath = Path.Combine(localConfigDir, fileName);

            Settings.Logger.Log(string.Format(
                "获得本地配置文件的路径{0}，读取配置文件内容", filePath),
                EnumLogLevel.Debug);

            return File.ReadAllText(filePath);
        }

        private IConfigReader GetConfigReader(string localConfigFormat)
        {
            Settings.Logger.Log(string.Format(
                "根据指定的配置文件格式{0}，获得ConfigReader", localConfigFormat),
                EnumLogLevel.Debug);

            switch (localConfigFormat)
            {
                case LocalConfigFileSource.XML_Config_Format:
                    return new XMLConfigReader();
                    break;
                case LocalConfigFileSource.JSON_Config_Format:
                    return new JSONConfigReader();
                    break;
                case LocalConfigFileSource.YAML_Config_Format:
                    return new YamlConfigReader();
                    break;
                default:
                    string message = string.Format(@"localConfigFormat为{0}没有对应的IConfigReader", localConfigFormat);
                    throw new InitConfigException("");
                    break;
            }
        }

        
    }
}
