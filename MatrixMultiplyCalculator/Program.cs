using System.Diagnostics;
namespace MatrixMultiplyCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("MENU:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Simple test");
                Console.WriteLine("2. Perforance test for Parallel");
                Console.WriteLine("3. Perforance test for Thread");

                var opt = int.Parse(Console.ReadLine());
                if (opt == 1) simpleTest();
                else if (opt == 0) break;
                else if (opt == 2) PerformanceTest("Parallel");
                else if (opt == 3) PerformanceTest("Thread");

                Console.ReadKey();
            }
            
            
        }
        static void simpleTest()
        {
            Matrix A = new Matrix(4, 3, 1);
            Matrix B = new Matrix(3, 5, 2);
            Matrix C = A.multiplyParallel(B);
            Matrix D = A.multiplyThread(B);
            Console.WriteLine("A:");
            Console.WriteLine(A);
            Console.WriteLine("B:");
            Console.WriteLine(B);
            Console.WriteLine("multiply by Parallel:");
            Console.WriteLine(C);
            Console.WriteLine("multiply by Thread:");
            Console.WriteLine(D);
            Console.WriteLine(C.check(D));
        }

        static void PerformanceTest(string str)
        {
            //int maxTH = Math.Max(32, Environment.ProcessorCount);
            List<int> threads = Enumerable.Range(1, Environment.ProcessorCount).ToList();
            //List<int> sizes = Enumerable.Range(1, 4).Select(x => x *250).ToList();
            List<int> seeds = Enumerable.Range(1, 4).ToList();
            Console.WriteLine("Enter the size of matrix");
            int size = int.Parse(Console.ReadLine());

            List<int> sizes = new List<int>(){ size };
            Int64 reference = 0;
            
            foreach (int s in sizes)
            {
                Console.WriteLine($"size: {s}");
                Int64[] times = new Int64[Environment.ProcessorCount];
                foreach (int th in threads)
                {
                    Console.Write($"th: {th} ");
                    foreach (int seed in seeds)
                    {
                        Console.Write($"seed: {seed} ");
                        Matrix A = new Matrix(s, s, seed);
                        Matrix B = new Matrix(s, s, seed + 100);

                        var watch = Stopwatch.StartNew();
                        if (str == "Parallel")
                            A.multiplyParallel(B, th);
                        else if (str == "Thread")
                            A.multiplyThread(B, th);
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.Write($"time : {elapsedMs} ");
                        times[th - 1] += elapsedMs;
                    }
                }
                times.Select(x => x / seeds.Count).ToArray();

                Console.WriteLine("\n Threads");
                Console.WriteLine(String.Join(" ",threads));
                Console.WriteLine("times");
                Console.WriteLine(String.Join(" ", times));
                var speedup = times.Select(x => Math.Round((double)times[0] / (double)x,2)).ToArray();
                Console.WriteLine("speedup");
                Console.WriteLine(String.Join(" ", speedup));
            }
            
        }
    }
}
