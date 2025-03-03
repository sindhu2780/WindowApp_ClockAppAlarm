using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WindowClockApp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;


namespace TrumpfMetamation_Task_AlarmApp
{
    class Program : AddClockAppAlarm
    {
        
        public static void Main(string[] args)
        {
            //set alarm with correct value
            try
            {
                // Launch Clock app
                var process = LaunchClockApp();
                if (process == null)
                {
                    Console.WriteLine("Failed to launch the Clock app.");
                    return;
                }
                Thread.Sleep(3000);

                using (var automation = new UIA3Automation())
                {

                    try
                    {
                        var windows = automation.GetDesktop().FindAllChildren(c => c.ByControlType(ControlType.Window));
                        var mainWindow = windows.FirstOrDefault(w => w.Name.Contains("Clock"));

                        if (mainWindow == null)
                        {
                            Console.WriteLine("Failed to find the main Clock app window.");
                            return;
                        }
                        Console.WriteLine("Main window of the Clock app found.");

                        var alarmTab = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AlarmButton")).AsButton();
                        alarmTab.Click();
                        Console.WriteLine("Navigated to Alarm tab.");

                        MaximizeClockAppWindow(mainWindow);
                        // Perform actions: Click on Alarm tab, Add alarm, Set time, etc.
                        Thread.Sleep(5000);
                        //NavigateToAlarmTab(mainWindow);
                        Thread.Sleep(5000);
                        AddAlarm(mainWindow, "09", "30", "Trumpf Metamation - Login Time");
                        Thread.Sleep(5000);
                        VerifyAlarm(mainWindow, "Trumpf Metamation - Login Time");
                        DeleteAlarm(mainWindow, "Trumpf Metamation - Login Time");
                        VerifyAlarmRemoval(mainWindow, "Trumpf Metamation - Login Time");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    //set alram with wrong value
                    try
                    {
                        var windows = automation.GetDesktop().FindAllChildren(c => c.ByControlType(ControlType.Window));
                        var mainWindow = windows.FirstOrDefault(w => w.Name.Contains("Clock"));

                        if (mainWindow == null)
                        {
                            Console.WriteLine("Failed to find the main Clock app window.");
                            return;
                        }
                        Console.WriteLine("Main window of the Clock app found.");

                        var alarmTab = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AlarmButton")).AsButton();
                        alarmTab.Click();
                        Console.WriteLine("Navigated to Alarm tab.");

                        MaximizeClockAppWindow(mainWindow);
                        // Perform actions: Click on Alarm tab, Add alarm, Set time, etc.
                        Thread.Sleep(5000);
                        //NavigateToAlarmTab(mainWindow);
                        Thread.Sleep(5000);
                        AddAlarm(mainWindow, "09", "99", "Trumpf Metamation - Login Time");
                        Thread.Sleep(5000);
                        VerifyAlarm(mainWindow, "Trumpf Metamation - Login Time");
                        DeleteAlarm(mainWindow, "Trumpf Metamation - Login Time");
                        VerifyAlarmRemoval(mainWindow, "Trumpf Metamation - Login Time");
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            catch(Exception ex)
                    {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
       
       
    }
}


