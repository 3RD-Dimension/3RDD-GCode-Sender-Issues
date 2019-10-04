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
        // $J=G91F800Y-100

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

        private void manualJogSendCommand(string direction)
        {
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }
    }
}
