using System;
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
        string password = GeneratePassword(length);
        Console.WriteLine($"Ваш случайный пароль: {password}");

    }
    static string GeneratePassword(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        Random random = new Random();
        char[] password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }
        return new string(password);

    }
}