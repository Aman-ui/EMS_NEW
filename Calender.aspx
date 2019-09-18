<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calender.aspx.cs" Inherits="EffortManagement.Calender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:::: Time Sheet Management ::::</title>
    <style type="text/css">
        /*
    Set the Style for parent CSS Class
    of Calendar control
    Parent [CssClass] = myCalendar
*/
        .myCalendar {
            background-color: #efefef;
            width: 200px;
        }

            /*
    Common style declaration for hyper linked text
*/
            .myCalendar a {
                text-decoration: none;
            }

            /*
    Styles declaration for top title
    [TitleStyle] [CssClass] = myCalendarTitle
*/
            .myCalendar .myCalendarTitle {
                font-weight: bold;
            }

            /*
    Styles declaration for date cells
    [DayStyle] [CssClass] = myCalendarDay
*/
            .myCalendar td.myCalendarDay {
                border: solid 2px #fff;
                border-left: 0;
                border-top: 0;
            }

            /*
    Styles declaration for next/previous month links
    [NextPrevStyle] [CssClass] = myCalendarNextPrev
*/
            .myCalendar .myCalendarNextPrev {
                text-align: center;
            }

            /*
    Styles declaration for Week/Month selector links cells
    [SelectorStyle] [CssClass] = myCalendarSelector
*/
            .myCalendar td.myCalendarSelector {
                background-color: #dddddd;
            }

            .myCalendar .myCalendarDay a,
            .myCalendar .myCalendarSelector a,
            .myCalendar .myCalendarNextPrev a {
                display: block;
                line-height: 18px;
            }

                .myCalendar .myCalendarDay a:hover,
                .myCalendar .myCalendarSelector a:hover {
                    background-color: #cccccc;
                }

                .myCalendar .myCalendarNextPrev a:hover {
                    background-color: #fff;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 80%">
            <tr>
                <td style="text-align: left">
                    <asp:LinkButton runat="server" Text="Export To Excel" CausesValidation="false" ID="LnkExport" OnClick="LnkExport_Click"></asp:LinkButton>
                </td>
                <td style="text-align: center">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" Text="Reports|" CausesValidation="false" ID="LnkReports" Visible="false"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" Text="Add Leave" CausesValidation="false" ID="lnkLeaveApply" Visible="false"></asp:LinkButton>

                            </td>
                            <td>
                                <asp:LinkButton runat="server" Text="Add CompOff" CausesValidation="false" ID="lnkAddCompOff" Visible="true"></asp:LinkButton>

                            </td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: center">
                    <asp:LinkButton runat="server" Text="Upload Tickets" CausesValidation="false" ID="LnkUploadTickets" Visible="false"></asp:LinkButton>
                </td>
                <td style="text-align: right">
                    <b>Welcome :
                        <asp:Label runat="server" ID="Lblresourcename"></asp:Label>
                    </b>&nbsp;&nbsp;
                    <asp:LinkButton runat="server" Text="Logout ?" CausesValidation="false" ID="LnkLogout" OnClick="LnkLogout_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>



        <div>
            <asp:Label runat="server" ID="Lblcurrentmonth" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="Lblcurrentyear" Visible="false"></asp:Label>
            <asp:Calendar ID="Calendar1"
                runat="server"
                DayNameFormat="Shortest"
                Font-Names="Verdana"
                Font-Size="8pt"
                NextMonthText="»"
                PrevMonthText="«"
                SelectMonthText="»"
                SelectWeekText="›"
                CssClass="myCalendar"
                CellPadding="1" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" ForeColor="#003399" Height="450px" Width="80%" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged">

                <OtherMonthDayStyle ForeColor="#999999" />

                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />

                <DayStyle CssClass="myCalendarDay" />

                <SelectedDayStyle Font-Bold="True" BackColor="#009999" ForeColor="#CCFF99" />

                <SelectorStyle CssClass="myCalendarSelector" BackColor="#99CCCC" ForeColor="#336666" />

                <NextPrevStyle CssClass="myCalendarNextPrev" Font-Size="8pt" ForeColor="#CCCCFF" />

                <TitleStyle CssClass="myCalendarTitle" BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                <WeekendDayStyle BackColor="#CCCCFF" />
            </asp:Calendar>
        </div>
    </form>
</body>
</html>
