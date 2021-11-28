using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            path = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            table = new DataTable();
            FillOrdersList();
        }

        private void FillOrdersList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(path))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Orders", connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbxOrdersList.Items.Add(reader.GetString(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnGetOrders_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(path))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT C.CustomerID, CompanyName FROM Orders AS O ");
                    sb.Append("JOIN Customers AS C ");
                    sb.Append("ON C.CustomerID = O.CustomerID ");
                    sb.Append("WHERE O.CustomerID = @p1 ");
                    sb.Append("GROUP BY C.CustomerID, CompanyName");

                    SqlDataAdapter adapter = new SqlDataAdapter(sb.ToString(), connection);

                    adapter.SelectCommand.Parameters.AddWithValue("@p1", cbxOrdersList.SelectedItem);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dataGridView1.ClearSelection();

                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOrderDetails_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(path))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT OD.* FROM Orders AS O ");
                    sb.Append("JOIN [Order Details] AS OD ");
                    sb.Append("ON OD.OrderID = O.OrderID ");
                    sb.Append("WHERE O.CustomerID = @p1 ");

                    SqlDataAdapter adapter = new SqlDataAdapter(sb.ToString(), connection);

                    adapter.SelectCommand.Parameters.AddWithValue("@p1", cbxOrdersList.SelectedItem);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dataGridView1.ClearSelection();

                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        SqlCommandBuilder command;
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable table;
        DataSet set;
        string path;
    }
}
