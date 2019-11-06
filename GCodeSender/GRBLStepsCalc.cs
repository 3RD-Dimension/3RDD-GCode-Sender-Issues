using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
/// <summary>
/// Class for GRBL Steps Calculation
/// </summary>
namespace GCodeSender
{
    partial class GrblSettingsWindow
    {
        #region Functions
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        string DecimalPlaceNoRounding(double d, int decimalPlaces)
        {
            d = d * Math.Pow(10, decimalPlaces);
            d = Math.Truncate(d);
            d = d / Math.Pow(10, decimalPlaces);
            return string.Format("{0:N" + Math.Abs(decimalPlaces) + "}", d);
        }
        #endregion

        #region Steps Fine Tune
        // Steps Fine Tune
        double _currentSteps = 0; // Current Step Value from GRBLSettingsWindow
        double _distanceCommanded = 0; // Distance the user wanted
        double _distanceTraveled = 0; // Actuall distance acheived
        double _newFineTuneSteps = 0; // New calculated Step Value
        TextBox _settingTextBox; // Holds the name of the selected Setting Textbox

        private void checkFineTuneInput(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSteps.Text) && !string.IsNullOrEmpty(distanceCommanded.Text) && !string.IsNullOrEmpty(distanceTraveled.Text))
            {
                _distanceTraveled = Convert.ToDouble(distanceTraveled.Text);
                _distanceCommanded = Convert.ToDouble(distanceCommanded.Text);
                calcNewSteps();
            }
        }

        private void calcNewSteps()
        {
            _newFineTuneSteps = _currentSteps * (_distanceCommanded / _distanceTraveled);
            newStepValue.Text = DecimalPlaceNoRounding(_newFineTuneSteps, 3); ;
        }

        public void openStepsCalc(object sender, RoutedEventArgs e)
        {
            // Clear Textboxes
            distanceTraveled.Clear();
            distanceCommanded.Clear();
            newStepValue.Clear();

            _settingTextBox = sender as TextBox;
            currentSteps.Text = _settingTextBox.Text;
            _currentSteps = Convert.ToDouble(currentSteps.Text);
            GRBLSettingsScroller.IsEnabled = false;
            StepsCalcPanel.Visibility = Visibility.Visible;
            Console.WriteLine(_settingTextBox.Name);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            GRBLSettingsScroller.IsEnabled = true;
            StepsCalcPanel.Visibility = Visibility.Hidden;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            // Use Calculated Value for Setting
            _settingTextBox.Text = newStepValue.Text;
            StepsCalcPanel.Visibility = Visibility.Hidden;
            GRBLSettingsScroller.IsEnabled = true;
        }
        #endregion
    }
}