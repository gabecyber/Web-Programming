using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hw05 {
    public partial class Patients3 : System.Web.UI.Page {
        string dbType = "Access_Patients";

        protected void Page_Load(object sender, EventArgs e) {
            IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
            txtVisitAndPreCharges.Text = "";
            if (!Page.IsPostBack) {

                // Select first team.
                GenerateFirstVisitsSQL(cmd);
                DisplayVisits(dbType, cmd);
                ddPatients.SelectedIndex = 0;
            }
            else {
                GenerateSelectVisitsSQL(cmd);
                DisplayVisits(dbType, cmd);
            }
        }
        protected void ddPatients_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void GenerateFirstVisitsSQL(IDbCommand cmd) {
            // This is not using Parameter approach, but probably should.

            cmd.CommandText =
                "SELECT " +
                    "Visits.VisitDate, " +
                    "Visits.VisitID, " +
                    "Visits.Charge " +

                "FROM " +
                    "Visits " +
                "WHERE " +
                    "Visits.PatientID = 1"   + " " +
                "ORDER BY " +
                    "Visits.VisitDate Asc ";
        }
        private void GenerateSelectVisitsSQL(IDbCommand cmd) {
            // This is not using Parameter approach, but probably should.
               
            cmd.CommandText =
                "SELECT " +
                    "Visits.VisitDate, " +
                    "Visits.VisitID, " +
                    "Visits.Charge " +

                "FROM " +
                    "Visits " +
                "WHERE " +
                    "Visits.PatientID = " + ddPatients.SelectedValue + " " +
                "ORDER BY " +
                    "Visits.VisitDate Asc ";
        }
        private void DisplayVisits(string dbType,IDbCommand cmd) {

           

            
            cmd.Connection.Open();
            displayPatients(dbType, cmd);

            cmd.Connection.Close();



        }
        private void displayPatients(string dbType, IDbCommand cmd) {
            try {
                IDataReader dr;
                
                
                
                dr = cmd.ExecuteReader();



                txtVisitAndPreCharges.Text = "";

                while (dr.Read()) {
                    DateTime dtDate = (DateTime)dr.GetValue(0);
                    string date = dtDate.ToString("MM/dd/yyyy");
                    
                    int id = dr.GetInt32(1);

                    decimal charge = dr.GetDecimal(2);

                    string visit = String.Format("{0,10:0} {1,9:$0,0.00} {2,-14:0} ", date, charge, id);
                    txtVisitAndPreCharges.Text += visit + Environment.NewLine;
                }

                dr.Close();
                

            }
            catch (Exception ex) {
                txtVisitAndPreCharges.Text += "Error";
                txtVisitAndPreCharges.Text += ex.ToString();
            }

        }
    }
}