using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace csv_Parser
{
    public partial class Queries : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private void EstablishConnection()
        {
            connection = new SqlConnection(@"Data Source=31.31.196.234;Initial Catalog=u0979199_springer_data;Persist Security Info=True;User ID=u0979199_spender;Password=LErwjfu4c9");
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
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
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
                chapter_str = "'Chapter', 'Conference Paper'";
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
                chapter_str = "Chapter, Conference Paper";
            }
            else
            {
                comma_str = ", ";
                chapter_str = "'Chapter', 'Conference Paper'";
                article_str = "'Article'";
            }
            

            if (cbYear.Checked && CheckYear())
            {
                int min_year = int.Parse(tbMinYear.Text);
                int max_year = int.Parse(tbMaxYear.Text);

                query = "select title " +
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
                $"where item_title like '{tbSource.Text}') " +
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
                string keywords = GetKeywords();
                query = "select count(publication_id) as 'num'" +
                        "from publications_keywords " +
                        "where keyword_id in ( " +
                        "select id " +
                        "from keywords " +
                        "where " +
                        $"lower(keyword) in ({keywords}));";

                return new SqlDataAdapter(query, connection);
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

            return false;
        }
        public string GetKeywords()
        {
            char[] delimiter = { ',' };
            string[] keywords = tbKeywords.Text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder keys = new StringBuilder("");
            foreach(string el in keywords)
            {
                keys.Append($"'{el.ToLower()}', ");
            }
            // Убрать запятую и пробел в конце.
            keys.Remove(keys.Length - 2, 2);

            return keys.ToString();
        }
        /*
         --количество публикаций по годам издания
select count(*) from publications 
where 
  year between 1980 and 2020;
  
  
--количество публикаций по источникам публикаций и годам (журналы)
select count(*) as 'num'
from publications
where 
  year between 1950 and 2020
  and id in (
  select publication_id 
  from publications_sources
  where source_id = (
    select id from sources 
    where item_title like 'The methods of breeding and the productive value of camels')
  intersect
  select publication_id
  from publications_types
  where
    type_id in (
    select id
    from types 
    where type_name = 'Article'));
             */
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

                cbSource.Enabled = false;
                cbSource.Checked = false;
                tbSource.Enabled = false;

                cbAuthorsNumber.Enabled = false;
                cbAuthorsNumber.Checked = false;
                tbAuthorsNumber.Enabled = false;

                cbChapter.Enabled = false;
                cbChapter.Checked = false;
                cbArticle.Enabled = false;

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

                cbYear.Enabled = false;
                cbYear.Checked = false;
                tbMaxYear.Enabled = false;
                tbMinYear.Enabled = false;

                cbChapter.Enabled = false;
                cbChapter.Checked = false;
                cbArticle.Enabled = false;
                cbArticle.Checked = false;

                cbSource.Enabled = false;
                cbSource.Checked = false;
                tbSource.Enabled = false;

                cbAuthorsNumber.Enabled = false;
                cbAuthorsNumber.Checked = false;
                tbAuthorsNumber.Enabled = false;


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
    }
}
