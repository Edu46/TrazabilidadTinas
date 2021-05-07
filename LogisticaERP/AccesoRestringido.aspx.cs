using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP
{
    public partial class AccesoRestringido : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            var masterPage = Page.Request.QueryString["masterPage"];
            if (!string.IsNullOrEmpty(masterPage))
            {
                Page.MasterPageFile = masterPage.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}