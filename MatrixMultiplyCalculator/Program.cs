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

                var opt = int.Parse(Console.ReadLine());
                if (opt == 1) simpleTest();
                else if (opt == 0) break;

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

    }
}
