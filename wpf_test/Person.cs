using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
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
    }
