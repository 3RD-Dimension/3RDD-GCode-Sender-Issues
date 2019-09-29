using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace GCodeSender
{
	public partial class GrblSettingsWindow : Window
	{
		Dictionary<int, double> CurrentSettings = new Dictionary<int, double>();
		Dictionary<int, TextBox> SettingsBoxes = new Dictionary<int, TextBox>();
       
		public event Action<string> SendLine;

		public GrblSettingsWindow()
		{
			InitializeComponent();
		}

		static Regex settingParser = new Regex(@"\$([0-9]+)=([0-9\.]+)");
		public void LineReceived(string line)
		{
			if (!line.StartsWith("$"))
				return;
           
			try
			{               
				Match m = settingParser.Match(line);
				int number = int.Parse(m.Groups[1].Value);
				double value = double.Parse(m.Groups[2].Value, Util.Constants.DecimalParseFormat);

				if (!CurrentSettings.ContainsKey(number))
				{
					RowDefinition rowDef = new RowDefinition();
					rowDef.Height = new GridLength(25);
					gridMain.RowDefinitions.Add(rowDef);

					TextBox valBox = new TextBox
					{
						Text = value.ToString(Util.Constants.DecimalOutputFormat),
						VerticalAlignment = VerticalAlignment.Center
					};
					Grid.SetRow(valBox, gridMain.RowDefinitions.Count - 1);
					Grid.SetColumn(valBox, 1);
					gridMain.Children.Add(valBox);

					TextBlock num = new TextBlock
					{
						Text = $"${number}=",
						HorizontalAlignment = HorizontalAlignment.Right,
						VerticalAlignment = VerticalAlignment.Center
					};
					Grid.SetRow(num, gridMain.RowDefinitions.Count - 1);
					Grid.SetColumn(num, 0);
					gridMain.Children.Add(num);

					if (Util.GrblCodeTranslator.Settings.ContainsKey(number))
					{
						Tuple<string, string, string> labels = Util.GrblCodeTranslator.Settings[number];

						TextBlock name = new TextBlock
						{
							Text = labels.Item1,
							VerticalAlignment = VerticalAlignment.Center
						};
						Grid.SetRow(name, gridMain.RowDefinitions.Count - 1);
						Grid.SetColumn(name, 0);
						gridMain.Children.Add(name);

						TextBlock unit = new TextBlock
						{
							Text = labels.Item2,
							VerticalAlignment = VerticalAlignment.Center
						};
						Grid.SetRow(unit, gridMain.RowDefinitions.Count - 1);
						Grid.SetColumn(unit, 2);
						gridMain.Children.Add(unit);

						valBox.ToolTip = $"{labels.Item1} ({labels.Item2}):\n{labels.Item3}";
					}

					CurrentSettings.Add(number, value);
					SettingsBoxes.Add(number, valBox);
				}
				else
				{
					SettingsBoxes[number].Text = value.ToString(Util.Constants.DecimalOutputFormat);
					CurrentSettings[number] = value;
				}
			}
			catch { }
		}

		private async void ButtonApply_Click(object sender, RoutedEventArgs e)
		{
			List<Tuple<int, double>> ToSend = new List<Tuple<int, double>>();

			foreach (KeyValuePair<int, double> kvp in CurrentSettings)
			{
				double newval;

				if (!double.TryParse(SettingsBoxes[kvp.Key].Text, System.Globalization.NumberStyles.Float, Util.Constants.DecimalParseFormat, out newval))
				{
					MessageBox.Show($"Value \"{SettingsBoxes[kvp.Key].Text}\" is invalid for Setting \"{Util.GrblCodeTranslator.Settings[kvp.Key].Item1}\"");
					return;
				}

				if (newval == kvp.Value)
					continue;

				ToSend.Add(new Tuple<int, double>(kvp.Key, newval));
			}

			if (SendLine == null)
				return;

			foreach (Tuple<int, double> setting in ToSend)
			{
				SendLine.Invoke($"${setting.Item1}={setting.Item2.ToString(Util.Constants.DecimalOutputFormat)}");
				CurrentSettings[setting.Item1] = setting.Item2;
				await Task.Delay(Properties.Settings.Default.SettingsSendDelay);
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
        private void ButtonGrblExport_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will export the currently displayed settings to a file - even any modified values that you have entered.  If you need to export the settings before modifications, please export before changing.  Continue To Export?", "Important", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            { // YES - Export               
                SaveFileDialog exportFileName = new SaveFileDialog();
                exportFileName.Filter = "Text File (*.txt)|*.txt";
                exportFileName.Title = "Save exported GRBL Settings";
                exportFileName.FileName = "Exported_GRBL_Settings.txt";

                if (exportFileName.ShowDialog() == true)
                {
                    List<Tuple<int, double>> ToExport = new List<Tuple<int, double>>();
                    foreach (KeyValuePair<int, double> kvp in CurrentSettings)
                    {
                        ToExport.Add(new Tuple<int, double>(kvp.Key, kvp.Value));
                    }
                    
                    using (StreamWriter sw = new StreamWriter(exportFileName.FileName))
                    {
                        sw.WriteLine("{0,-10} {1,-10} {2,-18} {3, -35}", "Setting", "Value", "Units", "Description");

                        foreach (Tuple<int, double> setting in ToExport)
                        // setting.Item1 = GRBL Code, setting.Item2 = Setting Value, Util.GrblCodeTranslator.Settings[setting.Item1].Item1 = Setting Description, Util.GrblCodeTranslator.Settings[setting.Item1].Item2 = Units
                        {
                            sw.WriteLine("{0,-10} {1,-10} {2,-18} {3, -35}", "$" + setting.Item1, setting.Item2.ToString(Util.Constants.DecimalOutputFormat), Util.GrblCodeTranslator.Settings[setting.Item1].Item2, Util.GrblCodeTranslator.Settings[setting.Item1].Item1);
                        }
                        MessageBox.Show("Settings Exported to " + exportFileName.FileName, "Exported GRBL Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }               
            }      
            else
            { // NO
                return;
            }
        }
    }
}
