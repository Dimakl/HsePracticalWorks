using System;

namespace MatrixCalculator
{
    class Program
    {


        /// <summary>
        /// Operation selector, repeated from main.
        /// </summary>
        /// <returns>False if app must exit.</returns>
        private static bool OperationTypeSelector()
        {
            WelcomeOutput();
            char input;
            do
            {
                input = Console.ReadKey().KeyChar;
                Console.Write("\n");
            } while (input > '9' || input < '1');
            int inputNum = input - '0';
            switch (inputNum)
            {
                case 1:
                    FindTrail();
                    return true;
                case 2:
                    TransposeMatrix();
                    return true;
                case 3:
                    SumMatrixes();
                    return true;
                case 4:
                    SubstractMatrixes();
                    return true;
                case 5:
                    MultiplyMatrixes();
                    return true;
                case 6:
                    MultiplyMatirxByScalar();
                    return true;
                case 7:
                    FindMatrixDeterminant();
                    return true;
                case 8:
                    SolveSystem();
                    return true;
                case 9:
                    return false;
            }
            return false;
        }


        /// <summary>
        /// Finds trail and prints it.
        /// </summary>
        private static void FindTrail()
        {
            GuaranteedSqMatrix(out SqMatrix m);
            Console.WriteLine($"След введенной матрицы: {m.FindTrail()}");
        }


        /// <summary>
        /// Finds matrix transposition and prints it.
        /// </summary>
        private static void TransposeMatrix()
        {
            GuaranteedMatrix(out Matrix m);
            m.Transpose();
            Console.WriteLine($"Транспозиция введенной матрицы:\n{m}");
        }


        /// <summary>
        /// Summarises 2 matrixes and prints result.
        /// </summary>
        private static void SumMatrixes()
        {
            Console.WriteLine("Ввод 2 матриц:");
            GuaranteedMatrix(out Matrix m1);
            GuaranteedMatrix(out Matrix m2);
            if (!m1.AreEqualSize(m2))
                Console.WriteLine("Размерности матриц не совпадают - их невозможно сложить");
            else
            {
                m1.Plus(m2, 1);
                Console.WriteLine($"Матрица суммы:\n{m1}");
            }
        }


        /// <summary>
        /// Substracts 2 matrixes and prints result.
        /// </summary>
        private static void SubstractMatrixes()
        {
            Console.WriteLine("Ввод 2 матриц:");
            GuaranteedMatrix(out Matrix m1);
            GuaranteedMatrix(out Matrix m2);
            if (!m1.AreEqualSize(m2))
                Console.WriteLine("Размерности матриц не совпадают - их невозможно вычесть");
            else
            {
                m1.Plus(m2, -1);
                Console.WriteLine($"Матрица разности:\n{m1}");
            }
        }


        /// <summary>
        /// Multiplies 2 matrixes and prints result.
        /// </summary>
        private static void MultiplyMatrixes()
        {
            Console.WriteLine("Ввод 2 матриц:");
            GuaranteedMatrix(out Matrix m1);
            GuaranteedMatrix(out Matrix m2);
            if (m1.M != m2.N)
                Console.WriteLine("Для переменожиения 2 матриц их размерности должны быть вида n*m и m*p, для введенных матриц такое не выполняется, так что перемножить их невозможно");
            else
            {
                m1.Multiply(m2);
                Console.WriteLine($"Полученная полсле умножения матрица:\n{m1}");
            }
        }


        /// <summary>
        /// Multiplies matrix by scalar and prints result.
        /// </summary>
        private static void MultiplyMatirxByScalar()
        {
            GuaranteedMatrix(out Matrix m);
            double coef = GuaranteedDoubleInput("Введите число на которое будет умножена матрица");
            m.Multiply(coef);
            Console.WriteLine($"Полученная после умножения матрица:\n{m}");
        }

        private static void FindMatrixDeterminant()
        {
            GuaranteedSqMatrix(out SqMatrix m);
            Console.WriteLine($"Детерминант введенной матрицы: {m.FindDeterminant()}");
        }


        /// <summary>
        /// Solves system using Krammers method, so matrix must be n * n, prints info about the system solutions.
        /// </summary>
        private static void SolveSystem()
        {
            GuaranteedSqMatrix(out SqMatrix m);
            double[] b = new double[m.N];
            bool flg = false;
            do
            {
                Console.WriteLine("Введите массив b - массив свободных членов размера N, в одну строку разделенные пробелом, где каждое число - double:");
                string s = Console.ReadLine();
                string[] el = s.Split();
                if (el.Length != m.N)
                    continue;
                for (int i = 0; i < m.N; i++)
                {
                    if (!double.TryParse(el[i], out b[i]))
                        continue;
                }
                flg = true;
            } while (!flg);
            m.SolveWithKrammerAndPrint(b);
        }



        /// <summary>
        /// Prints how to interact with the app.
        /// </summary>
        private static void WelcomeOutput()
        {
            Console.WriteLine("Все операции работают до матриц 10х10, составленных из чисел с плавающей точкой, т.к. очевидно:\n" +
                "любое целочисленное число можно представить как число с плавающей точкой, так что вести вычисления на целых числах не имеет смысла\n" +
                "Калькулятор матрицю\n" +
                "Возможные типы опервций:\n" +
                "1: нахождение следа матрицы\n" +
                "2: транспонирование матрицы\n" +
                "3: сумма двух матриц\n" +
                "4: разность двух матриц\n" +
                "5: произведение двух матриц\n" +
                "6: умножение матрицы на число\n" +
                "7: нахождение определителя матрицы\n" +
                "8: решение СЛАУ методом Краммера для квадратных матриц\n" +
                "9: завершить работу приложения" +
                "Введите номер необходимой вам операции: ");
        }


        /// <summary>
        /// Repeats OperationTypeSelector.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (OperationTypeSelector())
            {
                Console.WriteLine("\n\n----------------\n");
            }
        }


        /// <summary>
        /// Waits for user to input valid matrix.
        /// </summary>
        /// <param name="matrix">Input matrix.</param>
        static void GuaranteedMatrix(out Matrix matrix)
        {
            bool flg = false;
            do
            {
                if (flg)
                    Console.WriteLine("Ввод матрицы был неверным - повторите его, возможно изменив вид ввода или размерность матрицы:");
                int n = GuaranteedIntInput("Введите колчичество строк:");
                int m = GuaranteedIntInput("Введите количество столбцов:");
                matrix = new Matrix(n, m);
                flg = true;
            } while (matrix.IsEmpty());
        }


        /// <summary>
        /// Waits for user to input valid sqare matrix.
        /// </summary>
        /// <param name="matrix">Input sqare matrix.</param>
        static void GuaranteedSqMatrix(out SqMatrix matrix)
        {
            bool flg = false;
            do
            {
                if (flg)
                    Console.WriteLine("Ввод матрицы был неверным - повторите его, возможно изменив вид ввода:");
                int n = GuaranteedIntInput("Введите n, являющееся и количеством строк и количеством столбцов, тк матрица квадратная:");
                matrix = new SqMatrix(n, n);
                flg = true;
            } while (matrix.IsEmpty());
        }


        /// <summary>
        /// Waits for user to input valid int with invitation.
        /// </summary>
        /// <param name="invitation"> Is written each time user makes a mistake.</param>
        /// <returns>Input int.</returns>
        static int GuaranteedIntInput(string invitation)
        {
            int n;
            do
            {
                Console.WriteLine(invitation);

            } while (!int.TryParse(Console.ReadLine(), out n));
            return n;
        }


        /// <summary>
        /// Waits for user to input valid double with invitation.
        /// </summary>
        /// <param name="invitation"> Is written each time user makes a mistake.</param>
        /// <returns>Input double.</returns>
        static double GuaranteedDoubleInput(string invitation)
        {
            double n;
            do
            {
                Console.WriteLine(invitation);

            } while (!double.TryParse(Console.ReadLine(), out n));
            return n;
        }
    }
}
