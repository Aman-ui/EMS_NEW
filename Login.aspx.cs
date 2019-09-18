using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
//using WebApplication1.localhost;
using System.Xml;
using System.Web.Optimization;
using EffortManagement.localhost;
using EffortManagement.localhost1;
namespace EffortManagement
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (IsPostBack)
            {
                TxtUserId.Focus();

            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {


            try
            {
                if (validateuser(TxtUserId.Value, TxtPassword.Value))
                {
                    Response.Redirect("~/UI-Components/index.html");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), UniqueID, "Javascript:alert('Invalid User Id or Password !!!!')", true);
                }
            }
            catch (Exception M)
            {
            }
        }



        private bool validateuser(string userid, string password)
        {
            bool result = false;
            SqlConnection con = new SqlConnection();


            try
            {
                localhost1.WebService1 obj = new localhost1.WebService1();
                XmlDocument xmlDoc = new XmlDocument();
                
                xmlDoc.LoadXml(obj.GetResourceDetails(TxtUserId.Value, TxtPassword.Value));

                DataSet ds = new DataSet();
                XmlNodeReader xmlreader = new XmlNodeReader(xmlDoc);
                ds.ReadXml(xmlreader, XmlReadMode.Auto);


                string dbUserID = ds.Tables[0].Rows[0].Field<string>("RName");


                string Userpassword = ds.Tables[0].Rows[0].Field<string>("RPassword");

                // string ResourceId = ds.Tables["TblResource"].Rows[0]["RID"].ToString();
                //string dbUserID = ds.Tables["Tblresource"].Rows[0]["RName"].ToString();
                // string Userpassword = ds.Tables["TblResource"].Rows[0]["RPassword"].ToString();
                string ResourceId = ds.Tables[0].Rows[0].Field<string>("RID");
                string RRole = ds.Tables[0].Rows[0].Field<string>("RROLE");
                TxtUserId.Value = dbUserID;
                TxtPassword.Value = Userpassword;

                if (dbUserID.ToLower().Equals(userid.ToLower()) && Userpassword.Equals(password))
                {
                    Session["resourceid"] = ResourceId;
                    Session["Username"] = TxtUserId.Value;
                    Session["Role"] = RRole;
                    result = true;

                }
                else
                {
                    result = false;
                }

            }
            catch (Exception M)
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return result;
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            TxtUserId.Value = "";
            TxtPassword.Value = "";
        }
    }
}