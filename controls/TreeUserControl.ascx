<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeUserControl.ascx.cs" Inherits="TreeUserControl" %>
<%--<%@ Register Src="~/GoogleMapForASPNet.ascx" TagName="GoogleMapForASPNet" TagPrefix="uc1"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>--%>


<%--<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
--%>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate >
<asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged =" MyTreeView_SelectedNodeChanged"  >
    </asp:TreeView>
<asp:label runat="server" ID="label1"> </asp:label>
</ContentTemplate>

</asp:UpdatePanel>

    

