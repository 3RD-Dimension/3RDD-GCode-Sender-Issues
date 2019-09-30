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
        string direction = null;
        
        // $J=G91F800Y-100

        private void manualJogBtnXPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "X";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }

        private void manualJogBtnXNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "X-";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }

        private void manualJogBtnYPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "Y";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }

        private void manualJogBtnYNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "Y-";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }

        private void manualJogBtnZPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "Z";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }

        private void manualJogBtnZNegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Machine.OperatingMode.Manual)
                return;
            double feed = Properties.Settings.Default.JogFeed;
            double distance = Properties.Settings.Default.JogDistance;
            direction = "Z-";
            machine.SendLine(string.Format(Constants.DecimalOutputFormat, "$J=G91F{0:0.#}{1}{2:0.###}", feed, direction, distance));
        }        
    }
}
