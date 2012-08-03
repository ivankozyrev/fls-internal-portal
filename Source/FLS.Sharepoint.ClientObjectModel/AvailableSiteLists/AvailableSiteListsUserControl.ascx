<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailableSiteListsUserControl.ascx.cs" Inherits="FLS.Sharepoint.ClientObjectModel.AvailableSiteLists.AvailableSiteListsUserControl" %>
<%@ Import Namespace="Microsoft.SharePoint.Client" %>

<div>
  <table>
   <tr>
     <td>
        <asp:TreeView ID="siteCollectionTree" OnSelectedNodeChanged="SelectedNodeChanged" runat="server"></asp:TreeView>
     </td>
   </tr>
   <tr>
     <td>
       <asp:Label ID="SiteNameLabel" runat="server"/>
     </td>
   </tr>
   <tr>
     <td>
       <asp:Repeater ID="AvailableListsRpt" runat="server">
      <HeaderTemplate>
        <table>
          <tr>
              <th>List Name</th>
              <th>Item count</th>
          </tr>
      </HeaderTemplate>
      <ItemTemplate>
        <tr>
          <td>
            <a href="<%#DataBinder.Eval(Container.DataItem, "DefaultViewUrl")%>"><%#DataBinder.Eval(Container.DataItem, "Title")%></a>
          </td>
          <td><%#DataBinder.Eval(Container.DataItem, "ItemCount")%></td>
        </tr>
      </ItemTemplate>
      <FooterTemplate>
        </table>
      </FooterTemplate>
    </asp:Repeater>
     </td>
   </tr>
  </table>
</div>