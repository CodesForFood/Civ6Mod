using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civ6Changer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome");

            DocFiles doc = new DocFiles();

            doc.GetDirectory();

            ReadWriter readWrite = new ReadWriter(doc.CurrentDir);

            Console.WriteLine("Are you ready to mod the files? {Y/N}" );
            string ready = Console.ReadLine();

            if (ready == "Y" || ready == "y")
            {
                try
                {
                    readWrite.DoTechnologies();
                    readWrite.DoCivics();
                    readWrite.DoLeaders();
                    readWrite.DoUnits();
                    readWrite.DoMaps();
                    readWrite.DoRoutes();
                    Console.WriteLine("All Done!");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
               
                Console.ReadKey();
            }
            else { Console.WriteLine("You ain't ready yet, Peace out..."); Console.ReadKey(); }

           

        }
    }
}
