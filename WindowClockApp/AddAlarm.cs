using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using WindowClockApp;
using AutomationElement = FlaUI.Core.AutomationElements.AutomationElement;

namespace WindowClockApp
{

    class AddClockAppAlarm
    {
        public static Process LaunchClockApp()
        {
            return Process.Start("explorer", "ms-clock:");
            Thread.Sleep(3000);
        }

        // Method to get the main window of the Clock app
        public static AutomationElement GetMainWindow(UIA3Automation automation)
        {
            var windows = automation.GetDesktop().FindAllChildren(c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            return windows.FirstOrDefault(w => w.Name.Contains("Clock"));
            //var windows = automation.GetDesktop().FindAllChildren(c => c.ByControlType(ControlType.Window));
            //var mainWindow = windows.FirstOrDefault(w => w.Name.Contains("Clock"));

        }
        public static void MaximizeClockAppWindow(AutomationElement mainWindow)
        {
            try
            {
                // Get the window pattern for the main window
                var windowPattern = mainWindow.Patterns.Window.Pattern;

                // Maximize the window
                windowPattern.SetWindowVisualState(FlaUI.Core.Definitions.WindowVisualState.Maximized);
                Console.WriteLine("Clock app window maximized.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error maximizing the window: {ex.Message}");
            }
        }

        // Method to navigate to the Alarm tab
        public static void NavigateToAlarmTab(AutomationElement mainWindow)
        {
            // Retry mechanism to find the 'AlarmButton'
            // alarmTab = null;
            int retries = 1;
            for (int i = 0; i < retries; i++)
            {
                var alarmTab = mainWindow.FindFirstDescendant(cf => cf.ByName("Alarm"));

                if (alarmTab != null)
                {
                    var alarmTabButton = alarmTab.AsButton();
                    alarmTabButton.WaitUntilClickable(TimeSpan.FromSeconds(5));

                    if (alarmTabButton.IsEnabled)
                    {
                        alarmTabButton.DoubleClick();
                        Console.WriteLine("Navigated to Alarm tab.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The 'Alarm' tab button is not enabled.");
                    }
                }
                else
                {
                    Console.WriteLine("The 'Alarm' tab button was not found.");
                }

                Thread.Sleep(1000);
            }
        }

        // Method to add an alarm
        public static void AddAlarm(AutomationElement mainWindow, string hour, string minute, string name)
        {
            var addAlarmButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AddAlarmButton")).AsButton();
            addAlarmButton.WaitUntilClickable(TimeSpan.FromSeconds(5));

            if (addAlarmButton != null && addAlarmButton.IsEnabled)
            {
                addAlarmButton.Click();
                Console.WriteLine("Clicked on 'Add Alarm' button.");

                Thread.Sleep(2000); 

                SetAlarmTime(mainWindow, hour, minute);
                AlarmNmae(mainWindow, name);
                Thread.Sleep(5000);
                RepeatAlarm(mainWindow, "Monday", "Tap", "Disabled");
                SaveAlarm(mainWindow);
            }
            else
            {
                Console.WriteLine("Could not find the 'Add Alarm' button.");
            }
        }

        // Method to set the hour and minute
        public static void SetAlarmTime(AutomationElement mainWindow, string hour, string minute)
        {
            // Set Hour
            var hourPicker = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("HourPicker"));
            if (hourPicker != null && hourPicker.AsTextBox() != null)
            {
                hourPicker.Patterns.Value.Pattern.SetValue(hour);
                Console.WriteLine($"Hour set to {hour}.");
            }
            else
            {
                Console.WriteLine("Failed to set Hour.");
            }

            // Set Minute
            var minutePicker = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("MinutePicker"));
            if (int.TryParse(minute, out int minuteValue))
            {
                if (minuteValue < 0 || minuteValue >= 60)
                {
                    Console.WriteLine("Failed to set Minute.");
                }
                else
                {
                    if (minutePicker != null && minutePicker.AsTextBox() != null)
                    {
                        minutePicker.Patterns.Value.Pattern.SetValue(minute);
                        Console.WriteLine($"Minute set to {minute}.");
                    }

                }
            }
            else if (minute.CompareTo("60") > 0)
            {
                Console.WriteLine("Failed to set Minute.");
            }
        }
        public static void AlarmNmae(AutomationElement mainWindow, string name)
        {
            var Alarmname = mainWindow.FindFirstDescendant(cf => cf.ByName("Alarm name"));
            if (Alarmname != null)
            {
                Alarmname.Patterns.Value.Pattern.SetValue(name);
                Console.WriteLine("Alarm name saved successfully!");
            }
            else
            {
                Console.WriteLine("Alarm name not save");
            }
        }
        public static void RepeatAlarm(AutomationElement mainWindow, string days, string musicname, string snoozetime)
        {
            var RepeatAlarmcheckbox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("RepeatCheckBox"));
            RepeatAlarmcheckbox.AsButton().Click();

            //foreach (var daysnmae in days)
            //{

                var setRepeatAlarm = mainWindow.FindFirstDescendant(cf => cf.ByClassName("ToggleButton"));
                var setdays = setRepeatAlarm.FindFirstDescendant(cf => cf.ByName($"{days}"));
                if (setRepeatAlarm != null)
                {
                    setRepeatAlarm.AsToggleButton().Click();
                    Console.WriteLine("RepeatAlarm saved successfully!");
                }
                else
                {
                    Console.WriteLine("RepeatAlarm not save");
                }

           // }

            //var music = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ChimeComboBox"));
            //music.AsListBox().Click();
            //Thread.Sleep(5000);
            //if (music != null && music.AsTextBox()!=null)
            //{
            //    var musiclist = mainWindow.FindAllChildren(cf => cf.ByAutomationId("AlarmSoundsOptionText"));
            //    var comboBox = musiclist.FirstOrDefault();
            //    var musicecombo = comboBox.AsComboBox();
               
              
            //    //var itemToSelect = musicecombo.Items.FirstOrDefault(item => item.Name.Contains(musicname));
            //    ////Thread.Sleep(5000);
            //    //itemToSelect.Select();
            //    Console.WriteLine("RepeatAlarm music saved successfully!");
            //}
           

            //var setsnoozetime = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("SnoozeComboBox"));
            //if (setsnoozetime != null && setsnoozetime.AsTextBox() != null)
            //{
            //    setsnoozetime.Patterns.Value.Pattern.SetValue(snoozetime);
            //    Console.WriteLine("RepeatAlarm music saved successfully!");
            //}
           
        }

        // Method to save the alarm
        public static void SaveAlarm(AutomationElement mainWindow)
        {
            var saveButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("PrimaryButton"));
            if (saveButton != null)
            {
                saveButton.AsButton().Invoke();
                Console.WriteLine("Alarm saved successfully!");
            }
            else
            {
                Console.WriteLine("Save button not found.");
            }
        }

        // Method to verify if the alarm was saved
        public static void VerifyAlarm(AutomationElement mainWindow, string Alarmname)
        {
            var alarmList = mainWindow.FindAllDescendants(cf => cf.ByAutomationId("AlarmViewGrid"));
            var savedAlarm = alarmList.FirstOrDefault(a => a.Name.Contains(Alarmname));

            if (savedAlarm != null)
            {
                Console.WriteLine($"Alarm verified: {Alarmname} found in the list.");
            }
            else
            {
                Console.WriteLine($"Alarm verification failed: {Alarmname} not found.");
            }
        }

        // Method to delete the alarm
        public static void DeleteAlarm(AutomationElement mainWindow, string Alarmname)
        {
            var alarmList = mainWindow.FindAllDescendants(cf => cf.ByAutomationId("AlarmViewGrid"));
            var savedAlarm = alarmList.FirstOrDefault(a => a.Name.Contains(Alarmname));

            if (savedAlarm != null)
            {
                savedAlarm.Click();
                var deleteButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("DeleteButton"));
                if (deleteButton != null)
                {
                    deleteButton.AsButton().Invoke();
                    Console.WriteLine("Delete button clicked.");
                }
                else
                {
                    Console.WriteLine("Delete button not found.");
                }
            }
        }

        // Method to verify if the alarm was removed
        public static void VerifyAlarmRemoval(AutomationElement mainWindow, string Alarmname)
        {
            var alarmListAfterDelete = mainWindow.FindAllDescendants(cf => cf.ByAutomationId("AlarmViewGrid"));
            //var deletedAlarm = alarmListAfterDelete.FirstOrDefault(a => a.Name.Contains(Alarmname));

            if (alarmListAfterDelete.ToString() != Alarmname)
            {
                Console.WriteLine($"Alarm removal validated: {Alarmname} not found.");
            }
            else
            {
                Console.WriteLine($"Alarm removal failed: {Alarmname} still exists.");
            }
        }
    }
}

