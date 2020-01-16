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
	   
         private void Jogging_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            machine.JogCancel();
        }

        private void Jogging_KeyUp(object sender, KeyEventArgs e)
        {
            machine.JogCancel();
        }
    }
}
