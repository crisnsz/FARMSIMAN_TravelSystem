<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTravel.aspx.cs" Inherits="ERP_GMEDINA.Views.Travel.Reports.ReportTravel" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <%--<asp:Button ID="btnShowReport" runat="server" Text="Show Report" OnClientClick="fetchDataAndShowReport(); return false;" />--%>
        <rsweb:ReportViewer ID="ReportViewer" runat="server" Width="800px"></rsweb:ReportViewer>
    </form>
</body>
</html>
<script type="text/javascript">
    //async function fetchDataAndShowReport() {
    //    try {

    //        console.log('fetchDataAndShowReport');

    //        const data = await GenerateReport(transporter_ID, initialDate, finalDate);

          
    //    } catch (error) {
    //        console.error('Error:', error);
    //    }
    //}
</script>
