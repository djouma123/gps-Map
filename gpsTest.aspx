<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gpsTest.aspx.cs" Inherits="gpsTest"  MasterPageFile ="~/Page.master"  Async="true" AsyncTimeout="20"  %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/GoogleMapForASPNet.ascx" TagName="GoogleMapForASPNet" TagPrefix="uc1"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


<%@ Register src="controls/TreeUserControl.ascx" tagname="TreeUserControl" tagprefix="uc2" %>

    <asp:Content ID ="content" ContentPlaceHolderID ="ContentPlaceHolder1" runat ="server"  >


    <link rel="stylesheet" href="dist/themes/default/style.min.css" />
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="DragDropListStyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
        <link rel="stylesheet" href="/resources/demos/style.css" />
  

        <script src="Scripts/jquery-1.11.1.min.js"></script>
<link href="Content/bootstrap.min.css" rel="stylesheet" />
<script src="Content/bootstrap.min.js"></script>
  
    <style type="text/css">
        #form1
        {
            width: 750px;
        } 
          .modalBackground
        {
            background-color: black ;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }


        .auto-style1 {
            height: 24px;
        }


    </style>
   
     

      
         <asp:ScriptManager ID="ScriptManager1" runat="server">
<%--              <Scripts>
                 <asp:ScriptReference Name="PreviewScript.js" Assembly="Microsoft.Web.Preview" />
                 <asp:ScriptReference Name="PreviewDragDrop.js" Assembly="Microsoft.Web.Preview" />
                 <asp:ScriptReference Path="~/Scripts/custDragDrop.js" />
             </Scripts>--%>
         </asp:ScriptManager> 

        <asp:HiddenField ID="HiddenField1" runat="server" />

  
        <div class="" id ="po"">
            <div class="row" style="margin-right: 15px; margin-left: 15px;"
               oncontextmenu="return false;">
                <div class="col-lg-3 col-md-3" >

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                            <div class="panel panel-success">
                                <div class="panel-heading">
                                    <fieldset><legend>Boite a Outils </legend></fieldset>
                                </div>

                                <div class="panel-body">

                                    <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="MyTreeView_SelectedNodeChanged" >
                                        <SelectedNodeStyle ForeColor="Black" />
                                        <HoverNodeStyle Font-Underline="true" ForeColor="#DD5555" />
                                        <ParentNodeStyle Font-Bold="false" />
                                        <%-- <NodeStyle Font-Names="verdana" Font-Size="8pt" ForeColor="Black"
                                    HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />--%>
                                    </asp:TreeView>

                                </div>
                            </div>

                            <asp:TextBox ID="TextBox2" runat="server" Visible=" false "></asp:TextBox>
                            <asp:TextBox ID="TextBox1" runat="server" Visible ="false" ></asp:TextBox>
                            <asp:TextBox ID="TextBox8" runat="server" Visible="false"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>



                <div class=" col-lg-9 col-md-9  " id="map" style="position: relative;" onclick="reply_click(this.id)" >
                    <asp:HiddenField ID="HiddenField2" runat="server" />

                    <uc1:GoogleMapForASPNet ID="GoogleMapForASPNet1" runat="server" ShowControls="true" ViewStateMode="Enabled"  />

                </div>

            </div>
      
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
             
                    <div id="modal" style="visibility: hidden;">

                        <div>
                            Voulez vous vrement suprimer le marqueur?
                        </div>
                    </div>
                   
                    <%-- <asp:Button ID="Button2" runat="server" Text="oui " OnClick="Button2_Click1" style="visibility: hidden;"   />--%>
                    <asp:Button ID="Button3" runat="server" Text="non"  style="visibility: hidden;"  />

             <%--       </asp:Panel>--%>
                   


                </ContentTemplate>
            </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true" >

                        <ContentTemplate>
<%-- CancelControlID="btnAnnuler"--%>
                            <cc1:ModalPopupExtender ID="mp1" runat="server" TargetControlID="link" PopupControlID="Panel1" DropShadow="true" BackgroundCssClass="modalBackground" PopupDragHandleControlID="PopupHeader" Drag="true" CancelControlID="btnAnnuler" />


                            <asp:Panel ID="Panel1" runat="server" BorderStyle="None" style="visibility: hidden;"   >
                               <%-- <div class="modalPopup " id="Panel1 ">
                                <!-- Bootstrap Modal Dialog -->--%>

                                <div id="jet" class="modal-dialog ">
                                    <div class="modal-content">

                                        <div class="modal-header ">
                                            Info sur :
                                            <asp:Label ID="Lab_Tooltip" runat="server" Font-Size="Larger" Font-Italic="True" ForeColor="#FF33CC" Font-Bold="True"></asp:Label>
                                            <asp:Image ID="Image1" runat="server" Style="margin-left: 310px;" Height="30px" />
                                        </div>


                                        <div class="modal-body ">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lab_reference" runat="server" Text="Reference : " Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_reference" runat="server" Width="180px" Height="20px"  MaxLength ="6"></asp:TextBox> <asp:Label ID="txt_error" runat="server" Text="la reference existe deja !!" Visible ="false"  ></asp:Label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="saisissez une reference" ControlToValidate="Txt_reference" SetFocusOnError="True" ValidationGroup="so" />
                                                        
                                                    </td>


                                                </tr>



                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lab_latitude" runat="server" Text="latitude : " Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Latitude" runat="server" Width="180px" Height="20px"></asp:TextBox>
                                                    </td>




                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lab_longitude" runat="server" Text="longitude : " Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_longitude" runat="server" Width="180px" Height="20px"></asp:TextBox>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="Nom :" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_nom" runat="server" Width="180px" Height="20px" Visible="false"></asp:TextBox>
                                                    </td>



                                                </tr>
                                            </table>
                                        </div>

                                        <div class="modal-footer">
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="btn btn-primary " UseSubmitBehavior="false" CausesValidation="true" ValidationGroup="so" />
                                            <asp:Button ID="btnSupprimer" runat="server" Text="Suprrimer" CssClass="btn btn-primary " UseSubmitBehavior="false" OnClick="btnSupprimer_Click" />
                                            <asp:Button ID="BtnAnnuler" runat="server" Text="Annuler" CssClass="btn btn-primary " UseSubmitBehavior="false" OnClick="BtnAnnuler_Click" CausesValidation="false" ValidationGroup="so" />
                                            <asp:LinkButton ID="link" runat="server"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div> 
                            </asp:Panel>

                            </div>
                            </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>


 
   
         
  </div> 
        <script src="”Scripts/jquery-1.4.1.min.js”" type="”text/javascript”"></script>
        <link href="”http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/ui-lightness/jquery-ui.css”" rel="”stylesheet”" />
        <link href="”http://dotnettricks-abdul.in/Contents/keyboard.css”" rel="”stylesheet”" type="”text/css”" />
        <script src="”http://dotnettricks-abdul.in/Contents/jquery.keyboard.extension-typing.js”" type="”text/javascript”"></script>
        <script src="”http://dotnettricks-abdul.in/Contents/jquery.keyboard.js”" type="”text/javascript”"></script>

        <script type="text/javascript" src="http://www.google.com/jsapi"></script>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js"></script>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.12/jquery-ui.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js" type="text/javascript"></script>
        <script src="scripts/dist/jstree.min.js" type="text/javascript"></script>
        <script src="scripts/dist/jstree.js" type="text/javascript"></script>
        <script type="text/javascript" src="https://code.jquery.com/jquery.min.js"></script>
        <script type="text/javascript" src="https://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>  
        <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
        <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

        <script type="text/javascript" src="vendors/jquery-1.9.1.min.js"></script>
        <script type="text/javascript" src="bootstrap/js/bootstrap.min.js"></script>
        <script type="text/javascript" src="vendors/easypiechart/jquery.easy-pie-chart.js"></script>
        <script type="text/javascript" src="assets/scripts.js"></script>
        <script type="text/javascript">





            document.onkeydown = testKeyEvent;
            document.onkeypress = testKeyEvent;
            document.onkeyup = testKeyEvent;

            //document.oncontextmenu = RightMouse;
            document.onmousedown = testEvent;

           

     


                function testKeyEvent(e) {


                    if (e.keyCode == 13) //We are using Enter key press event for test purpose.
                    {
                      <%--  var fr = document.getElementById('<%=HiddenField2.ClientID %>').value;
                        if (fr != " ") {
                            document.getElementById('modal').style.visibility = 'visible';
                            $(document).ready(function () {
                           
                                $("#modal").dialog({
                                    height: 200,
                                    width: 400,
                                    show: { effect: 'drop', direction: "up" },
                                    //modal: true,

                                    buttons: {
                                        Ok: function () {
                                            $("[id*=Button2]").click();

                                        },
                                        Close: function () {
                                            $(this).dialog('close');
                                        }
                                    }
                                });

                            });

                        }
                        else
                        {
                            alert(" rien ne clike");
                        }--%>
                  
                    document.getElementById('<%=BtnSave.ClientID%>').click();
                        document.getElementById('<%=Txt_reference.ClientID%>').value = "";

                        
                    }

                }

                function fr()
                {
                    alert("saisire la reference");
                }



                function doClick(buttonName, e) {
                    //the purpose of this function is to allow the enter key to 
                    //point to the correct button to click.
                    var key;

                    if (window.event)
                        key = window.event.keyCode;     //IE
                    else
                        key = e.which;     //firefox

                    if (key == 13) {
                        //Get the button the user wants to have clicked
                        var btn = document.getElementById(buttonName);
                        if (btn != null) { //If we find the button click it
                            btn.click();
                            event.keyCode = 0
                        }
                    }
                }


                //    $("#TreeView1").draggable({

                //        helper: "clone",
                //        //revert: "invalid"

                //    });
                //    $("#GoogleMapForASPNet1").droppable({
                //        drop: function (event, ui) {
                //            //$(this).append(ui.helper.clone().removeClass("ui-draggable-dragging").draggable({ helper: 'original' }));
                //            event.preventDefault();
                //            var data = event.originalEvent.dataTransfer.getData("Text");
                //            $(this).append(data);
                //        }
                //    });


                //});--%>


                <%-- var treeViewData = window["<%=TreeView1.ClientID%>" + "_Data"];
         if (treeViewData.selectedNodeID.value != "") {
             console.log("  selected.")
             // console.log('log')
             var selectedNode = document.getElementById(treeViewData.selectedNodeID.value).draggable = "true"--%>



                <%--    function setEvents() {
             var lstProducts = window["<%=TreeView1.ClientID%>" + "_Data"];
             var selectnode = lstProducts.selectedNodeID.value;
             //Set Drag on Each 'li' in the list 
             $.each(selectnode, function (idx, val) {
                 $('TreeView1').on('dragstart', function (evt) {
                     evt.originalEvent.dataTransfer.setData("Text", evt.target.textContent);
                     evt.target.draggable = false;

                 });
             });

             //Set the Drop on the <div>
             $("#dvright").on('drop', function (evt) {
                 evt.preventDefault();
                 var data = evt.originalEvent.dataTransfer.getData("Text");
                 var lst = $("#lstselectedproducts");
                 var li = "<li>" + data + "</li>";
                 li.textContent = data;
                 lst.append(li);
             });

             //The dragover
             $("#dvright").on('dragover', function (evt) {
                 evt.preventDefault();
             });
         }--%>



                <%--            //function OnTreeClick(evt) {

            //    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            //    //var nodeClick = src.tagName.toLowerCase() == "a";
            //    //if (nodeClick) {
            //    $(src).draggable({

            //        helper: "clone",
            //        revert: "invalid"

            //    });
            //    //var nodeText = src.innerText || src.innerHTML;

            //    //alert("Text: " + nodeText +"");
            //    //}
            //    $("#GoogleMapForASPNet1").droppable({
            //        drop: function (event, ui) {
            //            $(this).append(ui.helper.clone().removeClass("ui-draggable-dragging").draggable({ helper: 'original' }));
            //            alert('drop it suscefully');
            //        }
            //    });
            //    return false;


            //}

            //function drop(ev) {
            //    ev.preventDefault();
            //    var data = ev.dataTransfer.getData("text");
            //    ev.target.appendChild(document.getElementById(data));
            //}--%>


                <%--         function GetSelectedNode() {

             var treeViewData = window["<%=TreeView1.ClientID%>" + "_Data"];

                 if (treeViewData.selectedNodeID.value != "") {

                     var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);

                     selectedNode.draggable({




                     });

                     //var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                     //var text = selectedNode.innerHTML;
                     //alert("Text: " + text + "\r\n" + "Value: " + value);



                 } else {
                     alert("No node selected.")

                 }
                 return false;

                        }
--%>

                <%--            function RightClick(event) {



                alert(event.innerHTML); //This will prompt selected Node Text
            }



            function returnA(lon, lat) {
                var hidden_lat = '<%= HiddenField_Lat.ClientID %>';
                 var hidden_long = '<%= HiddenField_Long.ClientID %>';
                 document.getElementById(hidden_lat).value = lat;
                 document.getElementById(hidden_long).value = lon;


             }--%>



                function initMap(lat, lng) {
                    var map = new google.maps.Map(document.getElementById('GoogleMapForASPNet1'));

                    var flightPlanCoordinates = [];
                    flightPlanCoordinates.push(lat, lng);

                    if (document.getElementById(HiddenField1).value == "2") {
                        var flightPath = new google.maps.Polyline({
                            path: flightPlanCoordinates,
                            geodesic: true,
                            strokeColor: '#FF0000',
                            strokeOpacity: 1.0,
                            strokeWeight: 2
                        });

                        flightPath.setMap(map);
                    }
                }


                //function reply_click(clicked_id) {
                //    alert(clicked_id);
                //}


                function right_click() {
                    if (document.layers) {
                        //Capture the MouseDown event.
                        document.captureEvents(Event.MOUSEDOWN);

                        //Disable the OnMouseDown event handler.
                        //document.onmousedown = function () {
                        //    return true;
                        //};
                    }
                    else {
                        //Disable the OnMouseUp event handler.
                        document.onmouseup = function (e) {
                            if (e != null && e.type == "mouseup") {
                                //Check the Mouse Button which is clicked.
                                if (e.which == 2 || e.which == 3) {
                                    //If the Button is middle or right then disable.
                                    alert("salam")
                                    // return false;
                                }
                            }
                        };
                    }

                    //Disable the Context Menu event.
                    document.oncontextmenu = function () {
                        return true;
                    };
                }
            </script>  

                     
 </asp:Content>

