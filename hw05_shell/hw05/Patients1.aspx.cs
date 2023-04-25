using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hw05 {
	public partial class Patients1 : System.Web.UI.Page {
		string dbType = "Access_Patients";

		protected void Page_Load(object sender, EventArgs e) {
			txtMsg.Text = "";

			if (!Page.IsPostBack) {
				displayPatients(dbType);
			}
		}
		protected void btnAddPatient_Click(object sender, EventArgs e) {
			IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
			
			try {
				BuildInsertPatientCommand(cmd);
				cmd.Connection.Open();
				int num = cmd.ExecuteNonQuery();
				cmd.Connection.Close();
				displayPatients(dbType);
			}
			catch (Exception ex) {
				txtMsg.Text = "Error adding player, TeamID probably incorrect\r\n";
				txtMsg.Text += ex.ToString();
			}

			
		}

		protected void btnDeletePatient_Click(object sender, EventArgs e) {
			IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
			GenerateDeletePatientSQL(cmd);

			try {
				cmd.Connection.Open();

				txtMsg.Text = "Attempting to delete player from: " + dbType + " database\r\n";
				txtMsg.Text += "IDbConnection.State: " + cmd.Connection.State.ToString() + "\r\n";

				int num = cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				txtMsg.Text += "Patients deleted: " + num + "\r\n";

				if (num == 0) {
					txtMsg.Text += "***Delete Failed. Invalid PatientID\r\n";
					
					
				}
				else {
					displayPatients(dbType);

				}
			}
			catch (Exception ex) {
				
				txtMsg.Text = "Delete Failed. Unknown Error\r\n";
				txtMsg.Text += ex.ToString();
			}
		}
		private void GenerateDeletePatientSQL(IDbCommand cmd) {
			// Set delete SQL statement.
			cmd.CommandText = "Delete From " +
				"Patients " +
				"Where " +
				"PatientID=@PatientID";

			cmd.Parameters.Clear();
			addParameter("@PatientID", txtDelID.Text, cmd);
		}
		protected void btnUpdatePatient_Click(object sender, EventArgs e) {
			IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
			GenerateUpdatePatientSQL(cmd);

			try {
				cmd.Connection.Open();

				txtMsg.Text = "Attempting to update in: " + dbType + " database\r\n";
				txtMsg.Text += "IDbConnection.State: " + cmd.Connection.State.ToString() + "\r\n";

				int num = cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				displayPatients(dbType);
				
				txtMsg.Text += "Players updated: " + num + "\r\n";
			}
			catch (Exception ex) {
				
				txtMsg.Text += "\r\nPlayer update error\r\n";
				txtMsg.Text += ex.ToString();
			}
		}
		private void BuildInsertPatientCommand(IDbCommand cmd) {
			
			
			cmd.Connection.Open();
			
			

			
			cmd.CommandText = getSQL();
			
			cmd.CommandText = "Insert Into Patients ( LastName, FirstName, Address ) " +
	"Values ( @LastName, @FirstName, @Address)";
			cmd.Parameters.Clear();                 // Clear any existing parameters
			IDbDataParameter param = cmd.CreateParameter();     // Create a parameter.
			       // Add the parameter to the command.

			param = cmd.CreateParameter();
			param.ParameterName = "@LastName";
			param.Value = txtAddLName.Text;
			cmd.Parameters.Add(param);

			param = cmd.CreateParameter();
			param.ParameterName = "@FirstName";
			param.Value = txtAddFName.Text;
			cmd.Parameters.Add(param);

			param = cmd.CreateParameter();
			param.ParameterName = "@Address";
			param.Value = txtAddAddress.Text;
            cmd.Parameters.Add(param);
			cmd.Connection.Close();
		}

        private void displayPatients(string dbType) {
			try {
				IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
				cmd.CommandText = getSQL();
				cmd.Connection.Open();
				IDataReader dr = cmd.ExecuteReader();

				txtMsg.Text += "IDbConnection.State: " + cmd.Connection.State.ToString() + Environment.NewLine; ;
				txtMsg.Text += "IDataReader.IsClosed: " + dr.IsClosed + Environment.NewLine;
				txtMsg.Text += "cmd.CommandText: " + cmd.CommandText + Environment.NewLine + Environment.NewLine;

				txtPatients.Text = "";

				while (dr.Read()) {
					int id = dr.GetInt32(0);
					String lname = dr.GetString(1);
					String fname = dr.GetString(2);
					String address = dr.GetString(3);

					String patient = String.Format("{0,2:0} {1,-10:0} {2,-8:0} {3,-40:0}", id, lname, fname, address);
					txtPatients.Text += patient + Environment.NewLine;
				}

				dr.Close();
				cmd.Connection.Close();

			}
			catch (Exception ex) {
				txtMsg.Text = "\r\nError reading data\r\n";
				txtMsg.Text += ex.ToString();
			}
		}

		private String getSQL() {
			String sql =
				"SELECT " +
					"Patients.PatientID, " +
					"Patients.LastName, " +
					"Patients.FirstName, " +
					"Patients.Address " +
				"FROM " +
					"Patients " +
				"ORDER BY " + "Patients.LastName Asc";

			return sql;

		}

		private void clearInputFields() {
			txtAddLName.Text = "";
			txtAddFName.Text = "";
			txtAddAddress.Text = "";
			txtDelID.Text = "";
			txtUpdID.Text = "";
			txtUpdLName.Text = "";
			txtUpdFName.Text = "";
			txtUpdAddress.Text = "";
        }
        private void addParameter(string paramName, string paramValue, IDbCommand cmd) {
            // Create a parameter.
            IDbDataParameter param = cmd.CreateParameter();
            // Name the parameter.
            param.ParameterName = paramName;
            // Assign a value to the parameter.
            param.Value = paramValue;
            // Add the parameter to the command.
            cmd.Parameters.Add(param);
        }
		private void GenerateUpdatePatientSQL(IDbCommand cmd) {
			
			cmd.CommandText = "UPDATE " +
				"Patients " +
				"Set " +
				"LastName=@LastName, " +
				"FirstName=@FirstName, " +
				"Address=@Address " +
				"Where " +
				"PatientID=@PatientID";
			cmd.Parameters.Clear();
			
			addParameter("@LastName", txtUpdLName.Text, cmd);
			addParameter("@FirstName", txtUpdFName.Text, cmd);
			addParameter("@Address", txtUpdAddress.Text, cmd);
			addParameter("@PatientID", txtUpdID.Text, cmd);
		}
	}
}