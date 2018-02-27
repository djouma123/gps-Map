<%@ Page Language="C#" AutoEventWireup="true" CodeFile="visua.aspx.cs" Inherits="visua" MasterPageFile ="~/Page.master" %>
<%@ Register Src="~/GoogleMapForASPNet.ascx" TagName="GoogleMapForASPNet" TagPrefix="uc1"%>
 <%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>






    <asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <%--    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />--%>
            <uc1:GoogleMapForASPNet ID="GoogleMapForASPNet1" runat="server" ShowControls="true" />
        
    </asp:Content>
