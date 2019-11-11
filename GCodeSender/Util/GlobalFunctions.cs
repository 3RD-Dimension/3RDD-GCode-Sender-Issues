using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

// Global Function Class
namespace GCodeSender.Util
{
    public class GlobalFunctions
    {
     
        // Validate input to only numbers and decimals
        public static void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        // Format Decimal Number with no rounding
       public static string DecimalPlaceNoRounding(double d, int decimalPlaces)
        {
            d = d * Math.Pow(10, decimalPlaces);
            d = Math.Truncate(d);
            d = d / Math.Pow(10, decimalPlaces);
            return string.Format("{0:N" + Math.Abs(decimalPlaces) + "}", d);
        }
    }
}
