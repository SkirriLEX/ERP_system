using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var pos = new Position();
            pos.InsertToTablePositions(3, "accountant");
            pos.GetTablePositions();
            pos.SearchInTablePositions("2");
                
        }
    }
}
