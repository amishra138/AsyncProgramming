using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingTest
{
    class Program
    {
        public static void Main()
        {
            //Thread mutex
            ThreadMutex mutex = new ThreadMutex();
            mutex.Example();

            //Thread Semaphore
            ThreadSemaphore threadSemaphore = new ThreadSemaphore();
            threadSemaphore.Example();

            Console.ReadLine();
        }



        public static void PrinPyramid(int num)
        {
            int i, j, gap;

            for (i = num; i >= 1; i--)
            {

                for (gap = num - 1; gap >= i; gap--)
                {
                    Console.WriteLine(" ");
                }

            }

            for (j = 1; j <= i; j++)
            {
                Console.WriteLine("*");
            }

            for (j = i - 1; j >= 0; j--)
            {
                Console.WriteLine("*");
            }

            Console.WriteLine(" ");
        }

        static async void CallToCheck()
        {

            //int[] values = new int[] { 4, 3, 3, 4, 1, 2, 2, 3, 6, 5, 4, 5 };
            //bool result = IsDominoPyramidValid(values);
            //Console.WriteLine(result ? "YES" : "NO");

            //Console.WriteLine("Thread start");
            ////CallToCheck();
            //ThreadWaitAll();
            //Console.WriteLine("Thread end");
            await ThreadWhenAll();
        }

        static async Task<int> ExampleTaskAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val));
            Console.WriteLine(val);
            return val;
        }

        static async Task AwaitAndProcessAsync(Task<int> task)
        {
            var result = await task;
            Console.WriteLine(result);
        }

        static async Task ThreadWhenAll()
        {
            Task<int> task1 = ExampleTaskAsync(2);
            Task<int> task2 = ExampleTaskAsync(3);
            Task<int> task3 = ExampleTaskAsync(1);

            var tasks = new[] { task1, task2, task3 };

            var processingTasks = tasks.Select(AwaitAndProcessAsync).ToList();

            await Task.WhenAll(processingTasks);
        }

        static void ThreadWaitAll()
        {
            Task<int> task1 = ExampleTaskAsync(2);
            Task<int> task2 = ExampleTaskAsync(3);
            Task<int> task3 = ExampleTaskAsync(1);

            var tasks = new[] { task1, task2, task3 };

            Task.WaitAll(tasks);
        }

        private static int DominoLength = 2;

        public static bool IsDominoPyramidValid(int[] values)
        {
            int arrayLength = values.Length;

            int offset = 0;
            int currentRow = 1; // Note: I'm using a 1-based value here as it helps the maths
            bool result = true;
            while (result)
            {
                int currentRowLength = currentRow * DominoLength;

                // Avoid checking final row: there is no row below it
                if (offset + currentRowLength >= arrayLength)
                {
                    break;
                }

                result = CheckValuesOnRowAgainstRowBelow(values, offset, currentRowLength);
                offset += currentRowLength;
                currentRowLength = currentRowLength + 2;
                currentRow++;
            }

            return result;
        }

        private static bool CheckValuesOnRowAgainstRowBelow(int[] values, int startOfCurrentRow, int currentRowLength)
        {
            int startOfNextRow = startOfCurrentRow + currentRowLength;
            int comparablePointOnNextRow = startOfNextRow + 1;
            for (int i = 0; i < currentRowLength; i++)
            {
                if (values[startOfCurrentRow + i] != values[comparablePointOnNextRow + i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
