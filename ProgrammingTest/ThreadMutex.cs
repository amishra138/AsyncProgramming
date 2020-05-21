using System.Threading;

namespace ProgrammingTest
{
    public class ThreadMutex
    {
        private static Mutex mutex = new Mutex();

        public void Example()
        {
            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(MutexDemo)
                {
                    Name = string.Format("Thread {0} : ", i + 1)
                };
                t.Start();
            }
        }

        //Method to implement synchronization using mutex
        private void MutexDemo(object obj)
        {
            try
            {
                //Block the current thread until the current waitHandle recieves a signal.
                //Wait until it is safe to enter.
                mutex.WaitOne();

                System.Console.WriteLine("{0} has entered in the Domain", Thread.CurrentThread.Name);
                Thread.Sleep(1000);
                System.Console.WriteLine("{0} is leaving the Domain", Thread.CurrentThread.Name);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
