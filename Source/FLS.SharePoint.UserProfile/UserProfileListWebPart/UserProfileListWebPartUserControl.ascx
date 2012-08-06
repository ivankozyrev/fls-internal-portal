<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileListWebPartUserControl.ascx.cs" Inherits="FLS.SharePoint.UserProfile.UserProfileListWebPart.UserProfileListWebPartUserControl" %>

<asp:Repeater ID="userProfileListRpt" runat="server">
  <HeaderTemplate>
    <table>
    <tr>
      <th>DisplayName</th>
      <th>Email</th>
      <th></th>
    </tr>
  </HeaderTemplate>
  <ItemTemplate>
    <tr>
      <td>
        <a href="<%#DataBinder.Eval(Container.DataItem, "PublicUrl")%>"><%#DataBinder.Eval(Container.DataItem, "DisplayName")%></a>
      </td>
      <td>
        <%#DataBinder.Eval(Container.DataItem, "Email")%>
      </td>
      <td>
         <a href="<%#DataBinder.Eval(Container.DataItem, "EditLink")%>">Edit</a>
      </td>
    </tr>
  </ItemTemplate>
  <FooterTemplate>
    </table>
  </FooterTemplate>
</asp:Repeater>