using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Введите длину пароля...");
            if (!int.TryParse(Console.ReadLine(), out int length) || length <= 0)
            {
                Console.WriteLine("Ошибка. Введите корректное число.");
                return;
            }

            Console.WriteLine("Использовать буквы? (y/n)");
            bool useLetters = GetUserChoice();

            Console.WriteLine("Использовать цифры? (y/n)");
            bool useNumbers = GetUserChoice();

            Console.WriteLine("Использовать спецсимволы? (y/n)");
            bool useSpecialChars = GetUserChoice();

            if (!useLetters && !useNumbers && !useSpecialChars)
            {
                Console.WriteLine("Нужно выбрать хотя бы один тип символов");
                return;
            }

            string password = GeneratePassword(length, useLetters, useNumbers, useSpecialChars);
            Console.WriteLine($"Ваш случайный пароль: {password}");

            string passwordStrength = EvaluatePasswordStrength(password);
            Console.WriteLine($"Надежность пароля: {passwordStrength}");

            SavePassword(password);
            Console.WriteLine("Пароль сохранен в passwords.txt");

            // Задержка, чтобы терминал не закрылся сразу
            Console.WriteLine("Нажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    // Метод для получения выбора пользователя с проверкой ввода
    static bool GetUserChoice()
    {
        string input;
        while (true)
        {
            input = Console.ReadLine().Trim().ToLower();
            if (input == "y")
            {
                return true;
            }
            else if (input == "n")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите 'y' или 'n'.");
            }
        }
    }

    static string GeneratePassword(int length, bool useLetters, bool useNumbers, bool useSpecialChars)
    {
        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";
        string specialChars = "!@#$%^&*";

        string allowedChars = "";
        if (useLetters) allowedChars += letters;
        if (useNumbers) allowedChars += numbers;
        if (useSpecialChars) allowedChars += specialChars;

        Random random = new Random();
        char[] password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = allowedChars[random.Next(allowedChars.Length)];
        }
        return new string(password);
    }

    static string EvaluatePasswordStrength(string password)
    {
        int lowerCase = password.Count(char.IsLower);
        int upperCase = password.Count(char.IsUpper);
        int digits = password.Count(char.IsDigit);
        int specialChars = password.Count(c => "!@#$%^&*()_+[]{}|;:,.<>?".Contains(c));

        int typesUsed = 0;
        if (lowerCase > 0) typesUsed++;
        if (upperCase > 0) typesUsed++;
        if (digits > 0) typesUsed++;
        if (specialChars > 0) typesUsed++;

        double lengthScore = Math.Min(password.Length / 2.0, 3.0);
        double typesScore = typesUsed;
        double diversityScore = password.Distinct().Count() / (double)password.Length;

        double totalScore = lengthScore + typesScore + diversityScore * 2;

        if (totalScore >= 8) return "Очень хороший";
        else if (totalScore >= 6) return "Хороший";
        else if (totalScore >= 4) return "Средний";
        else return "Слабый";
    }

    static void SavePassword(string password)
    {
        string filePath = "passwords.txt";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(filePath, $"{timestamp} - {password}{Environment.NewLine}");
    }
}
