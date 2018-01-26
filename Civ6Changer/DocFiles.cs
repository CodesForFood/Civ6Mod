using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Civ6Changer
{
    class DocFiles
    {
        public const string BR = "------------------------------------------------------------------------------------";


        public DirectoryInfo CurrentDir { get; set; }
        public FileInfo CurrentFile { get; set; }

        public List<string> FileNames = new List<string>
        {
            "Technologies.xml",
            "Civics.xml",
            "Leaders.xml",
            "Routes.xml",
            "Maps.xml",
            "Units.xml",
        };

        public static List<string> UnitList = new List<string>
        {
            "UNIT_SCOUT",
            "UNIT_RANGER",
            "UNIT_SUBMARINE",
            "UNIT_GERMAN_UBOAT",
            "UNIT_DESTROYER",
            "UNIT_MISSILE_CRUISER",
            "UNIT_NUCLEAR_SUBMARINE"
        };

        public DocFiles() { }

        
        public void GetDirectory()
        {
            Console.WriteLine("Please Enter the Directory Name:");
            var dirName = Console.ReadLine();

            CurrentDir = new DirectoryInfo(dirName);

            if (CurrentDir.Exists)
            {
                Console.WriteLine("Directory Loaded");
                CheckFiles();
            }
            else
            {
                Console.WriteLine("404: Directory NOT found" +
                    "\n" + BR);
                GetDirectory();
            }        
        }

        public void CheckFiles()
        {
            int counter = 0;
            Console.WriteLine("Checking in: " + CurrentDir.FullName);

            foreach (var file in FileNames)
            {
                CurrentFile = new FileInfo(Path.Combine(CurrentDir.FullName, file));
               
                if(CurrentFile.Exists)
                {
                    Console.WriteLine(file + " was found");
                }
                else
                {
                    Console.WriteLine("404: " + file + " was NOT found");
                    counter++;
                }
            }

            if(counter > 0)
            {
                Console.WriteLine("Some files were NOT found please check Directory");
                Console.WriteLine(BR);
                GetDirectory();
            }

            Console.WriteLine(BR);

        }



    }
}
