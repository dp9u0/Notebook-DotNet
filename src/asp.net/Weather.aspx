<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Weather.aspx.cs" Inherits="asp.net.Weather" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Weather Forecast</title>
    <style type="text/css">
        body {
            font-size: .85em;
            font-family: "Trebuchet MS", Verdana, Helvetica, Sans-Serif;
            color: #232323;
            background-color: #fff;
        }

        li {
            list-style-type: none;
        }

        ul {
            padding: 0px;
            margin: 0px;
        }
    </style>
    <script src="/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("td:even")
                .css("text-align", "right")
                .css("vertical-align", "top")
                .css("font-weight", "bold");
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>Router:</td>
                    <td><%= RouteData.Route != null ? RouteData.Route.GetType().FullName : "" %></td>
                </tr>
                <tr>
                    <td>RouteHandler:</td>
                    <td><%= RouteData.RouteHandler != null ? RouteData.RouteHandler.GetType().FullName : "" %></td>
                </tr>
                <tr>
                    <td>Values:</td>
                    <td>
                        <ul>
                            <% foreach (var variable in RouteData.Values)
                                { %>
                            <li>
                                <%= variable.Key %>=<%= variable.Value %>
                            </li>
                            <% } %>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>DataTokens:</td>
                    <td>
                        <ul>
                            <% foreach (var variable in RouteData.DataTokens)
                                { %>
                            <li>
                                <%= variable.Key %>=<%= variable.Value %>
                            </li>
                            <% } %>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>VirtualPath1:</td>
                    <td>
                        <%= VirtualPath1%>
                    </td>
                </tr>
                <tr>
                    <td>VirtualPath2:</td>
                    <td>
                        <%= VirtualPath2%>
                    </td>
                </tr>
                <tr>
                    <td>VirtualPath3:</td>
                    <td>
                        <%= VirtualPath3%>
                    </td>
                </tr>
                <tr>
                    <td>VirtualPath4:</td>
                    <td>
                        <%= VirtualPath4%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
