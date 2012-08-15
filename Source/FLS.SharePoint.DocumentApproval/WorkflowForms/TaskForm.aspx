<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskForm.aspx.cs" Inherits="DocumentApproval.WorkflowForms.TaskForm" MasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <div style="height: 200px">
 <div style="height: 40px;margin-left: 5px;"> 
    Status :   <input type="text" disabled="disabled" style="width: 30%;margin-left: 35px" runat="server" id="taskStatus" />
 </div>
  <div style="margin-left: 5px;height:40px;">
    Requested by: <input type="text" id="taskReqested" disabled="True" style="width: 80%;" runat="server" />
 </div>
  <div style="margin-left: 5px;height:40px;">
    Due Date: <input type="text" id="taskDueDate" disabled="True" style="width: 80%;margin-left: 25px" runat="server" />
 </div>
 <div style="margin-left: 5px;height:40px;">
     <div style="height: 40px;width: 80px; float: left">Consolidated comment:</div>
     <textarea id="taskComment" name="taskComment" rows="3" cols="1" style=";width: 80%;margin-left:10px; height: 40px;word-wrap: normal" runat="server" >
    </textarea>
 </div>
 </div>
 <div style="margin:5px">
    <asp:Button Text="On Advisement" runat="server" ID="btnOnAdvisement"  Visible="False" UseSubmitBehavior="False" />
    <asp:Button Text="Approve" runat="server" ID="btnApprove" Visible="False" UseSubmitBehavior="False" />
    <asp:Button Text="Reject" runat="server" ID="btnReject" Visible="False" UseSubmitBehavior="False" />
    <asp:Button Text="Cancel" runat="server" ID="btnCancel" UseSubmitBehavior="False" />
 </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
