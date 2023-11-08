
public class Program
{

    static int sharedData = 0;
    static int writerCount = 0;
    static int readerCount = 0;
    static object lockObj = new object();

    static void Main()
    {
        int writerCount = 3; // Количество писателей
        int readerCount = 3; // Количество читателей

        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < writerCount; i++)
        {
            Thread writerThread = new Thread(Writer);
            writerThread.Name = $"Writer {i + 1}";
            threads.Add(writerThread);
        }

        for (int i = 0; i < readerCount; i++)
        {
            Thread readerThread = new Thread(Reader);
            readerThread.Name = $"Reader {i + 1}";
            threads.Add(readerThread);
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    static void Writer()
    {
        while (true)
        {
            int dataToWrite = new Random().Next(1, 10);
            Thread.Sleep(new Random().Next(100, 1000));

            lock (lockObj)
            {
                sharedData += dataToWrite;
                Console.WriteLine($"{Thread.CurrentThread.Name} wrote {dataToWrite}. Shared Data: {sharedData}");
            }

            Thread.Sleep(new Random().Next(100, 1000));
        }
    }

    static void Reader()
    {
        while (true)
        {
            int dataRead = 0;
            Thread.Sleep(new Random().Next(100, 1000));

            lock (lockObj)
            {
                dataRead = sharedData;
                Console.WriteLine($"{Thread.CurrentThread.Name} read {dataRead}. Shared Data: {sharedData}");
            }

            Thread.Sleep(new Random().Next(100, 1000));
        }
    }
}