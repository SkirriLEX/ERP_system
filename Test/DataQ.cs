using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;

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
            //Builder.InitialCatalog = "WIN-NALRE9SA668\\SQLEXPRESS";
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

    public class Speciality //list
    {
        protected List<int> Code;
        protected List<string> Name;

        public Speciality()
        {
            Code = new List<int>();
            Name = new List<string>();
        }
        public List<int> GetCode()
        {
            return Code;
        }
        public List<string> GetName()
        {
            return Name;
        }

        public void GetTableSpeciality()
        {
            Code?.Clear();
            Name?.Clear();
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
                        Code.Add(Convert.ToInt32(reader[0]));
                        Name.Add(reader[1].ToString());
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
            Code.Clear();
            Name.Clear();
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
                                Code.Add(Convert.ToInt32(reader[0]));
                                Name.Add(reader[1].ToString());
                                Console.WriteLine($"{reader[0]} \t | {reader[1]} \n");
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
            Code.Clear();
            Name.Clear();
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
                        Code.Add(Convert.ToInt32(reader[0]));
                        Name.Add(reader[1].ToString());
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
                for (var i = 0; i < Code.Count; i++)
                    if (Code[i] == codeSpec) index.Add(i);
            }
            else
            {
                for (var i = 0; i < Code.Count; i++)
                    if (Name[i]==arg) index.Add(i);
            }

            var nameTmp = new List<string>();
            var codeTmp = new List<int>();

            foreach (var t in index)
            {
                nameTmp.Add(Name[t]);
                codeTmp.Add(Code[t]);
            }
            Name.Clear();
            Name = nameTmp;
            Code.Clear();
            Code = codeTmp;

            for (var i = 0; i < Name.Count; i++)
            {
                Console.WriteLine(Code[i]+"\t|\t"+Name[i]);
            }
        }
    }
        

        public class Specialization //list
        {
            private List<int> _specialityCode;
            private List<int> _specializationCode;
            private List<string> _nameSpecialization;

            private Specialization()
            {
                _specialityCode.Clear();
                _specializationCode.Clear();
                _nameSpecialization.Clear();
            }

            private Specialization(int specialityCode, int specializationCode, string nameSpecialization)
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
                            _nameSpecialization.Add(Convert.ToString(reader[2]));
                            Debug.WriteLine($"{_specialityCode} \t | {_specializationCode} \t | {_nameSpecialization}");
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

            public void InsertToTableSpecialization(int specialityCode, int specializationCode, int nameSpecialization)
            {
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Specialization" +
                                                 "(specialityCode, specializationCode, nameSpecialization)" +
                                                 $"VALUES ({specialityCode}, " +
                                                 $"{specializationCode}, " +
                                                 $"{nameSpecialization})", connection);
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

            public void SearchInTableSpecialization(string arg)
            {
                _specialityCode.Clear();
                _specializationCode.Clear();
                _nameSpecialization.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Specialization " +
                                                 $"where specialityCode like {arg} or " +
                                                 $"specializationCode like {arg} or" +
                                                 $"nameSpecialization like {arg}", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _specialityCode.Add(Convert.ToInt32(reader[0]));
                            _specializationCode.Add(Convert.ToInt32(reader[1]));
                            _nameSpecialization.Add(Convert.ToString(reader[2]));
                            Debug.WriteLine($"{_specialityCode} \t | {_specializationCode} \t | {_nameSpecialization}");
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

        public class InfLogin //list
        {
            private List<int> _tabNumPerson;
            private List<string> _loginStr;
            private List<string> _pass;

            public InfLogin()
            {
                _tabNumPerson.Clear();
                _loginStr.Clear();
                _pass.Clear();
            }

            public InfLogin(int tabNumPerson, string loginStr, string pass)
            {
                _tabNumPerson.Clear();
                _loginStr.Clear();
                _pass.Clear();
                _tabNumPerson.Add(tabNumPerson);
                _loginStr.Add(loginStr);
                _pass.Add(pass);
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

                            Debug.WriteLine($"{_tabNumPerson}\t|{_loginStr}\t|{_pass} ");
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
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
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

            public void SearchInTableInfLogin(string arg)
            {
                _tabNumPerson.Clear();
                _loginStr.Clear();
                _pass.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM InfLogin" +
                                                 $"where loginStr like {arg}", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        int i = 0, j = 0;
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _tabNumPerson.Add(Convert.ToInt32(reader[0]));

                            _loginStr.Add(reader[1].ToString());

                            _pass.Add(reader[2].ToString());

                            Debug.WriteLine($"{_tabNumPerson}\t|{_loginStr}\t|{_pass} ");
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

        public class Department //list
        {
            private List<int> _depCode;
            private List<string> _nameDep;
            private List<int> _specCode;

            public Department()
            {
                _depCode.Clear();
                _nameDep.Clear();
                _specCode.Clear();
            }

            public Department(int depCode, string nameDep, int specCode)
            {
                _depCode.Clear();
                _nameDep.Clear();
                _specCode.Clear();
                _depCode.Add(depCode);
                _nameDep.Add(nameDep);
                _specCode.Add(specCode);
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

            public void GetTableDep()
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

                            Debug.WriteLine($"{_depCode} \t | {_nameDep} \t | {_specCode}");
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

            public void InsertToTableDep(int departamentCode, string nameDepartment, int specialityCode)
            {
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Department" +
                                                 "(departamentCode, nameDepartment, specialityCode)" +
                                                 $"VALUES ({departamentCode}, {nameDepartment}, {specialityCode})",
                        connection);
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

            public void SearchInTableDepartament(string arg) //return an array with defined argument
            {
                _depCode.Clear();
                _nameDep.Clear();
                _specCode.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Department " +
                                                 $"where departamentCode like {arg} or " +
                                                 $"nameDepartment like {arg} or" +
                                                 $"specialityCode like {arg}", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _depCode.Add(Convert.ToInt32(reader[0]));

                            _nameDep.Add(reader[1].ToString());

                            _specCode.Add(Convert.ToInt32(reader[2]));

                            Debug.WriteLine($"{_depCode} \t | {_nameDep} \t | {_specCode}");
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

        public class Position //list
        {
            private List<int> _codePosition;
            private List<string> _namePosition;

            public Position()
            {
                _codePosition.Clear();
                _namePosition.Clear();
            }

            public Position(int codePosition, string namePosition)
            {
                _codePosition.Clear();
                _namePosition.Clear();
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

            public void GetTablePositions()
            {
                _codePosition.Clear();
                _namePosition.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
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
                            _codePosition.Add(Convert.ToInt32(reader[0]));
                            _namePosition.Add(reader[1].ToString());
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
            }

            public void InsertToTablePositions(string code, string name)
            {
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
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

            public void SearchInTablePositions(string arg)
            {
                _codePosition.Clear();
                _namePosition.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Positions " +
                                                 $"where codePosition like {arg} or " +
                                                 $"namePosition like {arg}", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _codePosition.Add(Convert.ToInt32(reader[0]));
                            _namePosition.Add(reader[1].ToString());
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
            }
        }

        public class Person //list
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

            public Person()
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

            public Person(int codePerson, string firstName, string lastName, string midName, DateTime dateOfBirth,
                int posCode, int depCode, string addrr, long phoneNum, string email, DateTime dateBegin,
                DateTime dateEnd)
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
                _addrr.Add(addrr);
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
            }

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

            public void GetPerson()
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
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Person", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        while (reader.Read())
                        {
                            // write the data on to the screen
                            _codePerson.Add(Convert.ToInt32(reader[0]));
                            _firstName.Add(reader[1].ToString());
                            _lastName.Add(reader[2].ToString());
                            _midName.Add(reader[3].ToString());
                            _dateOfBirth.Add(Convert.ToDateTime(reader[4]));
                            _posCode.Add(Convert.ToInt32(reader[5]));
                            _depCode.Add(Convert.ToInt32(reader[6]));
                            _addrr.Add(reader[7].ToString());
                            _phoneNum.Add(Convert.ToInt64(reader[8]));
                            _email.Add(reader[9].ToString());
                            _dateBegin.Add(Convert.ToDateTime(reader[10]));
                            _dateEnd.Add(Convert.ToDateTime(reader[11]));
                            Debug.WriteLine($"{_firstName}|{_lastName}|{_midName}|{_dateOfBirth}|{_posCode}|" +
                                            $"{_depCode}|{_addrr}|{_phoneNum}|{_email}|{_dateBegin}|{_dateEnd}");
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

            public void InsertPerson(int codePerson, string firstName, string lastName, string midName,
                DateTime dateofBirth, int positionCode, int departamentCode, string addrr, int phoneNum,
                string email, DateTime dateBegin, DateTime dateEnd)
            {
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
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

            public void SearchPerson(string arg)
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
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Person where" +
                                                 $"codePerson like {arg}" +
                                                 $"firstName like {arg}" +
                                                 $"lastName like {arg}" +
                                                 $"midName like {arg}" +
                                                 $"dateofBirth like {arg}" +
                                                 $"positionCode like {arg}" +
                                                 $"departamentCode like {arg}" +
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
                            _lastName.Add(reader[2].ToString());
                            _midName.Add(reader[3].ToString());
                            _dateOfBirth.Add(Convert.ToDateTime(reader[4]));
                            _posCode.Add(Convert.ToInt32(reader[5]));
                            _depCode.Add(Convert.ToInt32(reader[6]));
                            _addrr.Add(reader[7].ToString());
                            _phoneNum.Add(Convert.ToInt64(reader[8]));
                            _email.Add(reader[9].ToString());
                            _dateBegin.Add(Convert.ToDateTime(reader[10]));
                            _dateEnd.Add(Convert.ToDateTime(reader[11]));
                            Debug.WriteLine($"{_firstName}|{_lastName}|{_midName}|{_dateOfBirth}|{_posCode}|" +
                                            $"{_depCode}|{_addrr}|{_phoneNum}|{_email}|{_dateBegin}|{_dateEnd}");
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

        public class Gruppa //list
        {
            private List<int> _specializationCode;
            private List<int> _codeGrup;
            private List<string> _nameGroup;
            private List<string> _tutor;

            public Gruppa()
            {
                _specializationCode.Clear();
                _codeGrup.Clear();
                _nameGroup.Clear();
                _tutor.Clear();
            }

            public Gruppa(int specializationCode, int codeGrup, string nameGroup, string tutor)
            {
                _specializationCode.Clear();
                _codeGrup.Clear();
                _nameGroup.Clear();
                _tutor.Clear();
                _specializationCode.Add(specializationCode);
                _codeGrup.Add(codeGrup);
                _nameGroup.Add(nameGroup);
                _tutor.Add(tutor);
            }

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
                    var command = new SqlCommand("SELECT * FROM Gruppa", connection);
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

                            Debug.WriteLine($"{_specializationCode}\t|{_codeGrup}\t|{_nameGroup}\t|{_tutor} ");
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
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Gruppa" +
                                                 "(specializationCode, codeGrup, nameGroup, tutor)" +
                                                 $"VALUES ({specializationCode}, {codeGrup}, {nameGroup}, {tutor})",
                        connection);
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

            public void SearchInTableInfLogin(string arg)
            {
                _specializationCode.Clear();
                _codeGrup.Clear();
                _nameGroup.Clear();
                _tutor.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM InfLogin" +
                                                 $"where loginStr like {arg}", connection);
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

                            Debug.WriteLine($"{_specializationCode}\t|{_codeGrup}\t|{_nameGroup}\t|{_tutor} ");
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

        public class Subjects //list
        {
            private List<int> _codeTeacher;
            private List<int> _codeSpec;
            private List<string> _nameSubj;
            private List<int> _codeSubj;
            private List<float> _hoursForSubj;

            public Subjects()
            {
                _codeSpec.Clear();
                _codeTeacher.Clear();
                _codeSubj.Clear();
                _hoursForSubj.Clear();
                _nameSubj.Clear();
            }

            public Subjects(int codeTeacher, int codeSpec, int codeSubj,
                string nameSubj, float hoursForSubj)
            {
                _codeSpec.Clear();
                _codeTeacher.Clear();
                _codeSubj.Clear();
                _hoursForSubj.Clear();
                _nameSubj.Clear();
                _codeTeacher.Add(codeTeacher);
                _codeSpec.Add(codeSpec);
                _codeSubj.Add(codeSubj);
                _nameSubj.Add(nameSubj);
                _hoursForSubj.Add(hoursForSubj);
            }

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

            public void GetTableSubj()
            {
                _codeSpec.Clear();
                _codeTeacher.Clear();
                _codeSubj.Clear();
                _hoursForSubj.Clear();
                _nameSubj.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Subjects", connection);
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

                            Debug.WriteLine($"{_codeTeacher}\t|{_codeSpec}\t|{_nameSubj}\t|" +
                                            $"{_codeSubj}\t|{_hoursForSubj}");
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
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Subjects" +
                                                 "(codeTeacher, codeSpec, nameSubj, codeSubj, hoursForSubj)" +
                                                 $"VALUES ({codeTeacher}, {codeSpec}, {nameSubj}, {codeSubj}, " +
                                                 $"{hoursForSubj})", connection);
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

            public void SearchInTableSubject(string arg)
            {
                _codeSpec.Clear();
                _codeTeacher.Clear();
                _codeSubj.Clear();
                _hoursForSubj.Clear();
                _nameSubj.Clear();
                var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Subjects" +
                                                 $"where loginStr like {arg}", connection);
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

                            Debug.WriteLine($"{_codeTeacher}\t|{_codeSpec}\t|{_nameSubj}\t|" +
                                            $"{_codeSubj}\t|{_hoursForSubj}");
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

            public Student(int codePerson, string firstName, string midName,
                string lastName, DateTime dateofBirth, int grupCode, string roleStud,
                string addrr, int phoneNum, string email, DateTime dateBegin, DateTime dateEnd)
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
                return _dateofBirth;
            }

            public List<int> GetGroupCode()
            {
                return _grupCode;
            }

            public List<string> GetRole()
            {
                return _roleStud;
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

            public void GetStud()
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
                    var command = new SqlCommand("SELECT * FROM Student", connection);
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