// Manual Jogging
using GCodeSender.Communication;
using GCodeSender.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GCodeSender
{

    partial class MainWindow
    {
        private string zeroCommand = "G10 L20 P0";

        private void manualJogBtnXPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("X");
        }

        private void manualJogBtnXNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("X-");
        }

        private void manualJogBtnYPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("Y");
        }

        private void manualJogBtnYNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("Y-");
        }

        private void manualJogBtnZPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("Z");
        }

        private void manualJogBtnZNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            manualJogSendCommand("Z-");
        }

        // Zero X Axis
        private void ButtonManualResetX_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(zeroCommand + "X0");
        }
        // Zero Y Axis
        private void ButtonManualResetY_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(zeroCommand + "Y0");
        }
        // Zero Z Axis
        private void ButtonManualResetZ_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(zeroCommand + "Z0");
        }

        // Zero All Axis
        private void ButtonManualResetAll_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(zeroCommand + "X0 Y0 Z0");
        }

        private void ButtonManualReturnToZero_Click(object sender, RoutedEventArgs e)
        {
            // Return all Axis to Zero
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;

            machine.SendLine("G90 G0 X0 Y0");
            machine.SendLine("G90 G0 Z0");
        }

        public void manualJogSendCommand(string direction)
        {
            double feedX = Properties.Settings.Default.JogFeedX;
            double distanceX = Properties.Settings.Default.JogDistanceX;
            double feedY = Properties.Settings.Default.JogFeedY;
            double distanceY = Properties.Settings.Default.JogDistanceY;
            double feedZ = Properties.Settings.Default.JogFeedZ;
            double distanceZ = Properties.Settings.Default.JogDistanceZ;


            // If X+,X- or Y+,Y-
            if (direction == "X" || direction == "X-")
            {
                machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feedX, direction, distanceX));
            }
            else if (direction == "Y" || direction == "Y-")
            {
                machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feedY, direction, distanceY));
            }
            else if (direction == "Z" || direction == "Z-")
            {
                machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feedZ, direction, distanceZ));
            }
            
        }
       
    }
}
