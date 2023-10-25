using System.Numerics;
const int size = 5;
Thread[] threads = new Thread[size];

for (int i = 0; i < size; i++)
{
    threads[i] = new Thread(() => ThreadFunc(i));
    if(i == 4)
        threads[i].Priority = ThreadPriority.Highest;
    else if(i  == 0)
        threads[i].Priority = ThreadPriority.Lowest;

    threads[i].Start();
    Thread.Sleep(100);
}

Console.WriteLine("Ожидаем завершения");
foreach (Thread t in threads)
{
    t.Join();
}

static void ThreadFunc(int ind)
{
    Console.WriteLine($"Старт {ind} потока {Environment.CurrentManagedThreadId}");
    Random rand = new();
    BigInteger big1 = int.MaxValue;
    BigInteger big2 = int.MaxValue;

    for(int i = 0; i < 60000; i++)
    {
        big1 *= int.MaxValue;
        big2 *= int.MaxValue;
    }
    //Console.WriteLine(big1);
    for (int i = 0; i < 10000; i++)
    {
        //Console.WriteLine("a");
        big1 = big1 * int.MaxValue * int.MaxValue * int.MaxValue + big2 * int.MaxValue * int.MaxValue * int.MaxValue;
        big2 =  big1 * int.MaxValue * int.MaxValue * int.MaxValue + big2 * int.MaxValue * int.MaxValue * int.MaxValue;
        if(i % 100 == 0)
            Console.WriteLine($"Прогресс потока {Environment.CurrentManagedThreadId}: {(int)((double) i / 10000 * 100)}%");
    }

    Console.WriteLine($"Конец {ind} потока {Environment.CurrentManagedThreadId}");
}