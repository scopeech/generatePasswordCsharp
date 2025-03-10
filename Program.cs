using System;
using System.Data;
using System.Threading.Channels;

class Program
{
    static void Main()
    {
        // Запрашиваем длину пароля у пользователя
        Console.WriteLine("Введите длину пароля...");

        // Проверяем, что пользователь ввел корректное число для длины пароля
        if (!int.TryParse(Console.ReadLine(), out int length) || length <= 0)
        {
            Console.WriteLine("Ошибка. Введите корректное число.");
            return; // Завершаем программу, если введено неверное число
        }

        // Запрашиваем у пользователя, нужно ли использовать буквы
        Console.WriteLine("Использовать буквы? (y/n)");
        bool useLetters = Console.ReadLine().Trim().ToLower() == "y"; // Преобразуем ввод в нижний регистр и проверяем

        // Запрашиваем, нужно ли использовать цифры
        Console.WriteLine("Использовать цифры? (y/n)");
        bool useNumbers = Console.ReadLine().Trim().ToLower() == "y";

        // Запрашиваем, нужно ли использовать спецсимволы
        Console.WriteLine("Использовать спецсимволы? (y/n)");
        bool useSpecialChars = Console.ReadLine().Trim().ToLower() == "y";

        // Если пользователь не выбрал ни одного типа символов, показываем ошибку и завершаем программу
        if (!useLetters && !useNumbers && !useSpecialChars)
        {
            Console.WriteLine("Нужно выбрать хотя бы один тип символов");
            return;
        }

        // Генерируем пароль с выбранными параметрами
        string password = GeneratePassword(length, useLetters, useNumbers, useSpecialChars);
        Console.WriteLine($"Ваш случайный пароль: {password}");

        // Оцениваем надежность пароля
        string passwordStrength = EvaluatePasswordStrength(password);
        Console.WriteLine($"Надежность пароля: {passwordStrength}");

        // Сохраняем пароль в файл
        SavePassword(password);
        Console.WriteLine("Пароль сохранен в passwords.txt");
    }

    // Метод для генерации пароля
    static string GeneratePassword(int length, bool useLetters, bool useNumbers, bool useSpecialChars)
    {
        // Строки с возможными символами для пароля
        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";
        string specialChars = "!@#$%^&*";

        // Собираем строку из разрешенных символов в зависимости от выбора пользователя
        string allowedChars = "";
        if (useLetters) allowedChars += letters;
        if (useNumbers) allowedChars += numbers;
        if (useSpecialChars) allowedChars += specialChars;

        // Создаем случайный генератор
        Random random = new Random();
        char[] password = new char[length]; // Массив для пароля

        // Заполняем массив пароля случайными символами из allowedChars
        for (int i = 0; i < length; i++)
        {
            password[i] = allowedChars[random.Next(allowedChars.Length)];
        }

        // Возвращаем строку из массива пароля
        return new string(password);
    }

    // Метод для оценки надежности пароля
    static string EvaluatePasswordStrength(string password)
    {
        // Подсчитываем количество символов каждого типа
        int lowerCase = password.Count(char.IsLower); // Количество строчных букв
        int upperCase = password.Count(char.IsUpper); // Количество заглавных букв
        int digits = password.Count(char.IsDigit); // Количество цифр
        int specialChars = password.Count(c => "!@#$%^&*()_+[]{}|;:,.<>?".Contains(c)); // Количество спецсимволов

        // Считаем, сколько типов символов использовано
        int typesUsed = 0;
        if (lowerCase > 0) typesUsed++;
        if (upperCase > 0) typesUsed++;
        if (digits > 0) typesUsed++;
        if (specialChars > 0) typesUsed++;

        // Оценка на основе длины пароля (максимум 3 балла)
        double lengthScore = Math.Min(password.Length / 2.0, 3.0);

        // Оценка на основе использования различных типов символов
        double typesScore = typesUsed;

        // Оценка на основе разнообразия символов (чем больше уникальных символов, тем лучше)
        double diversityScore = password.Distinct().Count() / (double)password.Length;

        // Общая оценка пароля
        double totalScore = lengthScore + typesScore + diversityScore * 2; // Разнообразию даем больший вес (умножаем на 2)

        // На основе общей оценки возвращаем уровень надежности пароля
        if (totalScore >= 8) return "Очень хороший"; // Очень надежный
        else if (totalScore >= 6) return "Хороший"; // Хороший
        else if (totalScore >= 4) return "Средний"; // Средний
        else return "Слабый"; // Слабый
    }

    // Метод для сохранения пароля в файл
    static void SavePassword(string password)
    {
        // Путь к файлу, в который будем сохранять пароли
        string filePath = "passwords.txt";
        // Получаем текущее время для добавления метки времени
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // Записываем пароль в файл с меткой времени
        File.AppendAllText(filePath, $"{timestamp} - {password}{Environment.NewLine}");
    }
}
