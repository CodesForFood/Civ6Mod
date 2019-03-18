using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civ6Changer
{
    class Program
    {
        public static readonly string MENU1 = "<1>Do All Files \n" +
            "<2>Change Technologies and Civics \n" +
            "<3>Exit";       

        private static void ShowFirstMenu(ReadWriter readWrite, DocFiles doc)
        {
            int choice = 0;
            Console.WriteLine(MENU1);
            
            if(int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Are you ready to mod all files? {Y/N}");
                        string ready = Console.ReadLine();

                        if (ready == "Y" || ready == "y")
                        {
                            try
                            {
                                readWrite.ChangeGame();
                                if (doc.UseDLC)
                                {
                                    readWrite.ChangeDLC();
                                }

                                Console.WriteLine("All Done!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.ReadKey();
                            }
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("You ain't ready yet, Peace out...");
                            ShowFirstMenu(readWrite, doc);
                        }
                        break;
                    case 2:
                        Console.Clear();
                        DoTechCivics(readWrite, doc);
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        ShowFirstMenu(readWrite, doc);
                    break;                                                
                }
            }
            else
            {
                Console.WriteLine("You entered an invalid option");
                ShowFirstMenu(readWrite, doc);
            }

        }

        private static void DoTechCivics(ReadWriter readWrite, DocFiles doc)
        {
            int choice = 0;
            Console.WriteLine("Enter the rate you would like to multiply the speed of Technologies and Civics by, Enter -1 to quit: ");

            if(int.TryParse(Console.ReadLine(), out choice))
            {
                if(choice != -1)
                {
                    readWrite.DoCustomBaseTechCivics(choice);
                    if (doc.UseDLC)
                    {
                        readWrite.DoCustomDLCTechCivics(choice);
                    }
                    Console.WriteLine("All Done");
                }
                else
                {
                    Console.WriteLine("Exiting...");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Thats not a valid option try again");
                DoTechCivics(readWrite, doc);
            }
        }



        static void Main(string[] args)
        {

            Console.WriteLine("Welcome");

            DocFiles doc = new DocFiles();
            doc.GetDirectory();
            ReadWriter readWrite;

            if (doc.UseDLC)
                readWrite = new ReadWriter(doc.BaseDir.FullName, doc.DLCDir.FullName);
            else
                readWrite = new ReadWriter(doc.BaseDir.FullName);

            ShowFirstMenu(readWrite, doc);

            Console.ReadKey();
        }
      



    }
}
