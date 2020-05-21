using System;
using System.Threading;

namespace ProgrammingTest
{
    public class ThreadSemaphore
    {
        //Object of Semaphore class with two parameter.  
        //first parameter explain number of process to initial start  
        //second parameter explain maximum number of process can start  
        //second parameter must be equal or greate then first parameter
        private static Semaphore semaphore = new Semaphore(3, 5);

        public void Example()
        {
            for (int i = 0; i <= 5; i++)
            {
                Thread t = new Thread(SempStart);
                t.Start(i);
            }
        }

        static void SempStart(object id)
        {

            Console.WriteLine(id + " Wants to Get Enter for processing");
            try
            {
                //Blocks the current thread until the current WaitHandle receives a signal.   
                semaphore.WaitOne();
                Console.WriteLine(" Success: " + id + " is in!");
                Thread.Sleep(5000);
                Console.WriteLine(id + "<<-- is exit because it completed there operation");
            }
            finally
            {
                //Release() method to release semaphore  
                semaphore.Release();
            }
        }
    }
}
