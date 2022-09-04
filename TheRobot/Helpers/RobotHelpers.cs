using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TheRobot.Helpers;

public static class RobotHelpers
{
    public static Task DoOrTimeout(Action method, TimeSpan timeout)
    {
        bool stopTrying = false;
        DateTime time = DateTime.Now;
        while (!stopTrying)
        {
            try
            {
                method.Invoke();
                stopTrying = true;
            }
            catch (Exception ex)
            {
                if (DateTime.Now - time > timeout)
                {
                    stopTrying = true;
                    throw;
                }
            }
        }
        return Task.CompletedTask;

    } 
}
