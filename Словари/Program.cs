using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Словари
{
    internal class Program
    {
        static bool ContinueCondition(string message)
        {
            while(true)
            {
                Console.Write($"\n {message} (Y/N): ");
                ConsoleKeyInfo subChoice = Console.ReadKey();

                switch (subChoice.KeyChar)
                {
                    case 'Y':
                        Console.Clear();
                        return true;
                    case 'N':
                        Console.Clear();
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine($"\n Введенное значение {subChoice.KeyChar} некорректно, повторите ввод!");
                        break;
                }
            }
        }
        static bool CreateEmptyVocabulary(string vocType)
        {
            if (vocType.Contains("-"))
            {
                Console.Clear();
                if (File.Exists(vocType + ".txt"))
                {
                    while (true)
                    {
                        Console.Write("\n Такой тип словаря уже существует, желаете выбрать его для работы? (Y/N): ");
                        ConsoleKeyInfo subChoice = Console.ReadKey();

                        switch (subChoice.KeyChar)
                        {
                            case 'Y':
                                Console.Clear();
                                Console.WriteLine($"\n Для работы был выбран словарь \"{vocType}\"!");
                                return true;
                            case 'N':
                                Console.Clear();
                                Console.WriteLine("\n Введите корректно тип нового словаря в поле ниже!");
                                return false;
                            default:
                                Console.Clear();
                                Console.WriteLine($"\n Введенное значение \"{subChoice.KeyChar}\" некорректно, повторите ввод!");
                                break;
                        }
                    }
                }
                Console.Clear();
                File.Create(vocType + ".txt").Close();

                Console.WriteLine($"\n Словарь \"{vocType}\", был создан!");
                return true;
            }
            Console.Clear();
            Console.WriteLine("\n Введенный тип не соответствует шаблону, посмотрите пример и повторите ввод!");
            return false;
        }
        static bool ChoiceExistingVocabulary(ref string vocType)
        {
            if (vocType.Contains("-"))
            {
                Console.Clear();
                if (File.Exists(vocType + ".txt"))
                {
                    Console.WriteLine($"\n Выбран словарь \"{vocType}\"!");
                    return true;
                }

                Console.WriteLine("\n Словарь не найден!");
                while (true)
                {
                    Console.Write($" Желаете создать словарь с типом \"{vocType}\"? (Y/N): ");
                    ConsoleKeyInfo subChoice = Console.ReadKey();

                    switch (subChoice.KeyChar)
                    {
                        case 'Y':
                            Console.Clear();
                            CreateEmptyVocabulary(vocType);
                            Console.WriteLine($"\n Выбран словарь \"{vocType}\"!");
                            return true;
                        case 'N':
                            Console.Clear();
                            vocType = null;
                            return false;
                        default:
                            Console.Clear();
                            Console.WriteLine($"\n Введенное значение {subChoice.KeyChar} некорректо, повторите ввод!");
                            break;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("\n Введенный тип не соответствует шаблону, посмотрите пример и повторите ввод!");
            return false;
        }
        static bool AddingWords(ref Dictionary<string, string> voc)
        {
            Console.Write("\n Введите слово для добавления в словарь: ");
            string word = Console.ReadLine();
            string translation;

            if (voc.ContainsKey(word))
            {
                Console.Clear();
                Console.WriteLine("\n Слово уже есть в словаре!");
                if (ContinueCondition("Желаете добавить перевод?"))
                {
                    Console.Clear();
                    Console.Write("\n Введите перевод: ");
                    translation = Console.ReadLine();

                    if (!voc[word].Contains(translation))
                    {
                        Console.Clear();
                        voc[word] += ", " + translation;
                        Console.WriteLine($"\n Слово \"{word}\" теперь имеет несколько переводов: {voc[word]}!");
                        return true;
                    }
                    return !ContinueCondition("Желаете повторить ввод?");
                }
                return false;
            }
            Console.Clear();
            Console.Write($"\n Введите перевод для слова \"{word}\": ");
            translation = Console.ReadLine();
            voc[word] = translation;

            Console.Clear();
            Console.WriteLine($"\n Слово \"{word}\" теперь имеет перевод \"{voc[word]}\"!");
            return true;
        }
        static bool EditWords(ref Dictionary<string, string> voc, bool deleting)
        {
            while (true)
            {
                Console.WriteLine($"\n Выберите что нужно {(deleting ? "удалить" : "изменить")}:");
                Console.WriteLine("  1. Слово;");
                Console.WriteLine("  2. Перевод;");
                Console.WriteLine("  0. Вернуться в главное меню;");

                Console.Write("\n Поле для ввода: ");
                ConsoleKeyInfo subChoice = Console.ReadKey();
                Console.Clear();

                Console.WriteLine($"\n Содержание словаря:");
                foreach(KeyValuePair<string, string> kvp in voc)
                {
                    Console.Write($"\n {kvp.Key} - {kvp.Value}");
                }
                Console.Write("\n\n Введите слово: ");
                string searchWord = Console.ReadLine();
                if (voc.ContainsKey(searchWord))
                {
                    switch (subChoice.KeyChar)
                    {
                        case '1':
                            Console.Clear();
                            if (deleting)
                            {
                                voc.Remove(searchWord);
                                Console.WriteLine($"\n Слово \"{searchWord}\", было удалено вместе с его значениями!");
                                return true;
                            }
                            Console.Write("\n Введите новое значение слова: ");
                            string newWord = Console.ReadLine();
                            string oldValue = voc[searchWord];

                            voc.Remove(searchWord);
                            voc.Add(newWord, oldValue);
                            Console.WriteLine($"\n Слово \"{searchWord}\", было изменено на \"{newWord}\"!");
                            return true;
                        case '2':
                            Console.Clear();
                            Console.WriteLine($"\n Выбрано слово \"{searchWord}\": {voc[searchWord]}");
                            Console.Write($"\n Введите перевод для {(deleting ? "удаления" : "изменения")}: ");
                            string translate = Console.ReadLine();

                            if (voc[searchWord].Contains(translate))
                            {
                                Console.Clear();
                                if(deleting)
                                {
                                    if (voc[searchWord].Contains(","))
                                    {
                                        voc[searchWord] = voc[searchWord].Replace(", " + translate, "").Replace(translate, "").Trim(new char[] { ',', ' ' });
                                        Console.WriteLine("\n Перевод был удален!");
                                        return true;
                                    }

                                    Console.WriteLine("\n Невозможно удалить перевод если он последний для данного слова!");
                                    return false;
                                }

                                Console.Write("\n Введите новый перевод: ");
                                string newTranslate = Console.ReadLine();
                                voc[searchWord] = voc[searchWord].Replace(translate, newTranslate);
                                Console.WriteLine("\n Перевод был изменен!");
                                return true;
                            }
                            Console.WriteLine("\n Перевод не найден!");
                            return false;
                        case '0':
                            Console.Clear();
                            Console.WriteLine("\n Возвращение в главное меню...");
                            return true;
                        default:
                            Console.Clear();
                            Console.WriteLine($"\n Введенное значение {subChoice.KeyChar}, некорректно повторите ввод!");
                            break;
                    }
                }

                Console.WriteLine("\n Введенное слово отсутствует в словаре!");
                return false;
            }
        }
        static void SaveVocabulary(Dictionary<string, string> voc, string fileName)
        {
            using (FileStream fs = new FileStream(fileName + ".txt", FileMode.Truncate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (KeyValuePair<string, string> pair in voc)
                    {
                        sw.Write($"{pair.Key}:{pair.Value}\n");
                    }
                }
            }
        }
        static void LoadVocabulary(ref Dictionary<string,string> voc, ref string fileName)
        {
            using(FileStream fs = new FileStream(fileName + ".txt",FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string[] temp = sr.ReadToEnd().Split(':', '\n', '\r');

                    for (int i = 0; i + 2 < temp.Length; i += 2)
                    {
                        voc.Add(temp[i], temp[i + 1]);
                    }
                }
            }
        }
        static void DisplayVocabulary(Dictionary<string, string> voc, string vocType)
        {
            Console.WriteLine($"\n Словарь \"{vocType}\":\n");
            foreach (KeyValuePair<string, string> pair in voc)
            {
                Console.WriteLine($" {pair.Key} - {pair.Value}");
            }
        }

        static void Main(string[] args)
        {
            int attemps = 0;
            string vocabularyType = null;
            ConsoleKeyInfo mainMenuChoice;
            Dictionary<string, string> vocabulary = new Dictionary<string, string>();

            Console.Write("--------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n Немного правил во время работы со словарем:");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("  1. Одно слово может иметь несколько значений;");
            Console.WriteLine("  2. При удалении слова удаляются и все его переводы;");
            Console.WriteLine("  3. Одно и то же самое слово не может быть добавлено в словарь несколько раз;");
            Console.WriteLine("  4. При создании нового словаря для его корректной работы вводите его тип в формате: язык-язык;");
            Console.WriteLine("  5. После создания словаря не забудьте его выбрать;");
            Console.WriteLine("  6. ВАЖНО! Для того чтобы все данные сохранились успешно после завершения работы, не закрывайте окно с");
            Console.WriteLine("     приложением, для завершения работы выберите соответствующий пункт в главном меню!");
            Console.Write("--------------------------------------------------------------------------------------------------------\n\n");

            while (true)
            {
                Console.WriteLine($"\n Доступные действия {(vocabularyType != null ? $"для словаря \"{vocabularyType}\" " : "")}\b:\n");
                Console.WriteLine("  1. Создание пустого словаря;");
                Console.WriteLine("  2. Добавление слов и переводов;");
                Console.WriteLine("  3. Удаление слов или переводов;");
                Console.WriteLine("  4. Изменение слов или переводов;");
                Console.WriteLine("  5. Выбор одного из существующих словарей;");
                Console.WriteLine("  6. Сохранение отдельных слов и их переводов;");
                Console.WriteLine("  7. Вывести на экран словарь;");
                Console.WriteLine("  8. Поиск слов по выбраному словарю;");
                Console.WriteLine("  0. Завершение работы;");

                Console.Write("\n Поле для ввода: ");
                mainMenuChoice = Console.ReadKey();

                switch (mainMenuChoice.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Console.Write("\n Введите тип словаря (Например: англо-русский): ");
                        string newVocabularyType = Console.ReadLine();

                        CreateEmptyVocabulary(newVocabularyType);
                        break;
                    case '2':
                        Console.Clear();
                        if(vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }

                        while(!AddingWords(ref vocabulary))
                        {
                            if(++attemps % 3 == 0)
                            {
                                if (!ContinueCondition("Желаете продолжить ввод?")) break;
                            }
                        }
                        break;
                    case '3':
                        Console.Clear();
                        if(vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }
                        else if(vocabulary.Count == 0)
                        {
                            Console.WriteLine("\n Словарь пуст, нету слов для удаления!");
                            break;
                        }

                        EditWords(ref vocabulary, true);
                        SaveVocabulary(vocabulary, vocabularyType);
                        break;
                    case '4':
                        Console.Clear();
                        if (vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }
                        else if (vocabulary.Count == 0)
                        {
                            Console.WriteLine("\n Словарь пуст, нету слов для изменения!");
                            break;
                        }

                        EditWords(ref vocabulary, false);
                        SaveVocabulary(vocabulary, vocabularyType);
                        break;
                    case '5':
                        Console.Clear();
                        if (vocabulary.Count > 0)
                        {
                            SaveVocabulary(vocabulary, vocabularyType);
                        }
                        Console.WriteLine("\n Доступные словари:\n");
                        foreach(FileInfo file in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.txt"))
                        {
                            Console.WriteLine(" " + file.Name.TrimEnd(new char[] { '.', 't', 'x' }));
                        }
                        
                        Console.Write("\n Введите тип словаря с которым желаете работать: ");
                        vocabularyType = Console.ReadLine();

                        if (ChoiceExistingVocabulary(ref vocabularyType))
                        {
                            vocabulary.Clear();
                            LoadVocabulary(ref vocabulary, ref vocabularyType);
                            break;
                        }

                        vocabularyType = null;
                        break;
                    case '6':
                        Console.Clear();
                        if (vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }
                        else if (vocabulary.Count == 0)
                        {
                            Console.WriteLine("\n Словарь пуст, нету слов для сохранения!");
                            break;
                        }

                        DisplayVocabulary(vocabulary, vocabularyType);
                        Console.Write("\n Введите слово которое желаете сохранить в отдельный файл: ");
                        string searchKey = Console.ReadLine();
                        Console.Clear();

                        if (vocabulary.ContainsKey(searchKey))
                        {
                            using (FileStream fs = new FileStream(@"C:\Users\Chaklun\source\repos\Словари\SavedWords.txt", FileMode.Append))
                            {
                                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    sw.WriteLine($"{searchKey}:{vocabulary[searchKey]}");
                                }
                            }
                            Console.WriteLine($"\n Слово {searchKey} было записано в файл \"SavedWords.txt\" вместе с его переводом!");
                            break;
                        }

                        Console.WriteLine($"\n Введенное слово \"{searchKey}\" не найдено!");
                        break;
                    case '7':
                        Console.Clear();
                        if(vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }
                        else if(vocabulary.Count == 0)
                        {
                            Console.WriteLine("\n Словарь пуст!");
                            break;
                        }

                        DisplayVocabulary(vocabulary, vocabularyType);
                        break;
                    case '8':
                        Console.Clear();
                        if (vocabularyType == null)
                        {
                            Console.WriteLine("\n Не выбран словарь для работы!");
                            break;
                        }
                        else if (vocabulary.Count == 0)
                        {
                            Console.WriteLine("\n Словарь пуст, нету слов для поиска!");
                            break;
                        }

                        Console.Write($"\n Введите слово для поиска по словарю \"{vocabularyType}\": ");
                        searchKey = Console.ReadLine();
                        Console.Clear();

                        if(vocabulary.ContainsKey(searchKey))
                        {
                            Console.WriteLine($"\n Слово \"{searchKey}\" имеет следующий перевод(-ы): {vocabulary[searchKey]}");
                            break;
                        }

                        Console.WriteLine($"\n Введенное слово \"{searchKey}\" не найдено в словаре!");
                        break;
                    case '0':
                        Console.Clear();
                        SaveVocabulary(vocabulary, vocabularyType);

                        Console.WriteLine("\n Завершение работы...\n\n");
                        Console.WriteLine("\n Для закрытия консоли нажмите любую клавишу . . .");
                        Console.ReadKey();
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine($"\n Введенное значение \"{mainMenuChoice.KeyChar}\" некорректно, повторите ввод!");
                        break;
                }
            }
        }
    }
}