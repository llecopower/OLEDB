using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace OLEDB
{
    public partial class webDataReader : System.Web.UI.Page
    {
        static OleDbConnection myCon;
        OleDbCommand myCmd;
        OleDbDataReader rdStudents;



        protected void Page_Load(object sender, EventArgs e)
        {

            myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~/App_Data/College.mdb"));
            myCon.Open();

            myCmd = new OleDbCommand("SELECT [Number], RefCourse FROM Courses Order by [Number]", myCon);
            rdStudents = myCmd.ExecuteReader();

            lstCourse.DataTextField = "Number";
            lstCourse.DataValueField = "RefCourse";
            lstCourse.DataSource = rdStudents;
            lstCourse.DataBind();


        }

        protected void lstCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}