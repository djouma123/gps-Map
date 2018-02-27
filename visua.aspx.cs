using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class visua : System.Web.UI.Page
{
    GestionData ges = new GestionData();
    protected void Page_Load(object sender, EventArgs e)
    {

        GoogleMapForASPNet1.PushpinClicked += new GoogleMapForASPNet.PushpinClickedHandler(OnPushpinClicked);
        if (!IsPostBack)
        {

          //  GoogleMapForASPNet1.PushpinDrag += new GoogleMapForASPNet.PushpinDragHandler(OnPushpinDrag);
            GoogleMapForASPNet1.GoogleMapObject.APIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];

            //Specify width and height for map. You can specify either in pixels or in percentage relative to it's container.
            GoogleMapForASPNet1.GoogleMapObject.Width = "100%"; // You can also specify percentage(e.g. 80%) here
            GoogleMapForASPNet1.GoogleMapObject.Height = "1000px";

            //Specify initial Zoom level.
            GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 14;

            //Specify Center Point for map. Map will be centered on this point.
            GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1", 11.5867151352197, 43.1480500710693);

            DataSet ds = new System.Data.DataSet();


            ds = ges.ReturnData("select * from visu_all ", "visu_all");
          //  ds = ges.ReturnData("select distinct * from visu_poste  ", "visu_all");
            chargeData(ds);

        }
    }

    private void OnPushpinClicked(string pID)
    {
        throw new NotImplementedException();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GoogleMapForASPNet1.GoogleMapObject.APIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
        GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1", 11.5867151352197, 43.1480500710693);
        GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 14;

        DataSet ds = new System.Data.DataSet();
        ds = ges.ReturnData("select * from visu_all", "visu_all");
        chargeData(ds);
    }
    public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
    {
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
        return dtUniqRecords;
    }

    void chargeData(DataSet ds)
    {
        GoogleMapForASPNet1.GoogleMapObject.Polylines.Clear();
        GoogleMapForASPNet1.GoogleMapObject.Points.Clear();

        string[] TobeDistinct = { "nom", "latitude", "longitude","numero" };
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




        //GooglePoint POSTE = new GooglePoint();

        //double lat = double.Parse(la.ToString().Replace(",", "."), culture);

        //POSTE.ID = "8541";
        //POSTE.Latitude = 11.528576;
        //POSTE.Longitude = 42.854527;
        //POSTE.InfoHTML = "POSTE : 100 LOGEMENT";
        //POSTE.IconImage = "icons/home.png";
        //POSTE.IconImage = "icons\\poste_electrique.png";
        //POSTE.Draggable = true;
        //POSTE.ToolTip = "100 LOGEMENT";


        //GoogleMapForASPNet1.GoogleMapObject.Points.Add(POSTE);
        //GooglePoint POSTE1 = new GooglePoint();

        //POSTE1.ID = "8547";
        //POSTE1.Latitude = 11.587656;
        //POSTE1.Longitude = 43.075940;
        //POSTE1.InfoHTML = "POSTE : 160 LOGEMENT";
        //POSTE.IconImage = "icons/home.png";
        //POSTE1.IconImage = "icons\\poste_electrique.png";
        //POSTE1.Draggable = true;
        //POSTE1.ToolTip = "160 LOGEMENT";

        //GoogleMapForASPNet1.GoogleMapObject.Points.Add(POSTE1);

        foreach (DataRow r in ds.Tables[0].Rows)
        {

            if (r["nom"].ToString() != String.Empty)
            {
                if (r["nom"].ToString() != "line")
                {

                    GooglePoint POSTE = new GooglePoint();

                    POSTE.ID = r["id"].ToString().Trim();
                    POSTE.Latitude = double.Parse(r["latitude"].ToString().Replace(",", ".").Trim(), culture);
                    POSTE.Longitude = double.Parse(r["longitude"].ToString().Replace(",", ".").Trim(), culture);
                 //   POSTE.InfoHTML = "POSTE : " + r["nom"].ToString().ToLower().Trim();
                    POSTE.InfoHTML = inser_num(int.Parse( POSTE.ID.ToString ()));
                    POSTE.IconImage = r["url_image"].ToString().Trim();
                    POSTE.Draggable = true;
                    POSTE.ToolTip = r["nom"].ToString().Trim();
                    
                    try
                    {

                        GoogleMapForASPNet1.GoogleMapObject.Points.Add(POSTE);

                        POSTE = null;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (r["nom"].ToString() == "line")
                {
                    String la = r["latitude"].ToString();
                    String lo = r["longitude"].ToString();

                    string[] lati = la.Split('-');
                    string[] lon = lo.Split('-');

                    GooglePolyline POSTE1 = new GooglePolyline();
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

                    POSTE1.ID = r["nom"].ToString();
                    POSTE1.Points.Add(debu);
                    POSTE1.Points.Add(fin);
                    POSTE1.LineStatus = "POSTE : " + r["nom"].ToString();
                    POSTE1.ColorCode = "blue";
                    POSTE1.Width = 1;
                    GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(POSTE1);



                }

            }

        } 
     
        }
    




    public void OnPushpinDrag() 
    {


        
    }

    public string inser_num(int id)
    {

        DataSet ds = new DataSet();
        ds = ges.ReturnData("select * from visu_all where id ="+id+"", "visu_all");
        string html="";

        foreach (DataRow r in ds.Tables[0].Rows)
        {
           html = "<form id='form1' runat='server'> ";

           html += " <div> <label for='nom'> Nom du poste :</label>";
            html += " <div> <label for='nom'>"+ r["nom"].ToString () +"</label>";
         //   html += " <input type='text' id='nom' />  </div>";
            html += " <div> <label for='nom'> latitude :</label>";
            html += " <div> <label for='latitude'>" +r["latitude"].ToString ()+ ":</label>";
            html += " <div> <label for='nom'> longitude :</label>";
            html += " <div> <label for='longitude'> "+r["longitude" ].ToString ()+" :</label>";
          //  html += "   <input type='email' id='courriel' / > </div>";
           // html += " <div> <label for='message'>Message :</label>";
          //  html += "  <textarea id='message'></textarea>  </div>";


        //    html += " <div class='button'>";
            //<button id='mybutton' runat='server' onserverclick ='foo_OnClick'> envoyer <button />";
            //html += " <input id='Button1' type='button' value='button'  runat ='server' onserverclick='btnBeforeOk_ServerClick' />";
            html += " </div></form>";
        }

        return html;


    }

    public void OnPushpinDrag(string pID)
    {
        DataSet ds = new DataSet();
        ds = ges.ReturnData("", "");
    
    }

}