using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using EffortManagement;
using System.Web.UI.WebControls;


namespace EffortManagement
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

        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]

        public String GetLeaveAndCompOffDetails()
        {
            SqlConnection con = new SqlConnection(constr);


            SqlCommand cmd = new SqlCommand("sp_CompOffInsert");

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds);

            string str = ds.GetXml();
            return str;




        }


        [WebMethod]

        public static List<string> searchTickets(string prefixText, int count)
        {
            try
            {

                prefixText = prefixText.Trim();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select TicketNumber from TblTicketDetails with(Nolock) where TicketNumber like '" + prefixText + "%'";
                cmd.Connection = con;
                con.Open();
                List<string> Tikcets = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Tikcets.Add(sdr["TicketNumber"].ToString());
                    }
                }
                con.Close();
                return Tikcets;
            }
            catch (Exception M)
            {
                throw;
            }
        }

        [WebMethod]
        public DataTable BindCascadingDropList(string DListType, int id)
        {
            DataTable Dtsource = new DataTable();


            Dtsource.TableName = "Tbldropdowndata";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                string Query = string.Empty;

                if (DListType.ToLower().Equals("applications"))
                {
                    Query = "Select * from MST_Applications with(nolock) where application_status= 1 and Application_Id=" + id;
                }
                else if (DListType.ToLower().Equals("classification"))
                {
                    Query = "Select * from Mst_Tikcetclassification C with(nolock) inner join Tbltickettypeclassificationmapping M on C.TClassification_Id=M.TClassification_ID  where TClassification_status = 1 and TType_ID=" + id;
                }


                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = Query;
                Dr = cmd.ExecuteReader();
                Dtsource.Load(Dr);
                con.Close();

            }
            catch (Exception M)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return Dtsource;

        }


        [WebMethod]

        public int Deleteselectedefforts(int TEID)
        {
            SqlConnection con = new SqlConnection();
            int result = 0;
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = "Delete from TblTicketEfforts where TEID=@TEID";
                cmd.Parameters.AddWithValue("@TEID", TEID);


                result = cmd.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception M)
            {
                throw;
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


        [WebMethod]

        private DataTable SearchTicketbyTicketNumber(string TicketNumber)
        {
            DataTable DtTicketsInfo = new DataTable();


            DtTicketsInfo.TableName = "TblTicketInfo";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = "Select * from  TblTicketDetails with(Nolock) where TicketNumber=@TicketNumber";
                cmd.Parameters.AddWithValue("@TicketNumber", TicketNumber);


                Dr = cmd.ExecuteReader();
                DtTicketsInfo.Load(Dr);
                con.Close();
                return DtTicketsInfo;
            }
            catch (Exception M)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }

        [WebMethod]
        public string GetResourceDetails(string userid, string password)
        {
            bool result = false;
            // SqlConnection con = new SqlConnection();
            con = new SqlConnection(constr);
            con.Open();
            //MessageBox.Show("Connection Open  !");
            SqlDataAdapter da = new SqlDataAdapter("Select * from TblResource with(nolock) where RName='" + userid + "'   and RPassword= '" + password + "' and RStatus=1", con);
            //SqlDataAdapter da = new SqlDataAdapter("Select * from TblResource with(nolock) where RName=@RName and RPassword=@password and RStatus=1";

            DataSet ds = new DataSet();
            da.Fill(ds);

            //return ds;
            string str = ds.GetXml();

            return str;



        }

    }



}

