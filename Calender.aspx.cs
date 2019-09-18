using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.OleDb;
using System.Globalization;

namespace EffortManagement
{
    public partial class Calender : System.Web.UI.Page
    {
        int currentmonth = DateTime.Now.Month;
        int curryear = DateTime.Now.Year;

        int resourceid;
        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            Calendar1.DayRender += new DayRenderEventHandler(this.DayRender);
            resourceid = Convert.ToInt32(Session["resourceid"]);
            if (!IsPostBack)
            {
                if ((Session["resourceid"] == null || Session["Username"] == null || Session["Role"] == null))
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    lnkAddCompOff.PostBackUrl = "~/compoff.aspx";
                    string role = Convert.ToString(Session["Role"]);
                    if (role.ToLower().Equals("admin"))
                    {
                        LnkReports.Visible = true;
                        //LnkReports.PostBackUrl = "~/Reports/EffortsReport.aspx";
                        LnkReports.PostBackUrl = "~/ActivityReport.aspx";
                        LnkUploadTickets.Visible = true;
                        LnkUploadTickets.PostBackUrl = "~/UploadTickets.aspx";
                        lnkLeaveApply.Visible = true;
                        lnkLeaveApply.PostBackUrl = "~/LeaveRequest.aspx"; //lnkAddCompOff
                    }
                    else
                    {
                        LnkReports.Visible = false;
                        LnkUploadTickets.Visible = false;
                    }

                }

                string rname = Convert.ToString(Session["Username"]);
                Lblresourcename.Text = rname;
                //Session["resourceid"] = resourceid;


                Session["LeaveInCurrentmonth"] = GetLeaveInMonths(currentmonth, curryear, rname);
                Session["Currentmonthefforts"] = Getselectedmonthefforts(currentmonth, curryear, resourceid);
                Session["CompOffCurrentmonth"] = GetCompOffInMonths(currentmonth, curryear, rname);
            }

        }

        private object GetLeaveInMonths(int month, int year, string rname)
        {
            DataTable DtSelectmonthsefforts = new DataTable();

            DtSelectmonthsefforts.TableName = "TblEfforts";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "prc_getcurrentmonthLeave";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@resourceid", rname);
                Dr = cmd.ExecuteReader();
                DtSelectmonthsefforts.Load(Dr);
                con.Close();

                return DtSelectmonthsefforts;


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

        void DayRender(Object sender, DayRenderEventArgs e)
        {

            //if (e.Day.Date.Month != this.Calendar1.VisibleDate.Month)
            //{
            //    e.Cell.Text = "";
            //}

            e.Day.IsSelectable = false;

            string futureflag = "N";
            StringBuilder objtablebuilder = new StringBuilder();
            objtablebuilder.Append("<table style='width:80%'>");
            objtablebuilder.Append("<tr>");
            objtablebuilder.Append("<td colspan='2' align='left'>");

            // if(e.)
            if (!checkcutoffdate(e.Day.Date))
            {
                // e.Cell.Enabled = true;

                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                //Rajneeh-14-sep
                // if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 15) || (e.Day.Date.Day > 21 && e.Day.Date.Day < 29))) || e.Day.Date.ToString("dddd") == "Sunday")
                //comment on 21 March2018
                //if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 15) || (e.Day.Date.Day > 21 && e.Day.Date.Day < 29)) && e.Day.Date < Convert.ToDateTime("08/18/2017")))
                //{
                //    e.Cell.BackColor = System.Drawing.Color.LightGreen;// .LightSkyBlue;
                //    // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                //    e.Cell.Font.Bold = true;
                //    objtablebuilder.Append("Week-Off");
                //}
                ////else if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 21)) && e.Day.Date > Convert.ToDateTime("08/18/2017")) || e.Day.Date.ToString("dddd") == "Sunday")
                //else if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 22)) && e.Day.Date > Convert.ToDateTime("08/18/2017")) || e.Day.Date.ToString("dddd") == "Sunday")
                //{
                //    e.Cell.BackColor = System.Drawing.Color.LightGreen;// .LightSkyBlue;
                //    //// e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                //    //e.Cell.Font.Bold = true;
                //    //objtablebuilder.Append("Week-Off");
                //}

                //else
                //endcomment 21march 2018  
                if (e.Day.Date.ToString("dddd") == "Sunday")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;// .LightSkyBlue;
                    // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                    e.Cell.Font.Bold = true;
                    // objtablebuilder.Append("Week-Off");
                    objtablebuilder.Append("<a href='Addefforts.aspx?date=" + e.Day.Date.ToString("dd/MM/yyyy") + "'> Add Efforts</a>");
                }
                else if (IsOnLeave(e.Day.Date.Day) && e.Day.Date.Month == currentmonth)
                {
                    e.Cell.BackColor = System.Drawing.Color.MediumPurple;// .LightSkyBlue;
                    //e.Cell.Font.Bold = true; ;
                    //objtablebuilder.Append("On-Leave");
                    //e.Day.IsSelectable = false;
                    //futureflag = "N";
                }
                else if (IsOnCompOff(e.Day.Date.Day) && e.Day.Date.Month == currentmonth)
                {
                    e.Cell.BackColor = System.Drawing.Color.MediumOrchid;// .LightSkyBlue;
                    //e.Cell.Font.Bold = true; ;
                    //objtablebuilder.Append("On-Leave");
                    //e.Day.IsSelectable = false;
                    //futureflag = "N";
                }
                else if (!checkAdditioncutoffdate(e.Day.Date))
                {
                    // e.Cell.BackColor = System.Drawing.Color.Green;
                    objtablebuilder.Append("<a href='Addefforts.aspx?date=" + e.Day.Date.ToString("dd/MM/yyyy") + "'> Add Efforts</a>");
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                    e.Day.IsSelectable = false;
                    futureflag = "Y";
                }


            }
            else
            {
                // e.Cell.Enabled = false;
                e.Cell.BackColor = System.Drawing.Color.LightPink;
            }



            if (Session["Currentmonthefforts"] != null)
            {
                decimal Todaysefforts = 0;
                string dat = e.Day.Date.ToString("dd/MM/yyyy");
               dat= dat.Replace("-", "/");
                DataRow[] DR = ((DataTable)Session["Currentmonthefforts"]).Select("TEDate='" + e.Day.Date.ToString("dd/MM/yyyy") + "'");

                DataRow[] DR1 = ((DataTable)Session["Currentmonthefforts"]).Select("TEDate='" + dat + "'");
               
                DataTable dt = (DataTable)Session["Currentmonthefforts"];
                decimal check=0;
                if (DR1 != null && DR1.Count() > 0)
                {
                    check = Convert.ToDecimal(DR1[0]["ActualEfforts"]);
                }
                //if (IsOnLeave(e.Day.Date.Day))
                //     e.Cell.Text = "On-Leave";else
                if (e.Day.Date > this.Calendar1.TodaysDate && e.Day.Date.Month > this.Calendar1.TodaysDate.Month)
                {
                    e.Cell.Text = "";
                }
                else
                {

                    objtablebuilder.Append("</td>");

                    objtablebuilder.Append("</tr>");
                    objtablebuilder.Append("<tr>");
                    objtablebuilder.Append("<td align='left'>");

                    //Rajneesh-14Sep2017
                    //if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 15) || (e.Day.Date.Day > 21 && e.Day.Date.Day < 29))) || e.Day.Date.ToString("dddd") == "Sunday")
                    //if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 15) || (e.Day.Date.Day > 21 && e.Day.Date.Day < 29)) && e.Day.Date < Convert.ToDateTime("08/18/2017")))
                    //{
                    //    e.Cell.BackColor = System.Drawing.Color.LightGreen;// .LightSkyBlue;
                    //    // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                    //    e.Cell.Font.Bold = true;
                    //    objtablebuilder.Append("Week-Off");
                    //}
                    //else if ((e.Day.Date.ToString("dddd") == "Saturday" && ((e.Day.Date.Day > 7 & e.Day.Date.Day < 22)) && e.Day.Date > Convert.ToDateTime("08/18/2017")) || e.Day.Date.ToString("dddd") == "Sunday")
                    if (e.Day.Date.ToString("dddd") == "Sunday" && check<1)//((DR != null && DR.Count() > 0) ? (Convert.ToDecimal(DR[0]["ActualEfforts"]) < 1 ? true : false) : true))
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;// .LightSkyBlue;
                        // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                        e.Cell.Font.Bold = true;
                        objtablebuilder.Append("Week-Off");
                    }
                    else if (IsOnLeave(e.Day.Date.Day) && e.Day.Date.Month == currentmonth)
                    {
                        e.Cell.BackColor = System.Drawing.Color.MediumPurple;// .LightSkyBlue;
                        // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                        e.Cell.Font.Bold = true;
                        objtablebuilder.Append("On-Leave");
                    }
                    else if (IsOnCompOff(e.Day.Date.Day) && e.Day.Date.Month == currentmonth)
                    {
                        e.Cell.BackColor = System.Drawing.Color.MediumOrchid;// .LightSkyBlue;
                        // e.Cell.ForeColor = System.Drawing.Color.DarkGreen;
                        e.Cell.Font.Bold = true;
                        objtablebuilder.Append("Comp-Off");
                    }
                    else
                    {
                        objtablebuilder.Append("Actual Efforts :");
                        objtablebuilder.Append("</td>");
                        objtablebuilder.Append("<td align='left'>");

                        if (DR != null && DR.Count() > 0)
                        {
                            Todaysefforts = Convert.ToDecimal(DR[0]["ActualEfforts"]);



                            //e.Cell.Controls.Add(new LiteralControl("<br/>AE=" + Todaysefforts));

                        }
                        else if (DR1 != null && DR1.Count() > 0)
                        {
                            Todaysefforts = Convert.ToDecimal(DR1[0]["ActualEfforts"]);
                        }
                            
                     objtablebuilder.Append(Todaysefforts);
                    }
                    objtablebuilder.Append("</td>");
                    objtablebuilder.Append("</tr>");
                    objtablebuilder.Append("</table>");
                    if (futureflag == "N")
                        e.Cell.Controls.Add(new LiteralControl(objtablebuilder.ToString()));
                    objtablebuilder.Clear();
                }
            }


            // Change the background color of the days in the month
            // to yellow.
            //if (!e.Day.IsOtherMonth && !e.Day.IsWeekend)
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Yellow;
            //}

            //// Add custom text to cell in the Calendar control.
            //if (e.Day.Date.Day == 18)
            //{
            //    e.Cell.Controls.Add(new LiteralControl("<br />Holiday"));
            //}
            e.Cell.Height = 90;
            e.Cell.Width = 50;
        }

        private bool checkcutoffdate(DateTime Dt)
        {
            int Noofdays = Convert.ToInt32(ConfigurationManager.AppSettings["Cutoffdate"]);

            DateTime Cuttoffdate = DateTime.Now.AddDays(-Noofdays);

            if (Dt < Cuttoffdate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool checkAdditioncutoffdate(DateTime Dt)
        {
            int Noofdays = Convert.ToInt32(ConfigurationManager.AppSettings["Additiondate"]);

            DateTime Cuttoffdate = DateTime.Now.AddDays(Noofdays);

            if (Cuttoffdate < Dt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTime ConvertDate(string Dt)
        {
            DateTime Dtres = DateTime.ParseExact(Dt, "dd/MM/yyyy", null);
            return Dtres;
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            if (e.NewDate.Month != null && e.NewDate.Year != null)
            {
                currentmonth = e.NewDate.Month;
                curryear = e.NewDate.Year;
                Session["Currentmonthefforts"] = Getselectedmonthefforts(currentmonth, curryear, resourceid);
                Session["LeaveInCurrentmonth"] = GetLeaveInMonths(currentmonth, curryear, Convert.ToString(Session["Username"]));
                Session["CompOffCurrentmonth"] = GetCompOffInMonths(currentmonth, curryear, Convert.ToString(Session["Username"]));
                Lblcurrentmonth.Text = currentmonth.ToString();
                Lblcurrentyear.Text = curryear.ToString();
            }
        }

        private DataTable Getselectedmonthefforts(int month, int year, int resourceid)
        {
            DataTable DtSelectmonthsefforts = new DataTable();

            DtSelectmonthsefforts.TableName = "TblEfforts";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "prc_getcurrentmonthefforts";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@resourceid", resourceid);
                Dr = cmd.ExecuteReader();
                DtSelectmonthsefforts.Load(Dr);
                con.Close();

                return DtSelectmonthsefforts;


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

        protected void LnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        protected void LnkExport_Click(object sender, EventArgs e)
        {

            if (Lblcurrentmonth.Text != "" && Lblcurrentyear.Text != "")
            {
                currentmonth = Convert.ToInt32(Lblcurrentmonth.Text);
                curryear = Convert.ToInt32(Lblcurrentyear.Text);
            }


            DataTable DtCurrData = Getselectedmontheffortstoexcel(currentmonth, curryear, resourceid);
            if (DtCurrData != null && DtCurrData.Rows.Count > 0)
            {
                ExportDataTable(DtCurrData, "Efforts");
            }
        }

        private DataTable Getselectedmontheffortstoexcel(int month, int year, int resourceid)
        {
            DataTable DtSelectmonthsefforts = new DataTable();

            DtSelectmonthsefforts.TableName = "TblEfforts";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "prc_getcurrentmontheffortstoexcel";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@resourceid", resourceid);
                Dr = cmd.ExecuteReader();
                DtSelectmonthsefforts.Load(Dr);
                con.Close();

                return DtSelectmonthsefforts;


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



        public void ExportDataTable(DataTable dt, string FileName)
        {
            string attachment = "attachment; filename=" + FileName + ".xls";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string sTab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(sTab + dc.ColumnName);
                sTab = "\t";
            }
            HttpContext.Current.Response.Write("\n");

            int i;
            foreach (DataRow dr in dt.Rows)
            {
                sTab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(sTab + dr[i].ToString());
                    sTab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }

        private bool IsOnLeave(int Dt)
        {
            DataTable table = (DataTable)Session["LeaveInCurrentmonth"];

            foreach (DataRow dr in table.Rows)
            {
                if (Dt >= Convert.ToInt32(dr[0]) && Dt <= Convert.ToInt32(dr[1]))
                    return true;
                //else
                //    return false;
            }
            return false;
        }
        private bool IsOnCompOff(int Dt)
        {
            DataTable table = (DataTable)Session["CompOffCurrentmonth"];

            foreach (DataRow dr in table.Rows)
            {
                if (Dt == Convert.ToInt32(dr[0]))// && Dt <= Convert.ToInt32(dr[1]))
                    return true;
                //else
                //    return false;
            }
            return false;
        }
        private object GetCompOffInMonths(int month, int year, string rname)
        {
            DataTable DtSelectmonthsefforts = new DataTable();

            DtSelectmonthsefforts.TableName = "TblEfforts";
            IDataReader Dr;
            SqlConnection con = new SqlConnection();
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "sp_getcurrentmonthCompOff";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@resourceid", rname);
                Dr = cmd.ExecuteReader();
                DtSelectmonthsefforts.Load(Dr);
                con.Close();

                return DtSelectmonthsefforts;


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

    }
}