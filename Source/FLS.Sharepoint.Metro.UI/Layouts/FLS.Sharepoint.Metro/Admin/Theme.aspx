<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="/_controltemplates/InputFormSection.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="/_controltemplates/InputFormControl.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/ButtonSection.ascx" %> 
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Theme.aspx.cs" Inherits="FLS.Sharepoint.Metro.UI.Layouts.FLS.Sharepoint.Metro.Admin.Theme" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="ms-propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0">
<!-- Site Collection RSS -->
<wssuc:InputFormSection id="SiteMetroBranding" title="Metro UI Theme" runat="server">
	<Template_InputFormControls>
		<wssuc:InputFormControl runat="server">
			<Template_Control>
				<table border="0" width="100%" cellspacing="0" cellpadding="2">
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							<asp:DropDownList ID="SiteMetroBrandingDdb" title="Theme" runat="server" />
                            <br />
                            Currently Selected Theme : <asp:Label ID="ThemeLabel" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							<asp:CheckBox ID="InheritThemeFromParent" runat="server" Text="Inherit theme from Parent"/>
						</td>
					</tr>
				</table>
			</Template_Control>
		</wssuc:InputFormControl>
	</Template_InputFormControls>
</wssuc:InputFormSection>

<wssuc:InputFormSection
	id="SiteMetroBrandingAccordion"
	title="Side Nav Accordion"
	runat="server"
	>
	<Template_InputFormControls>
		<wssuc:InputFormControl runat="server">
			<Template_Control>
				<table border="0" width="100%" cellspacing="0" cellpadding="2">
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
                            <label id="AccordionStatusMessage" runat="server"></label>
							<asp:CheckBox ID="AccordionCheckbox" runat="server" Text="Activate Side Navigation Accordion on this site"/>
                            
						</td>
					</tr>
                    <tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							Accordion Type : <asp:DropDownList ID="AccordionTypeDropdown" title="Accordion Type Dropdown" runat="server" />                           
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							<asp:CheckBox ID="ApplyAccordionGloballyCB" runat="server" Text="Apply Accordion across all sites in Site Collection"/>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							<asp:DropDownList ID="QuickLaunchPosition" title="Quick Launch Position" runat="server" />
                            <br />
                            Quick Launch Position : <asp:Label ID="QuickLaunchPositionLabel" runat="server"></asp:Label>
						</td>
					</tr>
				</table>
			</Template_Control>
		</wssuc:InputFormControl>
	</Template_InputFormControls>
</wssuc:InputFormSection>

<wssuc:InputFormSection
	id="SiteMetroRibbonControl"
	title="Site Ribbon Settings"
	runat="server"
	>
	<Template_InputFormControls>
		<wssuc:InputFormControl runat="server">
			<Template_Control>
				<table border="0" width="100%" cellspacing="0" cellpadding="2">
					<tr>
						<td nowrap="nowrap" class="ms-authoringcontrols">
							<asp:CheckBox ID="FloatRibbonCB" runat="server" Text="Float the Ribbon over content"/>
						</td>
					</tr>
				</table>
			</Template_Control>
		</wssuc:InputFormControl>
	</Template_InputFormControls>
</wssuc:InputFormSection>
<wssuc:ButtonSection runat="server">
	<Template_Buttons>
		<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" id="BtnApply" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
	</Template_Buttons>
</wssuc:ButtonSection>
	</table>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="Metro UI Theme - Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<a href="settings.aspx"><SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,settings_pagetitle%>" EncodeMethod="HtmlEncode"/></a>&#32;&#32;&#32;<SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" /> <SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="Blog General Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="Use this page to manage your Metro UI theme settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
