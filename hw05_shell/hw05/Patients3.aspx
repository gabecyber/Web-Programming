<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Patients3.aspx.cs" Inherits="hw05.Patients3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HW 05 - Requirement 3</title>
    <link href="site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2><a href="Default.aspx">HW 05</a> - Patients 3 </h2>

        <hr />
        <p>
            Choose a Patient:
            <asp:DropDownList ID="ddPatients" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddPatients_SelectedIndexChanged" DataSourceID="Access_Patients" DataTextField="LastName" DataValueField="PatientID">
            </asp:DropDownList>
            <asp:SqlDataSource ID="Access_Patients" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [LastName], [PatientID] FROM [Patients]"></asp:SqlDataSource>
        </p>
        <p>
            Date Charge VisitID</p>
        <asp:TextBox ID="txtVisitAndPreCharges" runat="server" Height="193px" TextMode="MultiLine" 
            Width="371px"></asp:TextBox>

        <p>
            &nbsp;</p>
    </div>
    </form>
</body>
</html>
