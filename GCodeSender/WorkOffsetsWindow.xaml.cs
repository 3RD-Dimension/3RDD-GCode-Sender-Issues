using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using GCodeSender.Communication;

namespace GCodeSender
{
    /// <summary>
    /// Interaction logic for WorkOffsetsWindow.xaml
    /// </summary>

    public partial class WorkOffsetsWindow : Window
    {
        // TODO Refactor all these to make it simplier.  Maybe after changing and saving, refresh with issuing $# to make sure the changes have
        // been saved instead of manually setting the text boxes to the new values.
        public event Action<string> SendLine; // Allow Sendline to be used, used for saving Offsets back to Controller

        public WorkOffsetsWindow()
        {
            InitializeComponent();
        }

        #region Save Selected Offset G5x
        /// <summary>
        /// Saves work offset values for the selected work offset ie G54, G55, G56, G57, G58, G59
        /// </summary>
        /// <param name="selectedOffset"></param>
        private void saveWorkOffset(string selectedOffset)
        {
            
            string Xaxis = "0.000";
            string Yaxis = "0.000";
            string Zaxis = "0.000";

            // 1) Get Selected Offset ie G54-G59.
            // 2) Get values from the selected Offset Textboxes - X, Y, Z
            // 3) Send Command G10 L2 P1  X0.000, Y0.000, Z0.000 - G54=P1....G59=P6
            if (selectedOffset == "G54")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G54X.Text = Xaxis;
                G54Y.Text = Yaxis;
                G54Z.Text = Zaxis;

                SendLine.Invoke($"G10 L2 P1 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G55")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G55X.Text = Xaxis;
                G55Y.Text = Yaxis;
                G55Z.Text = Zaxis;

                SendLine.Invoke($"G10 L2 P2 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G56")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G56X.Text = Xaxis;
                G56Y.Text = Yaxis;
                G56Z.Text = Zaxis;

                SendLine.Invoke($"G10 L2 P3 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G57")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G57X.Text = Xaxis;
                G57Y.Text = Yaxis;
                G57Z.Text = Zaxis;

                SendLine.Invoke($"G10 L2 P4 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G58")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G58X.Text = Xaxis;
                G58Y.Text = Yaxis;
                G58Z.Text = Zaxis;

                SendLine.Invoke($"G10 L2 P5 X{Xaxis} Y{Yaxis} Z{Zaxis}");
            }
            else if (selectedOffset == "G59")
            {
                Xaxis = MachineX_Current.Text;
                Yaxis = MachineY_Current.Text;
                Zaxis = MachineZ_Current.Text;

                G59X.Text = Xaxis;
                G59Y.Text = Yaxis;
                G59Z.Text = Zaxis;

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

           
        #region Reset Selected Workoffset to Zero
        /// <summary>
        /// Reset Selected G54 X, Y, Z Offsets - All Reset buttons use this function - uses TAG= to desinguish between what button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnResetOffset_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b == null)
                return;

            switch (b.Tag as string)
            {
                case "ResetG54":
                    G54X.Text = "0.000";
                    G54Y.Text = "0.000";
                    G54Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P1 X0.000 Y0.000 Z0.000");
                    break;
                case "ResetG55":
                    G55X.Text = "0.000";
                    G55Y.Text = "0.000";
                    G55Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P2 X0.000 Y0.000 Z0.000");
                    break;
                case "ResetG56":
                    G56X.Text = "0.000";
                    G56Y.Text = "0.000";
                    G56Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P3 X0.000 Y0.000 Z0.000");
                    break;
                case "ResetG57":
                    G57X.Text = "0.000";
                    G57Y.Text = "0.000";
                    G57Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P4 X0.000 Y0.000 Z0.000");
                    break;
                case "ResetG58":
                    G58X.Text = "0.000";
                    G58Y.Text = "0.000";
                    G58Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P5 X0.000 Y0.000 Z0.000");
                    break;
                case "ResetG59":
                    G59X.Text = "0.000";
                    G59Y.Text = "0.000";
                    G59Z.Text = "0.000";
                    SendLine.Invoke($"G10 L2 P6 X0.000 Y0.000 Z0.000");
                    break;
            }
        }
        #endregion


        #region SaveG5x axis by Double-Clicking on the field after user has change one of the values manually.
        private void saveIndividualOffsetAxis(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string newValue;
                       
            TextBox b = sender as TextBox;

            if (b == null)
                return;

            switch (b.Tag as string)
            {
                // G10 L2 Px Axis+Value
                // G54 Axies
                case "G54X":
                    newValue = G54X.Text;
                    SendLine.Invoke($"G10 L2 P1 X{newValue}");
                    break;
                case "G54Y":
                    newValue = G54Y.Text;
                    SendLine.Invoke($"G10 L2 P1 Y{newValue}");
                    break;
                case "G54Z":
                    newValue = G54Z.Text;
                    SendLine.Invoke($"G10 L2 P1 Z{newValue}");
                    break;
                // G55 Axies
                case "G55X":
                    newValue = G55X.Text;
                    SendLine.Invoke($"G10 L2 P2 X{newValue}");
                    break;
                case "G55Y":
                    newValue = G55Y.Text;
                    SendLine.Invoke($"G10 L2 P2 Y{newValue}");
                    break;
                case "G55Z":
                    newValue = G55Z.Text;
                    SendLine.Invoke($"G10 L2 P2 Z{newValue}");
                    break;
                // G56 Axies
                case "G56X":
                    newValue = G56X.Text;
                    SendLine.Invoke($"G10 L2 P3 X{newValue}");
                    break;
                case "G56Y":
                    newValue = G56Y.Text;
                    SendLine.Invoke($"G10 L2 P3 Y{newValue}");
                    break;
                case "G56Z":
                    newValue = G56Z.Text;
                    SendLine.Invoke($"G10 L2 P3 Z{newValue}");
                    break;
                // G57 Axies
                case "G57X":
                    newValue = G57X.Text;
                    SendLine.Invoke($"G10 L2 P4 X{newValue}");
                    break;
                case "G57Y":
                    newValue = G57Y.Text;
                    SendLine.Invoke($"G10 L2 P4 Y{newValue}");
                    break;
                case "G57Z":
                    newValue = G57Z.Text;
                    SendLine.Invoke($"G10 L2 P4 Z{newValue}");
                    break;
                // G58 Axies
                case "G58X":
                    newValue = G58X.Text;
                    SendLine.Invoke($"G10 L2 P5 X{newValue}");
                    break;
                case "G58Y":
                    newValue = G58Y.Text;
                    SendLine.Invoke($"G10 L2 P5 Y{newValue}");
                    break;
                case "G58Z":
                    newValue = G58Z.Text;
                    SendLine.Invoke($"G10 L2 P5 Z{newValue}");
                    break;
                // G59 Axies
                case "G59X":
                    newValue = G59X.Text;
                    SendLine.Invoke($"G10 L2 P6 X{newValue}");
                    break;
                case "G59Y":
                    newValue = G59Y.Text;
                    SendLine.Invoke($"G10 L2 P6 Y{newValue}");
                    break;
                case "G59Z":
                    newValue = G59Z.Text;
                    SendLine.Invoke($"G10 L2 P6 Z{newValue}");
                    break;
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
