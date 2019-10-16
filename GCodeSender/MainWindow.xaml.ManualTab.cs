using GCodeSender.Communication;
using GCodeSender.Util;
using GCodeSender.Hotkey;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace GCodeSender
{
	partial class MainWindow
	{
        private List<string> ManualCommands = new List<string>();   //pos 0 is the last command sent, pos1+ are older
		private int ManualCommandIndex = -1;
               
        void ManualSend()
		{
			if (machine.Mode != Machine.OperatingMode.Manual)
				return;

			string tosend;

			tosend = TextBoxManual.Text;

			machine.SendLine(tosend);

			ManualCommands.Insert(0, tosend);
			ManualCommandIndex = -1;

			TextBoxManual.Text = "";
		}

		private void ButtonManualSend_Click(object sender, RoutedEventArgs e)
		{
			ManualSend();
		}

		private void TextBoxManual_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				ManualSend();
			}
			else if (e.Key == Key.Down)
			{
				e.Handled = true;

				if (ManualCommandIndex == 0)
				{
					TextBoxManual.Text = "";
					ManualCommandIndex = -1;
				}
				else if (ManualCommandIndex > 0)
				{
					ManualCommandIndex--;
					TextBoxManual.Text = ManualCommands[ManualCommandIndex];
					TextBoxManual.SelectionStart = TextBoxManual.Text.Length;
				}
			}
			else if (e.Key == Key.Up)
			{
				e.Handled = true;

				if (ManualCommands.Count > ManualCommandIndex + 1)
				{
					ManualCommandIndex++;
					TextBoxManual.Text = ManualCommands[ManualCommandIndex];
					TextBoxManual.SelectionStart = TextBoxManual.Text.Length;
				}
			}
		}
	   
        private void CheckBoxEnableJog_Checked(object sender, RoutedEventArgs e)
		{
			if (machine.Mode != Machine.OperatingMode.Manual)
			{
				CheckBoxEnableJog.IsChecked = false;
				return;
			}
		}

		private void CheckBoxEnableJog_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!machine.Connected)
				return;
			machine.JogCancel();
		}

        private void Jogging_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            machine.JogCancel();
        }

        private void Jogging_KeyUp(object sender, KeyEventArgs e)
        {
            machine.JogCancel();
        }
        
        // This is run when Jogging Keyboard Focus is Focused
        private void Jogging_KeyDown(object sender, KeyEventArgs e)
		{
			if (!machine.Connected)
				return;

            // Get Keycode
            string currentHotPressed = HotKeys.KeyProcess(sender, e); // Get Keycode
            if (currentHotPressed == null) return; // If currentHotPressed is null, Return (to avoid continuing with  blank)

			if (!CheckBoxEnableJog.IsChecked.Value)
				return;

			e.Handled = e.Key != Key.Tab;

			if (e.IsRepeat)
				return;

			if (machine.BufferState > 0 || machine.Status != "Idle")
				return;

			string direction = null;

            if (currentHotPressed == HotKeys.hotkeyCode["JogXPos"])
                direction = "X";
            else if (currentHotPressed == HotKeys.hotkeyCode["JogXNeg"])
                direction = "X-";
            else if (currentHotPressed == HotKeys.hotkeyCode["JogYPos"])
                direction = "Y";
            else if (currentHotPressed == HotKeys.hotkeyCode["JogYNeg"])
                direction = "Y-";
            else if (currentHotPressed == HotKeys.hotkeyCode["JogZPos"])
                direction = "Z";
            else if (currentHotPressed == HotKeys.hotkeyCode["JogZNeg"])
                direction = "Z-";
            else if (currentHotPressed == HotKeys.hotkeyCode["RTOrigin"]) // Return to Origin ie back to all axis Zero position
                ButtonManualReturnToZero.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            else if (currentHotPressed == HotKeys.hotkeyCode["FSStop"]) // Jog Cancel
                machine.JogCancel();
   
			double feed = Properties.Settings.Default.JogFeed;
			double distance = Properties.Settings.Default.JogDistance;

			if (direction != null)
			{
				machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
			}
		}
		
        // Any other keys during normal operation including when machine is in motion
		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
            string currentHotPressed = HotKeys.KeyProcess(sender, e); // Get Keycode
            if (currentHotPressed == null) return; // If currentHotPressed is null, Return (to avoid continuing with  blank)

            // Emergency Reset
            if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["EmgStop"])
                machine.SoftReset();
            // Cycle Start - Only allowed if Connected, and not currently sending a file
            if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["CycleStart"])
                ButtonFileStart.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Reload File (Re-Do)
            if (machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["ReDoReload"]) // Only if not currently sending
                ReloadCurrentFile();
            if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FSStop"]) // Put machine on Hold if sending a file
                ButtonFilePause.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Spindle and Flood  (firmware takes care if it will enable or disable so just need to know if it is connected to controller
            if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["SpindleOnOff"]) // Spindle Toggle
                SpindleControl();
            if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["CoolantOnOff"]) // Coolant Toggle
                FloodControl();
            if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["MistOnOff"]) // Mist Toggle
               MistControl();

            // Need to save back to Settings otherwise will not register unless it is focused, maybe try Focus() first....
            // Jog rate Increase and Decrease (This is saved to settings always)
            if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateInc"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeed.Text);
                TextBoxJogFeed.Text = (currentJogRate + 10).ToString();                
            }

            // Jog rate Increase and Decrease (This is saved to settings always)
            if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDec"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeed.Text);
                TextBoxJogFeed.Text = (currentJogRate - 10).ToString();
            }

            // Feed Rate
            // FeedRateIncrement & SpindleIncrement False = 1% Inc and Dec, True = 10% Inc and Dec
            if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FRateInc"]) // Feed Rate Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Increase
                {
                    Feed10Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Increase
                {
                    Feed1Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FRateDec"]) // Feed Rate Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Decrease
                {
                    Feed10Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Decrease
                {
                    Feed1Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            // Spindle Increase and Decrease
            if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["SpindleInc"]) // Spindle Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Increase
                {
                    Spindle10Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Increase
                {
                    Spindle1Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["SprindleDec"]) // Spindle Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Decrease
                {
                    Spindle10Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Decrease
                {
                    Spindle1Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
    }
}
