using System;

namespace Struct_Matrix
{
    public struct Matrix
    {
        private int n;  //rows
        private int m;  //columns
        private int[,] matrix;

        public Matrix(int rows, int columns)
        {
            n = rows;
            m = columns;
            matrix = new int[n, m];
        }

        public void randomInitialization()
        {
            Random rand = new Random();
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    matrix[i, j] = rand.Next(-8, 8);
                }
            }
        }

        public Matrix addition(Matrix obj)
        {
            if(n != obj.n || m != obj.m)
            {
                Console.WriteLine("Addition Error!!!");
                Environment.Exit(-1);
            }
            Matrix newMatrix = new Matrix(n, m);

            for(int i = 0; i < n; ++i)
            {
                for(int j = 0; j < m; ++j)
                {
                    newMatrix.matrix[i, j] = matrix[i, j] + obj.matrix[i, j];
                }
            }

            return newMatrix;
        }

        public Matrix multiplication(Matrix obj)
        {
            if (m != obj.n)
            {
                Console.WriteLine("Multiplication Error!!!");
                Environment.Exit(-1);
            }
            Matrix mulMatrix = new Matrix(n, obj.m);
            int sum = 0;
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < obj.m; ++j)
                {
                    sum = 0;
                    for(int k = 0; k < m; ++k)
                    {
                        sum += matrix[i, k] * obj.matrix[k, j];
                    }
                    mulMatrix.matrix[i, j] = sum;
                }
            }

            return mulMatrix;
        }
        public void scalarMultiplication(int k)
        {
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    matrix[i, j] *= k;
                }
            }
        }

        private void getCofactor(int[,] matrix, int[,] temp, int row, int column, int n)
        {
            int tox, syun;
            tox = syun = 0;
            for(int i = 0; i < n; ++i)
            {
                if (i == row) continue;
                for (int j = 0; j < n; ++j)
                {
                    if (j != column)
                    {
                        temp[tox, syun] = matrix[i, j];
                        if (++syun == n - 1)
                        {
                            tox++;
                            syun = 0;
                        }
                    }
                }
            }
        }

        public int determinate()
        {
            if (n != m)
            {
                Console.WriteLine("The matrix not is square");
                Environment.Exit(-1);
            }
            return determinate(matrix, n);
        }

        private int determinate(int[,] matrix, int n)
        {
            if (n == 1) return matrix[0,0];

            if (n == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0,1] * matrix[1, 0];

            int det = 0;
            int sign = 1;
            int[,] temp = new int[n - 1, n - 1];
            for(int i = 0; i < n; ++i)
            {
                getCofactor(matrix,temp, 0, i, n);
                det += sign * matrix[0, i] * determinate(temp, n - 1);
                sign *= -1;
            }
            
            return det;
        }

        private void adjoint(int[,] matrix, int[,] temp)
        {
            if (n == 1)
            {
                temp[0, 0] = 1;
                return;
            }

            int sign = 1;
            int[,] confactorMx = new int[n - 1, n - 1];

            for(int i = 0; i < n; ++i)
            {
                for(int j = 0; j < n; ++j)
                {
                    getCofactor(matrix, confactorMx, i, j, n);
                    sign = ((i + j) % 2 == 0) ? 1 : -1;
                    temp[j, i] = sign * determinate(confactorMx, n - 1);
                }
            }
        }

        public void inverse(float[,] inverseMx)
        {
            int det = determinate(matrix,n);
            if(det == 0)
            {
                Console.WriteLine("Singular matrix,cant find its inverse");
                Environment.Exit(-1);
            }

            int[,] temp = new int[n, n];
            adjoint(matrix, temp);

            for(int i = 0; i < n; ++i)
            {
                for(int j = 0; j < n; ++j)
                {
                    inverseMx[i, j] = temp[i, j] / (float)det;
                }
            }
        }

        public void transposeMatrix()
        {
            int[,] transposeMx = new int[m, n];
            
            for(int i = 0; i < m; ++i)
            {
                for(int j = 0; j < n; ++j)
                {
                    transposeMx[i,j] = matrix[j,i];
                }
            }
            swap(ref n,ref m);
            matrix = transposeMx;
        }

        private void swap(ref int n, ref int m)
        {
            int temp = n;
            n = m;
            m = temp;
        }

        public bool matrixIsOrthogonal()
        {
            return n != m;
        }

        public int findMinElements()
        {
            int min = Int32.MaxValue;

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (min > matrix[i, j]) min = matrix[i, j];
                }
            }
            return min;
        }

        public int findMaxElements()
        {
            int max = Int32.MinValue;

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (max < matrix[i, j]) max = matrix[i, j];
                }
            }
            return max;
        }

        public void printMatrix()
        {
            for(int i = 0; i < n; ++i)
            {
                for(int j = 0; j < m; ++j)
                {
                    Console.Write(matrix[i, j].ToString().PadLeft(5));
                }
                Console.WriteLine();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int n = 3;
            Matrix mx = new Matrix(n,n);
            mx.randomInitialization();
            mx.printMatrix();
            Console.WriteLine();
            Matrix mx1 = new Matrix(n, n);
            mx1.randomInitialization();
            mx1.printMatrix();
            Console.WriteLine();
            Matrix addMatrix = mx.multiplication(mx1);
            addMatrix.printMatrix();
            //float[,] floatMx = new float[n, n];
            //mx.inverse(floatMx);
            //printMatrix(floatMx,n);

        }

        static void printMatrix(float[,] matrix, int n)
        {
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write(matrix[i, j].ToString().PadLeft(15));
                }
                Console.WriteLine();
            }
        }
    }
}
