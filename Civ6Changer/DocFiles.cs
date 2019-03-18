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

        public const string BASEPATH = @"Base\Assets\Gameplay\Data";
        public const string DLCPATH = @"DLC\Expansion2\Data";

        public bool UseDLC { get; set; }

        public DirectoryInfo BaseDir { get; set; }
        public DirectoryInfo DLCDir { get; set; }

        public FileInfo CurrentFile { get; set; }

        public List<string> BaseFileNames = new List<string>
        {
            "Technologies.xml",
            "Civics.xml",
            "Leaders.xml",
            "Routes.xml",
            "Maps.xml",
            "Units.xml",
            "Policies.xml",
            "GlobalParameters.xml"
        };

        public List<string> DLCFileNames = new List<string>
        {
            "Expansion2_Units.xml",
            "Expansion2_Technologies.xml",
            "Expansion2_Routes.xml",
            "Expansion2_Civics.xml"
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

        public static List<string> DLCUnitList = new List<string>
        {
            "UNIT_INFANTRY",
            "UNIT_ARTILLERY",
            "UNIT_DESTROYER",
            "UNIT_ROCKET_ARTILLERY",
            "UNIT_AMERICAN_P51",
            "UNIT_IRONCLAD",
            "UNIT_BATTLESHIP",
            "UNIT_TANK",
            "UNIT_BIPLANE",
            "UNIT_SUBMARINE",
            "UNIT_FIGHTER",
            "UNIT_BOMBER",
            "UNIT_HELICOPTER",
            "UNIT_MECHANIZED_INFANTRY",
            "UNIT_MODERN_ARMOR",
            "UNIT_JET_FIGHTER",
            "UNIT_JET_BOMBER",
            "UNIT_MISSILE_CRUISER",
            "UNIT_NUCLEAR_SUBMARINE",
            "UNIT_GERMAN_UBOAT",
            "UNIT_BRAZILIAN_MINAS_GERAES"
        };


        public DocFiles() { }

        
        public void GetDirectory()
        {
            Console.WriteLine("Please enter the full path to your game directory:");
            var dirName = Console.ReadLine();
            try
            {
                BaseDir = new DirectoryInfo(Path.Combine(dirName, BASEPATH));
                DLCDir = new DirectoryInfo(Path.Combine(dirName, DLCPATH));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                GetDirectory();
            }
          

            if (!DLCDir.Exists)
            {
                Console.WriteLine("The Gathering Storm expansion was not found, continue with just base? Y/N");
                string choice = Console.ReadLine();

                if(choice.ToLower() != "y")
                {
                    GetDirectory();
                }

                UseDLC = false;
            }
            else { UseDLC = true; }

            if (BaseDir.Exists)
            {
                Console.WriteLine("Directories Loaded");
                CheckFiles(BaseDir, BaseFileNames);

                if (UseDLC)
                    CheckFiles(DLCDir, DLCFileNames);
            }
            else
            {
                Console.WriteLine("404: Base game data folder was NOT found" +
                    "\n" + BR);
                GetDirectory();
            }        
        }

        private void CheckFiles(DirectoryInfo dir, List<string> fileList)
        {
            int counter = 0;
            Console.WriteLine("Checking in: " + dir.FullName);

            foreach (var file in fileList)
            {
                CurrentFile = new FileInfo(Path.Combine(dir.FullName, file));
               
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
