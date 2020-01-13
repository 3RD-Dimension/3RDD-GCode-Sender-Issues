using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GCodeSender.Hotkey;
using System.Windows.Media;
using System.Text.RegularExpressions;
using GCodeSender.Util;

namespace GCodeSender
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
        private TextBox valBox;

        public SettingsWindow()
		{
			InitializeComponent();
            SerialPortBaud.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ControllerBufferSize.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            StatusPollInterval.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ViewportArcSplit.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            GridThickness.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ProbeFeed.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ProbeSafeHeight.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ProbeMaxDepth.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            ProbeTouchPlateThickness.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogSpeedXIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogSpeedYIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogSpeedZIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogDistXIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogDistYIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;
            JogDistZIncDec.PreviewTextInput += GlobalFunctions.NumberValidationTextBox;

            ComboBoxSerialPort_DropDownOpened(null, null);

            // Load Keycodes from HotKeys.hotkeycodes
            LoadHotKeys();
        }

        private void ComboBoxSerialPort_DropDownOpened(object sender, EventArgs e)
		{
			ComboBoxSerialPort.Items.Clear();

			Dictionary<string, string> ports = new Dictionary<string, string>();

			try
			{
				ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");
				foreach (ManagementObject queryObj in searcher.Get())
				{
					string id = queryObj["DeviceID"] as string;
					string name = queryObj["Name"] as string;

					ports.Add(id, name);
				}
			}
			catch (ManagementException ex)
			{
				MessageBox.Show("An error occurred while querying for WMI data: " + ex.Message);
			}

			// fix error of some boards not being listed properly
			foreach (string port in SerialPort.GetPortNames())
			{
				if (!ports.ContainsKey(port))
				{
					ports.Add(port, port);
				}
			}

			foreach (var port in ports)
			{
				ComboBoxSerialPort.Items.Add(new ComboBoxItem() { Content = port.Value, Tag = port.Key });
			}
		}

        #region Load and Edit Hotkeys in Settings
        private void LoadHotKeys()
        {
            // Load Current Hotkeys from File? or from Dictionary but will have to do TUPLE
            // Testing Iteration through dictonary
            foreach (var hotkey in HotKeys.hotkeyCode)
            {
                
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(25);
                gridMain.RowDefinitions.Add(rowDef);

                // KeyCode Name
                TextBlock keycodeDesc = new TextBlock
                {
                   Text = HotKeys.hotkeyDescription[hotkey.Key.ToString()], // Get Keycode description from HotKeys.hotkeyDescription
                   VerticalAlignment = VerticalAlignment.Center,
                   TextAlignment = TextAlignment.Left
                };
                Grid.SetRow(keycodeDesc, gridMain.RowDefinitions.Count - 1);
                Grid.SetColumn(keycodeDesc, 0);
                gridMain.Children.Add(keycodeDesc);

                // Keycode TextBox
                valBox = new TextBox
                {
                    Text = hotkey.Value,
                    Name = hotkey.Key,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Left                   
                };

                valBox.MouseDoubleClick += clearHotKey;
                valBox.PreviewKeyDown += previewKeyCode;
                valBox.IsReadOnly = true;
                Grid.SetRow(valBox, gridMain.RowDefinitions.Count - 1);
                Grid.SetColumn(valBox, 1);
                gridMain.Children.Add(valBox);
            }
        }

        /// <summary>
        /// Preview of New Keycode being entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewKeyCode(object sender, KeyEventArgs e)
        {
            var hotKeyInputTextBox = sender as TextBox;

            // Get Keycode
            string currentHotPressed = HotKeys.KeyProcess(sender, e); // Get Keycode
            if (currentHotPressed == null) return; // If currentHotPressed is null, Return (to avoid continuing with blank)

            hotKeyInputTextBox.Text = string.Empty;
            hotKeyInputTextBox.Text = string.Format("{0}", currentHotPressed);

            // Save New KeyCode value to Hotkeys XML
            HotKeys.UpdateHotkey(hotKeyInputTextBox.Name, hotKeyInputTextBox.Text);

            e.Handled = true;
        }

        private void clearHotKey(object sender, RoutedEventArgs e)
        {
            var hotKeyInputTextBox = sender as TextBox;
            hotKeyInputTextBox.Text = ""; // Clear Hotkey
            HotKeys.UpdateHotkey(hotKeyInputTextBox.Name, hotKeyInputTextBox.Text); // Save blank hotkey
        }

		private void Window_Closed(object sender, EventArgs e)
		{
            // Reload Hotkeys so dont need to close and re-open program
            HotKeys.LoadHotKeys(); // Load Hotkeys

            Properties.Settings.Default.Save();
		}
        #endregion
    }
}
