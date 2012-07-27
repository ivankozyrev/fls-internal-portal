<%@ Assembly Name="UploadSolutionPackagePage, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=1409993b7d98c43d"%>
<%@ Page Language="C#" Inherits="Devosis.SharePoint.ApplicationPages.UploadSolutionPackagePage" MasterPageFile="~/_admin/admin.master"  %> 
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages.Administration, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<asp:Content contentplaceholderid="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Upload Solution Package" EncodeMethod='HtmlEncode'/>
</asp:content>
<asp:Content contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Upload Solution Package" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:content contentplaceholderid="PlaceHolderPageDescription" runat="server">
</asp:content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">	
<table width="100%" class="propertysheet" cellspacing="0" cellpadding="0" border="0"> <tr> <td class="ms-descriptionText"> 
 </td> </tr> <tr> <td class="ms-error"></td> </tr> <tr> <td class="ms-descriptionText"> 
 </td> </tr> <tr> <td><img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></td> </tr> </table>	
	<table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
		<colgroup>
			<col style="WIDTH: 30%"></col>
			<col style="WIDTH: 70%"></col>
		</colgroup>
		
		<wssuc:InputFormSection 
			Id="UploadSection"
			Title="Upload Solution Package" 
			Description="Browse to the solution package you intend to add to the server."
			runat="server">
			<Template_InputFormControls>
				<wssuc:InputFormControl LabelText="File Name:" runat="server">
					<Template_Control>
						<span dir="ltr">  
						<INPUT TYPE="file" NAME="TxtFileName" id="TextFileName"
							Title="File Name"
							runat="server" />
							<!-- onkeydown="return txtName_OnKeyDown();"  Size = "40"  -->
						</span>
					</Template_Control>
					

				</wssuc:InputFormControl>
			</Template_InputFormControls>
		</wssuc:InputFormSection>
		<!-- End File name section -->
					<asp:Label runat="server" ID="lblErrorMessage" Text="" />

<wssuc:ButtonSection runat="server">
	   <Template_Buttons>
		  <asp:Button  runat="server" class="ms-ButtonHeightWidth" Text="Upload" id="BtnOk"  OnClick="BtnOk_Clicked"  accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
	   </Template_Buttons>
 </wssuc:ButtonSection>
<!-- OnClick="BtnOk_Clicked" -->
	</table>
	<SharePoint:FormDigest runat=server/>
</asp:Content>
