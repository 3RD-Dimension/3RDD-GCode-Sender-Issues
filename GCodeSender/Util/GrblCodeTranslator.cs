using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GCodeSender.Util
{
	static class GrblCodeTranslator
	{
		static Dictionary<int, string> Errors = new Dictionary<int, string>();
		static Dictionary<int, string> Alarms = new Dictionary<int, string>();
        public static Dictionary<string, string> BuildCodes = new Dictionary<string, string>(); // Build Codes
       
        /// <summary>
        /// setting name, unit, description
        /// </summary>
        public static Dictionary<int, Tuple<string, string, string>> Settings = new Dictionary<int, Tuple<string, string, string>>();

        // Load Build Codes from File
        private static void LoadBuildCoads(Dictionary<string, string> dict, string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Build Code File Missing: {0}", path);
            }

            try
            {
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        // Todo Remove Header -> First line
                        reader.ReadLine(); // Read and Discard Header line
                        var line = reader.ReadLine().Replace("\"", ""); // Remove " from each line
                        var values = line.Split(','); // Split into Values - values[0] Code, values[1] Desc, values [2] Enabled/Disabled

                        dict.Add(values[0], values[1] + " " + values[2]); // Add to BuildCodes Dictionary
                        Console.WriteLine(values[0] +"," + values[1] + " " + values[2]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }


		private static void LoadErr(Dictionary<int, string> dict, string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Error Code File Missing: {0}", path);
				return;
			}

			string FileContents;

			try
			{
				FileContents = File.ReadAllText(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			Regex LineParser = new Regex(@"""([0-9]+)"",""[^\n\r""]*"",""([^\n\r""]*)""");     //test here https://regex101.com/r/hO5zI1/4

			MatchCollection mc = LineParser.Matches(FileContents);

			foreach (Match m in mc)
			{
				try //shouldn't be needed as regex matched already
				{
					int number = int.Parse(m.Groups[1].Value);

					dict.Add(number, m.Groups[2].Value);
				}
				catch { }
			}
		}

		private static void LoadSettings(Dictionary<int, Tuple<string, string, string>> dict, string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("GRBL Settings File Missing: {0}", path);
				return;
			}

			string FileContents;

			try
			{
				FileContents = File.ReadAllText(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			Regex LineParser = new Regex(@"""([0-9]+)"",""([^\n\r""]*)"",""([^\n\r""]*)"",""([^\n\r""]*)""");

			MatchCollection mc = LineParser.Matches(FileContents);

			foreach (Match m in mc)
			{
				try //shouldn't be needed as regex matched already
				{
					int number = int.Parse(m.Groups[1].Value);

					dict.Add(number, new Tuple<string, string, string>(m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value));
				}
				catch { }
			}
		}

		static GrblCodeTranslator()
		{
			Console.WriteLine("Loading GRBL Code Database");

			LoadErr(Errors, "Resources\\error_codes_en_US.csv"); // Load Error Codes
			LoadErr(Alarms, "Resources\\alarm_codes_en_US.csv"); // Load Alarm Codes
			LoadSettings(Settings, "Resources\\setting_codes_en_US.csv"); // Load Settings Codes
            LoadBuildCoads(BuildCodes, "Resources\\build_option_codes_en_US.csv"); // Load Build Codes

			Console.WriteLine("Loaded GRBL Code Database");
		}

		public static string GetErrorMessage(int errorCode, bool alarm = false)
		{
			if (!alarm)
			{
				if (Errors.ContainsKey(errorCode))
					return Errors[errorCode];
				else
					return $"Unknown Error: {errorCode}";
			}
			else
			{
				if (Alarms.ContainsKey(errorCode))
					return Alarms[errorCode];
				else
					return $"Unknown Alarm: {errorCode}";
			}
		}

		static Regex ErrorExp = new Regex(@"error:(\d+)");
		private static string ErrorMatchEvaluator(Match m)
		{
			return GetErrorMessage(int.Parse(m.Groups[1].Value));
		}

		static Regex AlarmExp = new Regex(@"ALARM:(\d+)");
		private static string AlarmMatchEvaluator(Match m)
		{
			return GetErrorMessage(int.Parse(m.Groups[1].Value), true);
		}

		public static string ExpandError(string error)
		{
			string ret = ErrorExp.Replace(error, ErrorMatchEvaluator);
			return AlarmExp.Replace(ret, AlarmMatchEvaluator);
		}
	}
}
