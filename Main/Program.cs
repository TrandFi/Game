using System;
using System.Threading;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.IO;
using System.Collections.Generic;

namespace Main
{
    internal class Program
    {
        static int step = 0;
        static void Main(string[] args)
        {
            int input;
            string log, pas;
            var account = ReadDictFromFile();            
            bool flag = true;
            string otvet = RandNumb();
            Console.WriteLine(otvet);
            while (flag)
            {
                Console.Write("Введите логин для входа в приложение: \n>");
                log = Console.ReadLine();
                if (account.ContainsKey(log))
                {
                    Console.Write("Введите пароль: \n>");
                    pas = Console.ReadLine();
                    if (account[log] == pas)
                    {
                        while (flag)
                        {
                            Console.WriteLine("Загадано 4-х значное число, цифры не повторяются");
                            Console.WriteLine("Отгадайте число за наименьшее количество попыток\n\n");
                            GameStart(otvet);
                            Console.WriteLine($"\nПоздравляю, вы выиграли за {step} ход(ов)");
                            Win();
                            Console.WriteLine("\nЧтобы начать игру заного нажмите 'Enter'");
                            Console.WriteLine("Для выхода введите 'exit'");
                            string s = Console.ReadLine();
                            if (s == "exit")
                                flag = false;
                        }
                    }
                }
                else
                {
                    Console.Write("Такого аккаунта не существует. Хотите создать новый аккаунт? Введите yes/no \n>");
                    if (Console.ReadLine() == "yes")
                    {
                        Console.Write("Придумайте логин: \n>");
                        log = Console.ReadLine();
                        Console.Write("Придумай пароль: \n>");
                        pas = Console.ReadLine();
                        account.Add(log, pas);
                        WriteDictToFile(account);
                        Console.WriteLine("Нажмите 'Enter' для продолжения\n");
                    }
                    else
                    {
                        Console.WriteLine("Без аккаунта невозможно зайти в данную игру. Производится выход из игры.");
                        flag = false;
                    }
                }
                Console.ReadKey();
            }
        }
        static string RandNumb()
        {
            List<int> output = new List<int>();
            int next = 0;
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                next = rand.Next(0, 10);
                while (output.Contains(next))
                {
                    next = rand.Next(0, 10);
                }
                output.Add(next);
            }
            return string.Join("", output);
        }
        static string Durak()
        {
            string[] mas = new string[]
            {
                "Возможно вы ошиблись, но это не 4-х значное число",
                "Вы ошиблись снова, введите 4-х значное число",
                "Не нужно испытывать терпение, введи 4-х значное число",
                "Уже не смешно, введи 4-х значное число",
                "Ты совсем кретин? Объясняю для тупых - введи число ОТ 1000 ДО 9999",
                "ЕСЛИ ТЫ СЕЙЧАС ЖЕ НЕ ВВЕДЕШЬ 4-Х ЗНАЧНОЕ ЧИСЛО, ОТЛЕТИШЬ В БАН!!!",
                "Иди учи уроки. Пока!"
            };
            int i = 0;
            while (i != 7)
            {
                Console.WriteLine("----------------------------------------\n");
                Console.Write("Введите строку = ");
                string str = Console.ReadLine();
                bool result = int.TryParse(str, out int numb);
                if (result && str.Length == 4)
                {
                    return str;
                }
                Console.WriteLine(mas[i] + "\n");
                i++;
            }
            if (i == 7)
            {
                Thread.Sleep(1800);

                Environment.Exit(0);
            }
            return "";
        }
        static void GameStart(string otvet)
        {
            int Bull = 0, Cow = 0;
            List<char> mas_otvet = new List<char>();
            List<char> mas_input = new List<char>();
            while (Bull != 4)
            {
                Bull = 0;
                Cow = 0;
                mas_input.Clear();
                mas_otvet.Clear();
                string input = Durak();
                mas_otvet.Add(otvet[0]);
                mas_otvet.Add(otvet[1]);
                mas_otvet.Add(otvet[2]);
                mas_otvet.Add(otvet[3]);
                mas_input.Add(input[0]);
                mas_input.Add(input[1]);
                mas_input.Add(input[2]);
                mas_input.Add(input[3]);
                for (int i = 0; i < 4; i++)
                {
                    if (mas_otvet.Contains(mas_input[i]))
                    {
                        if (mas_otvet[i] == mas_input[i])
                        {
                            Bull++;
                        }
                        else
                        {
                            Cow++;
                        }
                    }
                }
                step++;
                Console.WriteLine($"Попытка #{step} - B{Bull}, C{Cow}");
            }
        }
        static void Win()
        {
            int k = 0;
            using (StreamWriter writetext = new StreamWriter("winlog.txt", true))
            {
                writetext.WriteLine(step);
            }
            string[] lines = File.ReadAllLines("winlog.txt");
            Array.Sort(lines);
            for (int i = 0; i < lines.Length; i++)
            {
                if (step <= int.Parse(lines[i]))
                {
                    k++;
                }
            }
            double percentage = (double)k / lines.Length * 100;
            Console.WriteLine($"Поздравляю, ваш результат лучше, чем у {Math.Round(percentage, 1)}% пользователей!");
        }
        static void WriteDictToFile(Dictionary<string, string> Dict)
        {
            using (StreamWriter fileWriter = new StreamWriter("Dictionary.txt", true))
            {
                foreach (KeyValuePair<string, string> kvPair in Dict)
                {
                    fileWriter.WriteLine("{0}:{1}", kvPair.Key, kvPair.Value);
                }
                fileWriter.Close();
            }            
        }
        static Dictionary<string, string> ReadDictFromFile()
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            if (File.Exists("Dictionary.txt"))
            {
                string[] lines = File.ReadAllLines("Dictionary.txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    var spt = lines[i].Split(':');
                    Dict.Add(spt[0], spt[1]);
                }
            }
            return Dict;
        }
    }
}
