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

            // Disable Viewport Rotation, Move and Pan in case hotkey is an arrow key
            viewport.IsRotationEnabled = false;
            viewport.IsMoveEnabled = false;
            viewport.IsPanEnabled = false;

            string currentHotPressed = HotKeys.KeyProcess(sender, e); // Get Keycode
            if (currentHotPressed == null) return; // If currentHotPressed is null, Return (to avoid continuing with  blank)

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
            // Spindle and Flood  (firmware takes care if it will enable or disable so just need to know if it is connected to controller
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
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedX.Text);
                TextBoxJogFeedX.Text = (currentJogRate + Properties.Settings.Default.JogFeedXIncDec).ToString();
                Properties.Settings.Default.JogFeedX = Convert.ToDouble(TextBoxJogFeedX.Text);
            }           
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecX"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedX.Text);
                TextBoxJogFeedX.Text = (currentJogRate - Properties.Settings.Default.JogFeedXIncDec).ToString();
                Properties.Settings.Default.JogFeedX = Convert.ToDouble(TextBoxJogFeedX.Text);
            }
            // JOG RATE AXIS Y
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateIncY"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedY.Text);
                TextBoxJogFeedY.Text = (currentJogRate + Properties.Settings.Default.JogFeedYIncDec).ToString();
                Properties.Settings.Default.JogFeedY = Convert.ToDouble(TextBoxJogFeedY.Text);
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecY"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedY.Text);
                TextBoxJogFeedY.Text = (currentJogRate - Properties.Settings.Default.JogFeedYIncDec).ToString();
                Properties.Settings.Default.JogFeedY = Convert.ToDouble(TextBoxJogFeedY.Text);
            }
            // JOG RATE AXIS Z
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateIncZ"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedZ.Text);
                TextBoxJogFeedZ.Text = (currentJogRate + Properties.Settings.Default.JogFeedZIncDec).ToString();
                Properties.Settings.Default.JogFeedZ = Convert.ToDouble(TextBoxJogFeedZ.Text);
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDecZ"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeedZ.Text);
                TextBoxJogFeedZ.Text = (currentJogRate - Properties.Settings.Default.JogFeedZIncDec).ToString();
                Properties.Settings.Default.JogFeedZ = Convert.ToDouble(TextBoxJogFeedZ.Text);
            }

            // JOG DISTANCE X
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncX"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceX.Text);
                TextBoxJogDistanceX.Text = (currentJogDist + Properties.Settings.Default.JogDistXIncDec).ToString();
                Properties.Settings.Default.JogDistanceX = Convert.ToDouble(TextBoxJogDistanceX.Text);
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecX"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceX.Text);
                TextBoxJogDistanceX.Text = (currentJogDist - Properties.Settings.Default.JogDistXIncDec).ToString();
                Properties.Settings.Default.JogDistanceX = Convert.ToDouble(TextBoxJogDistanceX.Text);
            }

            // JOG DISTANCE Y
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncY"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceY.Text);
                TextBoxJogDistanceY.Text = (currentJogDist + Properties.Settings.Default.JogDistYIncDec).ToString();
                Properties.Settings.Default.JogDistanceY = Convert.ToDouble(TextBoxJogDistanceY.Text);
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecY"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceY.Text);
                TextBoxJogDistanceY.Text = (currentJogDist - Properties.Settings.Default.JogDistYIncDec).ToString();
                Properties.Settings.Default.JogDistanceY = Convert.ToDouble(TextBoxJogDistanceY.Text);
            }

            // JOG DISTANCE Z
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistIncZ"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceZ.Text);
                TextBoxJogDistanceZ.Text = (currentJogDist + Properties.Settings.Default.JogDistZIncDec).ToString();
                Properties.Settings.Default.JogDistanceZ = Convert.ToDouble(TextBoxJogDistanceZ.Text);
            }
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JDistDecZ"])
            {
                int currentJogDist = Convert.ToInt32(TextBoxJogDistanceZ.Text);
                TextBoxJogDistanceZ.Text = (currentJogDist - Properties.Settings.Default.JogDistZIncDec).ToString();
                Properties.Settings.Default.JogDistanceZ = Convert.ToDouble(TextBoxJogDistanceZ.Text);
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
            // Enable Viewport Rotation, Move and Pan after jogging
            viewport.IsRotationEnabled = true;
            viewport.IsMoveEnabled = true;
            viewport.IsPanEnabled = true;
        }
    }
}
