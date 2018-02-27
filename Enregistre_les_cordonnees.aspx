<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Enregistre_les_cordonnees.aspx.cs" Inherits="Enregistre_les_cordonnees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <style type="text/css">
        #form1
       .ModalPopupBG
{
    background-color: #666699;
    filter: alpha(opacity=50);
    opacity: 0.7;
}

.HellowWorldPopup
{
    min-width:200px;
    min-height:150px;
    background:white;
}


    </style>
   
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
                   <div class="popup_Container">
        <div class="popup_Titlebar" id="PopupHeader">
            <div class="TitlebarLeft">
                Edit Expanse
            </div>
            <div class="TitlebarRight" onclick="cancel();">
            </div>
        </div>
        <div class="popup_Body">
            <%--The content will go here--%>
              <asp:Label ID="Label1" runat="server" Text="Label">Numero_Poste</asp:Label>
                                    <asp:TextBox ID="TextBox3" runat="server" Width="159px"></asp:TextBox>
                                    <br />

                                    <asp:Label ID="Label2" runat="server" Text="Label">Adresse</asp:Label>
                                    <asp:TextBox ID="TextBox4" runat="server" Width="180px"></asp:TextBox>
                                    <br />

                                    <asp:Label ID="Label3" runat="server" Text="Label">latitude</asp:Label>
                                    <asp:TextBox ID="TextBox5" runat="server" Width="159px"></asp:TextBox>
                                    <br />

                                    <asp:Label ID="Label4" runat="server" Text="Label">longitude</asp:Label>
                                    <asp:TextBox ID="TextBox6" runat="server" Width="159px"></asp:TextBox>
                                    <br />

                                    <asp:Label ID="Label5" runat="server" Text="Label">Nom</asp:Label>
                                    <asp:TextBox ID="TextBox7" runat="server" Width="159px"></asp:TextBox>
                                    <br />
                                </div>

                                <div class="modal-footer">
                              <%--      <asp:Button ID="Button2" runat="server" Text="Save" OnClick="Button2_Click" CssClass="btn btn-primary " UseSubmitBehavior="false" />--%>

                                    <asp:LinkButton ID="link" runat="server"></asp:LinkButton>
                                </div>


        </div>
        <div class="popup_Buttons">
            <input id="btnOkay" type="button" value="Done" runat="server" />
            <input id="btnCancel" onclick="cancel();" type="button" value="Cancel" />
        </div>
    </div>

    </div>
    </form>
</body>
</html>
