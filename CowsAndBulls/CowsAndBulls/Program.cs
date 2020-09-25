using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowsAndBulls
{
    class Program
    {
        /// <summary>
        /// Returns correct naming for n cows.
        /// </summary>
        /// <param name="n"> number of cows (guaranted to be less then eleven) </param>
        /// <returns> String with correct naming. </returns>
        private static string GetCorrectNamingForNCows(int n)
        {
            string naming = "коров";

            switch (n)
            {
                // Case 0 is included in default behaviour, but is explisitly written for better readablitly.
                case 0: return naming;
                case 1: return naming + "у";
                case 2:
                case 3:
                case 4: return naming + "ы";
                default: return naming;
            }
        }


        /// <summary>
        /// Returns correct naming for n bulls.
        /// </summary>
        /// <param name="n"> number of bulls (guaranteed to be less then eleven) </param>
        /// <returns> String with correct naming. </returns>
        private static string GetCorrectNamingForNBulls(int n) =>
            "бык" + (n == 1 ? "а" : "ов");



        /// <summary>
        /// Returns correct naming for n numeral.
        /// </summary>
        /// <param name="n"> numeral number (guaranteed to be less then eleven) </param>
        /// <returns> String with correct naming. </returns>
        private static string GetCorrectNamingForNumeral(int n) =>
            "цифр" + (n == 1 ? "ой" : "ами");


        /// <summary>
        /// Get number of digits that are on their correct position.
        /// </summary>
        /// <param name="suggestion"> number as string that was suggested by user </param>
        /// <param name="answer"> correct answer that user is guessing </param>
        /// <returns> Number of digits that are on their correct positions. </returns>
        private static int GetNumberOfBulls(string suggestion, string answer) =>
            // Using enumerable to merge to strings, and lambda for comparing their char values, then summing up amount of "right-placed" chars.
            // Enumerable - https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=netcore-3.1
            // Lamdas - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
            Enumerable.Zip(suggestion, answer, (l1, l2) => l1 == l2 ? 1 : 0).Sum();


        /// <summary>
        /// Get number of digits that are both in suggestion and answer. (by the rules both in suggestion and in answer digits must be unique).
        /// </summary>
        /// <param name="suggestion"> number as string that was suggested by user </param>
        /// <param name="answer"> correct answer that user is guessing </param>
        /// <returns> Number of digits that are both in suggestion and answer. </returns>
        private static int GetNumberOfCows(string suggestion, string answer)
        {
            // Creating HashSets from our strings - to use IntersectWith() method and get our result.
            // HashSet - https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?vi
            HashSet<char> suggestionSet = new HashSet<char>(suggestion);
            HashSet<char> answerSet = new HashSet<char>(answer);

            // Using this method we would have only intersecting digits in our suggestionSet object.
            suggestionSet.IntersectWith(answerSet);

            // Returning amount of intersecting digits. Bulls are substracted, because in rules bulls aren't counted as cows.
            return suggestionSet.Count() - GetNumberOfBulls(suggestion, answer);
        }


        /// <summary>
        /// Generates number (returned as string for better usability afterwards) with set length, which follows the rules of Cows and Bulls game (first digit is not 0, and all digits are different).
        /// </summary>
        /// <param name="length"> number of digits (guaranteed be less then eleven) </param>
        /// <returns> Returns correct number, envisioned for game of Cows and Bulls </returns>
        private static string GenerateEnvisionedString(int length)
        {
            // Object for generating random numbers.
            Random r = new Random();

            // Mutable string-like object for better optimisation.
            // StringBuilder -  https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=netcore-3.1
            StringBuilder resBuilder = new StringBuilder(10);

            // Using Enumerable to generate collection filled by numbers 0 to 9, which are then randomly sorted by lambda: n => r.Next(). All numbers would be unique.
            // Enumerable - https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=netcore-3.1
            // Lamdas - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
            foreach (var num in Enumerable.Range(0, 10).OrderBy(n => r.Next()))
                resBuilder.Append((char)(num + '0'));

            // Justifying that 0 is not the leading digit
            if (resBuilder[0] == '0')
            {
                int ind = r.Next(1, 10);

                // Swapping variable values with tuples construction.
                // Tuple - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
                (resBuilder[0], resBuilder[ind]) = (resBuilder[ind], resBuilder[0]);
            }

            return resBuilder.ToString().Substring(0, length);
        }

        /// <summary>
        /// Awaits for the correct input and returns it. 
        /// </summary>
        /// <returns> Wanted length for the number for playing Cows and Bulls. </returns>
        private static int InputLength()
        {
            bool wasInputErrorMade = false;
            int input = 0;

            do
            {
                if (wasInputErrorMade)
                    Console.WriteLine("Была допущена ошибка во вводе, пожалуйста повторите ввод:");
                else
                    Console.WriteLine("Добро пожаловать в игру коровы и быки." +
                        "\nПравила: Компьютер загадывает тайное 4-значное число с неповторяющимися цифрами." +
                        "\nВы делаете первую попытку отгадать число." +
                        "\nПопытка — это 4-значное число с неповторяющимися цифрами? записываемая в консоль." +
                        "\nКомпьютер сообщает в ответ, сколько цифр угадано без совпадения с их позициями в тайном числе (то есть количество коров)" +
                        "\nи сколько угадано вплоть до позиции в тайном числе (то есть количество быков) (быки не считаются коровами!). " +
                        "\nВведите длину числа с которым будет проходить игра: (число, очевидно, от 1 До 10 включительно),");
                // Next iterations of this cycle would give message about incorrect input
                wasInputErrorMade = true;
            } while (!(int.TryParse(Console.ReadLine(), out input) && input < 11 || input > 0));

            return input;
        }

        private static string InputSuggestion(int length)
        {
            bool wasInputErrorMade = false;
            string input;

            do
            {
                if (wasInputErrorMade)
                    Console.WriteLine("Была допущена ошибка во вводе, пожалуйста повторите ввод:");
                else
                    Console.WriteLine($"Введите положительное число с {length} {GetCorrectNamingForNumeral(length)}, каждая из которых уникальна.");
                // Next iterations of this cycle would give message about incorrect input
                wasInputErrorMade = true;
                input = Console.ReadLine();
            } while (!IsInputValid(length, input));

            return input;
        }


        /// <summary>
        /// Checks if input string is valid for <see cref="InputSuggestion(int)"> processing further </see>.
        /// </summary>
        /// <param name="length"> preset length of string </param>
        /// <param name="input"> string that is being validated</param>
        /// <returns> Is string valid. </returns>
        private static bool IsInputValid(int length, string input)
        {
            // Validating that input can be interpreted as unsigned number.
            return ulong.TryParse(input, out ulong _tmp) &&
            // Validating that all input chars are unique by comparing its length to HashSet's made of it, which only contains unique elements.
            // HashSet - https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?vi
                   input.Length == new HashSet<char>(input).Count() &&
            // Validating that input length is equal to preset length
                   input.Length == length;

        }


        /// <summary>
        /// One round of Cows and Bulls game is played.
        /// </summary>
        private static void PlayOneRound()
        {
            int length = InputLength();
            string answer = GenerateEnvisionedString(length);
            int bulls;
            int turn = 0;

            do
            {
                string suggestion = InputSuggestion(length);
                bulls = GetNumberOfBulls(suggestion, answer);
                int cows = GetNumberOfCows(suggestion, answer);
                Console.WriteLine($"Вы верно угадали {cows} {GetCorrectNamingForNCows(cows)}, и {bulls} {GetCorrectNamingForNBulls(bulls)}.");
                turn++;
            } while (bulls != length);
            Console.WriteLine($"Поздравляем, вы выиграли на {turn} ходу, загаданно было действительно {answer}!");
        }


        /// <summary>
        /// Asks player if he wants to play another round of Cows and Bulls.
        /// </summary>
        /// <returns> Does player want to play another round. </returns>
        private static bool IsAnotherRoundPlayed()
        {
            string input;

            do
            {
                Console.WriteLine("Хотите ли вы сыграть еще один раунд коров и быков?\nОтветьте да/нет, чтобы соотвестственно продолжить/выйти");
                input = Console.ReadLine();
            } while (!(input.Equals("да") || input.Equals("нет")));

            return input.Equals("да");
        }


        static void Main(string[] args)
        {
            do
            {
                PlayOneRound();
            } while (IsAnotherRoundPlayed());
            Console.WriteLine("Спасибо за игру!");
        }
    }
}
