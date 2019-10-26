// Manual Jogging
using GCodeSender.Communication;
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
            machine.SendLine(Properties.Settings.Default.ZeroXCmd);
        }
        // Zero Y Axis
        private void ButtonManualResetY_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(Properties.Settings.Default.ZeroYCmd);
        }
        // Zero Z Axis
        private void ButtonManualResetZ_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(Properties.Settings.Default.ZeroZCmd);
        }

        private void ButtonManualReturnToZero_Click(object sender, RoutedEventArgs e)
        {
            // Return all Axis to Zero
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;

            machine.SendLine("G90 G0 X0 Y0");
            machine.SendLine("G90 G0 Z0");
        }

         // Zero All Axis
        private void ButtonManualResetAll_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            machine.SendLine(Properties.Settings.Default.ZeroAllCmd);
        }

        public void manualJogSendCommand(string direction)
        {
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }
       
    }
}
