using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nana.Framework.Configuration;
using Nana.Framework.Utility;

namespace Nana.Common.Config
{
    [Serializable]
    public class DaoConfig : ConfigUnit
    {
        public string DBConnectionString1 { get; set; }
        public string DBConnectionString2 { get; set; }

        public override void Reload()
        {
            string configFileText = File.ReadAllText(this.FullPath);

            DaoConfig config = SerializeHelper.DeserializeFromXml<DaoConfig>(configFileText);

            this.DBConnectionString1 = config.DBConnectionString1;
            this.DBConnectionString2 = config.DBConnectionString2;
        }
    }
}
