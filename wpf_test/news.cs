using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    public class news
    {
        public List<int> authorID = new List<int>();
        public List<string> theme = new List<string>();
        public List<string> textOfNews = new List<string>();
        public List<DateTime> createdOn = new List<DateTime>();
        public List<string> newsId = new List<string>();

        public void getTableNews()
        {
            authorID.Clear();
            theme.Clear();
            textOfNews.Clear();
            createdOn.Clear();
            newsId.Clear();

            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("USE Faculty SELECT * FROM News GO", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        authorID.Add(Convert.ToInt32(reader[0]));
                        theme.Add(reader[1].ToString());
                        textOfNews.Add(reader[2].ToString());
                        createdOn.Add(Convert.ToDateTime(reader[3]));
                        newsId.Add(reader[4].ToString());
                        Debug.WriteLine($"{authorID}\t|{theme}\t|{textOfNews}\t{createdOn}\t{newsId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}