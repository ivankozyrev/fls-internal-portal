<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="EnterpriseSearchWebPartUserControl.ascx.cs" Inherits="FLS.SharePoint.EnterpriseSearchSite.EnterpriseSearchWebPart.EnterpriseSearchWebPartUserControl" %>

<span runat="server" ID="txtErrorMessage"></span>
<input type="text" runat="server" ID="txtQ"/>
<button type="submit" id="btnSearch" runat="server">search</button>

<SharePoint:SPGridView 
    runat="server" 
    ID="grdResults" 
    GridLines="None"
    CellPadding="4"
    AutoGenerateColumns="False"
    AllowPaging="True"
    PageSize="10"
    >
    <Columns>
        <asp:TemplateField HeaderText="Path" >
            <ItemTemplate>
                <asp:HyperLink runat="server" Text='<%# Eval("Title")%>' NavigateUrl='<%# Eval("Path") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# string.Format("{0:g}",((DateTime)Eval("Write")).ToLocalTime())%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</SharePoint:SPGridView>