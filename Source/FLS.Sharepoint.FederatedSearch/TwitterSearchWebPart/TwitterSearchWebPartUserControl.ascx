<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TwitterSearchWebPartUserControl.ascx.cs" Inherits="FederatedSearch.TwitterSearchWebPart.TwitterSearchWebPartUserControl" %>
<SharePoint:CssRegistration ID="cssReg" runat="server" Name="/_layouts/FLS.Sharepoint.FederatedSearch/search.css" />

<asp:Timer ID="Timer1" runat="server" Interval="600000" />
<asp:UpdatePanel ID="AsynchPanel" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1">
        </asp:AsyncPostBackTrigger>
    </Triggers>
    <ContentTemplate>
        <div class="twitter">
            <div class="box-header">
                <asp:Image ID="ProfileImage" ImageUrl="/_layouts/FLS.Sharepoint.FederatedSearch/twitter.png" runat="server" />
                <div class="user-details">
                    <asp:HyperLink ID="Title" runat="server">title</asp:HyperLink>
                </div>
            </div>
    
            <asp:ListView ID="TwitterResults" runat="server" >
                <LayoutTemplate>
                    <ul class="twitter-item-list">
                        <asp:PlaceHolder runat="server" ID="ItemPlaceholder"></asp:PlaceHolder>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <div class="tweet-container">
                            <img alt="" src="<%#Eval("User.ProfileImageUrl") %>"/>
                            <div class="details">
                            <div class="tweet">
                                <p><%# Eval("Text") %></p>
                                <p><%# Eval("CreatedAt") %></p>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <asp:Label ID="Error" runat="server" Visible="false" Text="Label"></asp:Label>
        </div>
   </ContentTemplate>
</asp:UpdatePanel>