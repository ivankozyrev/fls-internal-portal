<%@ Page Language="C#" MasterPageFile="~/_Layouts/ListViewArea/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Microsoft.SharePoint.SPListCollection>" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All lists in current web scope:</h2>
    <ul>
    <% foreach (SPList list in Model)
       { %>
         <li>
             <%= list.Title %> | <%= list.Author %>
         </li>   
    <% } %>
    </ul>
</asp:Content>
