using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCalculator
{


    /// <summary>
    /// Sqare matrix is extended From Matrix has its limitations: n = m and has more methods than original Matirx.
    /// </summary>
    class SqMatrix : Matrix
    {
        public SqMatrix(int n, int m) : base(n, m)
        {
            if (n != m)
                matrix = new double[0][];
        }


        /// <summary>
        /// Finds trail of a square matrix
        /// </summary>
        /// <returns>Matrix trail.</returns>
        public double FindTrail()
        {
            double trailSum = 0;
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[0].Length; j++)
                    if (i == j)
                        trailSum += matrix[i][j];
            return trailSum;
        }


        /// <summary>
        /// Finds determinant of a square matrix
        /// </summary>
        /// <returns>Matrix determinant.</returns>
        public double FindDeterminant()
        {
            for (int i = 0; i < N; i++)
            {
                int notNullLine = FindFirstWithoutLeadingZero(i);
                if (notNullLine == -1)
                    return 0;
                SwapLines(i, notNullLine);
                for (int j = i + 1; j < N; j++)
                {
                    MakeFirstLineElementNull(notNullLine, j, i);
                }
            }
            double deter = 1;
            for (int i = 0; i < N; i++)
                deter *= matrix[i][i];
            return deter;

        }


        /// <summary>
        /// Solves system using Krammers method, so matrix must be n * n, prints info about the system solutions.
        /// </summary>
        /// <param name="b">Free elements of system.</param>
        public void SolveWithKrammerAndPrint(double[] b)
        {
            double d = FindDeterminant();
            double[] di = new double[N];
            for (int i = 0; i < N; i++)
                di[i] = FindDeterMinantWithReplacedColumn(b, i);
            if (d == 0)
            {
                bool flg = true;
                for (int i = 0; i < N; i++)
                    if (di[i] != 0)
                    {
                        flg = false;
                        break;
                    }
                if (flg)
                    Console.WriteLine("Система линейных уравнений имеет бесчисленное множество решений");
                else
                    Console.WriteLine("Система линейных уравнений решений не имеет");
            }
            else
            {
                Console.WriteLine("система линейных уравнений имеет единственное решение, выпишем его:");
                for (int i = 0; i < N; i++)
                {
                    Console.WriteLine($"x{i+1} = {di[i]/d}");
                }
            }
        }


        /// <summary>
        /// Used in Krammers method replaces a column with b and finds determinant.
        /// </summary>
        /// <param name="b">Free elements of system.</param>
        /// <param name="c">Number of column to replace.</param>
        /// <returns>Determinant.</returns>
        private double FindDeterMinantWithReplacedColumn(double[] b, int c)
        {
            double[][] origM = matrix;
            for (int i = 0; i < N; i++)
                matrix[c][i] = b[i];
            double res = FindDeterminant();
            matrix = origM;
            return res;
        }

    }
}
