using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace erp{
    public class DataQ // all Function in our data connection 
    {
        protected readonly string ConnStr;
        public static readonly SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();

        public DataQ()
        {
            ConnStr = "178.136.14.234";
            Builder.DataSource = ConnStr;
            Builder.UserID = "resto";
            Builder.Password = "Resto#test01";
            Builder.InitialCatalog = "WIN-NALRE9SA668\\SQLEXPRESS";
        }

        public SqlConnectionStringBuilder getBuilder()
        {
            return Builder;
        }
        public bool tryConnect()
        {
            using (var connection = new SqlConnection(Builder.ConnectionString))
            {
                try
                {
                    Debug.Write(@"Connecting to SQL Server ... ");
                    var Builder = getBuilder();

                    connection.Open();
                    Debug.WriteLine("Done.");
                    return true;

                }
                catch (SqlException exception)
                {
                    DisplaySqlErrors(exception);
                    MessageBox.Show($"Нет связи с сервером!\n{exception}");
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private static void DisplaySqlErrors(SqlException exception)
        {
            for (var i = 0; i < exception.Errors.Count; i++)
            {
                Debug.WriteLine("Index #" + i + "\n" +
                                "Source: " + exception.Errors[i].Source + "\n" +
                                "Number: " + exception.Errors[i].Number + "\n" +
                                "State: " + exception.Errors[i].State + "\n" +
                                "Class: " + exception.Errors[i].Class + "\n" +
                                "Server: " + exception.Errors[i].Server + "\n" +
                                "Message: " + exception.Errors[i].Message + "\n" +
                                "Procedure: " + exception.Errors[i].Procedure + "\n" +
                                "LineNumber: " + exception.Errors[i].LineNumber);
            }
        }
        public bool Check(string login, string pass)
        {
            if (!tryConnect()) return false;
            using (var connection = new SqlConnection(Builder.ConnectionString))
            {
                var cmdText = "use ERP_system" +
                              $"select count(1) from InfLogin where loginStr like {login} and pass like {pass}";
                var command = new SqlCommand(cmdText, connection);
                // Add the parameters.
                command.Parameters.Add(new SqlParameter("0", 1));
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]) > 0;
                        // call the objects from their index
                        //reader[0], reader[1], reader[2], reader[3]));
                    }
                }
            }
            return false;
        }
    }
}