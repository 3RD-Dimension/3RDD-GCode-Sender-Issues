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
    //TODO DYNAMICALLY ADDING TEXT BOXES?  SIMILAR TO GrblSettingsWindow?
    /// <summary>
    /// Interaction logic for WorkOffsetsWindow.xaml
    /// </summary>
    public partial class WorkOffsetsWindow : Window
    {
              
        public WorkOffsetsWindow()
        {
            InitializeComponent();
           
        }
        
        public void parseG5xOffsets(string recievedG5x)
        {
            if (recievedG5x.StartsWith("[G54:") || recievedG5x.StartsWith("[G55:") || recievedG5x.StartsWith("[G56:") || recievedG5x.StartsWith("[G57:") || recievedG5x.StartsWith("[G58:") || recievedG5x.StartsWith("[G59:"))
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
        }             

        private void Window_Closed(object sender, EventArgs e)
        {
            return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
         
        }

    }
}
