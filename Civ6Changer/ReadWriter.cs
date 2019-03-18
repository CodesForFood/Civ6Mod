using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Civ6Changer
{
    public class ReadWriter
    {        
        XmlDocument Doc { get; set; }
        string PathName { get; set; }
        string BaseDirName { get; }
        string DLCDirName { get; }

        XmlNode MainNode { get; set; }
        XmlNodeList RowNodes { get { return MainNode.ChildNodes; } }

        public const int RATE = 12;




        public ReadWriter(string baseName, string dlcName)
        {
            Doc = new XmlDocument();
            BaseDirName = baseName;
            DLCDirName = dlcName;
        }    

        public ReadWriter(string baseName)
        {
            Doc = new XmlDocument();
            BaseDirName = baseName;
            DLCDirName = "None";
        }


        public void ChangeGame()
        {
            DoTechnologies();
            DoCivics();
            DoLeaders();
            DoUnits();
            DoMaps();
            DoRoutes();
            DoPolicies();
            DoGlobalParameters();
        }
        
        public void ChangeDLC()
        {
            DoExpansionRoutes();
            DoExpansionTechs();
            DoExpansionUnits();
        }


        public void DoCustomBaseTechCivics(int rate)
        {
            DoTechnologies(rate);
            DoCivics(rate);
        }

        public void DoCustomDLCTechCivics(int rate)
        {
            DoExpansionTechs(rate);
            DoExpansionCivics(rate);
        }




        XmlNode GetByName(string name, XmlDocument doc)
        {
            string xPathString = "/GameInfo/Units/Row[@UnitType= '" + name + "']";
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            return xmlNode;
        }

        private void DoTechnologies() { DoTechnologies(RATE); }
        private void DoTechnologies(int rate)
        {
            PathName = Path.Combine(BaseDirName, "Technologies.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Technologies/Row[@TechnologyType='TECH_POTTERY']");
            //Orginal pottery is 25
            int prevRate = int.Parse(MainNode.Attributes["Cost"].Value) / 25;

            XmlNodeList mainNodes = Doc.SelectNodes("GameInfo/Technologies/Row");
            foreach (XmlNode node in RowNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            Say("Technologies.xml Success");
            Doc.Save(PathName);
           
        }

        private void DoCivics() { DoCivics(RATE); }
        private void DoCivics(int rate)
        {
            PathName = Path.Combine(BaseDirName, "Civics.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Civics/Row[@CivicType = 'CIVIC_CODE_OF_LAWS']");
            //Original code of laws is 20
            int prevRate = int.Parse(MainNode.Attributes["Cost"].Value) / 20;
            
            
            XmlNodeList mainNodes = Doc.SelectNodes("/GameInfo/Civics/Row");
            foreach (XmlNode node in mainNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            Say("Civics.xml Success");
            Doc.Save(PathName);
           
        }

        private void DoLeaders()
        {         
            PathName = Path.Combine(BaseDirName, "Leaders.xml");
            Doc.Load(PathName);
            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/Modifiers/Row[ModifierId= 'HIGH_DIFFICULTY_COMBAT_SCALING']/OwnerRequirementSetId");
            MainNode.InnerText = "PLAYER_IS_HUMAN"; 
        

            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/ModifierArguments/Row[ModifierId='HIGH_DIFFICULTY_COMBAT_SCALING']/Extra");
            MainNode.InnerText = "4";

            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/Modifiers/Row[ModifierId= 'HIGH_DIFFICULTY_UNIT_XP_SCALING']/OwnerRequirementSetId");
             MainNode.LastChild.InnerText = "PLAYER_IS_HUMAN"; 
           

            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/ModifierArguments/Row[ModifierId='HIGH_DIFFICULTY_UNIT_XP_SCALING']/Extra");
            MainNode.InnerText = "10";

            Doc.Save(PathName);
            Say("Leaders.xml Success");
             
        }

        private void DoUnits()
        {
            PathName = Path.Combine(BaseDirName, "Units.xml");
            Doc.Load(PathName);
            MainNode = Doc.SelectSingleNode("/GameInfo/Units");

            List<XmlNode> wanted = new List<XmlNode>();

            foreach (var unit in DocFiles.UnitList) { wanted.Add(GetByName(unit, Doc)); }         

            if(int.Parse(wanted[0].Attributes["BaseMoves"].Value) != 5)
            {
                foreach (var node in wanted) { node.Attributes["BaseMoves"].Value = "5"; }
                Doc.Save(PathName);
                Say("Units.xml Success");
            }
            else { Say("Units.xml has already been changed."); }            
        }

        private bool DoMaps()
        {
            PathName = Path.Combine(BaseDirName, "Maps.xml");
            Doc.Load(PathName);

            if(Doc != null)
            {
                MainNode = Doc.SelectSingleNode("/GameInfo/Maps/Row[@MapSizeType = 'MAPSIZE_HUGE']");              

                if (int.Parse(MainNode.Attributes["GridWidth"].Value) != 128)
                {
                    MainNode.Attributes["GridWidth"].Value = "128";
                    MainNode.Attributes["GridHeight"].Value = "80";
                    Say("Maps.xml Success");
                    Doc.Save(PathName);
                }
                else { Say("Maps has already been changed"); }
            }
            return true;
        }

        private bool DoRoutes()
        {           
            PathName = Path.Combine(BaseDirName, "Routes.xml");
            Doc.Load(PathName);

            int counter = 0;

            if(Doc != null)
            {                                                     
                //This part for movement cost
                MainNode = Doc.SelectSingleNode("/GameInfo/Routes");               
                var check = (XmlElement)RowNodes[0];

                if (double.Parse(check.GetAttribute("MovementCost")) != .75 )
                {
                    RowNodes[0].Attributes["MovementCost"].Value = ".75";
                    RowNodes[1].Attributes["MovementCost"].Value = ".5";
                    RowNodes[2].Attributes["MovementCost"].Value = ".25";
                    RowNodes[3].Attributes["MovementCost"].Value = ".125";                 
                }else { counter++; }

                if(counter == 0)
                {
                    Say("Routes Success");
                    Doc.Save(PathName);
                }
                else { Say("Routes.xml has already been changed"); }             
            }

            return true;
        }

        private void DoPolicies()
        {
            PathName = Path.Combine(BaseDirName, "Policies.xml");
            Doc.Load(PathName);

            int counter = 0;

            MainNode = Doc.SelectSingleNode("/GameInfo/ModifierArguments/Row[ModifierId = 'CONSCRIPTION_UNITMAINTENANCEDISCOUNT']/Value");
            if (MainNode.InnerText != "3") { MainNode.InnerText = "3"; }
            else { counter++; }

            MainNode = Doc.SelectSingleNode("/GameInfo/ModifierArguments/Row[ModifierId = 'LEVEEENMASSE_UNITMAINTENANCEDISCOUNT']/Value");
            if(MainNode.InnerText != "6") { MainNode.InnerText = "6"; }
            else { counter++; }

            if(counter == 0)
            {
                Say("Policies Success");
                Doc.Save(PathName);
            }
            else { Say("Policies.xml has already been changed"); }                    
        }

        private void DoGlobalParameters()
        {
            PathName = Path.Combine(BaseDirName, "GlobalParameters.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/GlobalParameters/Replace[@Name = 'COMBAT_HEAL_NAVAL_NEUTRAL']");
            MainNode.Attributes["Value"].Value = "10";

            MainNode = Doc.SelectSingleNode("/GameInfo/GlobalParameters/Replace[@Name = 'EXPERIENCE_BARB_SOFT_CAP']");
            MainNode.Attributes["Value"].Value = "10";

            MainNode = Doc.SelectSingleNode("/GameInfo/GlobalParameters/Replace[@Name = 'EXPERIENCE_COMBAT_RANGED']");
            MainNode.Attributes["Value"].Value = "4";

            MainNode = Doc.SelectSingleNode("/GameInfo/GlobalParameters/Replace[@Name = 'EXPERIENCE_MAX_BARB_LEVEL']");
            MainNode.Attributes["Value"].Value = "6";         

            MainNode = Doc.SelectSingleNode("/GameInfo/GlobalParameters/Replace[@Name = 'EXPERIENCE_MAXIMUM_ONE_COMBAT']");
            MainNode.Attributes["Value"].Value = "30";

            Doc.Save(PathName);
            Say("Global Parameters Successfull");

        }
        

        //Do the Expansion stuff here now
        private void DoExpansionUnits()
        {
            PathName = Path.Combine(DLCDirName, "Expansion2_Units.xml");
            Doc.Load(PathName);                                                 

            foreach(var unit in DocFiles.DLCUnitList)
            {
                MainNode = Doc.SelectSingleNode("/GameInfo/Units_XP2/Row[@UnitType = '" + unit + "']");
                MainNode.Attributes["ResourceCost"].Value = "20";
                MainNode.Attributes.Remove(MainNode.Attributes["ResourceMaintenanceAmount"]);
            }

            Doc.Save(PathName);
            Say("Expansion Units Successfull");
        }

        private void DoExpansionTechs() { DoExpansionTechs(RATE); }
        private void DoExpansionTechs(int rate)
        {
            PathName = Path.Combine(DLCDirName, "Expansion2_Technologies.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Technologies/Row[@TechnologyType = 'TECH_BUTTRESS']");
            //Original Buttress is 300
            int prevRate = (int.Parse(MainNode.Attributes["Cost"].Value) / 300);
          
            XmlNodeList mainNodes = Doc.SelectNodes("/GameInfo/Technologies/Row");
            foreach (XmlNode node in mainNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            mainNodes = Doc.SelectNodes("/GameInfo/TechnologyRandomCosts/Row");
            foreach (XmlNode node in mainNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            Doc.Save(PathName);
            Say("Expansion Technologies Success");                      
        }

        private void DoExpansionCivics() { DoExpansionCivics(RATE); }

        private void DoExpansionCivics(int rate)
        {
            PathName = Path.Combine(DLCDirName, "Expansion2_Civics.xml");
            Doc.Load(PathName);           

            MainNode = Doc.SelectSingleNode("/GameInfo/Civics/Row[@CivicType = 'CIVIC_ENVIRONMENTALISM']");
            //Original envirionmentalism is 2880
            int prevRate = (int.Parse(MainNode.Attributes["Cost"].Value) / 2880);

            XmlNodeList mainNodes = Doc.SelectNodes("/GameInfo/Civics/Row");
            foreach(XmlNode node in mainNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            mainNodes = Doc.SelectNodes("/GameInfo/CivicRandomCosts/Row");
            foreach (XmlNode node in mainNodes)
            {
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) / prevRate).ToString();
                node.Attributes["Cost"].Value = (int.Parse(node.Attributes["Cost"].Value) * rate).ToString();
            }

            //Firaxis... why you no architecture?
            mainNodes = Doc.SelectNodes("/GameInfo/Civics/Update/Set/Cost");
            foreach (XmlNode node in mainNodes)
            {
                node.InnerText = (int.Parse(node.InnerText) / prevRate).ToString();
                node.InnerText = (int.Parse(node.InnerText) * rate).ToString();
            }

            Doc.Save(PathName);
            Console.WriteLine("Expansion Civics Success");
        }

        private void DoExpansionRoutes()
        {
            PathName = Path.Combine(DLCDirName, "Expansion2_Routes.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Routes/Row[@RouteType = 'ROUTE_RAILROAD']");
            MainNode.Attributes["MovementCost"].Value = ".1";

            Doc.Save(PathName);
            Say("Expansion Routes successfull");
        }               

        void Say(string text) { Console.WriteLine(text); }
    }
}
