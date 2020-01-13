using System.Xml.Linq;
using System.Xml;
using System;
using System.Linq;

namespace GCodeSender.Hotkey
{
    public class CheckCreateFile
    {
        private static string HotKeyFile = "Resources\\hotkeys.xml";
        public static int HotKeyFileVer = 2; // Hotkey File Version - Change this with each Hotkey File Change (ie when new hotkeys are added)
        private static XElement xElement;
        
        /// <summary>
        /// Checks if Hotkey file exists, if not create with defaults
        /// </summary>
        public static void CreateDefaultXML()
        {
            {
                MainWindow.Logger.Info("Creating new HotKey file");

                xElement = new XElement("Hotkeys", new XAttribute("Name", "3RDD GCode Sender"), new XAttribute("HotkeyFileVer", HotKeyFileVer),
                 // Default Keys
                 new XElement("bind", new XAttribute("key_description", "Jog X Axis +"), new XAttribute("keyfunction", "JogXPos"), new XAttribute("keycode", "Right")),
                 new XElement("bind", new XAttribute("key_description", "Jog X Axis -"), new XAttribute("keyfunction", "JogXNeg"), new XAttribute("keycode", "Left")),
                 new XElement("bind", new XAttribute("key_description", "Jog Y Axis +"), new XAttribute("keyfunction", "JogYPos"), new XAttribute("keycode", "Up")),
                 new XElement("bind", new XAttribute("key_description", "Jog Y Axis -"), new XAttribute("keyfunction", "JogYNeg"), new XAttribute("keycode", "Down")),
                 new XElement("bind", new XAttribute("key_description", "Jog Z Axis +"), new XAttribute("keyfunction", "JogZPos"), new XAttribute("keycode", "NumPad9")),
                 new XElement("bind", new XAttribute("key_description", "Jog Z Axis -"), new XAttribute("keyfunction", "JogZNeg"), new XAttribute("keycode", "NumPad3")),
                 // Optional Keys
                 new XElement("bind", new XAttribute("key_description", "Emergency Stop"), new XAttribute("keyfunction", "EmgStop"), new XAttribute("keycode", "Escape")),
                 new XElement("bind", new XAttribute("key_description", "Cycle Start"), new XAttribute("keyfunction", "CycleStart"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "File Hold / Jog Cancel"), new XAttribute("keyfunction", "FSStop"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Return to Origin (Zero)"), new XAttribute("keyfunction", "RTOrigin"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle Increase"), new XAttribute("keyfunction", "SpindleInc"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle Decrease"), new XAttribute("keyfunction", "SprindleDec"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Feed Rate Increase"), new XAttribute("keyfunction", "FRateInc"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Feed Rate Decrease"), new XAttribute("keyfunction", "FRateDec"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Increase X"), new XAttribute("keyfunction", "JRateIncX"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease X"), new XAttribute("keyfunction", "JRateDecX"), new XAttribute("keycode", "")),
                 // New Hotkeys v2.0.1.4
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Increase Y"), new XAttribute("keyfunction", "JRateIncY"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease Y"), new XAttribute("keyfunction", "JRateDecY"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Increase Z"), new XAttribute("keyfunction", "JRateIncZ"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease Z"), new XAttribute("keyfunction", "JRateDecZ"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Increase X"), new XAttribute("keyfunction", "JDistIncX"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease X"), new XAttribute("keyfunction", "JDistDecX"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Increase Y"), new XAttribute("keyfunction", "JDistIncY"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease Y"), new XAttribute("keyfunction", "JDistDecY"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Increase Z"), new XAttribute("keyfunction", "JDistIncZ"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease Z"), new XAttribute("keyfunction", "JDistDecZ"), new XAttribute("keycode", "")),
                 // End New Hotkeys v2.0.1.4
                 new XElement("bind", new XAttribute("key_description", "Re-Do/Reload File"), new XAttribute("keyfunction", "ReDoReload"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle On/Off"), new XAttribute("keyfunction", "SpindleOnOff"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Coolant On/Off"), new XAttribute("keyfunction", "CoolantOnOff"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Mist On/Off"), new XAttribute("keyfunction", "MistOnOff"), new XAttribute("keycode", "")));
                
                xElement.Save(HotKeyFile);
                MainWindow.Logger.Info("Hotkey File Created");
            }
        }
        /// <summary>
        /// Function to Update existing Hotkey XML File to new version
        /// </summary>
        /// <param name="CurrentKeyFileVersion"></param>
        public static void CheckAndUpdateXMLFile(int CurrentKeyFileVersion)
        {
            MainWindow.Logger.Info($"Updating HotkeyXML File. Current File Version {CurrentKeyFileVersion}, Update to File Version {HotKeyFileVer}");
            // Define new Hotkey fields - This changes every program update if needed   
            var xDoc = XDocument.Load(HotKeyFile);

            // NOTES | Sample Code for Add, Add at position, Rename:
            // Add to end of file: xDoc.Root.Add(new XElement("bind", new XAttribute("key_description", "Jog Distance Increase"), new XAttribute("keyfunction", "JDistInc"), new XAttribute("keycode", "")));
            // Add to specific Location: xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Increase"), new XAttribute("keyfunction", "JDistInc"), new XAttribute("keycode", "")));
            // Rename Hotkey Data
            //var hotKeyRename1 = xDoc.Descendants("bind").Where(arg => arg.Attribute("key_description").Value == "Feed Rate Increase").Single();
            //hotKeyRename1.Attribute("key_description").Value = "Feed Rate Increase X";

            // START FILE MANIPULATION
            // Insert at specific Location - Reverse Order - Bottom will be inserted at the top
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease Z"), new XAttribute("keyfunction", "JDistDecZ"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Increase Z"), new XAttribute("keyfunction", "JDistIncZ"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease Y"), new XAttribute("keyfunction", "JDistDecY"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Increase Y"), new XAttribute("keyfunction", "JDistIncY"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Decrease X"), new XAttribute("keyfunction", "JDistDecX"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Distance Increase X"), new XAttribute("keyfunction", "JDistIncX"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease Z"), new XAttribute("keyfunction", "JRateDecZ"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Rate Increase Z"), new XAttribute("keyfunction", "JRateIncZ"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease Y"), new XAttribute("keyfunction", "JRateDecY"), new XAttribute("keycode", "")));
            xDoc.Element("Hotkeys").Elements("bind").ElementAt(15).AddAfterSelf(new XElement("bind", new XAttribute("key_description", "Jog Rate Increase Y"), new XAttribute("keyfunction", "JRateIncY"), new XAttribute("keycode", "")));

            // Change Hotkey Desciptions and Keyfunction Name
            var hotKeyRename1 = xDoc.Descendants("bind").Where(arg => arg.Attribute("keyfunction").Value == "JRateInc").Single();
            var hotKeyRename2 = xDoc.Descendants("bind").Where(arg => arg.Attribute("keyfunction").Value == "JRateDec").Single();
            hotKeyRename1.Attribute("key_description").Value = "Jog Rate Increase X";
            hotKeyRename1.Attribute("keyfunction").Value = "JRateIncX";
            hotKeyRename2.Attribute("key_description").Value = "Jog Rate Decrease X";
            hotKeyRename2.Attribute("keyfunction").Value = "JRateDecX";

            // END FILE MANIPULATION
            xDoc.Root.Attribute("HotkeyFileVer").Value = HotKeyFileVer.ToString(); // Change HotkeyFileVer to current version

            //And save the XML file
            xDoc.Save(HotKeyFile);

            // Re-load Hotkeys
            HotKeys.LoadHotKeys();
        }
    }
}
