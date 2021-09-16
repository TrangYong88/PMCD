using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Lib.Elearn
{
    public class ElearnConstants
    {
        public static string ELEARN_CONSTR = (ConfigurationManager.AppSettings["ELEARN_CONSTR"] == null) ? "" : ConfigurationManager.AppSettings["ELEARN_CONSTR"];
    }
}
