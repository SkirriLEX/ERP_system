using System;
using System.Windows;

namespace Test
{
    class Program
    {
        private static void ShowDetailedMenu()
        {
            Console.Clear();
            Console.WriteLine("You can \n" +
                              "1) Insert any data\n" +
                              "2) Get all tuples in table\n" +
                              "3) Search data\n" +
                              "4) Back to main menu\n" +
                              "5) Exit\n" +
                              "Your choice?\t");
        }
        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Which class do you want to modify?");
            Console.WriteLine("1) Student");
            Console.WriteLine("2) Subjects");
            Console.WriteLine("3) Groups");
            Console.WriteLine("4) Employees");
            Console.WriteLine("5) Position");
            Console.WriteLine("6) Department");
            Console.WriteLine("7) Logins and passwords");
            Console.WriteLine("8) Specialization");
            Console.WriteLine("9) Specialities");
            Console.WriteLine("10) Exit");
            Console.Write("Your choice?\t");
        }
        private static bool Auth()
        {
            auth:
            Console.Write("Enter login:\t");
            var login = Console.ReadLine();
            Console.Write("Enter password:\t");
            var pass = Console.ReadLine();
            var success = DataQ.CheckLog(login, pass);
            if (success) return true;
            Console.Write("wrong login or password\n");
            goto auth;
        }
        static void Main()
        {
            if (Auth())
            {
                do
                {
                    main:
                    ShowMainMenu();
                    var i = Convert.ToInt16(Console.ReadLine());
                    switch (i)
                    {
                        case 1:
                        {
                            levelup:
                            var stud = new Student();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                var codePerson = Convert.ToInt32(Console.ReadLine());
                                var firstName = Console.ReadLine();
                                var midName = Console.ReadLine();
                                var lastName = Console.ReadLine();
                                var dateofBirth = Convert.ToDateTime(Console.ReadLine());
                                var grupCode = Convert.ToInt16(Console.ReadLine());
                                var roleStud = Console.ReadLine();
                                var addrr = Console.ReadLine();
                                var phoneNum = Convert.ToInt32(Console.ReadLine());
                                var email = Console.ReadLine();
                                var dateBegin = Convert.ToDateTime(Console.ReadLine());
                                var dateEnd = Convert.ToDateTime(Console.ReadLine());
                                stud.InsertStud(codePerson, firstName, midName,
                                    lastName, dateofBirth, grupCode, roleStud,
                                    addrr, phoneNum, email, dateBegin, dateEnd);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                stud.GetStud();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                stud.SearchStud(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 2: 
                        {
                            levelup:
                            var subj = new Subjects();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter code of teacher\t");
                                var tabNumPerson = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter code of speciality\t");
                                var tabNumSpec = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter code of teacher\t");
                                var tabNumSubj = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter name of subject\t");
                                var subject = Console.ReadLine();
                                Console.Write("Enter amount of hours\t");
                                var hour = Convert.ToSingle(Console.ReadLine());
                                subj.InsertToTableSubj(tabNumPerson, tabNumSpec, tabNumSubj, subject, hour);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                subj.GetTableSubj();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                subj.SearchInTableSubject(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 3: 
                        {
                            levelup:
                            var group = new Gruppa();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter specialization code\t");
                                var codeSpec = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter group code\t");
                                var codeGroup = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter group name\t");
                                var nameGroup = Console.ReadLine();
                                Console.Write("Enter group curator\t");
                                var teacher = Console.ReadLine();
                                group.InsertToTableGruppa(codeSpec, codeGroup,nameGroup, teacher);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                group.GetTableGroup();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                group.SearchInTableGroup(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 4: 
                        {
                            levelup:
                            var group = new Person();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter personnel number\t");
                                int codePerson = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter first name\t");
                                string firstName = Console.ReadLine();
                                Console.Write("Enter last name\t");
                                string lastName = Console.ReadLine();
                                Console.Write("Enter middle name\t");
                                string midName = Console.ReadLine();
                                Console.Write("Enter birthday\t");
                                DateTime dateofBirth;
                                DateTime.TryParse(Console.ReadLine(), out dateofBirth);
                                Console.Write("Enter position code\t");
                                int positionCode = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter department code\t");
                                int departamentCode = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter address\t");
                                string addrr = Console.ReadLine();
                                Console.Write("enter phone number\t");
                                int phoneNum = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter email\t");
                                string email = Console.ReadLine();
                                Console.Write("enter date of getting into the university\t");
                                DateTime dateBegin;
                                DateTime.TryParse(Console.ReadLine(), out dateBegin);
                                Console.Write("enter date of expulsion\t");
                                DateTime dateEnd;
                                DateTime.TryParse(Console.ReadLine(), out dateEnd);
                                group.InsertPerson(codePerson, firstName, lastName, midName, dateofBirth, positionCode,
                                    departamentCode, addrr, phoneNum, email, dateBegin, dateEnd);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                group.GetTablePerson();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                group.SearchPerson(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 5:
                        {
                            levelup:
                            var spec = new Position();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter code of position\t");
                                var code1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter name\t");
                                var name = Console.ReadLine();
                                spec.InsertToTablePositions(code1, name);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                spec.GetTablePositions();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                spec.SearchInTablePositions(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 6:
                        {
                            levelup:
                            var spec = new Department();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter code of department\t");
                                var code1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter name\t");
                                var name = Console.ReadLine();
                                Console.Write("Enter code of speciality\t");
                                var specCode = Convert.ToInt32(Console.ReadLine());
                                spec.InsertToTableDep(code1, name, specCode);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                spec.GetTableDep();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                spec.SearchInTableDepartament(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 7:
                        {
                            levelup:
                            var spec = new InfLogin();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter personnel number\t");
                                var code1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter login\t");
                                var name = Console.ReadLine();
                                Console.Write("Enter password\t");
                                var specCode = (Console.ReadLine());
                                spec.InsertToTableInfLogin(code1, name, specCode);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                spec.GetTableInfLogin();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                spec.SearchInTableInfLogin(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                        }
                        case 8:
                        {
                            levelup:
                            var spec = new Specialization();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter code of speciality\t");
                                var code1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter code of specialization\t");
                                var code2 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter name of specialization");
                                var name = Console.ReadLine();
                                spec.InsertToTableSpecialization(code1, code2, name);
                                
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                spec.GetTableSpecialization();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                spec.SearchInTableSpecialization(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 9:
                        {
                            levelup:
                            var spec = new Speciality();
                            ShowDetailedMenu();
                            var j = Convert.ToInt16(Console.ReadLine());
                            if (j == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter data:");
                                Console.Write("Enter code of speciality\t");
                                var code1 = (Console.ReadLine());
                                Console.Write("Enter name of specialty");
                                var name = Console.ReadLine();
                                spec.InsertToTableSpeciality(code1, name);
                                Console.Write("added successfully");
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 2)
                            {
                                Console.Clear();
                                spec.GetTableSpeciality();
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if (j == 3)
                            {
                                Console.Write("Enter argument\t");
                                spec.SearchInTableSpeciality(Console.ReadLine());
                                Console.ReadKey(true);
                                goto main;
                            }
                            else if(j == 4)
                            {
                                goto main;
                            }
                            else if (j == 5)
                            {
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("wrong number");
                                goto levelup;
                            }
                            break;
                        }
                        case 10:
                        {
                            Environment.Exit(0);
                            break;
                        }
                        default: goto main;
                    }
                } while (true);
            }
            MessageBox.Show("wrong pass"); //Environment.Exit(0);
        }
    }
}
