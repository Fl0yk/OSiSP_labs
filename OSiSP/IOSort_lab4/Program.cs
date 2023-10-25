
using System.Text;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    public static Mutex WriteMutetx = new();

    static async Task Main(string[] args)
    {
        if (File.Exists("output.txt"))
            File.Delete("output.txt");

        string inputPath = "input.txt";
        await GenerateAndWriteArraysToFileAsync(inputPath, 5, 15);
        await ReadAndSort(inputPath);
    }

    static async Task ReadAndSort(string filePath)
    {
        List<Thread> sortThread = new();
        await Console.Out.WriteLineAsync(" \n\nИтоговые массивы");
        using (var reader = new StreamReader(filePath))
        {
            //int i = 0;
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                List<int> values = new(line.Split(' ').Select(c => int.Parse(c)));
                sortThread.Add(new Thread(() => SortAndWrite(values)));
                sortThread.Last().Start();
            }
        }
    }

    static void SortAndWrite(List<int> values)
    {
        string outputPath = "output.txt";
        values.Sort();
        WriteMutetx.WaitOne();

        using (var writer = new StreamWriter(outputPath, true))
        {
            writer.WriteLine(String.Join(' ', values));
        }
        Console.WriteLine(String.Join(' ', values));
        WriteMutetx.ReleaseMutex();
    }

    static async Task GenerateAndWriteArraysToFileAsync(string filePath, int numArrays, int arraySize)
    {
        await Console.Out.WriteLineAsync("Изначальные массивы");
        using (var writer = new StreamWriter(filePath))
        {
            var random = new Random();

            for (int i = 0; i < numArrays; i++)
            {
                int[] array = new int[arraySize];
                StringBuilder arrayLine = new StringBuilder();

                for (int j = 0; j < arraySize; j++)
                {
                    int randomNumber = random.Next(1, 100);
                    array[j] = randomNumber;
                }

                arrayLine.Append(string.Join(" ", array));
                await Console.Out.WriteLineAsync(arrayLine);
                await writer.WriteLineAsync(arrayLine.ToString());
            }
        }
    }
}