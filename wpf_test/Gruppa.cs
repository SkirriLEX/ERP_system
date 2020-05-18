using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    public class Gruppa //+++++
    {
        private List<int> _specializationCode;
        private List<int> _codeGrup;
        private List<string> _nameGroup;
        private List<string> _tutor;

        //constructors
        public Gruppa()
        {
            _specializationCode = _codeGrup = new List<int>();
            _nameGroup = _tutor = new List<string>();
        }
        public Gruppa(int specializationCode, int codeGrup, string nameGroup, string tutor)
        {
            _specializationCode = _codeGrup = new List<int>();
            _nameGroup = _tutor = new List<string>();
            _specializationCode.Add(specializationCode);
            _codeGrup.Add(codeGrup);
            _nameGroup.Add(nameGroup);
            _tutor.Add(tutor);
        }
        //getters
        public List<int> GetSpecCode()
        {
            return _specializationCode;
        }
        public List<int> GetCodeGroup()
        {
            return _codeGrup;
        }
        public List<string> GetNameGroup()
        {
            return _nameGroup;
        }
        public List<string> GetTutor()
        {
            return _tutor;
        }

        public void GetTableGroup()
        {
            _specializationCode.Clear();
            _codeGrup.Clear();
            _nameGroup.Clear();
            _tutor.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Gruppa", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _specializationCode.Add(Convert.ToInt32(reader[0]));
                        _codeGrup.Add(Convert.ToInt32(reader[1]));
                        _nameGroup.Add(reader[2].ToString());
                        _tutor.Add(reader[3].ToString());
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]} ");
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
        public void InsertToTableGruppa(int specializationCode, int codeGrup, string nameGroup, string tutor)
        {
            _specializationCode.Clear();
            _codeGrup.Clear();
            _nameGroup.Clear();
            _tutor.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    //specializationCode (int), codeGrup (int), nameGroup (varchar(7)), tutor (varchar(20))
                    command.CommandText = $"INSERT INTO Gruppa (specializationCode, codeGrup, nameGroup, tutor)" +
                                          $"VALUES (@specializationCode, @codeGrup, @nameGroup, @tutor)";
                    command.Parameters.AddWithValue("@specializationCode", specializationCode);
                    command.Parameters.AddWithValue("@codeGrup", codeGrup);
                    command.Parameters.AddWithValue("@nameGroup", nameGroup);
                    command.Parameters.AddWithValue("@tutor", tutor);
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
                                _specializationCode.Add(Convert.ToInt32(reader[0]));
                                _codeGrup.Add(Convert.ToInt32(reader[1]));
                                _nameGroup.Add(reader[2].ToString());
                                _tutor.Add(reader[3].ToString());
                                Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]} ");
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

        public void SearchInTableInfLogin(string arg)//+++++
        {
            _specializationCode.Clear();
            _codeGrup.Clear();
            _nameGroup.Clear();
            _tutor.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                //specializationCode (int), codeGrup (int), nameGroup (varchar(7)), tutor (varchar(20))
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Gruppa where specializationCode like {arg} or" +
                                                        $"codeGrup like {arg} or nameGroup like '{arg}' or tutor like '{arg}'", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _specializationCode.Add(Convert.ToInt32(reader[0]));
                        _codeGrup.Add(Convert.ToInt32(reader[1]));
                        _nameGroup.Add(reader[2].ToString());
                        _tutor.Add(reader[3].ToString());
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|{reader[3]} ");
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