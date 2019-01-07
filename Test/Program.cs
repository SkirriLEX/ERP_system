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
            var specialization = new Specialization();
            specialization.GetTableSpecialization();
            Console.Write("\n\n\n\n");
            specialization.InsertToTableSpecialization(104, 1043, "Radiophysics");
            Console.Write("\n\n\n\n");
            specialization.SearchInTableSpecialization("Nanomaterials");
        }
    }
}
