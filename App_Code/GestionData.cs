using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Data ;
using System.Windows.Forms;

using System.Xml.XPath;



/// <summary>
/// Description résumée de GestionData
/// </summary>
public class GestionData
{
	public NpgsqlConnection objetConnection ;
	
	public NpgsqlConnection objetConnectionGescli;
	public NpgsqlDataAdapter ObjetDataAdapter;
	public DataRow objetdatarow;
	public string strSql;

	public NpgsqlCommand Mycommand;

    public string xmlpath =  System.AppDomain.CurrentDomain.BaseDirectory + "TableCpt_Xml" ;
    
  
	private bool IsConnected()
	{
		try {
			objetConnection = new NpgsqlConnection();
            //objetConnection.ConnectionString = "DATABASE=GpsTEST;HOST=127.0.0.1;PASSWORD=edd123;USER ID=postgres";
            objetConnection.ConnectionString = "DATABASE=gps1 ;HOST=192.168.9.7;PASSWORD=edd123;USER ID=postgres";
			//objetConnection.ConnectionString = "Data Source=gescli;User ID=gescli;Psword=siedd2009";
            //objetConnection.ConnectionString = ConfigurationManager.ConnectionStrings["GpsTEST.ConnectionString"].ConnectionString;
			//objetConnection.ConnectionString = "Data Source=gescli;Persist Security Info=True;User ID=gescli;Psword=siedd2009;Unicode=True"         
			objetConnection.Open();
			return true;
		} catch (Exception ex) {
			//System.Diagnostics.EventLog.WriteEntry("ServiceGescli", "GestionData.Connection Error # " + ex.Message.ToString + "  at " + DateTime.Now);
			return false;
		}
	}
    //public Datet ReturnData(string arg_SQL, string arg_Table)
    //{
    //    Datet tmp_dts = new Datet();
    //    if ()(IsConnected()) {

    //        try {
    //            ObjetDataAdapter = new OracleDataAdapter(arg_SQL, objetConnection);
    //            ObjetDataAdapter.Fill(tmp_dts, arg_Table);
    //            objetConnection.Close();

    //        } catch (Exception ex) {

    //        }
    //    }
    //    return tmp_dts;
    //}



		
		
    //}

    public DataSet ReturnData(string arg_SQL, string arg_Table)
    {
        DataSet  tmp_dts = new DataSet ();
        if (IsConnected())
        {

            try
            {
                ObjetDataAdapter = new NpgsqlDataAdapter(arg_SQL, objetConnection);
                ObjetDataAdapter.Fill(tmp_dts, arg_Table);
                objetConnection.Close();

            }
            catch (Exception ex)
            {

            }
        }
        return tmp_dts;
    }
    //public OracleDataReader ReturnDataReader(string arg_SQL)
    //{
    //    //  command  OracleCommand
    //    OracleDataReader tmp_dr = null;
    //    if ()(IsConnected()) {
    //        try {
    //            OracleCommand command = new OracleCommand(arg_SQL, objetConnection);
    //            command.CommandType = Data.CommandType.Text;

    //            tmp_dr = command.ExecuteReader();
    //            //objetConnection.Close()

    //        } catch (Exception ex) {
    //        }
    //    }

    //    return tmp_dr;
    //}
    public void DataUpdate(string arg_Sql)
    {
        if (IsConnected())
        {
            
            try
            {
                Mycommand = new NpgsqlCommand(arg_Sql, objetConnection);
                Mycommand.ExecuteNonQuery();
                objetConnection.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }


   public   XPathNodeIterator  ReturnDataXml ( string tableXml,string condi) {
       // 'data xml dans un dropdowlistnormal
         string val   = "";
          XPathNodeIterator xmlNI ;
         XPathDocument xpathDoc = new XPathDocument (xmlpath + tableXml  + ".xml");
          System.Xml.XPath.XPathNavigator  xmlNav = xpathDoc.CreateNavigator();
         
        xmlNI = xmlNav.Select("//" + tableXml + "[" + condi + "]");
        if (xmlNI.Count > 0) {
            return xmlNI;
        }
        return null ;
   }

      public string GetDataXmlValue(string tableXml  ,string  condi  ,string  SelectedField ,string  ValueIfNull = "")
     {
       // 'data xml dans un dropdowlistnormal
         string val = "";
         XPathNodeIterator xmlNI ;
         XPathDocument xpathDoc   = new XPathDocument ( xmlpath + tableXml + ".xml");
         System.Xml.XPath.XPathNavigator xmlNav = xpathDoc.CreateNavigator ();
        xmlNI = xmlNav.Select("//" + tableXml + "[" + condi + "]");
        if (xmlNI.Count == 0) {
            val = ValueIfNull;}
        else{
            while (xmlNI.MoveNext()) {
               
               var bar = xmlNI.Current ;
                    if (bar.IsEmptyElement) 

                        val = ValueIfNull ;
                    else 
                        val = bar.SelectSingleNode(SelectedField).Value;
                      }
                    

                   // 'cmb.Items.Add(New ListItem(.SelectSingleNode(libelle).Value, .SelectSingleNode(code).Value))
               }




       if (val == null) { 
           
           val = "" ;}

        return val;


    }

     public NpgsqlDataReader ReturnDataReader(string arg_SQL ) {
        IsConnected();
      
            NpgsqlCommand command = new NpgsqlCommand (arg_SQL, objetConnection);
             command.CommandType = CommandType.Text;
             
         NpgsqlDataReader tmp_dr ;
         tmp_dr= command.ExecuteReader ();
          
             //'objetConnection.Close()
            return tmp_dr;
   }

            }
        

       


