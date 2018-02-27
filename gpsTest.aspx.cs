using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;






public partial class gpsTest : System.Web.UI.Page
{
   static  GooglePoint centre  = new GooglePoint();
    Boolean bo = false;
    XmlDocument xmlDoc = new XmlDocument();
     static int i=0 ;
     static List<GooglePoint> listP=new List<GooglePoint>();
     static List<GooglePoint> group = new List<GooglePoint>();
    GestionData ges = new GestionData();
    DataSet ds = new DataSet();
    string nom;
   static  int incremn = 0;
   int  numero = 0;
   static string toolti ;
   static string id_mark ;




    
    protected void Page_Load(object sender, EventArgs e)
    {


        GoogleMapForASPNet1.PushpinClicked += new GoogleMapForASPNet.PushpinClickedHandler(OnPushpinClicked);
        GoogleMapForASPNet1.PushpinDrag += new GoogleMapForASPNet.PushpinDragHandler(OnPushpinDrag);
        GoogleMapForASPNet1.MapClicked += new GoogleMapForASPNet.MapClickedHandler(OnMapClicked);

  
        if (!IsPostBack)
        {



            ScriptManager.RegisterStartupScript(Page, this.GetType(), "key", "testKeyEvent();", true);
      
            GoogleMapForASPNet1.GoogleMapObject.APIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];

            //Specify width and height for map. You can specify either in pixels or in percentage relative to it's container.
            GoogleMapForASPNet1.GoogleMapObject.Width = "100%"; // You can also specify percentage(e.g. 80%) here
            GoogleMapForASPNet1.GoogleMapObject.Height = "1000px";

            //Specify initial Zoom level.
              GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 14;

            //Specify Center Point for map. Map will be centered on this point.
               GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1",11.5867151352197, 43.1480500710693);



               ScriptManager.RegisterStartupScript(Page, Page.GetType(), "unIdenti", "right_click", true);


               DataSet Tree = new DataSet();
               this.TreeView1.Nodes.Clear();
               try
               {
                   Tree = ges.ReturnData("select * from objet", "objet");
               }
               catch (Exception ex) {

                   Debug.Write(ex);
               }

               chargerTree(Tree, 0, null);

           

           
        }
        

    }

   

    // ***** charger les donnees de la base pour represente sur la carte *****//

    void chargeData(DataSet ds)
    {
        GoogleMapForASPNet1.GoogleMapObject.Polylines.Clear();
        GoogleMapForASPNet1.GoogleMapObject.Points.Clear();

        string[] TobeDistinct = { "name", "latitude", "longitude" };
        DataTable dtDistinct = GetDistinctRecords(ds.Tables[0], TobeDistinct);
        DataSet ds2 = new System.Data.DataSet();
        ds2.Tables.Add(dtDistinct);

        if (ds2.Tables[0].Rows.Count == 0)
        {
            //this.Label1.Text = "Aucune données trouvée.";
            return;
        }
        CultureInfo culture = new CultureInfo("en");
        Double lat_fin, lng_fin, lat_debu, lon_debu;

        foreach (DataRow r in ds.Tables[0].Rows)
        {
            if (r["name"].ToString() != String.Empty)
            {
                if (r["name"].ToString() != "line")
                {
                    String la = r["latitude"].ToString();
                    string[] words = la.Split('-');
                    GooglePoint POSTE = new GooglePoint();

                    lat_debu = Double.Parse(words[0].ToString().Replace(",", "."), culture);
                    lat_fin = Double.Parse(words[1].ToString().Replace(",", "."), culture);

                    POSTE.ID = r["name"].ToString();
                    POSTE.Latitude = lat_debu;
                    POSTE.Longitude = lat_fin;
                    POSTE.InfoHTML = "POSTE : " + r["name"].ToString();
                    //POSTE.IconImage = "icons/home.png";
                    POSTE.IconImage = r["url_image"].ToString();
                    POSTE.Draggable = true;
                    POSTE.ToolTip = r["name"].ToString();
               
                    GoogleMapForASPNet1.GoogleMapObject.Points.Add(POSTE);


                }
                else if (r["name"].ToString() == "line")
                {
                    String la = r["latitude"].ToString();
                    String lo = r["longitude"].ToString();

                    string[] lati = la.Split('-');
                    string[] lon = lo.Split('-');

                    GooglePolyline POSTE = new GooglePolyline();
                    GooglePoint debu = new GooglePoint();
                    GooglePoint fin = new GooglePoint();

                    lat_debu = Double.Parse(lati[0].ToString().Replace(",", "."), culture);
                    lon_debu = Double.Parse(lati[1].ToString().Replace(",", "."), culture);

                    debu.Latitude = lat_debu;
                    debu.Longitude = lon_debu;

                    lat_fin = Double.Parse(lon[0].ToString().Replace(",", "."), culture);
                    lng_fin = Double.Parse(lon[1].ToString().Replace(",", "."), culture);

                    fin.Latitude = lat_fin;
                    fin.Longitude = lng_fin;

                    POSTE.ID = r["name"].ToString();
                    POSTE.Points.Add(debu);
                    POSTE.Points.Add(fin);
                    POSTE.LineStatus = "POSTE : " + r["name"].ToString();
                    POSTE.ColorCode = "blue";
                    POSTE.Width = 1;
                    GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(POSTE);
                    
                }
            }

        }

    }

    //****** le treenode chossie*******//



    protected void MyTreeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        string nom1;
        int num;

        TextBox1.Text = TreeView1.SelectedNode.Text;
        DataSet ds;
        ds = ges.ReturnData("select * from objet", "objet");
        GooglePoint GP2 = new GooglePoint();
        foreach (DataRow r in ds.Tables[0].Rows)
        {

            if (r["nom_objet"].ToString() == TreeView1.SelectedNode.Text.ToString() && TreeView1.SelectedNode.Text.ToString() != "line")
            {
                    nom1 = Num_gene("numero_increm");
                 

                if( nom1 == null )
                {

                 num = 1;

                }

                else
                {
                   num = Convert.ToInt32(nom1) + 1;
                }

                incremn = incremn + 1;
    
                GP2.ID = r["nom_objet"] + num.ToString();
               

                if (centre.Longitude ==0 && centre.Latitude == 0)
                {
                    GP2.Latitude = 11.5867151352197;
                    GP2.Longitude =  43.1480500710693;

                }

                else 
                { 
                          GP2.Latitude = centre.Latitude;
                          GP2.Longitude = centre.Longitude;

                   }
                GP2.Draggable = true;
                GP2.IconImage = r["url_image"].ToString();
                //GP2.InfoHTML = inser_num();
         
               // GP2.ToolTip = num.ToString();
                GP2.ToolTip = r["nom_objet"] + num.ToString();
                toolti = GP2.ToolTip.ToString();
                

                ges.DataUpdate("insert into numero_increm (numero) values (" + num + ")");

                GoogleMapForASPNet1.GoogleMapObject.Points.Add(GP2);
 
            }

            



        }
        if (TreeView1.SelectedNode.Selected == true) {

                TreeView1.SelectedNode.Selected = false;
            }
    }
        


    public string  inser_num() 
    { 
  string html = "<form id='form1' runat='server'> ";
    
  // html+=" <div> <label for='nom'>Nom :</label>";
  // html+=" <input type='text' id='nom' />  </div>";
  // html+=" <div> <label for='courriel'>Courriel :</label>";
  // html+="   <input type='email' id='courriel' / > </div>";
  // html+=" <div> <label for='message'>Message :</label>";
  // html+="  <textarea id='message'></textarea>  </div>";


  // html += " <div class='button'>"; 
  //    //<button id='mybutton' runat='server' onserverclick ='foo_OnClick'> envoyer <button />";
  // html += " <input id='Button1' type='button' value='button'  runat ='server' onserverclick='btnBeforeOk_ServerClick' />";
  //html += " </div></form>";

  return html;

    
    }

    protected void btnBeforeOk_ServerClick(object Source, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Identifiant", "alert('salam')", true);
    }


    protected void foo_OnClick_Click(object sender, EventArgs e)
{
    GooglePoint GP2 = new GooglePoint();
    GP2.ID = "salam";
    GP2.Latitude = 11.5867151352197;
    GP2.Longitude = 43.1480500710693;
          GoogleMapForASPNet1.GoogleMapObject.Points.Add(GP2);
}


    public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
    {
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
        return dtUniqRecords;
    }






//**************function de deplacemnt d'un marqueur*************

   public  void OnPushpinDrag(string pID)
    {
        id_mark = pID.ToString();
        Txt_reference.Focus();
      

       // Lab_Tooltip.Text = pID.ToString().Substring (3,);

        //if (pID.ToString().Contains("poste"))
       if(GoogleMapForASPNet1.GoogleMapObject.Points [pID].ToolTip.ToString().Contains ("poste"))
        {

            //if (Lab_Tooltip.Text != GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString())

            //    {
              
            //        //   string nom = Num_gene("poste").Substring(3);
            //        //int genere = Convert.ToInt32(nom) + 1;
            //        mp1.Show();
            //        Panel1.Style.Add("visibility", "visible");

            //        Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
            //        Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");


            //        Txt_nom.Text = pID.ToString();
            //         toolti = pID.ToString();

            //        //GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip =pID.ToString ();
            //        TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
            //        Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();

                  
            //    } 
            DataSet ds = new DataSet();
           ds = ges.ReturnData("select * from poste2 where numero='"+pID.ToString()+"'", "poste2");
          //  foreach (DataRow r in ds.Tables[0].Rows)
           // {
                //if (pID.ToString() == r["numero"].ToString())
                if(ds.Tables[0].Rows.Count !=0)
                {



                    //int numero = Convert.ToInt32(GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString());
                    ges.DataUpdate("update poste set latitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".") + "', longitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".") + "'where numero ='" + pID.ToString () + "'");

                }
                else
                {

                    //   string nom = Num_gene("poste").Substring(3);
                    //int genere = Convert.ToInt32(nom) + 1;
                    mp1.Show();
                    Panel1.Style.Add("visibility", "visible");

                    Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
                    Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");


                    Txt_nom.Text = pID.ToString();
                    toolti = pID.ToString();

                    //GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip =pID.ToString ();
                    TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();

                    Lab_Tooltip.Text = pID.ToString().Replace("_", " ").Substring(0, pID.Length - 3);
                }
               
            }
        
                    
        else if(GoogleMapForASPNet1.GoogleMapObject.Points [pID].ToolTip.ToString().Contains ("compteure"))
        
        {

        /*    if (Lab_Tooltip.Text != GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString())
                {

                //string nom = Num_gene("compteure").Substring(3);
                //    int genere = Convert.ToInt32(nom) + 1;
                    mp1.Show();
                    Panel1.Style.Add("visibility", "visible");
                    Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
                    Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");
             
                    toolti = pID.ToString();
                    Txt_nom.Text = pID.ToString();
                   
                   // GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip = pID.ToString();
                    Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    
                }
                //else if (GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString() == r["numero"].ToString())
                //{ */


            DataSet ds = new DataSet();
            ds = ges.ReturnData("select * from compteure where numero='"+pID.ToString ()+"'", "compteure");
          
                //if (pID.ToString() == r["nom"].ToString())
                if (ds.Tables[0].Rows.Count != 0)
                {

                    //int numero = Convert.ToInt32(GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString());
                    ges.DataUpdate("update compteure set latitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".") + "', longitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".") + "'where numero ='" + pID.ToString() + "'");

                }
                else 
                
                {

                    mp1.Show();
                    Panel1.Style.Add("visibility", "visible");
                    Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
                    Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");

                    toolti = pID.ToString();
                    Txt_nom.Text = pID.ToString();

                    // GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip = pID.ToString();
                    Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    Lab_Tooltip.Text = pID.ToString().Replace("_", " ").Substring(0, pID.Length - 3);
                }
            }
        

//(pID.ToString().Contains("support"))
       else if (GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString().Contains("support"))
        {
       DataSet ds = new DataSet();
            ds = ges.ReturnData("select * from support where numero ='"+ pID.ToString () + "'", "support");
           
              
            if (ds.Tables[0].Rows.Count != 0)
                {

                    //int numero = Convert.ToInt32(GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString());
                    ges.DataUpdate("update support set latitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".") + "', longitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".") + "'where numero ='" + pID.ToString() + "'");

                }
                else 
                {
                    //string nom = Num_gene("support").Substring(3);
                    //int genere = Convert.ToInt32(nom) + 1;
                    mp1.Show();
                    Panel1.Style.Add("visibility", "visible");
                    Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
                    Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");
                    Txt_nom.Text = pID.ToString();
                    toolti = pID.ToString();

                    TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
                    Lab_Tooltip.Text = pID.ToString().Replace("_", " ").Substring(0, pID.Length - 3);
                }
            }




       else if (GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString().Contains("Hampe"))
       {
           DataSet ds = new DataSet();
           ds = ges.ReturnData("select * from hampe where numero ='" + pID.ToString() + "'", "hampe");


           if (ds.Tables[0].Rows.Count != 0)
           {

               //int numero = Convert.ToInt32(GoogleMapForASPNet1.GoogleMapObject.Points[pID].ToolTip.ToString());
               ges.DataUpdate("update support set latitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".") + "', longitude='" + GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".") + "'where numero ='" + pID.ToString() + "'");

           }
           else
           {
               //string nom = Num_gene("support").Substring(3);
               //int genere = Convert.ToInt32(nom) + 1;
               mp1.Show();
               Panel1.Style.Add("visibility", "visible");
               Txt_Latitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude.ToString().Replace(",", ".");
               Txt_longitude.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude.ToString().Replace(",", ".");
               Txt_nom.Text = pID.ToString();
               toolti = pID.ToString();

               TextBox2.Text = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
               Image1.ImageUrl = GoogleMapForASPNet1.GoogleMapObject.Points[pID].IconImage.ToString();
               Lab_Tooltip.Text = pID.ToString().Replace("_", " ").Substring(0, pID.Length - 3);
           }
       }
        }

    

//****************fonction retourne derniere enregistrement de la table numero_incremn **************

   public string  Num_gene( string nom_Tab) 
   {
       string num= "";
       DataSet ds;

     
           ds = ges.ReturnData("select numero from " + nom_Tab +  " ORDER BY " + nom_Tab +".id DESC LIMIT 1 ",nom_Tab );

           if (ds.Tables[0].Rows.Count == 0)
           {
               num = null ;
           }
           else { 

         num = ds.Tables[0].Rows[0][0].ToString();
     
           } 
       return num ;
   }




   /************** fonction du click sur un marqueure   *****************/
   void OnPushpinClicked(string pID)
   {
       id_mark = pID.ToString();
       string name = null;
       DataSet ds;
       ds = ges.ReturnData("select * from objet", "objet");
       nom = TextBox1.Text;
       GooglePoint GP = new GooglePoint();

       foreach (DataRow r in ds.Tables[0].Rows)
       {
       if (nom == "line" && r["nom_objet"].ToString() == nom)
       {


           if (HiddenField1.Value == string.Empty) { HiddenField1.Value = "0"; }
           TextBox8.Text = pID.ToString();
           ////name = r["nom_objet"].ToString();
           GP.Latitude = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude;
           GP.Longitude = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude;

           i = i + 1;

           listP.Add(GP);
           HiddenField1.Value = HiddenField1.Value + 1;
     
               ScriptManager.RegisterStartupScript(Page,this.GetType(), "tracer_line", "initMap(" + GP.Latitude + ","  + GP.Longitude +");",true);
           }



       }



       if (i >= 2)
       {
           group.Add(listP[1]);

           GooglePolyline PL1 = new GooglePolyline();
           if (group.Count == 2)
           {
               for (int h = 0; h > group.Count; h++)
               {
                   if (group[h].Longitude != GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude && group[h].Latitude != GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude)
                   {





                       if (group.Count == 2)
                       {

                           foreach (var k in group)
                           {

                               PL1.Points.Add(k);
                           }
                           GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(PL1);
                           //ges.DataUpdate("insert into poste1 (latitude , longitude ,name) values ( '" + listP[0].Latitude + " - " + listP[0].Longitude + " ' , '" + listP[1].Latitude + " - " + listP[1].Longitude + "','" + name + "' )");
                           i = 0;
                           listP.Clear();
                           group.Clear();
                       }

                       i = 0;
                       listP.Clear();
                   }
                   else

                       if (group.Count == 2)
                       {

                           foreach (var k in group)
                           {

                               PL1.Points.Add(k);
                           }
                           GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(PL1);

                       }


               }

           }
           //    GooglePolyline PL1 = new GooglePolyline();


           //    if (listP.Count == 2)
           //    {
           //        foreach (var k in listP )
           //        {

           //            PL1.Points.Add(k);
           //        }
           //    }


           //    GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(PL1);
           //    ges.DataUpdate("insert into poste1 (latitude , longitude ,name) values ( '" + listP[0].Latitude + " - " + listP[0].Longitude + " ' , '" + listP[1].Latitude + " - " + listP[1].Longitude + "','" + name + "' )");
           //     i = 0;
           //     listP.Clear();



           //}



           //}
           //catch (Exception e)
           //{

           //    Label6.Text = e.Message;
           ////}
           //group.Clear();
       }





  


   }


 void tracer_ligne() 
 {

    

 
 }


    void LIS (double lat,double lon)
{
    GooglePoint GP = new GooglePoint();
    if (nom == "line")
    {
        GP.Latitude = lat;
        GP.Longitude = lon;

        listP.Add(GP);
       
    
    }


}

/**************fonction du click sur la map*************/ 

    void OnMapClicked(double lat, double longi)
    {



       
        centre.Latitude = lat;
        centre.Longitude = longi;

        string name1 = null;
        DataSet ds;
        ds = ges.ReturnData("select * from objet", "objet");
        nom = TextBox1.Text;
      
        GooglePoint GP = new GooglePoint();
        //Session ["sesion"] = new List<GooglePoint>();
        foreach (DataRow r in ds.Tables[0].Rows)
        {

            if (nom == "line" && r["nom_objet"].ToString() == nom)
            {

                if (HiddenField1.Value == string.Empty) { HiddenField1.Value = "0"; }


                        if (ViewState["increm"] == null)
                        {
                            ViewState["increm"] = "0";

                        }

                        name1 = r["nom_objet"].ToString();

                        GP.Latitude = lat;
                        GP.Longitude = longi;

                        listP.Add (GP);

                  
                        ViewState["increm"] = Convert.ToInt16(ViewState["increm"]) + 1;
                        i = Convert.ToInt16(ViewState["increm"]);

                    }

                }


                if (i == 2)
                {
                    GooglePolyline PL1 = new GooglePolyline();
                    GooglePoint p = new GooglePoint();

                     //listP =(List<GooglePoint>)ViewState["point"]; 




                    if (listP.Count == 2)
                    {
                        foreach (var k in listP)
                        {
                            PL1.Points.Add(k);

                        }
                    }



                    GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(PL1);
                    //ges.DataUpdate("insert into poste (latitude , longitude ,name) values ( '" + listP[0].Latitude + " - " + listP[0].Longitude + " ' , '" + listP[1].Latitude + " - " + listP[1].Longitude + "','" + name1 + "' )");
                    i = 0;
                    ViewState["increm"] = null;
                    listP.Clear();
                }

   
   
            }
      
    


    //***** pour charge le treeview****//




    void chargerTree(DataSet ds, int parentid, TreeNode Treenode)
    {
        TreeNode parent = new TreeNode();
        parent.Text = "Tools";
        parent.SelectAction = TreeNodeSelectAction.None;

        try { 
        foreach (DataRow r in ds.Tables[0].Rows)
        {

            TreeNode child = new TreeNode();
            child.Text = r["nom_objet"].ToString();
            child.Value = r["objet_id"].ToString();

            if (r["url_image"] != null)
            {
                child.ImageUrl = r["url_image"].ToString();
            }
         


            TreeNode child2 = null;

            foreach (DataRow r2 in ds.Tables[0].Rows)
            {
                if (r["objet_id"].ToString() == r2["id_parent"].ToString())
                {
                    // parent.ChildNodes.Add(child);
                    child2 = new TreeNode();
                    child2.Text = r2["nom_objet"].ToString();
                    child2.Value = r2["objet_id"].ToString();
                    if (r2["url_image"] != null)
                    {
                        child2.ImageUrl = r2["url_image"].ToString();

                    }
                    //TreeView1.Nodes.Add(child);
                    child.ChildNodes.Add(seacherChild(child2, ds));

                    child.SelectAction = TreeNodeSelectAction.None ;
                    child2.Expand();
                  
                }
                


            }
            if (child.ChildNodes.Count != 0)
            {
                if (ischild(child, ds) == true) {
                    parent.ChildNodes.Add(child);
                child.SelectAction = TreeNodeSelectAction.None;
 }
            }
            else
            {
                if (ischild(child, ds) == true)
               parent.ChildNodes.Add(child);

            }
        

        }

}
        catch(Exception ex){

            Debug.Write(ex);
        }
        
        TreeView1.Nodes.Add(parent);
   


        // seacherChild(parent, ds);
    }



    Boolean ischild(TreeNode t, DataSet ds)
    {
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            if (t.Value == r["objet_id"].ToString())
            {
                if (r["id_parent"].ToString() == "0")
                    return true;
            }
        }
        return false;
    }

    TreeNode seacherChild(TreeNode t, DataSet ds)
    {
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            if (t.Value == r["id_parent"].ToString())
            {

                TreeNode child2 = new TreeNode();
                child2.Text = r["nom_objet"].ToString();
                child2.Value = r["objet_id"].ToString();


                t.ChildNodes.Add(seacherChild(child2, ds));

                t.Expand();
             

            }


        }


        return t;
    }

    protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {




    }

    //**** transforme le treeview en xml *****//

    void charegement(DataSet ds)
    {

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = Encoding.UTF8;

        string xmlDoc2 = Server.MapPath("treeview.xml");

        using (XmlWriter writer = XmlWriter.Create(xmlDoc2, settings))
        {

            //DataSet ds;

            //ds = ges.ReturnData("select * from objet ", "objet");


            writer.WriteStartDocument();
            writer.WriteStartElement("all");

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                writer.WriteStartElement(r["nom_objet"].ToString(), r["nom_objet"].ToString());

                foreach (DataRow r2 in ds.Tables[0].Rows)
                {
                    if (r["objet_id"].ToString() == r2["id_parent"].ToString())
                    {

                        //writer.WriteStartElement(r["nom_objet"].ToString());
                        writer.WriteElementString("Nom", r2["nom_objet"].ToString());
                        charegement(ds);
                        //writer.WriteEndElement();
                    }


                }

                writer.WriteEndElement();


            }
            writer.WriteEndDocument();
            Response.Redirect("treeview.xml");
        }

    }

    private void creat(string bug, string explain, XmlTextWriter writer)
    {

        //writer.WriteStartElement("bug");
        //writer.WriteStartElement("nom");
        //writer.WriteString(bug);
        //writer.WriteEndElement();
        ////writer.WriteStartElement("resolv");
        ////writer.WriteString(explain);
        ////writer.WriteEndElement();
        //writer.WriteEndElement();

    }
    //lire les fichier xml

    private void read()
    {

        //    XmlDocument xmldoc = new XmlDocument ();
        //xmldoc.Load("treeview.xml");
        ////Dim nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/explain/bug")
        //    XmlNodeList nodes= xmldoc.DocumentElement .SelectNodes ("/treeview/bug");

        ////Dim i As Integer = 0
        //DataGrid.Rows.Clear() 'vider la datagridview
        //For Each n As XmlNode In nodes
        //    Dim listP As String() = New String() {CStr(i + 1), n.SelectSingleNode("nom").InnerText, n.SelectSingleNode("resolv").InnerText}
        //    DataGrid.Rows.Add(listP)
        //Next



    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        ds = ges.ReturnData("select * from objet", "objet");

        chargement2(ds, 0);


    }

    //fonction pour cree un fichier xml

    void chargement2(DataSet ds, int id_parent)
    {

        //xmlDoc.Load("xml_doc.xml");
        XmlNode rootNode;
        rootNode = xmlDoc.CreateElement("Outils");
        xmlDoc.AppendChild(rootNode);


        foreach (DataRow r in ds.Tables[0].Rows)
        {
            XmlAttribute attribute1 = xmlDoc.CreateAttribute("objet_id");

            XmlNode p = xmlDoc.CreateElement(r["nom_objet"].ToString());
            attribute1.Value = r["objet_id"].ToString();
            p.Attributes.Append(attribute1);

            foreach (DataRow r2 in ds.Tables[0].Rows)
            {
                if (r["objet_id"].ToString() == r2["id_parent"].ToString())
                {
                    XmlNode usernode = xmlDoc.CreateElement(r2["nom_objet"].ToString());
                    XmlAttribute attribute = xmlDoc.CreateAttribute("objet_id");

                    //usernode.InnerText = r2["nom_objet"].ToString();
                    attribute.Value = (r2["objet_id"].ToString());

                    usernode.Attributes.Append(attribute);

                    p.AppendChild(seacherChild(usernode, ds));

                }

            }

            if (p.HasChildNodes == true)
            {
                if (ischild(p, ds) == true)
                    rootNode.AppendChild(p);
            }

        }
        xmlDoc.Save(Server.MapPath("xml_doc4.xml"));
        //xmlDoc.Save("xml_doc2.xml");
        Response.Redirect("xml_doc4.xml");
    }

    XmlNode seacherChild(XmlNode t, DataSet ds)
    {

        foreach (DataRow r in ds.Tables[0].Rows)
        {

            if (t.Attributes[0].Value == r["id_parent"].ToString())
            {

                XmlNode child2 = xmlDoc.CreateElement(r["nom_objet"].ToString());
                XmlAttribute attribute = xmlDoc.CreateAttribute("objet_id");



                //child2.InnerText = r["nom_objet"].ToString();
                attribute.Value = r["objet_id"].ToString();
                child2.Attributes.Append(attribute);
                t.AppendChild(seacherChild(child2, ds));
                if (child2.HasChildNodes == false)
                {
                    child2.InnerText = r["nom_objet"].ToString();
                    attribute.Value = r["objet_id"].ToString();
                    child2.Attributes.Append(attribute);
                    t.AppendChild(seacherChild(child2, ds));
                }

            }
        }

        return t;
    }

    Boolean ischild(XmlNode t, DataSet ds)
    {
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            if (t.Attributes[0].Value == r["objet_id"].ToString())
            {
                if (r["id_parent"].ToString() == "0")
                    return true;
            }
        }
        return false;
    }

    //protected void recursivité(TreeNode tn)
    //{


    //    DataSet mon_ds = new DataSet();

    //    TreeNode fils;

    //    string son_nom;
    //    son_nom = tn.Value;//nom du père
    //    int child = Convert.ToInt16(son_nom);



    //    mon_ds = ges.ReturnData("select * from  objet where id_parent =" + child, "objet");

    //    if (mon_ds.Tables[0].Rows.Count != 0)//si a des fil
    //    {
    //        foreach (DataRow r in mon_ds.Tables[0].Rows)//pour chaque fils
    //        {

    //            fils = new TreeNode(r["nom_objet"].ToString(), r["objet_id"].ToString());
    //            tn.ChildNodes.Add(fils);
    //            recursivité(fils);


    //            TreeView1.Nodes.Add(tn);

    //        }

    //    }


    //}



    //public void LoadTreeView(DataSet ds)
    //{


    //    foreach (DataRow r in ds.Tables[0].Rows)
    //    {
    //        string chilcode = r["enfant"].ToString();
    //        string libelle = r["libelle"].ToString();
    //        string Parent = r["parent"].ToString();

    //        TreeNode tnparent;

    //        tnparent = null;
    //        tnparent = TreeView1.FindNode(Parent);



    //        if (tnparent == null)
    //        {

    //            TreeNode tr = new TreeNode();
    //            tr.Value = chilcode;
    //            tr.Text = libelle;
    //            TreeView1.Nodes.Add(tr);
    //        }
    //        else
    //        {
    //            TreeNode tr = new TreeNode();
    //            tr.Value = chilcode;
    //            tr.Text = libelle;
    //            tnparent.ChildNodes.Add(tr);

    //            foreach (DataRow r2 in ds.Tables[0].Rows)
    //            {
    //                if (chilcode == r2["parent"].ToString())
    //                {
    //                    TreeNode td = new TreeNode();
    //                    td.Value = r2["enfant"].ToString();
    //                    td.Text = r2["libelle"].ToString();
    //                    tr.ChildNodes.Add(td);



    //                }



    //            }

    //        }

    //    }

    //}


    //catch (Exception ex)
    //{
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        
        ClientScript.RegisterStartupScript(this.GetType(), "ClientScript","$('#myModal').modal();", true);
    }
   


  public void save ()
  {


     //string name = Txt_nom.Text.ToString();
      string name = TextBox1.Text.ToString();
   
      
      if (Txt_reference.Text.Length > 0)
      {  
         
          
         DataSet ds= new DataSet () ;
         
          List<string> subStrings = new List<string> { "poste", "compteure", "support" };
          try
          {
              switch (subStrings.FirstOrDefault(name.Contains))
              {


                  case "poste":
                      ds = ges.ReturnData("select * from poste2 where numero ='" + Txt_reference.Text.Trim() + "'", "poste2");
                      //refresh("poste2"); 
                      if (ds.Tables[0].Rows.Count == 0 || ds.Equals(null))
                      {
                          //Txt_reference.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                          ges.DataUpdate("insert into poste2 (numero,nom,latitude,longitude,url_image)  values ('" + Txt_reference.Text + "','" + Txt_nom.Text + "','" + Txt_Latitude.Text + "','" + Txt_longitude.Text + "','" + TextBox2.Text + "')");


                          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ToolTip = Lab_Tooltip.Text;
                          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ID = Txt_reference.Text;

                          mp1.Hide();
                          Txt_reference.Text = " ";
                          Txt_Latitude.Text = " ";
                          Txt_longitude.Text = " ";
                          Txt_nom.Text = " ";
                          TextBox2.Text = " ";




                      }
                      else
                      {
                          mp1.Show();
                          txt_error.Visible = true;
                          txt_error.ForeColor = Color.Red;
                          Txt_reference.Text = " ";

                          break;

                      }

                      break;
                     
                  case "compteure":
                           ds = ges.ReturnData("select * from compteure where numero ='" + Txt_reference.Text.Trim() + "'", "compteure");
                         //refresh("poste2"); 
                      if (ds.Tables[0].Rows.Count == 0 || ds.Equals(null))
                      {

                          ges.DataUpdate("insert into compteure (numero,nom,latitude,longitude,url_image)  values ('" + Txt_reference.Text + "','" + Txt_nom.Text + "','" + Txt_Latitude.Text + "','" + Txt_longitude.Text + "','" + TextBox2.Text + "')");

                          //     GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ToolTip = Lab_Tooltip.Text;
                          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ID = Txt_reference.Text;
                      }
                      else
                      {
                          mp1.Show();
                          txt_error.Visible = true;
                          txt_error.ForeColor = Color.Red;
                          Txt_reference.Text = " ";

                          break;

                      }

                      break ;



                  case "support":
                      ds = ges.ReturnData("select * from support where numero ='" + Txt_reference.Text.Trim() + "'", "support");
                      //refresh("poste2"); 
                      if (ds.Tables[0].Rows.Count == 0 || ds.Equals(null))
                      {
                       
                              //string url = TextBox2.Text.ToString().Replace("", "\\").Substring(0, TextBox2.Text .Length - 5);
                          ges.DataUpdate("insert into support (numero,nom,latitude,longitude,url_image)  values ('" + Txt_reference.Text + "', '" + Txt_nom.Text + "','" + Txt_Latitude.Text + "','" + Txt_longitude.Text + "','" + TextBox2.Text + "')");
                          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ID = Txt_reference.Text;
                          break;
                      }
                      else
                      {
                          mp1.Show();
                          txt_error.Visible = true;
                          txt_error.ForeColor = Color.Red;
                          Txt_reference.Text = " ";

                          break;

                      }
                      break;

                  case "hampe":
                      ds = ges.ReturnData("select * from hampe where numero ='" + Txt_reference.Text.Trim() + "'", "hampe");
                      //refresh("poste2"); 
                      if (ds.Tables[0].Rows.Count == 0 || ds.Equals(null))
                      {

                          //string url = TextBox2.Text.ToString().Replace("", "\\").Substring(0, TextBox2.Text .Length - 5);
                          ges.DataUpdate("insert into hampe (numero,nom,latitude,longitude,url_image)  values ('" + Txt_reference.Text + "', '" + Txt_nom.Text + "','" + Txt_Latitude.Text + "','" + Txt_longitude.Text + "','" + TextBox2.Text + "')");
                          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].ID = Txt_reference.Text;
                          break;
                      }
                      else
                      {
                          mp1.Show();
                          txt_error.Visible = true;
                          txt_error.ForeColor = Color.Red;
                          Txt_reference.Text = " ";

                          break;

                      }


              }
          }
          catch (Exception ex)
          {
              Console.Write(ex.Message);
          }
         
          

          //mp1.Hide();
          //Txt_reference.Text = " ";
          //Txt_Latitude.Text = " ";
          //Txt_longitude.Text = " ";
          //Txt_nom.Text = " ";
          //TextBox2.Text = " ";

          }
         
     

     Txt_reference.Text = " ";
      //Txt_Latitude.Text = " ";
      //Txt_longitude.Text = " ";
      //Txt_nom.Text = " ";
      //TextBox2.Text = " ";
  
  }
  protected void BtnAnnuler_Click(object sender, EventArgs e)
  {
  

          mp1.Hide();
          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].Latitude = 11.5867151352197;
          GoogleMapForASPNet1.GoogleMapObject.Points[id_mark].Longitude = 43.1480500710693;
          Txt_reference.Text = " ";

          Txt_Latitude.Text = " ";
          Txt_longitude.Text = " ";
          Txt_nom.Text = " ";
          TextBox2.Text = " ";
    
    
     
  }


  protected void btnSupprimer_Click(object sender, EventArgs e)
  {
      GoogleMapForASPNet1.GoogleMapObject.Points.Remove(id_mark);
      mp1.Hide();
  }
  protected void BtnSave_Click(object sender, EventArgs e)
  {
     save();
      
  }

  public void refresh(string nom_table) 
  {

    ges.DataUpdate ("select * from '" + nom_table + "'");
  
  }






}



 



 