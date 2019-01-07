using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var speciality = new Speciality();
            speciality.GetTableSpeciality();
            Console.Write("\n\n\n\n");
            speciality.SearchInTableSpeciality("Physics");
            Console.ReadKey(true);
        }
    }
}
