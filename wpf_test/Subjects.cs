using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    public class Subjects //++++++
    {
        private List<int> _codeTeacher;
        private List<int> _codeSpec;
        private List<string> _nameSubj;
        private List<int> _codeSubj;
        private List<float> _hoursForSubj;

        //constructor
        public Subjects()
        {
            _codeSpec = _codeSubj = _codeTeacher = new List<int>();
            _nameSubj = new List<string>();
            _hoursForSubj = new List<float>();
        }
        public Subjects(int codeTeacher, int codeSpec, int codeSubj,
            string nameSubj, float hoursForSubj)
        {
            _codeSpec = _codeSubj = _codeTeacher = new List<int>();
            _nameSubj = new List<string>();
            _hoursForSubj = new List<float>();
            _codeTeacher.Add(codeTeacher);
            _codeSpec.Add(codeSpec);
            _codeSubj.Add(codeSubj);
            _nameSubj.Add(nameSubj);
            _hoursForSubj.Add(hoursForSubj);
        }

        //getters
        public List<int> GetCodeTeacher()
        {
            return _codeTeacher;
        }
        public List<int> GetCodeSpec()
        {
            return _codeSpec;
        }
        public List<string> GetNameSubj()
        {
            return _nameSubj;
        }
        public List<int> GetCodeSubj()
        {
            return _codeSubj;
        }
        public List<float> GetHours()
        {
            return _hoursForSubj;
        }
        private void ClearAll()
        {
            _codeSpec.Clear();
            _codeTeacher.Clear();
            _codeSubj.Clear();
            _hoursForSubj.Clear();
            _nameSubj.Clear();
        }
        public void GetTableSubj()
        {
            ClearAll();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Subjects", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codeTeacher.Add(Convert.ToInt32(reader[0]));
                        _codeSpec.Add(Convert.ToInt32(reader[1]));
                        _nameSubj.Add(reader[2].ToString());
                        _codeSubj.Add(Convert.ToInt32(reader[3]));
                        _hoursForSubj.Add(Convert.ToSingle(reader[4]));
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]}\t|{reader[4]}");
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
        public void SearchInTableSubject(string arg)
        {
            ClearAll();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                //codeTeacher (int), codeSpec (int), nameSubj (varchar(25)), codeSubj (int), hoursForSubj (float)
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Subjects where codeTeacher like {arg} or " +
                                                    $"codeSpec like {arg} or" +
                                                    $"nameSubj like {arg} or" +
                                                    $"codeSubj like {arg} or" +
                                                    $"hoursForSubj like {arg}", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codeTeacher.Add(Convert.ToInt32(reader[0]));
                        _codeSpec.Add(Convert.ToInt32(reader[1]));
                        _nameSubj.Add(reader[2].ToString());
                        _codeSubj.Add(Convert.ToInt32(reader[3]));
                        _hoursForSubj.Add(Convert.ToSingle(reader[4]));
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]}\t|{reader[4]}");
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
        public void InsertToTableSubj(int codeTeacher, int codeSpec, int codeSubj,
            string nameSubj, float hoursForSubj)
        {
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"INSERT INTO Subjects (codeTeacher, codeSpec, nameSubj, codeSubj, hoursForSubj)" +
                                          $"VALUES (@codeTeacher, @codeSpec, @nameSubj, @codeSubj, @hoursForSubj)";
                    command.Parameters.AddWithValue("@codeTeacher", codeTeacher);
                    command.Parameters.AddWithValue("@codeSpec", codeSpec);
                    command.Parameters.AddWithValue("@nameSubj", codeSubj);
                    command.Parameters.AddWithValue("@codeSubj", nameSubj);
                    command.Parameters.AddWithValue("@hoursForSubj", hoursForSubj);
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
                                _codeTeacher.Add(Convert.ToInt32(reader[0]));
                                _codeSpec.Add(Convert.ToInt32(reader[1]));
                                _nameSubj.Add(reader[2].ToString());
                                _codeSubj.Add(Convert.ToInt32(reader[3]));
                                _hoursForSubj.Add(Convert.ToSingle(reader[4]));
                                Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]}\t|{reader[4]}");
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
    }
}