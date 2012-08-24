<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditJobCustomLayout.aspx.cs" Inherits="FLS.SharePoint.ContentDeployment.EditJobCustomLayout" MasterPageFile="~masterurl/default.master"  %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
 It is head
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div style="height: 80px;margin: 10px">
 <div style="height: 40px;margin-left: 5px;"> 
    Source Site :   <input type="text" value="http://" style="width: 20%;margin-left: 35px" runat="server" id="sourceSite" />
 </div>
  <div style="margin-left: 5px;height:40px;">
    Destination Site: <input type="text" id="destinationSite" value="http://"  style="width: 20%;margin-left: 15px" runat="server" />
 </div>
 </div>
 <div style="margin:5px;margin: 10px">
    <input value="Run" runat="server" ID="btnRun" type="button"    />
    <input value="Ok" runat="server" ID="btnSave" type="button"  />
    <input value="Cancel" runat="server" ID="btnCancel" type="button"  />
 </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
