using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace csv_Parser
{
    public partial class Main : Form
    {
        SqlCommand cmd;
        SqlConnection con;

        string fileName = null;
        public Main()
        {
            InitializeComponent();
        }
        private void EstablishConnection()
        {
            con = new SqlConnection(@"Data Source=31.31.196.234;Initial Catalog=u0979199_springer_data;Persist Security Info=True;User ID=u0979199_spender;Password=LErwjfu4c9");
            con.Open();
        }

        private void btnSetPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "csv files (*.csv)|*.csv";
            fileDialog.FileName = txtPath.Text;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fileDialog.FileName;
                fileName = fileDialog.FileName;
                btnLoad.Enabled = true;
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            label1.Visible = true;          

            using (StreamReader sr = new StreamReader(fileName))
            {
                sr.ReadLine();
                string currentLine;



                cmd = new SqlCommand("select count(*)" +
                    "from publications");
                cmd.Connection = con;              
                int publication_id = (int)cmd.ExecuteScalar() + 1;

                while ((currentLine = sr.ReadLine()) != null)
                {
                    currentLine = currentLine.Replace("\'", "`");
                    ParseString(currentLine, publication_id);
                    publication_id++;
                }
            }
            label1.Visible = false;
            MessageBox.Show("Готово!");
            con.Close();
        }
        private void ParseString(string fileLine, int publication_id)
        {

            Regex linePattern = new Regex("(?<item_title>\".*\"),(?<publication_title>\".*\"),(?<book_series_title>\".*\"),(?<journal_volume>\".*\"),(?<journal_issue>\".*\"),(?<item_DOI>\".*\"),(?<authors>\".*\"),(?<publication_year>\".*\"),(?<url>\".*\"),(?<content_type>\".*\")");
            Match patternMatch = linePattern.Match(fileLine);

            if (patternMatch.Success)
            {
                string item_title = patternMatch.Groups["item_title"].Value;
                string publication_title = patternMatch.Groups["publication_title"].Value;
                string book_series_title = patternMatch.Groups["book_series_title"].Value;
                string journal_volume = patternMatch.Groups["journal_volume"].Value;
                string journal_issue = patternMatch.Groups["journal_issue"].Value;
                string item_DOI = patternMatch.Groups["item_DOI"].Value;
                string authors = patternMatch.Groups["authors"].Value;
                string publication_year = patternMatch.Groups["publication_year"].Value;
                string url = patternMatch.Groups["url"].Value;
                string content_type = patternMatch.Groups["content_type"].Value;


                InsertToPublications(publication_id, publication_title, publication_year);
                FillAuthors(authors, publication_id);
                FillSource(item_title, book_series_title, journal_volume, journal_issue, publication_id);
                FillTypes(content_type, publication_id);
                FillKeywords(publication_title, publication_id);

            }
            else
            {
                throw new Exception("Regex does not match" + "\\" + fileLine);
            }


        }
        public void InsertToPublications(int i, string publication_title, string publication_year)
        {
            publication_title = publication_title.Replace("\"", "");
            publication_year = publication_year.Replace("\"", " ");
            int a;
          
            if (int.TryParse(publication_year, out a))
            {
                int year = int.Parse(publication_year);
                cmd = new SqlCommand("insert into publications(id, title, year) " +
                   $"values({i},'{publication_title}',{year})");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd = new SqlCommand("insert into publications(id, title, year) " +
                   $"values({i},'{publication_title}','')");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }

            
        }
        public void FillAuthors(string authors, int publication_id)
        {
            string[] names = GetAuthorsNames(authors);

            // Для определения индекса считаем количество уже имеющихся элементов
            cmd = new SqlCommand("select count(id) from authors");
            cmd.Connection = con;
            int current_id = (int)cmd.ExecuteScalar();
            current_id++;


            foreach (string author in names)
            {
                int idx = 0;

                // Определяем, есть ли уже этот автор в базе
                cmd = new SqlCommand($"select count(*) from authors where initials like '{author}'");
                cmd.Connection = con;

                // Если этого автора нет в таблице authors
                if ((int)cmd.ExecuteScalar() == 0)
                {
                    // Добавляем его в таблицу authors
                    cmd = new SqlCommand("insert into authors(id, initials) " +
                        $"values({current_id},'{author}')");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();


                    // Добавляем запись в таблицу publications_authors
                    cmd = new SqlCommand("insert into publications_authors(author_id,publication_id)" +
                        $"values({current_id}, {publication_id})");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
   
                    current_id++;
                }
                // Если такой автор в базе есть
                else
                {
                    // Ищем его id
                    cmd = new SqlCommand($"select id from authors where initials like '{author}'");
                    cmd.Connection = con;
                    idx = (int)cmd.ExecuteScalar();

                    // Проверяем, связан ли этоn автор с публикацией уже
                    cmd = new SqlCommand("select count(*) " +
                        "from publications_authors " +
                        $"where author_id = {idx}" +
                        $"and publication_id = {publication_id}");
                    cmd.Connection = con;

                    // Если нет, то добавляем связь
                    if((int)cmd.ExecuteScalar() == 0)
                    {
                        cmd = new SqlCommand("insert into publications_authors(author_id,publication_id)" +
                        $"values({idx}, {publication_id})");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    // Если да, то не делаем ничего, такая запись уже есть.
                }
            }
        }
        public string[] GetAuthorsNames(string authors)
        {
            // Здесь кроется ловушка. Имена авторов в csv файле склеиваются следующим образом:
            // Maya JohnHadil Shaiba  <- Maya John, Hadil Shaiba
            // то есть, имя первого автора склеивается с фамилией второго автора.
            // Необходимо разделить их и правильно выписать имена и фамилии авторов.
            // Для этого был выбран следующий алгоритм:
            // 1) строка с авторами становится stringBuilder
            // 2) stringBuilder перебирается по паре символов: первый-второй, второй-третий и т.д.
            // когда находится такая пара, что слева маленькая буква, а справа - большая, между ними вставляется нейтральный символ, например, нолик (0).
            // 3) обработанный stringBuilder становится строкой. Строка сплитится через нолик в массив строк. Каждый элемент - имя автора.


            authors = authors.Replace("\"", "");
            StringBuilder a = new StringBuilder(authors);
            int add = 1;

            for (int i = 0; i < authors.Length - 1; i++)
            {
                if (Char.IsLetter(authors[i]) && !Char.IsUpper(authors[i]) && Char.IsUpper(authors[i + 1]))
                {
                    a.Insert(i + add, '0');
                    add++;
                }
            }
            string line = a.ToString();
            return line.Split('0');

        }

        public void FillSource(string item_title, string book_series_title, string journal_volume, string journal_issue, int publication_id)
        {
            int idx = 0;

            item_title = item_title.Replace("\"", "");
            book_series_title = book_series_title.Replace("\"", "");

            cmd = new SqlCommand("select count(id) from sources");
            cmd.Connection = con;
            int current_id = (int)cmd.ExecuteScalar();
            current_id++;

            cmd = new SqlCommand($"select count(*) from sources where item_title like '{item_title}'");
            cmd.Connection = con;

            // Если в таблице sources этого источника нет.
            if ((int)cmd.ExecuteScalar() == 0)
            {
                // To sources.
                
                int a;
                journal_volume = journal_volume.Replace("\"", "");
                journal_issue = journal_issue.Replace("\"", "");

                if (int.TryParse(journal_volume, out a))
                {
                    int volume = int.Parse(journal_volume);
                    if (int.TryParse(journal_issue, out a))
                    {
                        int issue = int.Parse(journal_issue);
                        cmd = new SqlCommand("insert into sources(id,item_title,book_series_title,journal_volume,journal_issue)" +
                           $"values({current_id},'{item_title}', '{book_series_title}', {volume}, {issue})");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new SqlCommand("insert into sources(id,item_title,book_series_title,journal_volume,journal_issue)" +
                           $"values({current_id},'{item_title}', '{book_series_title}', {volume}, '')");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    if (int.TryParse(journal_issue, out a))
                    {
                        int issue = int.Parse(journal_issue);
                        cmd = new SqlCommand("insert into sources(id,item_title,book_series_title,journal_volume,journal_issue)" +
                           $"values({current_id},'{item_title}', '{book_series_title}', '', {issue})");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new SqlCommand("insert into sources(id,item_title,book_series_title,journal_volume,journal_issue)" +
                           $"values({current_id},'{item_title}', '{book_series_title}', '', '')");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                } 
                
                // For publications_sources
                idx = current_id;
            }
            else
            {
                cmd = new SqlCommand($"select id from sources where item_title like '{item_title}'");
                cmd.Connection = con;

                idx = (int)cmd.ExecuteScalar();
            }

            // To publications_sources
            cmd = new SqlCommand("insert into publications_sources(publication_id,source_id)" +
                $"values({publication_id}, {idx})");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();

        }
        public void FillTypes(string content_type, int publication_id)
        {
            int idx = 0;

            content_type = content_type.Replace("\"", "");
            if (content_type != "Book")
            {
                cmd = new SqlCommand("select count(id) from types");
                cmd.Connection = con;
                int current_id = (int)cmd.ExecuteScalar();
                current_id++;

                cmd = new SqlCommand($"select count(*) from types where type_name like '{content_type}'");
                cmd.Connection = con;
                if ((int)cmd.ExecuteScalar() == 0)
                {
                    // To types.
                    cmd = new SqlCommand("insert into types(id, type_name) " +
                   $"values({current_id},'{content_type}')");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    // For publication_type_link.
                    idx = current_id;

                }
                else
                {
                    cmd = new SqlCommand($"select id from types where type_name like '{content_type}'");
                    cmd.Connection = con;

                    idx = (int)cmd.ExecuteScalar();
                }
                // To publication_type_link.
                cmd = new SqlCommand("insert into publications_types(publication_id,type_id)" +
                    $"values({publication_id}, {idx})");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }       

        }
        public void FillKeywords(string publication_title, int publication_id)
        {
            int idx = 0; 
            
            publication_title = publication_title.Replace("\"", "");
            string[] keyInstances = publication_title.Split(new[] { '-', ':', ',', ';', '.'});

            for(int i = 0; i < keyInstances.Length; i++)
            {

                string[] keywords = FillKeywords(keyInstances[i]);

                cmd = new SqlCommand("select count(id) from keywords");
                cmd.Connection = con;
                int current_id = (int)cmd.ExecuteScalar();
                current_id++;

                foreach (string word in keywords)
                {
                    cmd = new SqlCommand($"select count(*) from keywords where keyword like '{word}'");
                    cmd.Connection = con;
                    // Если данное слово не записано в таблицу keywords
                    if ((int)cmd.ExecuteScalar() == 0)
                    {
                        // Добавляем это слово в таблицу keywords
                        cmd = new SqlCommand("insert into keywords(id, keyword) " +
                       $"values({current_id},'{word}')");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                        // Добавляем связь этого слова с публикацией в таблицу publications_keywords
                        cmd = new SqlCommand("insert into publications_keywords(publication_id,keyword_id)" +
                        $"values({publication_id}, {current_id})");
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                        current_id++;
                    }
                    // Если данное слово уже есть в таблице keywords
                    else
                    { 
                        // 1. Берем его id
                        cmd = new SqlCommand($"select id from keywords where keyword like '{word}'");
                        cmd.Connection = con;
                        idx = (int)cmd.ExecuteScalar();

                        // 2. Проверяем, связано ли это слово с публикацией уже
                        cmd = new SqlCommand("select count(*) from publications_keywords " +
                            $"where keyword_id = {idx}" +
                            $"and publication_id = {publication_id} ");
                        cmd.Connection = con;

                        // Если нет, то добавляем запись
                        if ((int)cmd.ExecuteScalar() == 0)
                        {
                            cmd = new SqlCommand("insert into publications_keywords(publication_id,keyword_id)" +
                        $"values({publication_id}, {idx})");
                            cmd.Connection = con;
                            cmd.ExecuteNonQuery();
                        }
                        // Если да, то не делаем ничего
                    }                  
                }
            }       
        }
        private string[] FillKeywords(string line)
        {
            List<string> keywords = new List<string>();

            string prepositions = "a an and of in the at from with into for on by about but $ & @";
            string[] allWords = line.Split(' ');

            foreach(string word in allWords)
            {
                if (!prepositions.Contains(word))
                {
                    keywords.Add(word);
                }
            }

            return keywords.ToArray();
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            EstablishConnection();


            cmd = new SqlCommand("drop table publications_authors;" +
                "drop table publications_keywords;" +
                "drop table publications_sources;" +
                "drop table publications_types;");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("truncate table sources;" +
                "truncate table types;" +
                "truncate table authors;" +
                "truncate table publications;" +
                "truncate table keywords;");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("create table publications_authors(" +
                "author_id int references authors(id)," +
                "publication_id int references publications(id)," +
                "primary key (author_id, publication_id)" +
                ");");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("create table publications_keywords(" +
                "publication_id int references publications(id)," +
                "keyword_id int references keywords(id)," +
                "primary key (publication_id, keyword_id)" +
                ");");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("create table publications_sources(" +
                "publication_id int references publications(id)," +
                "source_id int references sources(id)," +
                "primary key (publication_id, source_id)" +
                ");");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("create table publications_types(" +
                "publication_id int references publications(id)," +
                "type_id int references types(id)," +
                "primary key (publication_id, type_id)" +
                ");");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();

            MessageBox.Show("База очищена");

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            EstablishConnection();
            // Проверить, пуста ли база
            cmd = new SqlCommand("select count(*)" +
                "from publications");
            cmd.Connection = con;
            if ((int)cmd.ExecuteScalar() == 0)
            {
                MessageBox.Show("База пуста");
            }
            else
            {
                Queries queries = new Queries();
                queries.ShowDialog();
            }            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            EstablishConnection();
            btnLoad.Enabled = false;
        }
    }
}
