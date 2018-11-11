using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace erp{
    public class DataQ // all Function in our data connection 
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
        public bool TryConnect()
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
        public bool CheckLog(string login, string pass)
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
    public class Speciality//all done
    {
        private int Code { get; set; }
        private string Name { get; set; }

        private Speciality()
        {
            Code = 0;
            Name = String.Empty;
        } 
        private Speciality(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int GetCode(){ return Code; }
        public string GetName(){ return Name; }

        public void GetTableSpeciality()
        {
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
                        Code = Convert.ToInt32(reader[0]);
                        Name = reader[1].ToString();
                        Debug.WriteLine($"{reader[0]} \t | {reader[1]} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show($"Нет связи с сервером!\n{ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        public void InsertToTableSpeciality(string code, string name)
        {
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
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
                MessageBox.Show($"Нет связи с сервером!\n{ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        public void SearchInTableSpeciality(string arg)//return an array with defined argument
        {
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Speciality where " +
                                             $"specialityCode like {arg} " +
                                             $"or nameSpec like {arg}", connection);
                using (var reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        Code = Convert.ToInt32(reader[0]);
                        Name = reader[1].ToString();
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
    public class Specialization//all done
    {
        private int _specialityCode;
        private int _specializationCode;
        private string _nameSpecialization;

        private Specialization()
        {
            _specialityCode = _specializationCode = 0;
            _nameSpecialization = String.Empty;
        }
        private Specialization(int specialityCode, int specializationCode, string nameSpecialization)
        {
            _specialityCode = specialityCode;
            _specializationCode = specializationCode;
            _nameSpecialization = nameSpecialization;
        }

        public int GetSpecialityCode() { return _specialityCode;}
        public int GetSpecializationCode() { return _specializationCode;}
        public string GetNameSpecialization() { return _nameSpecialization;}
        
        public void GetTableSpecialization()
        {
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
                        _specialityCode = Convert.ToInt32(reader[0]);
                        _specializationCode = Convert.ToInt32(reader[1]);
                        _nameSpecialization = Convert.ToString(reader[2]);
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
                                             "(specialityCode, specializationCode, )" +
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
        public void SearchInTableSpecialization(string arg)//return an array with defined argument
        {
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
                        _specialityCode = Convert.ToInt32(reader[0]);
                        _specializationCode = Convert.ToInt32(reader[1]);
                        _nameSpecialization = Convert.ToString(reader[2]);
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
    public class InfLogin//all done
    {
        private int _tabNumPerson;
        private string _loginStr;
        private string _pass;

        public InfLogin()
        {
            _tabNumPerson = 0;
            _loginStr = _pass = String.Empty;
        }
        public InfLogin(int tabNumPerson, string loginStr, string pass)
        {
            _tabNumPerson = tabNumPerson;
            _loginStr = loginStr;
            _pass = pass;
        }

        public int GetTabNaumPerson() { return _tabNumPerson;}
        public string GetLoginStr() { return _loginStr;}
        public string GetPass() { return _pass;}
        
        public void GetTableInfLogin()
        {
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
                        _tabNumPerson = Convert.ToInt32(reader[0]);
                        
                        _loginStr = reader[1].ToString();
                        
                        _pass = reader[2].ToString();
                        
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
        public string[,] SearchInTableInfLogin(string arg)
        {
            var connection = new SqlConnection(Utils.Connect.Builder.ConnectionString);
            var tuples = Utils.GetCountTuples("InfLogin");
            var retInfLogin = new string[3, tuples];
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
    }
    public class Department//all done
    {
        private int _depCode;
        private string _nameDep;
        private int _specCode;

        public Department()
        {
            _depCode = _specCode = 0;
            _nameDep = String.Empty;
        }
        public Department(int depCode, string nameDep, int specCode)
        {
            _depCode = depCode;
            _nameDep = nameDep;
            _specCode = specCode;
        }
        
        public int GetDepCode(){return _depCode;}
        public string GetNameDep(){return _nameDep;}
        public int GetSpecCode(){return _specCode;}
        
        public void GetTableDep()
        {
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
                        _depCode = Convert.ToInt32(reader[0]);
                        
                        _nameDep = reader[1].ToString();
                        
                        _specCode = Convert.ToInt32(reader[2]);
                        
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
        public void SearchInTableDepartament(string arg)//return an array with defined argument
        {
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
                        _depCode = Convert.ToInt32(reader[0]);
                        
                        _nameDep = reader[1].ToString();
                        
                        _specCode = Convert.ToInt32(reader[2]);
                        
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
    public class Position
    {
        private int _codePosition;
        private string _namePosition;

        public Position()
        {
            _codePosition = 0;
            _namePosition = String.Empty;
        }
        public Position(int codePosition, string namePosition)
        {
            _codePosition = codePosition;
            _namePosition = namePosition;
        }
        
        public int GetCodePos(){return _codePosition;}
        public string GetNamePos(){return _namePosition;}
        
        public void GetTablePositions()
        {
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
                        _codePosition = Convert.ToInt32(reader[0]);
                        _namePosition = reader[1].ToString();
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
                        _codePosition = Convert.ToInt32(reader[0]);
                        _namePosition = reader[1].ToString();
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
    public class Person
    {
        private int CodePerson;
        private string FirstName;
        private string LastName;
        private string MidName;
        private DateTime DateOfBirth;
        private int PosCode;
        private int DepCode;
        private string Addrr;
        private long PhoneNum;
        private string Email;
        private DateTime DateBegin;
        private DateTime DateEnd;
        
        public Person()
        {
            CodePerson = PosCode = DepCode = 0;
            FirstName = LastName = MidName = Addrr = Email = String.Empty;
            DateOfBirth = DateBegin = DateEnd = DateTime.MinValue;
            PhoneNum = 0;
        }
        public Person(int CodePerson, string FirstName, string LastName, string MidName, DateTime DateOfBirth,
            int PosCode, int DepCode, string Addrr, long PhoneNum, string Email, DateTime DateBegin, DateTime DateEnd)
        {
            this.Addrr = Addrr;
            this.CodePerson = CodePerson;
            this.DateBegin = DateBegin;
            this.DateEnd = DateEnd;
            this.DateOfBirth = DateOfBirth;
            this.DepCode = DepCode;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.MidName = MidName;
            this.PhoneNum = PhoneNum;
            this.PosCode = PosCode;
        }
        
        public void GetPerson()
        {
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
                        var codePerson = Convert.ToInt32(reader[0]);
                        CodePerson = Convert.ToInt32(codePerson);
                        
                        FirstName = reader[1].ToString();
                        LastName = reader[2].ToString();
                        MidName = reader[3].ToString();
                        DateOfBirth = Convert.ToDateTime(reader[4]);
                        PosCode = Convert.ToInt32(reader[5]);
                        DepCode = Convert.ToInt32(reader[6]);
                        Addrr = reader[7].ToString();
                        PhoneNum = Convert.ToInt32(reader[8]);
                        Email = reader[9].ToString();
                        DateBegin = Convert.ToDateTime(reader[10]);
                        DateEnd = Convert.ToDateTime(reader[11]);
                        Debug.WriteLine($"{FirstName}|{LastName}|{MidName}|{DateOfBirth}|{PosCode}|" +
                                        $"{DepCode}|{Addrr}|{PhoneNum}|{Email}|{DateBegin}|{DateEnd}");
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
                        var codePerson = Convert.ToInt32(reader[0]);
                        CodePerson = Convert.ToInt32(codePerson);
                        
                        FirstName = reader[1].ToString();
                        LastName = reader[2].ToString();
                        MidName = reader[3].ToString();
                        DateOfBirth = Convert.ToDateTime(reader[4]);
                        PosCode = Convert.ToInt32(reader[5]);
                        DepCode = Convert.ToInt32(reader[6]);
                        Addrr = reader[7].ToString();
                        PhoneNum = Convert.ToInt32(reader[8]);
                        Email = reader[9].ToString();
                        DateBegin = Convert.ToDateTime(reader[10]);
                        DateEnd = Convert.ToDateTime(reader[11]);
                        Debug.WriteLine($"{FirstName}|{LastName}|{MidName}|{DateOfBirth}|{PosCode}|" +
                                        $"{DepCode}|{Addrr}|{PhoneNum}|{Email}|{DateBegin}|{DateEnd}");
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
    /*create table Gruppa(
	specializationCode int foreign key references Specialization(specializationCode),
	codeGrup int primary key,
	nameGroup varchar(7),
	tutor varchar(20)
);
create table Subjects(
	codeTeacher int foreign key references Person(codePerson) not null,
	codeSpec int foreign key references Specialization(specializationCode) not null,
	nameSubj varchar(25) not null,
	codeSubj int primary key,
	hoursForSubj float not null
);
create table Student(
	codePerson int primary key not null,
	firstName varchar(20) not null,
	midName varchar(20),
	lastName varchar(20) not null,
	dateofBirth date,
	grupCode int foreign key references Gruppa(codeGrup) not null,
	roleStud varchar(25) not null,
	addrr varchar(25),
	phoneNum int unique,
	email varchar(20) unique, 
	dateBegin datetime not null,
	dateEnd datetime
);
*/
}