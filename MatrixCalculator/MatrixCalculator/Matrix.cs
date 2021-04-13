using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MatrixCalculator
{
    class Matrix
    {

        protected double[][] matrix;
        private int m;
        private int n;

        
        /// <summary>
        /// Max and min values for matrix values.
        /// </summary>
        const double max = 1000, min = -1000;


        /// <summary>
        /// Returns number of lines.
        /// </summary>
        public int N { get => n; }


        /// <summary>
        /// Returns number of columns.
        /// </summary>
        public int M { get => m; }


        /// <summary>
        /// Returnes matrix in readonly.
        /// </summary>
        public double[][] MatrixValue { get => matrix; }


        /// <summary>
        /// Constructor that asks for type of filling matrix.
        /// </summary>
        /// <param name="n">Number of lines.</param>
        /// <param name="m">Number of columns.</param>
        public Matrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            matrix = new double[0][];
            if ((n < 1 || n > 10) || (m < 1 || m > 10))
            {
                Console.WriteLine("Введена неверная размерность матрицы");
            }
            else
            {
                matrix = new double[n][];
                SelectMatrixFillWay();
            }
        }


        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        public void Transpose()
        {
            double[][] tMatrix = new double[m][];
            for (int i = 0; i < m; i++)
            {
                tMatrix[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    tMatrix[i][j] = matrix[j][i];
                }
            }
            int tmp = n;
            n = m;
            m = tmp;
            matrix = tMatrix;
        }


        /// <summary>
        /// Used to fill matrix and select the way of filling.
        /// </summary>
        private void SelectMatrixFillWay()
        {
            Console.WriteLine("Выберите способ заполнения матрицы:\n" +
                    "1: заполнение случайными числами от -1000 до 1000\n" +
                    "2: заполнение через консоль\n" +
                    "3: заполнение через файл (где матрица представлена в виде набора чисел, где элементы строки разделены пробелом, а столбцы - переходом на новую строку, числа от -1000 до 1000)\n" +
                    "Введите номер необходимой вам операции: ");
            char input;
            do
            {
                input = Console.ReadKey().KeyChar;
                Console.Write("\n");

            } while (input > '3' || input < '1');
            switch ((int)(input - '0'))
            {
                case 1:
                    FillByRandom();
                    return;
                case 2:
                    RepeatFill("console");
                    return;
                case 3:
                    RepeatFill("file");
                    return;
            }
        }


        /// <summary>
        /// Waits for correct filling of matrix.
        /// </summary>
        /// <param name="fillType">Is specifiyed by "console" and "file"</param>
        private void RepeatFill(string fillType)
        {
            bool flg = false;
            do
                try
                {
                    if (fillType == "console")
                        FillByConsole();
                    else if (fillType == "file") // Else if is used instead of else, in order to specify that only "console" and "random" are expected. (Мне лень писать enum).
                        FillByFile();
                    flg = true;
                }
                catch
                {
                    Console.WriteLine("Допущена ошибка во введенных данных, повторите попытку");
                }
            while (!flg);
            Console.WriteLine($"Введенная матрица:\n{this}");
        }



        /// <summary>
        /// Fills matrix with random values from const min to const max.
        /// </summary>
        private void FillByRandom()
        {
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new double[m];
                for (int j = 0; j < m; j++)
                    matrix[i][j] = r.NextDouble() * (max - min) + min;
            }
            Console.WriteLine($"Полученная матрица:\n{this}");
        }


        /// <summary>
        /// Matrix is filled by user in console.
        /// </summary>
        private void FillByConsole()
        {
            try
            {
                Console.WriteLine("Введите матрицу, в виде набора чисел, где элементы строки разделены пробелом, а столбцы - переходом на новую строку, числа от -1000 до 1000");
                for (int i = 0; i < n; i++)
                {
                    matrix[i] = new double[m];
                    string s = Console.ReadLine();
                    string[] el = s.Split();
                    if (el.Length != m)
                        throw new Exception();
                    for (int j = 0; j < m; j++)
                    {
                        matrix[i][j] = double.Parse(el[j]);
                        if (matrix[i][j] > max || matrix[i][j] < min)
                            throw new Exception();
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Matrix is filled using file specified by user.
        /// </summary>
        private void FillByFile()
        {
            try
            {
                Console.WriteLine("Введите путь к файлу, где и находится матрица. Путь может быть как относителен от исполняемого файла, так и абсолютен:\n" +
                    "(Можете написать matrix.txt - это уже существующая матрица размера 3 на 3, заполненная числами от 1 до 9)");
                string path = Console.ReadLine();
                string[] lines = File.ReadAllLines(path);
                for (int i = 0; i < n; i++)
                {
                    matrix[i] = new double[m];
                    string[] el = lines[i].Split();
                    if (el.Length != m)
                        throw new Exception();
                    for (int j = 0; j < m; j++)
                    {
                        matrix[i][j] = double.Parse(el[j]);
                        if (matrix[i][j] > max || matrix[i][j] < min)
                            throw new Exception();
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Overrided ToString() method for better usability in writing matrix.
        /// </summary>
        /// <returns>Matrix casted to string.</returns>
        public override string ToString()
        {
            if (IsEmpty())
                return "";
            StringBuilder res = new StringBuilder("");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                    res.Append((matrix[i][j] + " ").PadLeft(25));
                res.Append("\n");
            }
            return res.ToString();
        }


        /// <summary>
        /// Checks if matrix is empty.
        /// </summary>
        /// <returns>If matrix is empty.</returns>
        public bool IsEmpty()
        {
            return matrix.Length == 0;
        }

        /// <summary>
        /// Checks if this and given matrix are equal size.
        /// </summary>
        /// <param name="mat">Another matrix.</param>
        /// <returns>If 2 matrixes are equally-sized</returns>
        public bool AreEqualSize(Matrix mat)
        {
            return (n == mat.N) && (m == mat.M);
        }


        /// <summary>
        /// Summarises this matrix with second matrix multiplied to coefficient.
        /// </summary>
        /// <param name="mat">Second matrix.</param>
        /// <param name="coeff">Coefficient.</param>
        public void Plus(Matrix mat, int coeff)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i][j] += mat.MatrixValue[i][j] * coeff;
        }

        /// <summary>
        /// Multiplies this matrix on coeffient.
        /// </summary>
        /// <param name="coef">Coefficient.</param>
        public void Multiply(double coef)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i][j] *= coef;
        }


        /// <summary>
        /// Multiplies this matrix and another one.
        /// </summary>
        /// <param name="mat">Second matrix.</param>
        public void Multiply(Matrix mat)
        {
            if (m != mat.N)
                throw new FormatException();
            double[][] newMatrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                newMatrix[i] = new double[mat.M];
                for (int j = 0; j < mat.M; j++)
                {
                    newMatrix[i][j] = 0;
                    for (int it = 0; it < m; it++)
                        newMatrix[i][j] += matrix[i][it] * mat.MatrixValue[it][j];
                }
            }
            m = mat.M;
            matrix = newMatrix;
        }


        /// <summary>
        /// Swap 2 lines, elementary transformation.
        /// </summary>
        /// <param name="x">First line to swap.</param>
        /// <param name="y">Second line to swap.</param>
        protected void SwapLines(int x, int y)
        {
            for (int i = 0; i < n; i++)
            {
                double tmp = matrix[x][i];
                matrix[x][i] = matrix[y][i];
                matrix[y][i] = tmp;
            }
        }


        /// <summary>
        /// Substract one line from another, elementary transformation.
        /// </summary>
        /// <param name="x">Line to substact.</param>
        /// <param name="y">Line to be substacted.</param>
        /// <param name="f">First not null element.</param>
        protected void MakeFirstLineElementNull(int x, int y, int f)
        {
            double coef = matrix[y][f] / matrix[x][f];
            for (int i = 0; i < n; i++)
                matrix[y][i] -= matrix[x][i] * coef;
        }



        /// <summary>
        /// Finds first element with not leading zero in lineNum postition.
        /// </summary>
        /// <param name="lineNum">Number of line to fill, previous are already filled by algorithm.</param>
        /// <returns>Number of line, or -1 if there is no such line</returns>
        protected int FindFirstWithoutLeadingZero(int lineNum)
        {
            for (int i = lineNum; i < n; i++)
            {
                if (matrix[i][lineNum] != 0)
                    return i;
            }
            return -1;
        }


    }
}
