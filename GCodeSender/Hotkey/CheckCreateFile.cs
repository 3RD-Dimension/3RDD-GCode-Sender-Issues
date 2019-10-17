using System.Xml.Linq;
using System.Xml;
using System;

namespace GCodeSender.Hotkey
{
    public class CheckCreateFile
    {
        private static string HotKeyFile = "Resources\\hotkeys.xml";
        private static string HotKeyFileVer = "1"; // Hotkey File Version
        /// <summary>
        /// Checks if Hotkey file exists, if not create with defaults
        /// </summary>
        public static void CreateDefaultXML()
        {
            MainWindow.Logger.Info("Creating new HotKey file");

            XElement newXML = new XElement("Hotkeys", new XAttribute("Name", "3RDD GCode Sender"), new XAttribute("HotkeyFileVer", HotKeyFileVer),
                 // Default Keys
                 new XElement("bind", new XAttribute("key_description", "Jog X Axis +"), new XAttribute("keyfunction", "JogXPos"), new XAttribute("keycode", "Right")),
                 new XElement("bind", new XAttribute("key_description", "Jog X Axis -"), new XAttribute("keyfunction", "JogXNeg"), new XAttribute("keycode", "Left")),
                 new XElement("bind", new XAttribute("key_description", "Jog Y Axis +"), new XAttribute("keyfunction", "JogYPos"), new XAttribute("keycode", "Up")),
                 new XElement("bind", new XAttribute("key_description", "Jog Y Axis -"), new XAttribute("keyfunction", "JogYNeg"), new XAttribute("keycode", "Down")),
                 new XElement("bind", new XAttribute("key_description", "Jog Z Axis +"), new XAttribute("keyfunction", "JogZPos"), new XAttribute("keycode", "NumPad9")),
                 new XElement("bind", new XAttribute("key_description", "Jog Z Axis -"), new XAttribute("keyfunction", "JogZNeg"), new XAttribute("keycode", "NumPad3")),
                 // Optional Keys
                 new XElement("bind", new XAttribute("key_description", "Emergency Stop"), new XAttribute("keyfunction", "EmgStop"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Cycle Start"), new XAttribute("keyfunction", "CycleStart"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "File Hold / Jog Cancel"), new XAttribute("keyfunction", "FSStop"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Return to Origin (Zero)"), new XAttribute("keyfunction", "RTOrigin"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Single Step"), new XAttribute("keyfunction", "SStep"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle Increase"), new XAttribute("keyfunction", "SpindleInc"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle Decrease"), new XAttribute("keyfunction", "SprindleDec"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Feed Rate Increase"), new XAttribute("keyfunction", "FRateInc"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Feed Rate Decrease"), new XAttribute("keyfunction", "FRateDec"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Increase"), new XAttribute("keyfunction", "JRateInc"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Jog Rate Decrease"), new XAttribute("keyfunction", "JRateDec"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Re-Do/Reload File"), new XAttribute("keyfunction", "ReDoReload"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Spindle On/Off"), new XAttribute("keyfunction", "SpindleOnOff"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Coolant On/Off"), new XAttribute("keyfunction", "CoolantOnOff"), new XAttribute("keycode", "")),
                 new XElement("bind", new XAttribute("key_description", "Mist On/Off"), new XAttribute("keyfunction", "MistOnOff"), new XAttribute("keycode", "")));

            newXML.Save(HotKeyFile);
            MainWindow.Logger.Info("Hotkey File Created");
        }

        // Update hotkey file with new version
        public static void UpdateHotKeyFile()
        {
           // XDocument doc = XDocument.Load(HotKeyFile);
           // XElement NewElement = doc.Element("Hotkeys");
            // Add new Element
            //NewElement.Add(new XElement("bind", new XAttribute("key_description", ""), new XAttribute("keyfunction", ""), new XAttribute("keycode", "")));
            //doc.Save(HotKeyFile);
        }
    }
}
