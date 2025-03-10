using System;
using System.Threading.Channels;
class Program
{
    static void Main()
    {
        Console.WriteLine("Введите длину пароля...");
        if (!int.TryParse(Console.ReadLine(), out int length) || length <= 0)
        {
            Console.WriteLine("Ошибка. Введите корректное число.");
            return;
        }

        Console.WriteLine("Использовать буквы? (y/n)");
        bool useLetters = Console.ReadLine().Trim().ToLower() == "y";

        Console.WriteLine("Использовать цифры? (y/n)");
        bool useNumbers = Console.ReadLine().Trim().ToLower() == "y";

        Console.WriteLine("Использовать спецсимволы? (y/n)");
        bool useSpecialChars = Console.ReadLine().Trim().ToLower() == "y";

        if (!useLetters && !useNumbers && !useSpecialChars)
        {
            Console.WriteLine("Нужно выбрать хотя бы один тип символов");
            return;
        }

        string password = GeneratePassword(length, useLetters, useNumbers, useSpecialChars);
        Console.WriteLine($"Ваш случайный пароль: {password}");

        SavePassword(password);
        Console.WriteLine("Пароль сохранен в passwords.txt");

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
    static void SavePassword(string password)
    {
        string filePath = "passwords.txt";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(filePath, $"{timestamp} - {password}{Environment.NewLine}");
    }
}