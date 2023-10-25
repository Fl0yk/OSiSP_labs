using System.Data;

namespace OSiSP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void changeFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK )
            {
                try
                {
                    StreamReader streamReader = new StreamReader(ofd.FileName);
                    string line = streamReader.ReadLine();
                    if (line is null)
                        throw new Exception("Даже первая строка пустая...");

                    DataTable dataTable = new DataTable();
                    string[] args = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string arg in args) {
                        dataTable.Columns.Add(arg);
                    }

                    while((line = streamReader.ReadLine()) is not null)
                    {
                        dataTable.Rows.Add(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    }
                    
                    dataGridView1.DataSource = dataTable;
                }
                catch ( Exception ex )
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}