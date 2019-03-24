using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;

namespace Test
{
    public class DataQ
    {
        public readonly SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();

        public DataQ()
        {
            const string connStr = "178.136.14.234";
            Builder.DataSource = connStr;
            Builder.UserID = "resto";
            Builder.Password = "Resto#test01";
        }

        public SqlConnectionStringBuilder GetBuilder()
        {
            return Builder;
        }

        private bool TryConnect()
        {
            using (var connection = new SqlConnection(Builder.ConnectionString))
            {
                try
                {
                    Debug.Write(@"Connecting to SQL Server ... ");

                    connection.Open();
                    Debug.WriteLine("Done.");
                    return true;
                }
                catch (SqlException exception)
                {
                    DisplaySqlErrors(exception);
                    Debug.WriteLine($"Нет связи с сервером!\n{exception}");
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
                Console.WriteLine("Index #" + i + "\n" +
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

        public static bool CheckLog(string login, string pass)
        {
            if (!Utils.Connect.TryConnect()) return false;
            using (var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString))
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

                cmdText = "use ERP_system;\n" +
                          $"select count(1) from InfLogin where loginStr like '{login}' and pass like '{pass}'";
                command = new SqlCommand(cmdText, connection);
                // Add the parameters.
                command.Parameters.Add(new SqlParameter("0", 1));
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]) > 0;
                    }
                }

                connection.Close();
            }

            return false;
        }
    }
    internal static class Utils
    {
        public static readonly DataQ Connect = new DataQ();

        public static int GetCountTuples(string tableName)
        {
            var count = 0;
            using (var connection = new SqlConnection(Connect.Builder.ConnectionString))
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
    }
    public class Speciality //+++++
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
    public class Department //+++++
    {
        private List<int> _depCode;
        private List<string> _nameDep;
        private List<int> _specCode;

        public Department()
        {
            _depCode = _specCode = new List<int>();
            _nameDep = new List<string>();
        }

        public List<int> GetDepCode()
        {
            return _depCode;
        }

        public List<string> GetNameDep()
        {
            return _nameDep;
        }

        public List<int> GetSpecCode()
        {
            return _specCode;
        }

        public void GetTableDep() //tested
        {
            _depCode.Clear();
            _nameDep.Clear();
            _specCode.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Department", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _depCode.Add(Convert.ToInt32(reader[0]));
                        _nameDep.Add(reader[1].ToString());
                        _specCode.Add(Convert.ToInt32(reader[2]));
                        Console.WriteLine($"{_depCode}\t|{_nameDep}\t|{_specCode}");
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

        public void InsertToTableDep(int departamentCode, string nameDepartment, int specialityCode)
        {
            _depCode.Clear();
            _nameDep.Clear();
            _specCode.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection; // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"INSERT INTO Department (departamentCode, nameDepartment, specialityCode) " +
                                          "values (@departamentCode, @nameDepartment, @specialityCode)";
                    command.Parameters.AddWithValue("@departamentCode", departamentCode);
                    command.Parameters.AddWithValue("@nameDepartment", nameDepartment);
                    command.Parameters.AddWithValue("@specialityCode", specialityCode);

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
                                _depCode.Add(Convert.ToInt32(reader[0]));
                                _nameDep.Add(reader[1].ToString());
                                _specCode.Add(Convert.ToInt32(reader[2]));
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

        public void SearchInTableDepartament(string arg) //tested
        {
            _depCode.Clear();
            _nameDep.Clear();
            _specCode.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Department where depCode like arg or "
                                             + "nameDep like arg or specCode like arg", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _depCode.Add(Convert.ToInt32(reader[0]));
                        _nameDep.Add(reader[1].ToString());
                        _specCode.Add(Convert.ToInt32(reader[2]));
                        Console.WriteLine($"{_depCode}\t|{_nameDep}\t|{_specCode}");
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
    public class Position //+++++++
    {
        private List<int> _codePosition;
        private List<string> _namePosition;

        public Position()
        {
            _codePosition = new List<int>();
            _namePosition = new List<string>();
        }

        public Position(int codePosition, string namePosition)
        {
            _codePosition = new List<int>();
            _namePosition = new List<string>();
            _codePosition.Add(codePosition);
            _namePosition.Add(namePosition);
        }

        public List<int> GetCodePos()
        {
            return _codePosition;
        }

        public List<string> GetNamePos()
        {
            return _namePosition;
        }

        public void GetTablePositions() //clear
        {
            _codePosition.Clear();
            _namePosition.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Positions", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codePosition.Add(Convert.ToInt32(reader[0]));
                        _namePosition.Add(reader[1].ToString());
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}");
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

        public void InsertToTablePositions(int code, string name) //++++++
        {
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"INSERT INTO Positions " +
                                          $"VALUES ({code}, '{name}')";
                    //command.Parameters.AddWithValue("@codePosition", _codePosition);
                    //command.Parameters.AddWithValue("@namePosition", _namePosition);
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
                                _codePosition.Add(Convert.ToInt32(reader[0]));
                                _namePosition.Add(reader[1].ToString());
                                Console.WriteLine($"{reader[0]} \t | {reader[1]} \t ");
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

        public void SearchInTablePositions(string arg)
        {
            _codePosition.Clear();
            _namePosition.Clear();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Positions " +
                                             $"where codePosition like {arg} or " +
                                             $"namePosition like '{arg}'", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codePosition.Add(Convert.ToInt32(reader[0]));
                        _namePosition.Add(reader[1].ToString());
                        Console.WriteLine($"{reader[0]} \t | {reader[1]} ");
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
    public class Person //++++
    {
        private List<int> _codePerson;
        private List<string> _firstName;
        private List<string> _lastName;
        private List<string> _midName;
        private List<DateTime> _dateOfBirth;
        private List<int> _posCode;
        private List<int> _depCode;
        private List<string> _addrr;
        private List<long> _phoneNum;
        private List<string> _email;
        private List<DateTime> _dateBegin;
        private List<DateTime> _dateEnd;

        //constructor
        public Person()
        {
            _codePerson = _posCode = _depCode = new List<int>();
            _firstName = _lastName = _midName = _addrr = _email = new List<string>();
            _dateEnd = _dateBegin = _dateOfBirth = new List<DateTime>();
            _phoneNum = new List<long>();
        }
        public Person(int codePerson, string firstName, string lastName, string midName, DateTime dateOfBirth,
            int posCode, int depCode, string addrr, long phoneNum, string email, DateTime dateBegin,
            DateTime dateEnd)
        {
            _codePerson = _posCode = _depCode = new List<int>();
            _firstName = _lastName = _midName = _addrr = _email = new List<string>();
            _dateEnd = _dateBegin = _dateOfBirth = new List<DateTime>();
            _phoneNum = new List<long>();
            _codePerson.Add(codePerson);
            _dateBegin.Add(dateBegin);
            _dateEnd.Add(dateEnd);
            _dateOfBirth.Add(dateOfBirth);
            _depCode.Add(depCode);
            _email.Add(email);
            _firstName.Add(firstName);
            _lastName.Add(lastName);
            _midName.Add(midName);
            _phoneNum.Add(phoneNum);
            _posCode.Add(posCode);
            _addrr.Add(addrr);
        }

        //getters
        public List<int> GetCodePerson()
        {
            return _codePerson;
        }
        public List<string> GetFirstName()
        {
            return _firstName;
        }
        public List<string> GetLastName()
        {
            return _lastName;
        }
        public List<string> GetMidName()
        {
            return _midName;
        }
        public List<DateTime> GetDateBirth()
        {
            return _dateOfBirth;
        }
        public List<int> GetPositionCode()
        {
            return _posCode;
        }
        public List<int> GetDepartmentCode()
        {
            return _depCode;
        }
        public List<string> GetAddrress()
        {
            return _addrr;
        }
        public List<long> GetPhone()
        {
            return _phoneNum;
        }
        public List<string> GetEmail()
        {
            return _email;
        }
        public List<DateTime> GetDateBegin()
        {
            return _dateBegin;
        }
        public List<DateTime> GetDateEnd()
        {
            return _dateEnd;
        }

        private void ClearAll()
        {
            _codePerson.Clear();
            _posCode.Clear();
            _depCode.Clear();
            _firstName.Clear();
            _lastName.Clear();
            _midName.Clear();
            _addrr.Clear();
            _email.Clear();
            _dateOfBirth.Clear();
            _dateBegin.Clear();
            _dateEnd.Clear();
            _phoneNum.Clear();
        }
        public void GetPerson()//get all records in table
        {
            ClearAll();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Person", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codePerson.Add(Convert.ToInt32(reader[0]));
                        _firstName.Add(reader[1].ToString());
                        _lastName.Add(reader[2].ToString());
                        _midName.Add(reader[2].ToString());
                        _dateOfBirth.Add(Convert.ToDateTime(reader[3]));
                        _posCode.Add(Convert.ToInt32(reader[4]));
                        _depCode.Add(Convert.ToInt32(reader[5]));
                        _addrr.Add(reader[6].ToString());
                        _phoneNum.Add(Convert.ToInt64(reader[7]));
                        _email.Add(reader[8].ToString());
                        _dateBegin.Add(Convert.ToDateTime(reader[9]));
                        _dateEnd.Add(Convert.ToDateTime(reader[10]));
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|" +
                                          $"{reader[3]}\t|{reader[4]}\t|{reader[5]}\t|" +
                                          $"{reader[6]}\t|{reader[7]}\t|{reader[8]}\t|" +
                                          $"{reader[9]}\t|{reader[10]} ");
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
        public void InsertPerson(int codePerson, string firstName, string lastName, string midName,
            DateTime dateofBirth, int positionCode, int departamentCode, string addrr, int phoneNum,
            string email, DateTime dateBegin, DateTime dateEnd)
        {
            ClearAll();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            using (connection)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        $"INSERT INTO Person (codePerson, firstName, lastName, midName, dateofBirth, " +
                        $"positionCode, departamentCode, addrr, phoneNum, email, dateBegin, dateEnd)" +
                        $"VALUES (@codePerson, @firstName, @lastName, @midName, @dateofBirth, @positionCode, " +
                        $"@departamentCode, @addrr, @phoneNum, @email, @dateBegin, @dateEnd)";
                    command.Parameters.AddWithValue("@codePerson", codePerson);
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@midName", midName);
                    command.Parameters.AddWithValue("@dateofBirth", dateofBirth);
                    command.Parameters.AddWithValue("@positionCode", positionCode);
                    command.Parameters.AddWithValue("@departamentCode", departamentCode);
                    command.Parameters.AddWithValue("@addrr", addrr);
                    command.Parameters.AddWithValue("@phoneNum", phoneNum);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@dateBegin", dateBegin);
                    command.Parameters.AddWithValue("@dateEnd", dateEnd);
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
                                _codePerson.Add(Convert.ToInt32(reader[0]));
                                _firstName.Add(reader[1].ToString());
                                _lastName.Add(reader[2].ToString());
                                _midName.Add(reader[2].ToString());
                                _dateOfBirth.Add(Convert.ToDateTime(reader[3]));
                                _posCode.Add(Convert.ToInt32(reader[4]));
                                _depCode.Add(Convert.ToInt32(reader[5]));
                                _addrr.Add(reader[6].ToString());
                                _phoneNum.Add(Convert.ToInt64(reader[7]));
                                _email.Add(reader[8].ToString());
                                _dateBegin.Add(Convert.ToDateTime(reader[9]));
                                _dateEnd.Add(Convert.ToDateTime(reader[10]));
                                Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|" +
                                                  $"{reader[3]}\t|{reader[4]}\t|{reader[5]}\t|" +
                                                  $"{reader[6]}\t|{reader[7]}\t|{reader[8]}\t|" +
                                                  $"{reader[9]}\t|{reader[10]} ");
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
        public void SearchPerson(string arg)
        {
            ClearAll();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Person where codePerson like {arg} or" +
                                             $"firstName like {arg} or" +
                                             $"lastName like {arg} or" +
                                             $"midName like {arg} or" +
                                             $"dateofBirth like {arg} or" +
                                             $"positionCode like {arg} or" +
                                             $"departamentCode like {arg} or" +
                                             $"addrr like {arg} or" +
                                             $"phoneNum like {arg} or" +
                                             $"email like {arg} or" +
                                             $"dateBegin like {arg} or" +
                                             $"dateEnd like {arg}", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codePerson.Add(Convert.ToInt32(reader[0]));
                        _firstName.Add(reader[1].ToString());
                        _lastName.Add(reader[2].ToString());
                        _midName.Add(reader[2].ToString());
                        _dateOfBirth.Add(Convert.ToDateTime(reader[3]));
                        _posCode.Add(Convert.ToInt32(reader[4]));
                        _depCode.Add(Convert.ToInt32(reader[5]));
                        _addrr.Add(reader[6].ToString());
                        _phoneNum.Add(Convert.ToInt64(reader[7]));
                        _email.Add(reader[8].ToString());
                        _dateBegin.Add(Convert.ToDateTime(reader[9]));
                        _dateEnd.Add(Convert.ToDateTime(reader[10]));
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|" +
                                          $"{reader[3]}\t|{reader[4]}\t|{reader[5]}\t|" +
                                          $"{reader[6]}\t|{reader[7]}\t|{reader[8]}\t|" +
                                          $"{reader[9]}\t|{reader[10]} ");
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

    public class Student
    {
        private List<int> _codePerson;
        private List<string> _firstName;
        private List<string> _midName;
        private List<string> _lastName;
        private List<DateTime> _dateofBirth;
        private List<int> _grupCode;
        private List<string> _roleStud;
        private List<string> _addrr;
        private List<long> _phoneNum;
        private List<string> _email;
        private List<DateTime> _dateBegin;
        private List<DateTime> _dateEnd;

        public Student()
        {
            _codePerson = _grupCode = new List<int>();
            _firstName = _midName = _lastName = _roleStud = _addrr = _email = new List<string>();
            _dateEnd = _dateBegin = _dateofBirth = new List<DateTime>();
            _phoneNum = new List<long>();
        }
        public Student(int codePerson, string firstName, string midName,
                string lastName, DateTime dateofBirth, int grupCode, string roleStud,
                string addrr, int phoneNum, string email, DateTime dateBegin, DateTime dateEnd)
        {
            _codePerson = _grupCode = new List<int>();
            _firstName = _midName = _lastName = _roleStud = _addrr = _email = new List<string>();
            _dateEnd = _dateBegin = _dateofBirth = new List<DateTime>();
            _phoneNum = new List<long>();
            _codePerson.Add(codePerson);
            _firstName.Add(firstName);
            _midName.Add(midName);
            _lastName.Add(lastName);
            _dateofBirth.Add(dateofBirth);
            _grupCode.Add(grupCode);
            _roleStud.Add(roleStud);
            _addrr.Add(addrr);
            _phoneNum.Add(phoneNum);
            _email.Add(email);
            _dateBegin.Add(dateBegin);
            _dateEnd.Add(dateEnd);
        }
        public List<int> GetCodePerson() { return _codePerson; }
        public List<string> GetFirstName() { return _firstName; }
        public List<string> GetLastName() { return _lastName; }
        public List<string> GetMidName() { return _midName; }
        public List<DateTime> GetDateBirth() { return _dateofBirth; }
        public List<int> GetGroupCode() { return _grupCode; }
        public List<string> GetRole() { return _roleStud; }
        public List<string> GetAddrress() { return _addrr; }
        public List<long> GetPhone() { return _phoneNum; }
        public List<string> GetEmail() { return _email; }
        public List<DateTime> GetDateBegin() { return _dateBegin; }
        public List<DateTime> GetDateEnd() { return _dateEnd; }

        private void ClearData()
        {
            _codePerson.Clear();
            _grupCode.Clear();
            _phoneNum.Clear();
            _firstName.Clear();
            _midName.Clear();
            _lastName.Clear();
            _roleStud.Clear();
            _addrr.Clear();
            _email.Clear();
            _dateBegin.Clear();
            _dateEnd.Clear();
            _dateofBirth.Clear();
        }
        public void GetStud()
        {
            ClearData();
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Student", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        _codePerson.Add(Convert.ToInt32(reader[0]));
                        _firstName.Add(reader[1].ToString());
                        _midName.Add(reader[2].ToString());
                        _lastName.Add(reader[2].ToString());
                        _dateofBirth.Add(Convert.ToDateTime(reader[3]));
                        _grupCode.Add(Convert.ToInt32(reader[4]));
                        _roleStud.Add(reader[5].ToString());
                        _addrr.Add(reader[6].ToString());
                        _phoneNum.Add(Convert.ToInt64(reader[7]));
                        _email.Add(reader[8].ToString());
                        _dateBegin.Add(Convert.ToDateTime(reader[9]));
                        _dateEnd.Add(Convert.ToDateTime(reader[10]));
                        Console.WriteLine($"{reader[0]}\t|{reader[1]}\t|{reader[2]}\t|" +
                                          $"{reader[3]}\t|{reader[4]}\t|{reader[5]}\t|" +
                                          $"{reader[6]}\t|{reader[7]}\t|{reader[8]}\t|" +
                                          $"{reader[9]}\t|{reader[10]} ");
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

            public void InsertStud(int codePerson, string firstName, string midName,
                string lastName, DateTime dateofBirth, int grupCode, string roleStud,
                string addrr, int phoneNum, string email, DateTime dateBegin, DateTime dateEnd)
            {
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Student" +
                                                 "(codePerson, firstName, midName, lastName, dateofBirth, grupCode, " +
                                                 "roleStud, addrr, phoneNum, email, dateBegin, dateEnd)" +
                                                 $"VALUES ({codePerson}, {firstName}, {midName}, {lastName}, " +
                                                 $"{dateofBirth}, {grupCode}, {roleStud}, {addrr}, {phoneNum}, " +
                                                 $"{email}, {dateBegin}, {dateEnd})", connection);
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

            public void SearchStud(string arg)
            {
                _codePerson.Clear();
                _grupCode.Clear();
                _phoneNum.Clear();
                _firstName.Clear();
                _midName.Clear();
                _lastName.Clear();
                _roleStud.Clear();
                _addrr.Clear();
                _email.Clear();
                _dateBegin.Clear();
                _dateEnd.Clear();
                _dateofBirth.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();

                    var command = new SqlCommand("SELECT * FROM Student where" +
                                                 $"codePerson like {arg}" +
                                                 $"firstName like {arg}" +
                                                 $"midName like {arg}" +
                                                 $"lastName like {arg}" +
                                                 $"dateofBirth like {arg}" +
                                                 $"grupCode like {arg}" +
                                                 $"roleStud like {arg}" +
                                                 $"addrr like {arg}" +
                                                 $"phoneNum like {arg}" +
                                                 $"email like {arg}" +
                                                 $"dateBegin like {arg}" +
                                                 $"dateEnd like {arg}", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _codePerson.Add(Convert.ToInt32(reader[0]));
                            _firstName.Add(reader[1].ToString());
                            _midName.Add(reader[2].ToString());
                            _lastName.Add(reader[3].ToString());
                            _dateofBirth.Add(Convert.ToDateTime(reader[4]));
                            _grupCode.Add(Convert.ToInt32(reader[5]));
                            _roleStud.Add(reader[6].ToString());
                            _addrr.Add(reader[7].ToString());
                            _phoneNum.Add(Convert.ToInt32(reader[8]));
                            _email.Add(reader[9].ToString());
                            _dateBegin.Add(Convert.ToDateTime(reader[10]));
                            _dateEnd.Add(Convert.ToDateTime(reader[11]));

                            Debug.WriteLine($"{_codePerson}\t|{_firstName}\t|{_midName}\t|" +
                                            $"{_lastName}\t|{_dateofBirth}\t|{_grupCode}\t|{_roleStud}\t|" +
                                            $"{_addrr}\t|{_phoneNum}\t|{_email}\t|{_dateBegin}\t|{_dateEnd}\t");
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