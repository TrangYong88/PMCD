using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for MyConstants
/// </summary>
public class MyConstants
{
    public static string LogFilePath = (ConfigurationManager.AppSettings["LogFilePath"] == null) ? "" : ConfigurationManager.AppSettings["LogFilePath"].ToString();
    public static string LogFileName = (ConfigurationManager.AppSettings["LogFileName"] == null) ? "" : ConfigurationManager.AppSettings["LogFileName"].ToString();
    public static byte DISTRIBUTED_PROCESS = Convert.ToByte((ConfigurationManager.AppSettings["DISTRIBUTED_PROCESS"] == null) ? "0" : ConfigurationManager.AppSettings["DISTRIBUTED_PROCESS"]);
    public static string ELEARN_CONSTR = (ConfigurationManager.AppSettings["ELEARN_CONSTR"] == null) ? "" : ((string.IsNullOrEmpty(ConfigurationManager.AppSettings["ELEARN_CONSTR"].ToString().Trim())) ? "" : ConfigurationManager.AppSettings["ELEARN_CONSTR"].ToString().Trim());
    public static string ROOT_PATH = ConfigurationManager.AppSettings["ROOT_PATH"];
    public static string PRJ_ROOT = ROOT_PATH + "Admin/";
    public static string AdminFolder = "";
    public MyConstants()
    {
        //
        // TODO: Add constructor logic here
        //
    }

}
