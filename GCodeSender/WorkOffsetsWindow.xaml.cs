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
    /// <summary>
    /// Interaction logic for WorkOffsetsWindow.xaml
    /// </summary>

    public partial class WorkOffsetsWindow : Window
    {
        public event Action<string> SendLine; // Allow Sendline to be used, used for saving Offsets back to Controller

        public WorkOffsetsWindow()
        {
            InitializeComponent();        
        }

        #region Save Selected Offset
        /// <summary>
        /// Saves work offset values for the selected work offset ie G54, G55, G56, G57, G58, G59
        /// </summary>
        /// <param name="selectedOffset"></param>
        private void saveWorkOffset(string selectedOffset)
        {
            // TODO Refactor all these to make it simplier
            string Xaxis = "0.000";
            string Yaxis = "0.000";
            string Zaxis = "0.000";

            // 1) Get Selected Offset ie G54-G59.
            // 2) Get values from the selected Offset Textboxes - X, Y, Z
            // 3) Send Command G10 L2 P1  X0.000, Y0.000, Z0.000 - G54=P1....G59=P6
            if (selectedOffset == "G54")
            {
                Xaxis = G54X.Text;
                Yaxis = G54Y.Text;
                Zaxis = G54Z.Text;

                SendLine.Invoke($"G10 L2 P1 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G55")
            {
                Xaxis = G55X.Text;
                Yaxis = G55Y.Text;
                Zaxis = G55Z.Text;

                SendLine.Invoke($"G10 L2 P2 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G56")
            {
                Xaxis = G56X.Text;
                Yaxis = G56Y.Text;
                Zaxis = G56Z.Text;

                SendLine.Invoke($"G10 L2 P3 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G57")
            {
                Xaxis = G57X.Text;
                Yaxis = G57Y.Text;
                Zaxis = G57Z.Text;

                SendLine.Invoke($"G10 L2 P4 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G58")
            {
                Xaxis = G58X.Text;
                Yaxis = G58Y.Text;
                Zaxis = G58Z.Text;

                SendLine.Invoke($"G10 L2 P5 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G59")
            {
                Xaxis = G59X.Text;
                Yaxis = G59Y.Text;
                Zaxis = G59Z.Text;

                SendLine.Invoke($"G10 L2 P6 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else
            {
                return;
            }

        }
        #endregion


        #region WorkOffset Save Buttons
        // Buttons for Setting Workoffset - send command saveWorkOffset("G54") for example
        private void BtnSetG54_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G54");
        }

        private void BtnSetG55_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G55");
        }

        private void BtnSetG56_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G56");
        }

        private void BtnSetG57_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G57");
        }

        private void BtnSetG58_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G58");
        }

        private void BtnSetG59_Click(object sender, RoutedEventArgs e)
        {
            saveWorkOffset("G59");
        }
          #endregion


        #region Load Work Offsets
        public void parseG5xOffsets(string recievedG5x)
        {
            // Need someway to ignore this until the machine is idle and not operating
            if (recievedG5x.StartsWith("[G54:") || recievedG5x.StartsWith("[G55:") || recievedG5x.StartsWith("[G56:") || recievedG5x.StartsWith("[G57:") || recievedG5x.StartsWith("[G58:") || recievedG5x.StartsWith("[G59:"))
            {
                // Splitting each recieved axis and value
                string label;
                string[] axes;

                recievedG5x = recievedG5x.Remove(0, 1); // remove the leading [
                recievedG5x = recievedG5x.Remove(recievedG5x.Length - 1, 1); // remove the trailing "] <vbLf>"
                label = recievedG5x.Substring(0, 3);
                recievedG5x = recievedG5x.Remove(0, 4); // finally remove the label:
                axes = recievedG5x.Split(',');

                switch (label)
                {
                    case "G54":
                        {
                            G54X.Text = axes[0].ToString();
                            G54Y.Text = axes[1].ToString();
                            G54Z.Text = axes[2].ToString();
                            break;
                        }
                    case "G55":
                        {
                            G55X.Text = axes[0].ToString();
                            G55Y.Text = axes[1].ToString();
                            G55Z.Text = axes[2].ToString();
                            break;
                        }
                    case "G56":
                        {
                            G56X.Text = axes[0].ToString();
                            G56Y.Text = axes[1].ToString();
                            G56Z.Text = axes[2].ToString();
                            break;
                        }
                    case "G57":
                        {
                            G57X.Text = axes[0].ToString();
                            G57Y.Text = axes[1].ToString();
                            G57Z.Text = axes[2].ToString();
                            break;
                        }
                    case "G58":
                        {
                            G58X.Text = axes[0].ToString();
                            G58Y.Text = axes[1].ToString();
                            G58Z.Text = axes[2].ToString();
                            break;
                        }
                    case "G59":
                        {                         
                           G59X.Text = axes[0].ToString();
                           G59Y.Text = axes[1].ToString();
                           G59Z.Text = axes[2].ToString();
                           break;
                        }
                }
            }
            else
            {
                return;
            }
        }
        #endregion


        #region Closing Window Functions
        private void Window_Closed(object sender, EventArgs e)
        {
            return;
        } 

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        #endregion
        
    }
}
