using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace OLEDB
{
    public partial class webCollege : System.Web.UI.Page
    {

        static OleDbConnection myCon;
        OleDbDataReader myRd;
       // OleDbCommand myCmd;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                panCourse.Visible = panPrograms.Visible = false;

                myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Server.MapPath("~/App_Data/College2.mdb"));
                myCon.Open();
                OleDbCommand myCmd = new OleDbCommand("Select refSchool, Title from Schools order by Title", myCon);
                myRd = myCmd.ExecuteReader();

                radlistSchool.DataTextField = "Title";
                radlistSchool.DataValueField = "refSchool";
                radlistSchool.DataSource = myRd;
                radlistSchool.DataBind();
            }
        }

        protected void radlistSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            panPrograms.Visible = true;

            OleDbCommand myCmd = new OleDbCommand("SELECT refProgram, Title FROM Programs WHERE referSchool=@refS ORDER BY Title", myCon);
            myCmd.Parameters.AddWithValue("refS", radlistSchool.SelectedItem.Value);
            myRd = myCmd.ExecuteReader();

            radlstPrograms.DataTextField = "Title";
            radlstPrograms.DataValueField = "refProgram";
            radlstPrograms.DataSource = myRd;
            radlstPrograms.DataBind();

            chklstCourses.Items.Clear();
            gridStudents.DataSource = null;
            gridStudents.DataBind();
        }

        protected void radlstPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            panCourse.Visible = true;

            OleDbCommand myCmd = new OleDbCommand("SELECT RefCourse,[Number] FROM Courses WHERE referProgram=@refP ORDER BY [Number]", myCon);
            myCmd.Parameters.AddWithValue("refP", radlstPrograms.SelectedItem.Value);
            myRd = myCmd.ExecuteReader();

            chklstCourses.DataTextField = "Number";
            chklstCourses.DataValueField = "RefCourse";
            chklstCourses.DataSource = myRd;
            chklstCourses.DataBind();

            gridStudents.DataSource = null;
            gridStudents.DataBind();
        }

        protected void chklstCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chklstCourses.SelectedIndex > -1)
            {
                string sql = "SELECT StudentName AS [Names],BirthDate AS [Birth Dates],Telephone,Average,Email FROM Students WHERE ReferCourse = " + chklstCourses.SelectedItem.Value;
                foreach (ListItem item in chklstCourses.Items)
                {
                    if (item.Selected)
                    {
                        sql += " or ReferCourse=" + item.Value;

                    }
                }
                sql += " ORDER BY StudentName";
                OleDbCommand myCmd = new OleDbCommand(sql, myCon);
                myRd = myCmd.ExecuteReader();
                gridStudents.DataSource = myRd;
                gridStudents.DataBind();
            }
            else
            {
                gridStudents.DataSource = null;
                gridStudents.DataBind();
            }

        }

        protected void gridStudents_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}