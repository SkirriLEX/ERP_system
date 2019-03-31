using System;

namespace Test
{
    class Program
    {
        static void showDetailedMenu()
        {
            Console.Clear();
            Console.WriteLine("You can \n" +
                              "1) Insert any data\n" +
                              "2) Get all tuples in table\n" +
                              "3) Search data\n" +
                              "Your choice?\t");
        }
        static void showMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Which class do you want to modify?");
            Console.WriteLine("1) Student");
            Console.WriteLine("2) Subjects");
            Console.WriteLine("3) Gruppa");
            Console.WriteLine("4) Person");
            Console.WriteLine("5) Position");
            Console.WriteLine("6) Department");
            Console.WriteLine("7) InfLogin");
            Console.WriteLine("8) Specialization");
            Console.WriteLine("9) Speciality");
            Console.WriteLine("10) Exit");
            Console.Write("Your choice?\t");
        }
        static void Main(string[] args)
        {
            short i = 0;
            do
            {
                showMainMenu();
                i = Convert.ToInt16(Console.ReadLine());
                switch (i)
                {
                    case 1:
                    {
                        var stud = new Student();
                        showDetailedMenu();
                        var j = Convert.ToInt16(Console.ReadLine());
                        if (j == 1)
                        {
                            Console.WriteLine("Enter data:");
                            var codePerson = Convert.ToInt32(Console.ReadLine());
                            var firstName = Console.ReadLine();
                            var midName = Console.ReadLine();
                            var lastName = Console.ReadLine();
                            var dateofBirth = Convert.ToDateTime(Console.ReadLine());;
                            var grupCode = Convert.ToInt16(Console.ReadLine());
                            var roleStud = Console.ReadLine();
                            var addrr = Console.ReadLine();
                            var phoneNum = Convert.ToInt32(Console.ReadLine());
                            var email = Console.ReadLine();
                            var dateBegin = Convert.ToDateTime(Console.ReadLine());;
                            var dateEnd = Convert.ToDateTime(Console.ReadLine());
                            stud.InsertStud(codePerson, firstName, midName,
                                            lastName, dateofBirth, grupCode, roleStud,
                                            addrr, phoneNum, email, dateBegin, dateEnd);
                        }else if (j == 2)
                        {
                            stud.GetStud();
                        }
                        else if (j == 3)
                        {
                            Console.Write("Enter argument\t");
                            stud.SearchStud(Console.ReadLine());
                        }
                        break;
                    }
                }
            } while (true);
        }
    }
}
