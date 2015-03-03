using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ThreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //测试CLR线程池占满后,会不会影响IO线程池的正常使用？
            ThreadPool.SetMaxThreads(5, 5);
            ThreadPool.SetMinThreads(5, 5);

            int workerThreads, completionPortThreads;

            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("线程池中辅助线程的最大数目：{0}.线程池中异步 I/O 线程的最大数目：{1}", workerThreads, completionPortThreads);

            ManualResetEvent waitHandle = new ManualResetEvent(false);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //IO线程池
            //WebRequest request = HttpWebRequest.Create("http://www.taobao.com/");
            //request.BeginGetResponse(ar =>
            //{
            //    var response = request.EndGetResponse(ar);
            //    Console.WriteLine(watch.Elapsed + ": Response Get");
            //}, null);

            //CLR线程池
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(index =>
                    {
                        Console.WriteLine(string.Format("{0}: Task {1} started", watch.Elapsed, index));
                        waitHandle.WaitOne();
                    }, i);
            }

            //IO线程池
            WebRequest request = HttpWebRequest.Create("http://www.hao123.com/");
            request.BeginGetResponse(ar =>
            {
                var response = request.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get hao123");
            }, null);
            Console.Read();

            //证明CLR线程池占满后会影响到IO线程池的使用。

        }
    }
}
