using System;
using System.Threading;

/*
                                            Тестовое задание №1
Требуется написать консольную программу, которая выводит на экран числа от 1 до 10 в порядке возрастания.
При этом числа выводятся из двух параллельно запущенных потоков, каждый из которых выводит свой массив чисел.
Первый поток – числа «1, 3, 5, 7, 9», второй – «2, 4, 6, 8, 10».
Числа должны выводиться корректно не зависимо от порядка запуска потоков.
*/

namespace ISBC_Test_ConsoleThreads
{
    /// <summary>
    ///  Я использовал Interlocked Класс и атомарные операции
    /// </summary>
    class Program
    {
        // Вывожу 10 чисел 2 потоками, поэтому 5 итераций
        private const int NUM_THREAD_ITERATIONS = 5;
        private const int NUM_THREADS = 2;

        //0 for false, 1 for true.
        private static int s_usingResource = 0;                

        private static int s_valueToWrite = 1;

        static void Main()
        {
            Thread thread;

            // создаю и запускаю NUM_THREADS потоков
            for (int i = 0; i < NUM_THREADS; i++)
            {
                thread = new Thread(RunThreadsWork);
                thread.Name = String.Format($"Thread_{i + 1}");

                // Ожидание перед запуском следующего потока (если не ждать, то будет работать некорректно)
                Thread.Sleep(100);

                thread.Start();
            }
        }

        private static void RunThreadsWork()
        {
            for (int i = 0; i < NUM_THREAD_ITERATIONS; i++)
            {
                WriteNumbers();

                // Ожидание перед запуском следующей итерацией (если не ждать, то будет работать некорректно)
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Метод запрещает повторный вход
        /// </summary>
        /// <returns></returns>
        private static bool WriteNumbers()
        {
            // 0 означает, что метод не используется
            
            // Указываю, что метод используется
            if (0 == Interlocked.Exchange(ref s_usingResource, 1))
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.Name} вывел: {s_valueToWrite} / 10");
                s_valueToWrite++;

                // Освобождаю метод
                Interlocked.Exchange(ref s_usingResource, 0);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
