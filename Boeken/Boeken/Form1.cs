using Database_Connect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boeken
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySQLDB Database = new MySQLDB();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Database.Connect("localhost", "books", "root", ""))
            {
                Query query = new Query(Query.Database.MySQLDB);
                query.From("book");

                List<Dictionary<string, object>> result = Database.Select(query.Build());

                if (result.Count > 0)
                {
                    Dictionary<string, object> firstrow = result[0];
                    foreach(KeyValuePair<string, object> pair in firstrow)
                    {
                        dataGridView1.Columns.Add("dg" + pair.Key, pair.Key);
                    }
                }

                foreach (Dictionary<string, object> dbrow in result)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    //DataGridViewCell cell = new DataGridViewCell();
                    foreach (KeyValuePair<string, object> pair in dbrow)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = pair.Value.ToString() });
                    }

                    this.dataGridView1.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("Verdomme");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, object>> saved = new List<Dictionary<string, object>>();


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Dictionary<string, object> dbrow = new Dictionary<string, object>();

                foreach(DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {

                        string column = cell.OwningColumn.HeaderText;

                        dbrow.Add(column, cell.Value.ToString());
                    }
                }

                if (dbrow.Count > 0)
                {
                    saved.Add(dbrow);
                }
            }
            
            foreach (Dictionary<string, object> dbrow in saved)
            {
                int id = Convert.ToInt32(dbrow["id"]);
                dbrow.Remove("id");
                Database.Update(dbrow , "book", id);
            }
        }
    }
}
