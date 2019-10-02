using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace GCodeSender
{
    /// <summary>
    /// Interaction logic for WorkOffsetsWindow.xaml
    /// </summary>
    public partial class WorkOffsetsWindow : Window
    {
        //private Thread WorkerThread;

        public WorkOffsetsWindow()
        {
            InitializeComponent();

            //WorkerThread = new Thread(Work);
            //WorkerThread.Priority = ThreadPriority.AboveNormal;
            //WorkerThread.Start();

        }

        // Get and Parse Offsets for G54, G55, G56, G57, G58, G59. Called from UpdateStatus
        public void parseG5xOffsets(string recievedG5x)
        {
            // Splitting each recieved axis and value

            string label;
            string[] axes;

            recievedG5x = recievedG5x.Remove(0, 1);                   // remove the leading [
            recievedG5x = recievedG5x.Remove(recievedG5x.Length - 1, 1);     // remove the trailing "] <vbLf>"
            label = recievedG5x.Substring(0, 3);
            recievedG5x = recievedG5x.Remove(0, 4);                   // finally remove the label:
            axes = recievedG5x.Split(',');

            switch (label)
            {
                case "G54":
                case "G55":
                case "G56":
                case "G57":
                case "G58":
                case "G59":
                    {
                        int i = 0;
                        foreach (var axi in new[] { "X", "Y", "Z" })

                        // Label=G5x, Axi= X, Y or Z, Axes(i)= Value for that Axis
                        // Text box format is G54X
                        {
                            var offsetTextBox = (TextBox)this.FindName(label + axi);
                            offsetTextBox.Text = axes[i];
                           
                            i += 1;
                        }

                        break;
                    }
            }
        }
 
         private void Window_Closed(object sender, EventArgs e)
        {
            return;
        }
    }
}
