using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    /// <summary>
    /// 配置单元，所有的应用配置应实现此接口
    /// </summary>
    public interface IConfigUnit
    {
        void Load(string configContent);
    }
}
