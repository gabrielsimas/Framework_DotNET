using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = false)]
namespace Framework.Log4NetImpl
{

    public class Log4NetHelper
    {
        #region Atributos

        protected ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);        

        #endregion

        #region Métodos
        public static void Inicializacao()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion
    }
}
