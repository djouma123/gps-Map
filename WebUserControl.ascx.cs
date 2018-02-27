using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data ;

public partial class WebUserControl : System.Web.UI.UserControl
{ GestionData ges= new GestionData ();

    protected void Page_Load(object sender, EventArgs e)

    {
        
        
         DataSet  Tree;
        Tree = ges.ReturnData("select * from TreeEnfant ,TreeParent", "treeview");

        chargerTree(Tree, 0, null);}
        
        
        void chargerTree(DataSet  ds,int parentid,TreeNode Treenode) 
    {


        foreach (DataRow r in  ds.Tables[0].Rows )
        {


            TreeNode child = new TreeNode
            {
                Text = r["nom_parent"].ToString(),
                Value = r["id"].ToString()

            };




            if (parentid == 0)
            {
                TreeView1.Nodes.Add(child);

                DataSet dtchild = ges.ReturnData("select * from TreeEnfant where id_parent =" + child.Value, "tree");

                chargerTree(dtchild, int.Parse(child.Value), child);


            }
            else
            {
                Treenode.ChildNodes.Add(child);
            }
        }
    
    }
    }
