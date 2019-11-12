using GCodeSender.Util;
using System;
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
            newStepValue.Text = GlobalFunctions.DecimalPlaceNoRounding(_newFineTuneSteps, 3); ;
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