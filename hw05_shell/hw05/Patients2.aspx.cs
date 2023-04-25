using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hw05 {
	public partial class Patients2 : System.Web.UI.Page {
		string dbType = "Access_Patients";

		protected void Page_Load(object sender, EventArgs e) {
			txtPatientsAboveAvg.Text = "";

			if (!Page.IsPostBack) {
				DisplayVisits(dbType);
			}
		}
		private void DisplayVisits(string dbType) {
			
			IDbCommand cmd = ConnectionFactory.GetCommand(dbType);

			GenerateSelectPatientsSQL_Join(cmd);
			cmd.Connection.Open();
			displayPatients(dbType);
			
			cmd.Connection.Close();
			

			
		}
		private void displayPatients(string dbType) {
			try {
				IDataReader dr;
				IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
				GenerateSelectPatientsSQL_Join(cmd);
				cmd.Connection.Open();
				dr = cmd.ExecuteReader();

				

				txtPatientsAboveAvg.Text = "";

				while (dr.Read()) {
                    DateTime dtDate = (DateTime)dr.GetValue(0);
					string date = dtDate.ToString("MM/dd/yyyy");
					String lname = dr.GetString(1);
					decimal charge = dr.GetDecimal(2);

					string visit = String.Format("{0,10:0} {1,-14:0} {2,9:$0,0.00}", date, lname, charge);
					txtPatientsAboveAvg.Text += visit + Environment.NewLine;
				}

				dr.Close();
				cmd.Connection.Close();

			}
			catch (Exception ex) {
				txtPatientsAboveAvg.Text = "\r\nError reading data\r\n";
				txtPatientsAboveAvg.Text += ex.ToString();
			}
		}
		private void GenerateSelectPatientsSQL_Join(IDbCommand cmd) {
			cmd.CommandText =
				"SELECT " +
					"Visits.VisitDate, " +
					"Patients.LastName, " +
					"Visits.Charge " +
					
				"FROM " +
					"Patients " +
				"INNER JOIN " +
					"Visits " +
				"ON " +
					"Patients.PatientID = Visits.PatientID " +
				"ORDER BY " +
					"Visits.VisitDate Asc ";
					
		}
	}
}