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
            string configFileText = this.GetLocalConfigFileText(this.Settings.LocalConfigDirectory, 
                this.Settings.AppName, confName, version, this.Settings.LocalConfigFileExt);
            IConfigReader configReader = this.GetConfigReader(this.Settings.LocalConfigFormat);
            var result = configReader.Read(configFileText);
            return result;
        }

        private string GetLocalConfigFileText(string localConfigDir, 
            string appName, string confName, string version, string configFileExt)
        {
            string fileName = string.Format("{0}_{1}_{2}{3}", appName, confName, version, configFileExt);
            string filePath = Path.Combine(localConfigDir, fileName);
            return File.ReadAllText(filePath);
        }

        private IConfigReader GetConfigReader(string localConfigFormat)
        {
            switch (localConfigFormat)
            {
                case Settings.XML_Config_Format:
                    return new XMLConfigReader();
                    break;
                case Settings.JSON_Config_Format:
                    return new JSONConfigReader();
                    break;
                case Settings.YAML_Config_Format:
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
