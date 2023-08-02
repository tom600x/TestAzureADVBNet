<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="TestAzureADVBNet._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 <dl>


  <dt>IsAuthenticated</dt> 
     <dd><%= HttpContext.Current.User.Identity.IsAuthenticated %></dd>
     <dt>AuthenticationType</dt> 
     <dd><%= HttpContext.Current.User.Identity.AuthenticationType %></dd>
     <dt>Name</dt> <dd><%= HttpContext.Current.User.Identity.Name %></dd>
   
 
 </dl>

</asp:Content>
