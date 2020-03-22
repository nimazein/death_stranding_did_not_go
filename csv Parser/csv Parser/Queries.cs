using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;

namespace csv_Parser
{
    public partial class Queries : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private System.Data.DataTable dataTable;
        private void EstablishConnection()
        {
            connection = new SqlConnection(@"Data Source=31.31.196.234;Initial Catalog=u0979199_springer_data;Persist Security Info=True;User ID=u0979199_spender;Password=*****");
            connection.Open();
        }
        public Queries()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            EstablishConnection();
            dataAdapter = GenerateQuery();
            if (dataAdapter != null)
            {
                dataTable = new System.Data.DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            
        }
        private SqlDataAdapter GenerateQuery()
        {
            string query = "";
            string comma_str = "";
            string article_str = "";
            string chapter_str = "";


            if (cbArticle.Checked && cbChapter.Checked)
            {
                comma_str = ", ";
                chapter_str = "'Chapter','ConferencePaper','ReferenceWorkEntry'";
                article_str = "'Article'";
            }
            else if (cbArticle.Checked)
            {
                comma_str = "";
                article_str = "'Article'";
                chapter_str = "";
            }
            else if (cbChapter.Checked)
            {
                comma_str = "";
                article_str = "";
                chapter_str = "'Chapter', 'ConferencePaper', 'ReferenceWorkEntry'";
            }
            else
            {
                comma_str = ", ";
                chapter_str = "'Chapter', 'ConferencePaper', 'ReferenceWorkEntry' ";
                article_str = "'Article'";
            }
            

            if (cbYear.Checked && CheckYear())
            {
                int min_year = int.Parse(tbMinYear.Text);
                int max_year = int.Parse(tbMaxYear.Text);

                query = "select " +
                    "distinct year, " +
                    "count(*) over (partition by year) as 'num' " +
                    "from publications " +
                    $"where year between {min_year} and {max_year}";

                if (cbSource.Checked && tbSource.Text != "")
                {
                    query = "select count(*) as 'num' " +
                "from publications " +
                "where " +
                $"year between {min_year} and {max_year} " +
                "and id in ( " +
                "select publication_id " +
                "from publications_sources " +
                "where source_id = ( " +
                "select id from sources " +
                $"where item_title like '%{tbSource.Text}%') " +
                "intersect " +
                "select publication_id " +
                "from publications_types " +
                "where " +
                "type_id in ( " +
                "select id " +
                "from types " +
                $"where type_name in ({article_str}{comma_str}{chapter_str})))";

                }             
                if (cbAuthorsNumber.Checked && CheckNumber(tbAuthorsNumber.Text))
                {
                    int num = int.Parse(tbAuthorsNumber.Text);
                    query = "select count(*) as 'num' " +
                        " from publications " +
                        "where " +
                        $"year between {min_year} and {max_year} " +
                        "and id in ( " +
                        "select publication_id " +
                        "from publications_authors " +
                        "group by publication_id " +
                        $"having count(author_id) = {num}); ";                      
                }

                return new SqlDataAdapter(query, connection);
            }
            if (cbKeywords.Checked && tbKeywords.Text != "")
            {
                SqlDataAdapter sqlAd;
                dataTable = new System.Data.DataTable();
                string[] keywords = GetKeywords();
                foreach(string el in keywords)
                {
                    query = "select distinct k.keyword, " +
                    "count(*) over (partition by p_k.keyword_id) as 'num' " +
                    "from publications_keywords p_k " +
                    "join keywords k " +
                    "on k.id = p_k.keyword_id " +
                    $"where lower(k.keyword) in ('{el}'); ";

                    sqlAd = new SqlDataAdapter(query, connection);   
                    sqlAd.Fill(dataTable);
                }
                
                dataGridView1.DataSource = dataTable;

                return null;
            }
            if (cbUnique.Checked)
            {
                query = "select distinct p.title, a.initials as 'author', s.item_title as 'source', p.year, s.journal_issue " +
                    "from publications p " +
                    "join publications_authors p_a " +
                    "on p.id = p_a.publication_id " +
                    "join authors a " +
                    "on a.id = p_a.author_id " +
                    "join publications_sources p_s " +
                    "on p.id = p_s.publication_id " +
                    "join sources s " +
                    "on p_s.source_id = s.id ";

                return new SqlDataAdapter(query, connection);
            }
            
            
            return null;

            //SqlCommand("select");   
        }
        private bool CheckNumber(string authorsNumber)
        {
            int a;

            if (int.TryParse(authorsNumber, out a))
            {
                int num = int.Parse(authorsNumber);
                if (num > 0 && num < 100)
                {
                    return true;
                }
            }
            MessageBox.Show("Количество авторов введено некорректно");
            return false;
        }
        public string[] GetKeywords()
        {
            char[] delimiter = { ',' };
            string[] keywords = tbKeywords.Text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keywords.Length; i++)
            {
                keywords[i] = keywords[i].ToLower().Replace(" ","");
            }
            return keywords;
        }
        private bool CheckYear()
        {
            string min_year_str = tbMinYear.Text;
            string max_year_str = tbMaxYear.Text;

            if (min_year_str != "" && max_year_str != "")
            {
                int a;
                if (int.TryParse(min_year_str, out a) && int.TryParse(max_year_str, out a))
                {
                    int min_year = int.Parse(min_year_str);
                    int max_year = int.Parse(max_year_str);

                    if (min_year >= 1900 && max_year <= 2020)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Queries_Load(object sender, EventArgs e)
        {
            
        }

        #region inputChecks
        private void cbYear_CheckedChanged(object sender, EventArgs e)
        {
            if (cbYear.Checked)
            {
                cbUnique.Enabled = false;
                cbUnique.Checked = false;

                cbKeywords.Enabled = false;
                cbKeywords.Checked = false;
                tbKeywords.Enabled = false;
                tbKeywords.Clear();
            }
            else
            {
                cbUnique.Enabled = true;
                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;
            }
            
        }

        private void cbKeywords_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeywords.Checked)
            {
                cbUnique.Checked = false;
                cbUnique.Enabled = false;
                
                cbYear.Enabled = false;
                cbYear.Checked = false;
                tbMinYear.Enabled = false;
                tbMaxYear.Enabled = false;
                tbMaxYear.Clear();
                tbMinYear.Clear();

                cbSource.Enabled = false;
                cbSource.Checked = false;
                tbSource.Enabled = false;
                tbSource.Clear();

                cbAuthorsNumber.Enabled = false;
                cbAuthorsNumber.Checked = false;
                tbAuthorsNumber.Enabled = false;
                tbAuthorsNumber.Clear();

                cbChapter.Enabled = false;
                cbChapter.Checked = false;
                cbArticle.Enabled = false;
                cbArticle.Checked = false;

            }
            else
            {
                cbUnique.Enabled = true;

                cbYear.Enabled = true;
                tbMinYear.Enabled = true;
                tbMaxYear.Enabled = true;

                cbSource.Enabled = true;
                tbSource.Enabled = true;

                cbAuthorsNumber.Enabled = true;
                tbAuthorsNumber.Enabled = true;

                cbChapter.Enabled = true;
                cbArticle.Enabled = true;

            }
        }

        private void cbSource_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSource.Checked)
            {
                cbAuthorsNumber.Enabled = false;
                cbAuthorsNumber.Checked = false;
                tbAuthorsNumber.Enabled = false;

                cbUnique.Enabled = false;
                cbUnique.Checked = false;

                cbKeywords.Enabled = false;
                cbKeywords.Checked = false;
                tbKeywords.Enabled = false;
                tbKeywords.Clear();
            }
            else
            {
                cbAuthorsNumber.Enabled = true;
                tbAuthorsNumber.Enabled = true;

                cbUnique.Enabled = true;

                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;


            }
        }

        private void cbAuthorsNumber_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAuthorsNumber.Checked)
            {
                cbUnique.Enabled = false;
                cbUnique.Checked = false;

                cbKeywords.Checked = false;
                cbKeywords.Enabled = false;              
                tbKeywords.Enabled = false;
                tbKeywords.Clear();

                cbChapter.Enabled = false;
                cbChapter.Checked = false;
                cbArticle.Enabled = false;
                cbArticle.Checked = false;

                cbSource.Enabled = false;
                cbSource.Checked = false;
                tbSource.Enabled = false;
            }
            else
            {
                cbUnique.Enabled = true;

                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;

                cbChapter.Enabled = true;
                cbArticle.Enabled = true;

                cbSource.Enabled = true;
                tbSource.Enabled = true;
            }
        
        }

        private void cbChapter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbChapter.Checked)
            {
                cbUnique.Enabled = false;
                cbUnique.Checked = false;

                cbKeywords.Enabled = false;
                cbKeywords.Checked = false;
                tbKeywords.Enabled = false;

            }
            else
            {
                cbUnique.Enabled = true;

                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;
            }

        }

        private void cbArticle_CheckedChanged(object sender, EventArgs e)
        {
            if (cbChapter.Checked)
            {
                cbUnique.Enabled = false;
                cbUnique.Checked = false;

                cbKeywords.Enabled = false;
                cbKeywords.Checked = false;
                tbKeywords.Enabled = false;

                cbYear.Enabled = false;
                cbYear.Checked = false;
                tbMaxYear.Enabled = false;
                tbMinYear.Enabled = false;


            }
            else
            {
                cbUnique.Enabled = true;

                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;

                cbYear.Enabled = true;
                tbMaxYear.Enabled = true;
                tbMinYear.Enabled = true;
            }
        }

        private void cbUnique_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUnique.Checked)
            {
                cbKeywords.Enabled = false;
                cbKeywords.Checked = false;
                tbKeywords.Enabled = false;
                tbKeywords.Clear();

                cbYear.Enabled = false;
                cbYear.Checked = false;
                tbMaxYear.Enabled = false;
                tbMinYear.Enabled = false;
                tbMaxYear.Clear();
                tbMinYear.Clear();

                cbChapter.Enabled = false;
                cbChapter.Checked = false;
                cbArticle.Enabled = false;
                cbArticle.Checked = false;

                cbSource.Enabled = false;
                cbSource.Checked = false;
                tbSource.Enabled = false;
                tbSource.Clear();

                cbAuthorsNumber.Enabled = false;
                cbAuthorsNumber.Checked = false;
                tbAuthorsNumber.Enabled = false;
                tbAuthorsNumber.Clear();


            }
            else
            {
                cbKeywords.Enabled = true;
                tbKeywords.Enabled = true;

                cbYear.Enabled = true;
                tbMaxYear.Enabled = true;
                tbMinYear.Enabled = true;

                cbChapter.Enabled = true;
                cbArticle.Enabled = true;

                cbSource.Enabled = true;
                tbSource.Enabled = true;

                cbAuthorsNumber.Enabled = true;
                cbAuthorsNumber.Checked = true;
                tbAuthorsNumber.Enabled = true;
            }
        }
        #endregion

        private void tbKeywords_TextChanged(object sender, EventArgs e)
        {

        }
        private Microsoft.Office.Interop.Excel.Application app;
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (app == null)
                {
                    app = new Microsoft.Office.Interop.Excel.Application();
                    app.Application.Workbooks.Add(Type.Missing);
                }
                    
                
                // Запись заголовков 
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    app.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        app.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                app.Visible = true;
            }

        }

        private void Queries_FormClosed(object sender, FormClosedEventArgs e)
        {
            app.Quit();
        }
    }
}
