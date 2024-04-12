using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplyCalculator
{
    internal class Matrix
    {
        int rows;
        int cols;
        double[,] values;
        public Matrix(int _n, int _m)
        {
            this.rows = _n;
            this.cols = _m;
            values = new double[rows, cols];
        }
        public Matrix(int _n, int _m, int _seed)
        {
            this.rows = _n;
            this.cols = _m;
            values = new double[rows,cols];
            Random random = new Random(_seed);
            for (int i = 0; i < rows; i++) 
            {
                for (int j = 0; j < cols; j++)
                {
                    values[i,j] = random.NextDouble();
                }
            }
        }

        public override string ToString()
        {
            string str = string.Empty;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    str += $"{values[i, j]:0.00} ";
                }
                str += "\n";
            }

            return str;
        }

        public bool check(Matrix other)
        {
            bool compatible = true;
            if (cols == other.cols && rows == other.rows) 
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (values[i, j] != other.values[i, j])
                            return false;
                    }
                }
            }
            else compatible = false;
            return compatible;
        }

        public Matrix? multiplyParallel(Matrix other, int numthreads = 4)
        {
            Matrix? result = null;

            if (other != null && cols == other.rows)
            {
                result = new Matrix(rows, other.cols);
                ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = numthreads };
                Parallel.For(0, numthreads, opt, id =>
                {
                    for (int v = id; v < rows * other.cols; v += numthreads)
                    {

                        int row = v / other.cols;
                        int col = v % other.cols;
                        for (int k = 0; k < cols; k++)
                            result.values[row, col] += values[row, k] * other.values[k, col];
                    }
                    ;
                });

            }
            return result;
        }

        public  static volatile Matrix? matrix;
        public static Matrix? A;
        public static Matrix? B;
        public Matrix? multiplyThread(Matrix other, int numthreads = 4) {

            matrix = null;

            if (other != null && cols == other.rows)
            {
                matrix = new Matrix(rows, other.cols);
                A = this;
                B = other;
                Thread[] threads = new Thread[numthreads];
                for (int i = 0; i < numthreads; i++)
                {
                    var id = i;
                    threads[i] = new Thread(() => couting(id, numthreads));
                    threads[i].Name = $"{i}";
                }
                foreach (Thread th in threads)
                    th.Start();
                foreach (Thread th in threads)
                    th.Join();

            }
            return matrix;
        }
        static void couting(int id, int numthreads)
        {
            for (int v = id; v < A.rows * B.cols; v += numthreads)
            {

                int row = v / B.cols;
                int col = v % B.cols;
                for (int k = 0; k < A.cols; k++)
                    matrix.values[row, col] += A.values[row, k] * B.values[k, col];
            }   
        }
    }
}
