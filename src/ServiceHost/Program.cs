using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace ServiceHost
{

    //https://topshelf.readthedocs.io/en/latest/configuration/quickstart.html
    //https://github.com/Topshelf/Topshelf/blob/develop/src/Topshelf/HostStartedContext.cs


    //https://github.com/Topshelf/Topshelf/issues/306
    //https://github.com/Topshelf/Topshelf/pull/311/files#diff-8

    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += (sender, eventArgs) => {
                Console.WriteLine("It is {0} and all is well", DateTime.Now);
                throw new Exception("MyException");
                }
            ;
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }

    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<TownCrier>(s =>                        //2
                {
                    s.ConstructUsing(name => new TownCrier());     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });

            x.OnException((exception) =>
                   {
                Console.WriteLine("Exception thrown - " + exception.Message);
                                   });
            x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("Stuff");                       //9
            });                                                  //10
        }
    }
}
