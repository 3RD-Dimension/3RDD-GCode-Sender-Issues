using GCodeSender.Communication;
using GCodeSender.Hotkey;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GCodeSender
{
    /// <summary>
    /// Main Hotkey Control
    /// TODO: Optimize hotkey functions
    /// </summary>
    partial class MainWindow
    {
        // Any other keys during normal operation including when machine is in motion
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!machine.Connected)
                return;     

            string currentHotPressed = HotKeys.KeyProcess(sender, e); // Get Keycode
            if (currentHotPressed == null) return; // If currentHotPressed is null, Return (to avoid continuing with  blank)

            if (currentHotPressed == "Left" || currentHotPressed == "Right" || currentHotPressed == "Up" || currentHotPressed == "Down")
            {
                viewport.IsPanEnabled = false;
                viewport.IsRotationEnabled = false;
                viewport.IsMoveEnabled = false;
            }

            if (machine.Mode == Machine.OperatingMode.Manual)
            {
                if (machine.BufferState > 0 || machine.Status != "Idle")
                    return;

                string direction = null;

                if (currentHotPressed == HotKeys.hotkeyCode["JogXPos"])
                    direction = "X";
                else if (currentHotPressed == HotKeys.hotkeyCode["JogXNeg"])
                    direction = "X-";
                else if (currentHotPressed == HotKeys.hotkeyCode["JogYPos"])
                    direction = "Y";
                else if (currentHotPressed == HotKeys.hotkeyCode["JogYNeg"])
                    direction = "Y-";
                else if (currentHotPressed == HotKeys.hotkeyCode["JogZPos"])
                    direction = "Z";
                else if (currentHotPressed == HotKeys.hotkeyCode["JogZNeg"])
                    direction = "Z-";
                else if (currentHotPressed == HotKeys.hotkeyCode["RTOrigin"]) // Return to Origin ie back to all axis Zero position
                    ButtonManualReturnToZero.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                else if (currentHotPressed == HotKeys.hotkeyCode["FSStop"]) // Jog Cancel
                    machine.JogCancel();

                if (direction != null)
                {
                    manualJogSendCommand(direction);
                }
            }

            viewport.IsPanEnabled = true;
            viewport.IsRotationEnabled = true;
            viewport.IsMoveEnabled = true;

            // Emergency Reset
            if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["EmgStop"])
                machine.SoftReset();
            // Cycle Start - Only allowed if Connected, and not currently sending a file
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["CycleStart"])
                ButtonFileStart.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Reload File (Re-Do)
            else if (machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["ReDoReload"]) // Only if not currently sending
                ReloadCurrentFile();
            else if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FSStop"]) // Put machine on Hold if sending a file
                ButtonFilePause.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Spindle and Flood  (firmware takes care if it will enable or disable so just need to know if it is connected to controller to turn on Manually.           
            else if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["SpindleOnOff"]) // Spindle Toggle
                SpindleControl();
            else if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["CoolantOnOff"]) // Coolant Toggle
                FloodControl();
            else if (machine.Connected && currentHotPressed == HotKeys.hotkeyCode["MistOnOff"]) // Mist Toggle
                MistControl();

            // Need to save back to Settings otherwise will not register unless it is focused, maybe try Focus() first....
            
            // JOG RATE AXIS X
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateIncX"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedX.Text);
                double newJogRate = currentJogRate + Properties.Settings.Default.JogFeedXIncDec;
                TextBoxJogFeedX.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedX = newJogRate;
            }           
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecX"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedX.Text);
                double newJogRate = currentJogRate - Properties.Settings.Default.JogFeedXIncDec;
                if (newJogRate < Properties.Settings.Default.JogFeedXIncDec)
                    newJogRate = Properties.Settings.Default.JogFeedXIncDec;
                TextBoxJogFeedX.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedX = newJogRate;
            }
            // JOG RATE AXIS Y
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateIncY"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedY.Text);
                double newJogRate = currentJogRate + Properties.Settings.Default.JogFeedYIncDec;
                TextBoxJogFeedY.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedY = newJogRate;
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecY"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedY.Text);
                double newJogRate = currentJogRate - Properties.Settings.Default.JogFeedYIncDec;
                if (newJogRate < Properties.Settings.Default.JogFeedYIncDec)
                    newJogRate = Properties.Settings.Default.JogFeedYIncDec;
                TextBoxJogFeedY.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedY = newJogRate;
            }
            // JOG RATE AXIS Z
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateIncZ"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedZ.Text);
                double newJogRate = currentJogRate + Properties.Settings.Default.JogFeedZIncDec;
                TextBoxJogFeedZ.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedZ = newJogRate;
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecZ"])
            {
                double currentJogRate = Convert.ToDouble(TextBoxJogFeedZ.Text);
                double newJogRate = currentJogRate - Properties.Settings.Default.JogFeedZIncDec;
                if (newJogRate < Properties.Settings.Default.JogFeedZIncDec)
                    newJogRate = Properties.Settings.Default.JogFeedZIncDec;
                TextBoxJogFeedZ.Text = newJogRate.ToString();
                Properties.Settings.Default.JogFeedZ = newJogRate;
            }

            // JOG DISTANCE X
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncX"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceX.Text);
                double newJogDist = currentJogDist + Properties.Settings.Default.JogDistXIncDec;
                TextBoxJogDistanceX.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceX = newJogDist;
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecX"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceX.Text);
                double newJogDist = currentJogDist - Properties.Settings.Default.JogDistXIncDec;
                if (newJogDist < Properties.Settings.Default.JogDistXIncDec)  
                    newJogDist = Properties.Settings.Default.JogDistXIncDec;
                TextBoxJogDistanceX.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceX = newJogDist;
            }

            // JOG DISTANCE Y
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncY"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceY.Text);
                double newJogDist = currentJogDist + Properties.Settings.Default.JogDistYIncDec;
                TextBoxJogDistanceY.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceY = newJogDist;
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecY"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceY.Text);
                double newJogDist = currentJogDist - Properties.Settings.Default.JogDistYIncDec;
                if (newJogDist < Properties.Settings.Default.JogDistYIncDec)
                    newJogDist = Properties.Settings.Default.JogDistYIncDec;
                TextBoxJogDistanceY.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceY = newJogDist;
            }

            // JOG DISTANCE Z
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncZ"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceZ.Text);
                double newJogDist = currentJogDist + Properties.Settings.Default.JogDistZIncDec;
                TextBoxJogDistanceZ.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceZ = newJogDist;
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecZ"])
            {
                double currentJogDist = Convert.ToDouble(TextBoxJogDistanceZ.Text);
                double newJogDist = currentJogDist - Properties.Settings.Default.JogDistZIncDec;
                if (newJogDist < Properties.Settings.Default.JogDistZIncDec)
                    newJogDist = Properties.Settings.Default.JogDistZIncDec;
                TextBoxJogDistanceZ.Text = newJogDist.ToString();
                Properties.Settings.Default.JogDistanceZ = newJogDist;
            }
            
            // Feed Rate
            // FeedRateIncrement & SpindleIncrement False = 1% Inc and Dec, True = 10% Inc and Dec
            else if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FRateInc"]) // Feed Rate Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Increase
                {
                    Feed10Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Increase
                {
                    Feed1Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            else if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["FRateDec"]) // Feed Rate Incease
            {               
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Decrease
                {
                    Feed10Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Decrease
                {
                    Feed1Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            // Spindle Increase and Decrease
            else if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["SpindleInc"]) // Spindle Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Increase
                {
                    Spindle10Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Increase
                {
                    Spindle1Inc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            else if (machine.Mode == Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["SprindleDec"]) // Spindle Incease
            {
                if (Properties.Settings.Default.FeedRateIncrement == true) // 10% Decrease
                {
                    Spindle10Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else if (Properties.Settings.Default.FeedRateIncrement == false) // 1% Decrease
                {
                    Spindle1Dec.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
    }
}
