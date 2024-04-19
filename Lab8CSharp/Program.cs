using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab8CSharp
{    
    class Program
    {
        static void Main(string[] args)
        {
            char n;
            while (true)
            {
                Console.Write("\nSelect the task number(1/5):");
                n = Convert.ToChar(Console.ReadLine());

                switch (n)
                {
                    case '1': Task1(); break;
                    case '2': Task2(); break;
                    case '3': Task3(); break;
                    case '4': Task4(); break;
                    case '5': Task5(); break;
                    default: Console.WriteLine("\nProgram has ended "); return;
                }
            }
        }

        private static void Task1()
        {
            string filePath = "input.txt";
            string text = File.ReadAllText(filePath);

            string emailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";
            MatchCollection emailMatches = Regex.Matches(text, emailPattern);

            string outputFilePath = "output.txt";
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (Match match in emailMatches)
                {
                    writer.WriteLine(match.Value);
                }
            }

            Console.WriteLine($"Знайдено та записано {emailMatches.Count} електронних адрес у файл {outputFilePath}");
        }

        //public int FindMaxNumber(string text) => text.Split().Select(s => int.TryParse(s, out int n) ? n : 0).Max();

        private static void Task2()
        {
            string filePath = "input.txt";
            string text = File.ReadAllText(filePath);

            int maxNumber = text.Split().Select(s => int.TryParse(s, out int n) ? n : 0).Max();

            string outputFilePath = "output.txt";
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine($"Максимальне цiле число у текстi: {maxNumber}\n");
            }

            Console.WriteLine($"Максимальне цiле число у текстi: {maxNumber}. Результат записано у файл {outputFilePath}");
        }

        private static void Task3()
        {
            // Читаємо вміст файлу
            string filePath = "input2.txt";
            string text = File.ReadAllText(filePath);

            text = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
            text = string.Join(" ", text.Split().Where(word => text.Split().Count(w => w == word) > 1));

            // Записуємо результат у новий файл
            string outputFilePath = "output1.txt";
            File.WriteAllText(outputFilePath, text);

            Console.WriteLine("Результат записано у файл output.txt");
        }

        private static void Task4()
        {
            // Введення кількості чисел
            Console.Write("Введiть кiлькiсть чисел: ");
            int n = int.Parse(Console.ReadLine());

            // Введення діапазону чисел
            Console.Write("Введiть початок дiапазону: ");
            double startRange = double.Parse(Console.ReadLine());
            Console.Write("Введiть кiнець дiапазону: ");
            double endRange = double.Parse(Console.ReadLine());

            // Запис чисел у файл
            string filePath = "numbers.bin";
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                Console.WriteLine("Числа записані в numbers.bin:");
                Random rand = new Random();
                for (int i = 0; i < n; i++)
                {
                    double number = rand.NextDouble() * 10;
                    Console.WriteLine(number);
                    writer.Write(number);
                }
            }

            Console.WriteLine($"Числа були записанi у файл {filePath}");

            // Виведення компонентів, що не попадають у діапазон
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                Console.WriteLine("Числа, що не попадають у даний дiапазон:");
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    double number = reader.ReadDouble();
                    if (number < startRange || number > endRange)
                    {
                        Console.WriteLine(number);
                    }
                }
            }
        }

        private static void Task5()
        {
            string studentLastName = "Rusnak"; 
            string studentFirstName = "Maksim"; 

            // 1. Створення папок
            string folder1Path = $@"d:\temp\{studentLastName}1";
            string folder2Path = $@"d:\temp\{studentLastName}2";
            Directory.CreateDirectory(folder1Path);
            Directory.CreateDirectory(folder2Path);

            // 2. Створення файлів і запис тексту
            string t1Text = $"<{studentLastName} {studentFirstName}, 2004> року народження, мiсце проживання <м. Чернiвцi>";
            string t2Text = $"<Комар Сергiй Федорович, 2000> року народження, мiсце проживання <м. Київ>";

            File.WriteAllText(Path.Combine(folder1Path, "t1.txt"), t1Text);
            File.WriteAllText(Path.Combine(folder1Path, "t2.txt"), t2Text);

            // 3. Читання тексту з файлів t1.txt та t2.txt та запис у файл t3.txt
            string t1FilePath = Path.Combine(folder1Path, "t1.txt");
            string t2FilePath = Path.Combine(folder1Path, "t2.txt");
            string t3FilePath = Path.Combine(folder2Path, "t3.txt");

            string t1Content = File.ReadAllText(t1FilePath);
            string t2Content = File.ReadAllText(t2FilePath);

            File.WriteAllText(t3FilePath, t1Content + Environment.NewLine + t2Content);

            // 4. Виведення інформації про створені файли
            Console.WriteLine($"Створено файли:");
            Console.WriteLine($"- {t1FilePath}");
            Console.WriteLine($"- {t2FilePath}");
            Console.WriteLine($"- {t3FilePath}");

            // 5. Переміщення файлу t2.txt в папку <прізвище_студента>2
            string folder2FilePath = Path.Combine(folder2Path, "t2.txt");
            File.Move(t2FilePath, folder2FilePath);

            // 6. Копіювання файлу t1.txt в папку <прізвище_студента>2
            string folder2CopyFilePath = Path.Combine(folder2Path, "t1.txt");
            File.Copy(t1FilePath, folder2CopyFilePath);

            // 7. Перейменування папки та вилучення іншої
            string allFolderPath = Path.Combine(Path.GetDirectoryName(folder2Path), "ALL");
            Directory.Move(folder2Path, allFolderPath);
            Directory.Delete(folder1Path, true);

            // 8. Виведення інформації про файли папки ALL
            Console.WriteLine($"Файли папки ALL:");
            string[] allFiles = Directory.GetFiles(allFolderPath);
            foreach (var file in allFiles)
            {
                Console.WriteLine($"- {file}");
            }
        }
    }
}
