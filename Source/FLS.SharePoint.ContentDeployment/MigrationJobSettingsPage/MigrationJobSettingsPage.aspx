<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<%@ Page Language="C#" 
    AutoEventWireup="false" 
    CodeBehind="MigrationJobSettingsPage.aspx.cs"
    Inherits="FLS.SharePoint.ContentDeployment.MigrationJobSettingsPage" 
    MasterPageFile="~/_admin/admin.master" %>
<%@ Register TagPrefix="wssuc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="inputControls" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td width="50%">
                    <!-- **************************************
                     use sharepoint buttonsection control
                     to display the "ok" and "cancel" buttons -->
                    <wssuc:buttonsection runat="server" topbuttons="true" bottomspacing="5" showsectionline="false"
                        showstandardcancelbutton="false">
                        <Template_Buttons>
                            <asp:Button UseSubmitBehavior="false" runat="server"
                                class="ms-ButtonHeightWidth"
                                Text="<%$Resources:wss,multipages_okbutton_text%>"
                                id="BtnOk" ToolTip="Save current settings and run migration job"
                                accesskey="<%$Resources:wss,okbutton_accesskey%>"
                                Enabled="true"/>
                            <asp:Button UseSubmitBehavior="false" runat="server"
                                class="ms-ButtonHeightWidth"
                                Text="<%$Resources:wss,multipages_cancelbutton_text%>"
                                CausesValidation="False"
                                id="BtnCancel"
                                CommandName="Cancel"
                                accesskey="<%$Resources:wss,multipages_cancelbutton_accesskey%>"
                                Enabled="true"/>
                        </Template_Buttons>
                    </wssuc:buttonsection>
                    <!-- ************************************** -->
                    <!-- **************************************
                     display the web application selector
                     using the inputformsecton and
                     webapplicationselector controls -->
                    <wssuc:inputformsection runat="server" title="Web Application" description="Select a Web Application">
                    <Template_InputFormControls>
                    <tr>
                        <td>
                        <SharePoint:WebApplicationSelector id="WebAppSelector" runat="server"
						  			    TypeLabelCssClass="ms-listheaderlabel"
						  			    HoverCellActiveCssClass = "ms-viewselectorhover"
									    HoverCellInActiveCssClass = "ms-viewselector" /><br />
							    <span style="padding-left:6px;"><asp:Literal ID="litWebAppName" runat="server" /></span>
                        </td>
                    </tr>
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                    <wssuc:inputformsection runat="server" title="Source Site Collection" description="Select source site collection">
                    <Template_InputFormControls>
                    <tr>
                        <td>
                            <wssuc:InputFormControl runat="server">
				                <Template_Control>
                                    <wssuc:ExpandableDropDownList runat="server" ID="SourceSiteCollectionSelector" Width="200px"/>
                                </Template_Control>
                            </wssuc:InputFormControl>
                           
						</td>
                    </tr>
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                    <wssuc:inputformsection runat="server" title="Destination Site Collection" description="Select destination site collection">
                    <Template_InputFormControls>
                    <tr>
                        <td>
                            <wssuc:InputFormControl runat="server">
				                <Template_Control>
                                    <wssuc:ExpandableDropDownList runat="server" ID="DestinationSiteCollectionSelector" Width="200px"/>
                                </Template_Control>
                            </wssuc:InputFormControl>
						</td>
                    </tr>
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                     <wssuc:inputformsection runat="server" title="Common Migration Options" description="Select common migration options">
                    <Template_InputFormControls>
                     <tr>
                        <td>
                                                     
                      <wssuc:InputFormControl runat="server" LabelText="Migration File Location:">
				            <Template_Control> 
                                <wssawc:InputFormTextBox Width="200px"  class="ms-input" ID="FileLocation"  Runat="server"  EnableViewState="true" />                                
                            </Template_Control>
                        </wssuc:InputFormControl>
                        <wssuc:InputFormControl runat="server" LabelText="Migration File Name:">
				            <Template_Control> 
                                <wssawc:InputFormTextBox  class="ms-input" ID="FileName"  Runat="server"  EnableViewState="true" />                                
                            </Template_Control>
                        </wssuc:InputFormControl>
                        
                        </td>
                    </tr>                  
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                    <wssuc:inputformsection runat="server" title="Export Options" description="Select export options">
                    <Template_InputFormControls>
                     <tr>
                        <td>
                        <wssuc:InputFormControl runat="server" LabelText="Export objects:">
				            <Template_Control>
                                <wssuc:ExpandableDropDownList runat="server" ID="ExportObjects" Width="150px"/>
                            </Template_Control>
                        </wssuc:InputFormControl>
                        <wssuc:InputFormControl runat="server" LabelText="Export Type:">
				            <Template_Control>
                                <wssuc:ExpandableDropDownList runat="server" ID="ExportTypes" Width="150px"/>
                            </Template_Control>
                        </wssuc:InputFormControl>   
                        </td>
                    </tr>                  
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                    <wssuc:inputformsection runat="server" title="Import Options" description="Select import options">
                    <Template_InputFormControls>
                     <tr>
                        <td>
                        
                        </td>
                    </tr>                  
                    </Template_InputFormControls>
                    </wssuc:inputformsection>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- Literal Control to display messages -->
    <div style="font-size: 12pt; color: red; font-weight: bold;">
        <asp:Literal ID="litMessages" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Migration Content Job Settings Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Migration Content Job Settings Page
</asp:Content>

<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>

