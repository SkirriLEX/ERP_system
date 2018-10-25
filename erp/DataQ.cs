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
                        var specializationCode = Convert.ToInt32(reader[1]);
                        retSpecialization[i, j] = specializationCode.ToString();
                        j += 1;
                        var nameSpecialization = Convert.ToString(reader[2]);
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

        public string[,] getDep()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var tuples = getCountTuples("Department");
            var retDepartment = new string[3, tuples];
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Department", connection);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0, j = 0;
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        var departamentCode = Convert.ToInt32(reader[0]);
                        retDepartment[i, j] = departamentCode.ToString();
                        j += 1;
                        var nameDepartment = reader[1].ToString();
                        retDepartment[i, j] = nameDepartment;
                        j += 1;
                        var specialityCode = Convert.ToInt32(reader[2]);
                        retDepartment[i, j] = specialityCode.ToString();
                        j += 1;
                        Debug.WriteLine($"{departamentCode} \t | {nameDepartment} \t | {specialityCode}");
                        i += 1;
                    }
                }
                return retDepartment;
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
        public void insertDep(int departamentCode, string nameDepartment, int specialityCode)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Department" +
                                             "(departamentCode, nameDepartment, specialityCode)" +
                                             $"VALUES ({departamentCode}, {nameDepartment}, {specialityCode})", connection);
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

        public Dictionary<string, string> getPositions()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var specStrings = new Dictionary<string, string>();
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Positions", connection);
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
        public void insertPositions(string code, string name)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Positions" +
                                             "(codePosition, namePosition)" +
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

        public string[,] getPerson()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var tuples = getCountTuples("Person");
            var retPerson = new string[11, tuples];
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Person", connection);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0, j = 0;
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        var codePerson = Convert.ToInt32(reader[0]);
                        retPerson[i, j] = codePerson.ToString();
                        j += 1;
                        var firstName = reader[1].ToString();
                        retPerson[i, j] = firstName;
                        j += 1;
                        var lastName = reader[2].ToString();
                        retPerson[i, j] = lastName;
                        j += 1;
                        var midName = reader[3].ToString();
                        retPerson[i, j] = midName;
                        j += 1;
                        var dateofBirth = Convert.ToDateTime(reader[4]);
                        retPerson[i, j] = dateofBirth.ToLongDateString();
                        j += 1;
                        var positionCode = Convert.ToInt32(reader[5]);
                        retPerson[i, j] = positionCode.ToString();
                        j += 1;
                        var departamentCode = Convert.ToInt32(reader[6]);
                        retPerson[i, j] = departamentCode.ToString();
                        j += 1;
                        var addrr = reader[7].ToString();
                        retPerson[i, j] = addrr;
                        j += 1;
                        var phoneNum = Convert.ToInt32(reader[8]);
                        retPerson[i, j] = phoneNum.ToString();
                        j += 1;
                        var email = reader[9].ToString();
                        retPerson[i, j] = email;
                        j += 1;
                        var dateBegin = Convert.ToDateTime(reader[10]);
                        retPerson[i, j] = dateBegin.ToLongDateString();
                        j += 1;
                        var dateEnd = Convert.ToDateTime(reader[11]);
                        retPerson[i, j] = dateEnd.ToLongDateString();
                        j += 1;
                        
                        Debug.WriteLine(retPerson.ToString());
                        i += 1;
                    }
                }
                return retPerson;
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
        public void insertPerson(int codePerson, string firstName, string lastName, string midName,
            DateTime dateofBirth, int positionCode, int departamentCode, string addrr, int phoneNum,
            string email, DateTime dateBegin, DateTime dateEnd)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Person" +
                                             "(codePerson, firstName, lastName, midName, dateofBirth, " +
                                             "positionCode, departamentCode, addrr, phoneNum, email, " +
                                             "dateBegin, dateEnd)" +
                                             $"VALUES ({codePerson}, {firstName}, {lastName}, {midName}, {dateofBirth}, " +
                                             $"{positionCode}, {departamentCode}, {addrr}, {phoneNum}, {email}, " +
                                             $"{dateBegin}, {dateEnd})", connection);
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

        public string[,] getInfLogin()
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            var tuples = getCountTuples("InfLogin");
            var retInfLogin = new string[3, tuples];
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM InfLogin", connection);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0, j = 0;
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        var tabNumPerson = Convert.ToInt32(reader[0]);
                        retInfLogin[i, j] = tabNumPerson.ToString();
                        j += 1;
                        var loginStr = reader[1].ToString();
                        retInfLogin[i, j] = loginStr;
                        j += 1;
                        var pass = Convert.ToInt32(reader[2]);
                        retInfLogin[i, j] = pass.ToString();
                        j += 1;
                        Debug.WriteLine(retInfLogin.ToString());
                        i += 1;
                    }
                }
                return retInfLogin;
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
        public void insertInfLogin(int tabNumPerson, string loginStr, string pass)
        {
            var connection = new SqlConnection(_connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO InfLogin" +
                                             "(tabNumPerson, loginStr, pass)" +
                                             $"VALUES ({tabNumPerson}, {loginStr}, {pass})", connection);
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