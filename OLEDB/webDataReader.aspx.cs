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

            if (!Page.IsPostBack)
            {

                myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~/App_Data/College.mdb"));
                myCon.Open();

                myCmd = new OleDbCommand("SELECT [Number], RefCourse FROM Courses Order by [Number]", myCon);
                rdStudents = myCmd.ExecuteReader();

                lstCourse.DataTextField = "Number";
                lstCourse.DataValueField = "RefCourse";
                lstCourse.DataSource = rdStudents;
                lstCourse.DataBind();

                //THIS IS JUST TO TEST THE CONNECTION WITH HARD CODE
                //Optional
                string sql = "Select * from Courses where Teacher =@teach and Duration <@dur";
                OleDbCommand myCmdTest = new OleDbCommand(sql, myCon);
                OleDbParameter myPar = new OleDbParameter("teach", "Houria Houmel");
                myPar.DbType = System.Data.DbType.String;

                myCmdTest.Parameters.Add(myPar);
                myCmdTest.Parameters.AddWithValue("dur", 50);

                OleDbDataReader rdTest = myCmdTest.ExecuteReader();

                gridTest.DataSource = rdTest;
                gridTest.DataBind();

            }

        }

        protected void lstCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCmd = new OleDbCommand("Select * from Courses where RefCourse =@ref", myCon);
            myCmd.Parameters.AddWithValue("ref", lstCourse.SelectedItem.Value);

            rdStudents = myCmd.ExecuteReader();

            if (rdStudents.Read())
            {
                txtNumber.Text = rdStudents["Number"].ToString();
                txtTitle.Text = rdStudents["Title"].ToString();
                txtDuration.Text = rdStudents["Duration"].ToString();
                txtTeacher.Text = rdStudents["Teacher"].ToString();
                txtDescription.Text = rdStudents["Description"].ToString();
            }

            rdStudents.Close();
            myCmd.CommandText = "Select StudentName as [Names],BirthDate as [Birth Dates],Telephone,Average,Email from Students where ReferCourse =@ref";

            rdStudents = myCmd.ExecuteReader();
            gridResults.DataSource = rdStudents;
            gridResults.DataBind();




        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int refC = Convert.ToInt32(lstCourse.SelectedItem.Value);
            string sql = "UPDATE Courses SET Duration=@dur,Teacher=@tea,Description=@des WHERE RefCourse=@courseId";
            OleDbCommand myCmd = new OleDbCommand(sql, myCon);
            myCmd.Parameters.AddWithValue("dur", Convert.ToInt32(txtDuration.Text));
            myCmd.Parameters.AddWithValue("tea", txtTeacher.Text);
            myCmd.Parameters.AddWithValue("des", txtDescription.Text);
            myCmd.Parameters.AddWithValue("courseId", refC);

            int result = myCmd.ExecuteNonQuery();

            Response.Write("<script>alert(\"Updated\");</script>");
        }
    }
}