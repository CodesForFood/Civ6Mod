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
        string DirName { get; }

        XmlNode MainNode { get; set; }
        XmlNodeList RowNodes { get { return MainNode.ChildNodes; } }
       


        public ReadWriter(DirectoryInfo dir)
        {
            Doc = new XmlDocument();
            DirName = dir.FullName;
        }    

        XmlNode GetByName(string name, XmlDocument doc)
        {
            string xPathString = "/GameInfo/Units/Row[@UnitType= '" + name + "']";
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            return xmlNode;
        }

        public void DoTechnologies()
        {
            PathName = Path.Combine(DirName, "Technologies.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Technologies");

            if (int.Parse(RowNodes[1].Attributes["Cost"].Value) != 500)
            {
                foreach (XmlNode row in RowNodes)
                {
                    if (row.NodeType == XmlNodeType.Element)
                    {
                        var cost = int.Parse(row.Attributes["Cost"].Value);
                        cost *= 20;
                        row.Attributes["Cost"].Value = cost.ToString();
                    }
                }

                Say("Technologies.xml Success");
                Doc.Save(PathName);
            }
            else { Say("Technologies.xml has already been changed."); }
        }


        public void DoCivics()
        {
            PathName = Path.Combine(DirName, "Civics.xml");
            Doc.Load(PathName);

            MainNode = Doc.SelectSingleNode("/GameInfo/Civics");

            if(int.Parse(RowNodes[1].Attributes["Cost"].Value) != 400)
            {
                foreach(XmlNode row in RowNodes)
                {
                    if (row.NodeType == XmlNodeType.Element)
                    {
                        var cost = int.Parse(row.Attributes["Cost"].Value);
                        cost *= 20;
                        row.Attributes["Cost"].Value = cost.ToString();
                    }
                }

                Say("Civics.xml Success");
                Doc.Save(PathName);
            }
            else { Say("Civics.xml has already been changed."); }
        }

        public void DoLeaders()
        {
            int counter = 0;
            PathName = Path.Combine(DirName, "Leaders.xml");
            Doc.Load(PathName);
            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/Modifiers/Row[ModifierId= 'HIGH_DIFFICULTY_COMBAT_SCALING']");

            if (MainNode.LastChild.InnerText != "PLAYER_IS_HUMAN") { MainNode.LastChild.InnerText = "PLAYER_IS_HUMAN"; }
            else { counter++; }

            MainNode = Doc.DocumentElement.SelectSingleNode("/GameInfo/Modifiers/Row[ModifierId= 'HIGH_DIFFICULTY_UNIT_XP_SCALING']");

            if (MainNode.LastChild.InnerText != "PLAYER_IS_HUMAN") { MainNode.LastChild.InnerText = "PLAYER_IS_HUMAN"; }
            else { counter++; }

            if(counter > 0) { Say("Leaders.xml has already been changed"); }
            else { Say("Leaders.xml Success"); Doc.Save(PathName); }

        }

        public void DoUnits()
        {
            PathName = Path.Combine(DirName, "Units.xml");
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

        public bool DoMaps()
        {
            PathName = Path.Combine(DirName, "Maps.xml");
            Doc.Load(PathName);

            if(Doc != null)
            {
                MainNode = Doc.SelectSingleNode("/GameInfo/Maps");
                var hugeNode = MainNode.LastChild; // Should be Huge Map

                if (int.Parse(hugeNode.Attributes["GridWidth"].Value) != 128)
                {
                    hugeNode.Attributes["GridWidth"].Value = "128";
                    hugeNode.Attributes["GridHeight"].Value = "80";
                    Say("Maps.xml Success");
                    Doc.Save(PathName);
                }
                else { Say("Maps.xml has already been changed"); }
            }
            return true;
        }

        public bool DoRoutes()
        {           
            PathName = Path.Combine(DirName, "Routes.xml");
            Doc.Load(PathName);

            int counter = 0;

            if(Doc != null)
            {
                //This part for the ValidBuildUnits
                MainNode = Doc.SelectSingleNode("/GameInfo/Route_ValidBuildUnits");            
                
                var check = (XmlElement)RowNodes[0];

                if (check.GetAttribute("UnitType") != "UNIT_BUILDER")
                {
                    foreach (XmlNode node in RowNodes) { node.Attributes["UnitType"].Value = "UNIT_BUILDER"; }                                    
                }
                else { counter++; }

                //This part for movement cost
                MainNode = Doc.SelectSingleNode("/GameInfo/Routes");               
                check = (XmlElement)RowNodes[0];

                if (double.Parse(check.GetAttribute("MovementCost")) != .75 )
                {
                    RowNodes[0].Attributes["MovementCost"].Value = ".75";
                    RowNodes[1].Attributes["MovementCost"].Value = ".5";
                    RowNodes[2].Attributes["MovementCost"].Value = ".25";
                    RowNodes[3].Attributes["MovementCost"].Value = ".125";                 
                }else { counter++; }

                if(counter == 0) { Say("Routes.xml Success"); Doc.Save(PathName); }
                else { Say("Routes.xml has already been changed"); }             
            }

            return true;
        }

        void Say(string text) { Console.WriteLine(text); }

    }
}
