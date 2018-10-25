using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace erp{
    public class DataQ // all Function in our data connection 
    {
        protected readonly string ConnStr;
        public readonly SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();

        public DataQ()
        {
            ConnStr = "178.136.14.234";
            Builder.DataSource = ConnStr;
            Builder.UserID = "resto";
            Builder.Password = "Resto#test01";
            //Builder.InitialCatalog = "WIN-NALRE9SA668\\SQLEXPRESS";
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
        public static void DisplaySqlErrors(SqlException exception)
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
    }

    public class dbInteract
    {
        private readonly DataQ _connect = new DataQ();
        private int getCountTuples(string tableName)
        {
            var count = 0;
            using (var connection = new SqlConnection(_connect.Builder.ConnectionString))
            {
                connection.Open();
                var cmdText = "use ERP_system;\n" +
                              $"select count(1) from {tableName}'";
                var command = new SqlCommand(cmdText, connection);
                // Add the parameters.
                command.Parameters.Add(new SqlParameter("0", 1));

                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader[0]);
                        // call the objects from their index
                        //reader[0], reader[1], reader[2], reader[3]));
                    }
                }
            }
            return count;
        }
        public bool CheckLog(string login, string pass)//поменять тут табличку
        {
            if (!_connect.tryConnect()) return false;
            using (var connection = new SqlConnection(_connect.Builder.ConnectionString))
            {
                connection.Open();
                var cmdText = "use ERP_system;\n" +
                              $"select count(1) from Logn where loginStr like '{login}' and pass like '{pass}'";
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

        public Dictionary<string, string> getSpeciality()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var specStrings = new Dictionary<string, string>();
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
                        specStrings.Add(reader[0].ToString(), reader[1].ToString());
                        Debug.WriteLine($"{reader[0]} \t | {reader[1]} ");
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
            return specStrings;
        }
        public void insertSpeciality(string code, string name)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Speciality" +
                                             "(specialityCode, nameSpec)" +
                                             $"VALUES ({code}, {name})", connection);
                command.ExecuteNonQuery();
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

        public string[,] getSpecialization()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var tuples = getCountTuples("Specialization");
            var retSpecialization = new string[3, tuples];
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Specialization", connection);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0, j = 0;
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        var specialityCode = Convert.ToInt32(reader[0]);
                        retSpecialization[i, j] = specialityCode.ToString();
                        j += 1;
                        var specializationCode = Convert.ToInt32(reader[0]);
                        retSpecialization[i, j] = specializationCode.ToString();
                        j += 1;
                        var nameSpecialization = Convert.ToString(reader[0]);
                        retSpecialization[i, j] = nameSpecialization;
                        j += 1;
                        Debug.WriteLine($"{specialityCode} \t | {specializationCode} \t | {nameSpecialization}");
                        i += 1;
                    }
                }
                return retSpecialization;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        public void insertSpecialization(int specialityCode, int specializationCode, int nameSpecialization)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Specialization" +
                                             "(specialityCode, specializationCode, )" +
                                             $"VALUES ({specialityCode}, {specializationCode}, {nameSpecialization})", connection);
                command.ExecuteNonQuery();
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