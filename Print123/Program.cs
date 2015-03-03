using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Print123
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadPool.SetMaxThreads(3, 3);
            var mt = new MyThread();
            mt.Run();
            Console.Read();
        }

        
    }

    class MyThread
    {

        private int number = 0;

        public void Run()
        {
            var t1 = new Thread(PrientNumber);
            var t2 = new Thread(PrientNumber);
            var t3 = new Thread(PrientNumber);
            t1.Start("1");
            //Thread.Sleep(1);
            t2.Start("2");
            //Thread.Sleep(2);
            t3.Start("3");
            //Thread.Sleep(3);
        }

        void PrientNumber(object num)
        {
            for (int i = 0; i < 9; i++)
            {
                lock ("a")
                {
                    if (Convert.ToInt32(num) == number + 1 || Convert.ToInt32(num) == number - 2)
                    {
                        Console.Write(num);
                        number = Convert.ToInt32(num);

                        Monitor.PulseAll("a");
                    }
                    else
                    {
                        i--;
                        Monitor.Wait("a");
                    }
                }
                
            }
        }
    }
}
