using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Performance_Log
{
    internal class Program
    {
        private static string machineName = System.Environment.MachineName;

        public static PerformanceCounter cpuCounter = null;
        public static PerformanceCounter ramCounter = null;
        private static System.Timers.Timer tTimer = null;

        static void Main(string[] args)
        {
            tTimer = new System.Timers.Timer(2000);
            tTimer.Enabled = true;
            tTimer.Elapsed += onTick;

            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", machineName);
                ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use", String.Empty, machineName);
            }
            catch(Exception ex) { }
            try
            {
                if(cpuCounter != null)
                {
                    cpuCounter.Dispose();
                }
                if(ramCounter != null)
                {
                    ramCounter.Dispose();
                }
            } finally
            {
                PerformanceCounter.CloseSharedResources();
            }
            tTimer.Start();
            while(true) { }
        }

        public static void onTick(Object source, ElapsedEventArgs e)
        {
            try
            {
                int cpuUsage = (int)cpuCounter.NextValue();
                int ramUsage = (int)ramCounter.NextValue();
                Console.WriteLine("----------------------------");
                Console.WriteLine("CPU: " + cpuUsage);
                Console.WriteLine("RAM: " + ramUsage);
                Console.WriteLine("----------------------------");
            }
            catch (Exception ex)
            { }
        }
    }
}
