using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace erp
{
    public class Specialization //+++++
    {
        private List<int> _specialityCode = new List<int>();
        private List<int> _specializationCode = new List<int>();
        private List<string> _nameSpecialization = new List<string>();

        public Specialization()
        {
            _specialityCode.Clear();
            _specializationCode.Clear();
            _nameSpecialization.Clear();
        }

        public Specialization(int specialityCode, int specializationCode, string nameSpecialization)
        {
            _specialityCode.Clear();
            _specializationCode.Clear();
            _nameSpecialization.Clear();
            _specialityCode.Add(specialityCode);
            _specializationCode.Add(specializationCode);
            _nameSpecialization.Add(nameSpecialization);
        }

        public List<int> GetSpecialityCode()
        {
            return _specialityCode;
        }

        public List<int> GetSpecializationCode()
        {
            return _specializationCode;
        }

        public List<string> GetNameSpecialization()
        {
            return _nameSpecialization;
        }

        public void GetTableSpecialization()
        {
            _specialityCode.Clear();
            _specializationCode.Clear();
            _nameSpecialization.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Specialization", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _specialityCode.Add(Convert.ToInt32(reader[0]));
                        _specializationCode.Add(Convert.ToInt32(reader[1]));
                        _nameSpecialization.Add(reader[2].ToString());
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

        public void InsertToTableSpecialization(int specialityCode, int specializationCode, string nameSpecialization)
        {
            _nameSpecialization.Clear();
            _specializationCode.Clear();
            _specialityCode.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection; // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "INSERT INTO Specialization (specialityCode, specializationCode, nameSpecialization) " +
                        "values (@specialityCode, @specializationCode, @nameSpecialization)";
                    command.Parameters.AddWithValue("@specialityCode", specialityCode);
                    command.Parameters.AddWithValue("@specializationCode", specializationCode);
                    command.Parameters.AddWithValue("@nameSpecialization", nameSpecialization);

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
                                _specialityCode.Add(Convert.ToInt32(reader[0]));
                                _specializationCode.Add(Convert.ToInt32(reader[1]));
                                _nameSpecialization.Add(reader[2].ToString());
                                Console.WriteLine($"{reader[0]} \t | {reader[1]} \t | {reader[2]}");
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

        public void SearchInTableSpecialization(string arg)
        {
            GetTableSpecialization();
            var index = new List<int>();
            if (int.TryParse(arg, out var code))
            {
                for (var i = 0; i < _specialityCode.Count; i++)
                    if (_specialityCode[i] == code || _specializationCode[i] == code)
                        index.Add(i);
            }
            else
            {
                for (var i = 0; i < _specialityCode.Count; i++)
                    if (_nameSpecialization[i] == arg)
                        index.Add(i);
            }

            var specialityCodeTmp = new List<int>();
            var specializationCodeTmp = new List<int>();
            var nameTmp = new List<string>();

            foreach (var t in index)
            {
                nameTmp.Add(_nameSpecialization[t]);
                specialityCodeTmp.Add(_specialityCode[t]);
                specializationCodeTmp.Add(_specializationCode[t]);
            }

            _specialityCode.Clear();
            _specialityCode = specialityCodeTmp;
            _specializationCode.Clear();
            _specializationCode = specializationCodeTmp;
            _nameSpecialization.Clear();
            _nameSpecialization = nameTmp;

            for (var i = 0; i < _specialityCode.Count; i++)
            {
                Console.WriteLine($"{_specialityCode[i]} \t | {_specializationCode[i]} \t | {_nameSpecialization[i]}");
            }
        }
    }
}