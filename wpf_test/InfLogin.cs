using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    public class InfLogin //++++++
    {
        private List<int> _tabNumPerson;
        private List<string> _loginStr;
        private List<string> _pass;

        public InfLogin()
        {
            _tabNumPerson = new List<int>();
            _loginStr = _pass = new List<string>();
        }

        public List<int> GetTabNumPerson()
        {
            return _tabNumPerson;
        }

        public List<string> GetLoginStr()
        {
            return _loginStr;
        }

        public List<string> GetPass()
        {
            return _pass;
        }

        public void GetTableInfLogin()
        {
            _tabNumPerson.Clear();
            _loginStr.Clear();
            _pass.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM InfLogin", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _tabNumPerson.Add(Convert.ToInt32(reader[0]));
                        _loginStr.Add(reader[1].ToString());
                        _pass.Add(reader[2].ToString());
                        Console.WriteLine($"{_tabNumPerson}\t|{_loginStr}\t|{_pass} ");
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

        public void InsertToTableInfLogin(int tabNumPerson, string loginStr, string pass)
        {
            _tabNumPerson.Clear();
            _loginStr.Clear();
            _pass.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO InfLogin (tabNumPerson, loginStr, pass)" +
                                          $"VALUES (@tabNumPerson, @loginStr, @pass)";
                    command.Parameters.AddWithValue("@tabNumPerson", tabNumPerson);
                    command.Parameters.AddWithValue("@loginStr", loginStr);
                    command.Parameters.AddWithValue("@pass", pass);
                    try
                    {
                        connection.Open();
                        var recordsAffected = command.ExecuteNonQuery();
                        using (var reader = command.ExecuteReader())
                        {
                            // while there is another record present
                            while (reader.Read())
                            {
                                // write the data on to the screen
                                _tabNumPerson.Add(Convert.ToInt32(reader[0]));
                                _loginStr.Add(reader[1].ToString());
                                _pass.Add(reader[2].ToString());
                                Console.WriteLine($"{reader[0]} \t | {reader[1]} \t | {reader[2]}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void SearchInTableInfLogin(string arg)
        {
            _tabNumPerson.Clear();
            _tabNumPerson.Clear();
            _loginStr.Clear();
            _pass.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM InfLogin where tabNumPerson like {arg} or"
                                             + $"loginStr like {arg} or pass like {arg}", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _tabNumPerson.Add(Convert.ToInt32(reader[0]));
                        _loginStr.Add(reader[1].ToString());
                        _pass.Add(reader[2].ToString());
                        Console.WriteLine($"{_tabNumPerson}\t|{_loginStr}\t|{_pass} ");
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