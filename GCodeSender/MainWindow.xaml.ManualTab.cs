﻿using GCodeSender.Communication;
using GCodeSender.Util;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GCodeSender
{
	partial class MainWindow
	{
		private List<string> ManualCommands = new List<string>();   //pos 0 is the last command sent, pos1+ are older
		private int ManualCommandIndex = -1;
		private bool ManualExpressionSuccess = true;

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

	    // Zero All Axis
		private void ButtonManualResetAll_Click(object sender, RoutedEventArgs e)
		{
			if (machine.Mode != Machine.OperatingMode.Manual)
				return;
            TextBoxManual.Text = Properties.Settings.Default.ZeroAllCmd;
            ManualSend(); // Automatically Send Command
		}

        // TODO Get the commands from Settings
        // Zero X Axis
        private void ButtonManualResetX_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            TextBoxManual.Text = Properties.Settings.Default.ZeroXCmd;               
            ManualSend(); // Automatically Send Command
        }
        // Zero Y Axis
        private void ButtonManualResetY_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            TextBoxManual.Text = Properties.Settings.Default.ZeroYCmd;
            ManualSend(); // Automatically Send Command
        }
        // Zero Z Axis
        private void ButtonManualResetZ_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            TextBoxManual.Text = Properties.Settings.Default.ZeroZCmd;
            ManualSend(); // Automatically Send Command
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

         // TODO Add Configurable Keys 
		private void Jogging_KeyDown(object sender, KeyEventArgs e)
		{
			if (!machine.Connected)
				return;

            // Pressing ESC Key Soft Resets Machine
			if (e.Key == Key.Escape)
			{
				if (Properties.Settings.Default.EnableEscapeSoftReset)
					machine.SoftReset();
				else
					machine.JogCancel();
			}

			if (!CheckBoxEnableJog.IsChecked.Value)
				return;

			e.Handled = e.Key != Key.Tab;

			if (e.IsRepeat)
				return;

			if (machine.BufferState > 0 || machine.Status != "Idle")
				return;

			string direction = null;

            // If Left or Right Shift is held down...
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				if (e.Key == Key.Up)
					direction = "Z";
				else if (e.Key == Key.Down)
					direction = "Z-";
			}
			else // Otherwise If Left or Right Shift is not held down
			{
				if (e.Key == Key.Right)
					direction = "X";
				else if (e.Key == Key.Left)
					direction = "X-";
				else if (e.Key == Key.Up)
					direction = "Y";
				else if (e.Key == Key.Down)
					direction = "Y-";
				else if (e.Key == Key.PageUp)
					direction = "Z";
				else if (e.Key == Key.PageDown)
					direction = "Z-";
			}

			double feed = Properties.Settings.Default.JogFeed;
			double distance = Properties.Settings.Default.JogDistance;

			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				feed = Properties.Settings.Default.JogFeedCtrl;
				distance = Properties.Settings.Default.JogDistanceCtrl;
			}

			if (direction != null)
			{
				machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
			}
		}

		private void Jogging_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			machine.JogCancel();
		}

		private void Jogging_KeyUp(object sender, KeyEventArgs e)
		{
			machine.JogCancel();
		}

		private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (machine.Connected && e.Key == Key.Escape && Properties.Settings.Default.EnableEscapeSoftReset)
				machine.SoftReset();
		}
	}
}
