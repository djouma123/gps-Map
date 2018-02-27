using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Npgsql;
using System.Globalization;
using System.Configuration;


public delegate void nom_value();
public partial class TreeUserControl : System.Web.UI.UserControl
{
    public event  nom_value nom_node;
    GestionData ges = new GestionData();
    public string nom;

    public string nom_noeux { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {


        DataSet Tree;
        this.TreeView1.Nodes.Clear();
        Tree = ges.ReturnData("select * from objet", "objet");

        chargerTree(Tree, 0, null);
    }


    void chargerTree(DataSet ds, int parentid, TreeNode Treenode)
    {


        TreeNode parent = new TreeNode();
        parent.Text = "Tools";


        foreach (DataRow r in ds.Tables[0].Rows)
        {

            TreeNode child = new TreeNode();
            child.Text = r["nom_objet"].ToString();
            child.Value  = r["objet_id"].ToString();

            TreeNode child2 = null;

            foreach (DataRow r2 in ds.Tables[0].Rows)
            {
                if (r["objet_id"].ToString() == r2["id_parent"].ToString())
                {
                    child2 = new TreeNode();
                    child2.Text = r2["nom_objet"].ToString();
                    child2.Value  = r2["objet_id"].ToString();

                    child.ChildNodes.Add(child2);
                    child.Expand();
                    child2.Expand();
                }

            }
            if (child.ChildNodes.Count != 0)
            {
                parent.ChildNodes.Add(child);
            }

        }


        TreeView1.Nodes.Add(parent);
        parent.Expand();


        //foreach (DataRow r in ds.Tables[0].Rows)
        //{

        //    TreeNode child = new TreeNode();
        //    child.Text = r["nom_objet"].ToString();
        //    child.Value = r["objet_id"].ToString();
        //    //child.ImageToolTip = "~/icons/pushpin-yellow.png";
        //    child.ImageUrl = "~/icons/pushpin-yellow.png";


        //    if (parentid == 0)
        //    {
        //       this.TreeView1.Nodes.Add(child);
        //       //child.SelectAction = TreeNodeSelectAction.Select ;

        //        DataSet dtchild = ges. ReturnData("select nom_objet,objet_id  from  objet where id_parent =" + child.Value, "objet");

        //        chargerTree(dtchild, int.Parse(child.Value), child);

        //    }

        //    else
        //    {
        //        DataSet dtchild = ges.ReturnData("select nom_objet,objet_id  from  objet where id_parent =" + child.Value, "objet");
        //        chargerTree(dtchild, int.Parse(child.Value), child);

        //        Treenode.ChildNodes.Add(child);
        //        //child.SelectAction = TreeNodeSelectAction.Select ;


        //    }

        //}

    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
      {

          
 
      }

   
    protected void MyTreeView_SelectedNodeChanged(object sender, EventArgs e)
    {

        nom_noeux = TreeView1.SelectedNode.Text.ToString();

        label1.Text = TreeView1.SelectedNode.Text.ToString();

    //    string nodeVal = "";
    //    string nodeText = "";

    //    nodeVal = TreeView1.SelectedNode.Value;
    //    nodeText = TreeView1.SelectedNode.Text;

    //    for (int k = 0; k < Session.Keys.Count; k++)
    //    {
    //        if (Session.Keys[k].IndexOf("--") != -1)
    //        {
    //            Session[Session.Keys[k]] = null;
    //        }
    //    }

    //    if (!TreeView1.Nodes[0].Selected)
    //        TreeView1.RootNodeStyle.BackColor = System.Drawing.Color.Transparent;

    //    string colSess = nodeText + "--" + nodeVal;
    //    Session[colSess] = "selected";


    }


    public TreeNode child { get; set; }
}




