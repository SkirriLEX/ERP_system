using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    public class Speciality 
    {
        private List<int> _code;
        private List<string> _name;

        public Speciality()
        {
            _code = new List<int>();
            _name = new List<string>();
        }

        public List<int> GetCode()
        {
            return _code;
        }

        public List<string> GetName()
        {
            return _name;
        }

        public void GetTableSpeciality()
        {
            _code?.Clear();
            _name?.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Speciality", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _code.Add(Convert.ToInt32(reader[0]));
                        _name.Add(reader[1].ToString());
                        Console.WriteLine($"{reader[0]} \t | {reader[1]} \n");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine($"Нет связи с сервером!\n{ex}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void InsertToTableSpeciality(string code, string name)
        {
            _code.Clear();
            _name.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection; // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Speciality (specialityCode, nameSpec) " +
                                          "values (@specialityCode, @nameSpec)";
                    command.Parameters.AddWithValue("@specialityCode", code);
                    command.Parameters.AddWithValue("@nameSpec", name);

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
                                _code.Add(Convert.ToInt32(reader[0]));
                                _name.Add(reader[1].ToString());
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        DataQ.DisplaySqlErrors(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void SearchInTableSpeciality(string arg) //return an array with defined argument
        {
            _code.Clear();
            _name.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Speciality", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _code.Add(Convert.ToInt32(reader[0]));
                        _name.Add(reader[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine($"Нет связи с сервером!\n{ex}");
            }
            finally
            {
                connection.Close();
            }

            var index = new List<int>();
            if (int.TryParse(arg, out var codeSpec))
            {
                for (var i = 0; i < _code.Count; i++)
                    if (_code[i] == codeSpec)
                        index.Add(i);
            }
            else
            {
                for (var i = 0; i < _code.Count; i++)
                    if (_name[i] == arg)
                        index.Add(i);
            }

            var nameTmp = new List<string>();
            var codeTmp = new List<int>();

            foreach (var t in index)
            {
                nameTmp.Add(_name[t]);
                codeTmp.Add(_code[t]);
            }

            _name.Clear();
            _name = nameTmp;
            _code.Clear();
            _code = codeTmp;

            for (var i = 0; i < _name.Count; i++)
            {
                Console.WriteLine(_code[i] + "\t|\t" + _name[i]);
            }
        }
    }
}