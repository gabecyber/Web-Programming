using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hw05 {
	public partial class Default : System.Web.UI.Page {
		string dbType = "Access_Property";
        protected void Page_Load(object sender, EventArgs e) {
			Double amount = 0;
            IDbCommand cmd = ConnectionFactory.GetCommand(dbType);
			
				if (rblSortType.SelectedIndex == 0) {
					cmd.CommandText = getSQL();
				
			}
				if (rblSortType.SelectedIndex == 1) {
					cmd.CommandText = getSQLSqFeet();
				
			}
			cmd.Connection.Open();
			IDataReader dr = cmd.ExecuteReader();
			List<Property> properties = BuildPropertiesList(dr);
			foreach (Property p in properties) {
				amount++;
			}
			dr.Close();
			cmd.Connection.Close();
			DisplayPropertiesList(properties);
			lblAveragePrice.Text = String.Format("{0,8:$0,0}", FindAverage(properties, amount));
			lblNumAboveAvgPrice.Text = numAboveAverage(properties, FindAverage(properties, amount)).ToString();
			lblNumProperties.Text = amount.ToString();








		}
		private Double FindAverage(List<Property> properties, Double amount) {
			Double currPriceAverage = 0;
			
			foreach (Property p in properties) {
				currPriceAverage += p.ListPrice;
				
			}
			Double FinalAverage = currPriceAverage / amount;
			return FinalAverage;
		}
		private Double numAboveAverage(List<Property> properties, Double FinalAverage ) {
			Double numAbove = 0;
			foreach (Property p in properties) {
				if (p.ListPrice > FinalAverage) {
					numAbove++;
				}

			}
			return numAbove;
		}
		protected void rblSortType_SelectedIndexChanged(object sender, EventArgs e) {
			
		}
        private List<Property> BuildPropertiesList(IDataReader dr) {
            List<Property> properties = new List<Property>();

            // Read the data from the data reader. Note that this is one-pass, forward only.
            while (dr.Read()) {
                Double price = dr.GetDouble(0);
                Double sqFeet = dr.GetDouble(1);
                Double beds = dr.GetDouble(2);
                Double baths = dr.GetDouble(3);
                Double year = dr.GetDouble(4);


                Property p = new Property(price, sqFeet, beds, baths, year);
                properties.Add(p);
            }
            return properties;


        }
		private String getSQLSqFeet() {
			String sql =
				"SELECT " +
					"Properties.ListPrice, " +
					"Properties.SqFeet, " +
					"Properties.Beds, " +
					"Properties.Baths, " +
					"Properties.YearBuilt " +
				"FROM " +
					"Properties " +
			"ORDER BY " +
				"Properties.SqFeet Asc ";


			return sql;

		}
		private String getSQL() {
			String sql =
				"SELECT " +
					"Properties.ListPrice, " +
					"Properties.SqFeet, " +
					"Properties.Beds, " +
					"Properties.Baths, " +
					"Properties.YearBuilt " +
				"FROM " +
					"Properties " +
			"ORDER BY " +
				"Properties.ListPrice Asc ";
		

			return sql;

		}
		private void DisplayPropertiesList(List<Property> properties) {
			txtProperties.Text = "";
			foreach (Property p in properties) {
				
				String prop = String.Format("{0,8:$0,0} {1,5:0}   {2,2:0}    {3,2:0}    {4,4:0}      {5,6:$0.00}", p.ListPrice, p.SqFeet, p.Beds, p.Baths, p.YearBuilt, p.PricePerSqFoot);
				txtProperties.Text += prop +
									 
							   Environment.NewLine;
			}
		}
		
	}
}