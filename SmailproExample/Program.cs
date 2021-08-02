using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmailproLib;
using System.Text.RegularExpressions;

namespace SmailproExample
{
    class Program
    {
        /// <summary>
        /// example to use SmailProLib
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // create new config
            SmailProConfig config = new SmailProConfig();
            // set username is random , you can set is config.username = "myusername";
            config.username = SmailProConfig.randomUserName;
            // set domain is random , you can set is config.domain = "domain";
            // domains can use is : { "cardgener.com", "ugener.com", "ychecker.com", "storegmail.com", "instasmail.com" }
            config.domain = SmailProConfig.randomDomain;
            // create new smailpro class
            Smailpro smailpro = new Smailpro(config);
            // check task is created or not
            Console.WriteLine("Creating email....");
            var isSuccessCreateTask = smailpro.CreateTask();
            // if task is success created
            if(isSuccessCreateTask.Result)
            {
                int TimeWait = 50; // max time wait mail ( senconds )
                int tickCount = Environment.TickCount; // get Environment TickCount
                Task<SmailProResponse> taskCheckEmail = null;
                Console.WriteLine("Waiting email....");
                while (Environment.TickCount - tickCount <= TimeWait * 1000) // wait email in 50 seconds
                {
                    // check mailbox task ( awaitable )
                    taskCheckEmail = smailpro.GetResult();
                    // wait task finsish
                    taskCheckEmail.Wait();
                    // check result code is sucess or not
                    if (taskCheckEmail.Result.Status == SmailProResponse.StatusCode.Success)
                    {
                        Console.WriteLine($"[] Email : {smailpro.Auth.email} ===> {taskCheckEmail.Result.RawText}");
                    }
                    else
                    {
                        Console.WriteLine("Unknow Error");
                    }
                    Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                }
                taskCheckEmail.Dispose();
                Console.WriteLine($"Done Task : {taskCheckEmail.Id}");
            }    
            // fail to create task
            else
            {
                Console.WriteLine("Failed Create Task !");
            }
        }

    }
}
