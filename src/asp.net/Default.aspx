<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="asp.net._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工管理系统</title>
    <script language="javascript" type="text/javascript"
            src="~/Scripts/jquery-3.3.1.min.js"></script>
    <style type="text/css">
        #page {
            width: 600px;
            margin: 10px auto;
        }

        body {
            font-size: 12px;
            text-align: center;
        }

        table {
            border: 1px solid #000000;
            margin: 10px auto;
            background-color: #eee;
        }

        tr { line-height: 23px; }

        th {
            background-color: #ccc;
            color: #fff;
        }

        .oddRow { background-color: #fff; }
    </style>
    <script type="text/javascript">
        $(function() {
            $("#DetailsViewEmployee tr td:even")
                .css("width", "100px")
                .css("background-color", "#fff")
                .css("text-align", "right")
                .css("padding-right", "2px");
            $("#DetailsViewEmployee tr td:odd")
                .css("text-align", "left")
                .css("padding-left", "2px");
            $("#GridViewEmployees tr:odd").addClass("oddRow");
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div id="page">
        <asp:GridView ID="GridViewEmployees" runat="server" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:HyperLinkField HeaderText="姓名" DataTextField="Name" DataNavigateUrlFields="Name,Id" DataNavigateUrlFormatString="~/employees/{0}/{1}"/>
                <asp:BoundField DataField="Gender" HeaderText="性别"/>
                <asp:BoundField DataField="BirthDate" HeaderText="出生日期" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Department" HeaderText="部门"/>
            </Columns>
        </asp:GridView>
        <asp:DetailsView ID="DetailsViewEmployee" runat="server" AutoGenerateRows="false" Width="100%">
            <Fields>
                <asp:BoundField DataField="ID" HeaderText="ID"/>
                <asp:BoundField DataField="Name" HeaderText="姓名"/>
                <asp:BoundField DataField="Gender" HeaderText="性别"/>
                <asp:BoundField DataField="BirthDate" HeaderText="出生日期" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Department" HeaderText="部门"/>
            </Fields>
        </asp:DetailsView>
    </div>
</form>
</body>
</html>