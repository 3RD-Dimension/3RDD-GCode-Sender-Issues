using GCodeSender.Communication;
using GCodeSender.Hotkey;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GCodeSender
{
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
            // Jog rate Increase and Decrease (This is saved to settings always)
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateInc"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeed.Text);
                TextBoxJogFeed.Text = (currentJogRate + 10).ToString();
                Properties.Settings.Default.JogFeed = Convert.ToDouble(TextBoxJogFeed.Text);
            }

            // Jog rate Increase and Decrease (This is saved to settings always)
            else if (machine.Connected && machine.Mode != Machine.OperatingMode.SendFile && currentHotPressed == HotKeys.hotkeyCode["JRateDec"])
            {
                int currentJogRate = Convert.ToInt32(TextBoxJogFeed.Text);
                TextBoxJogFeed.Text = (currentJogRate - 10).ToString();
                Properties.Settings.Default.JogFeed = Convert.ToDouble(TextBoxJogFeed.Text);
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
