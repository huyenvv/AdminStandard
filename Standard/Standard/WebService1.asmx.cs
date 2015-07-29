using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Services;
using WebLib;
using WebLib.DAL;
using WebLib.Models;
namespace Standard
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public List<fwMenu> HelloWorld(int x)
        {
            new fwMenuDAL().Delete(60);
            return new fwMenuDAL().ListAll();
        }


        [WebMethod]
        public string Test(int x)
        {
            return "fuck";
        }
    }
}
