using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ConsoleToDoApp
{
    public class TodoApp
    {
        // エラー番号
        public const int ERR_MENU = 1;
        public const int ERR_TASK_NUMBER = 2;
        public const int ERR_TASK_NOT_FOUND = 3;

        // タスクを保存するファイル名
        private const string TASK_FILE = "tasks.txt";

        private List<TaskItem> taskList = new();

        /*
         * タスクを追加する
         */
        public void AddTask()
        {
            Console.WriteLine("タスク名を入力してください");
            Console.Write("> ");
            string? newTaskName = Console.ReadLine();
            if (String.IsNullOrEmpty(newTaskName))
            {
                Console.WriteLine("error: タスク名の追加に失敗しました");
            }
            else
            {
                // タスクを追加
                DateTime inputDueData = DateTime.Now;
                if (InputDueDate(ref inputDueData))
                {
                    taskList.Add(new TaskItem
                    {
                        Name = newTaskName,
                        CreatedAt = DateTime.Now,
                        DueDate = inputDueData
                    });
                }
                else
                {
                    taskList.Add(new TaskItem
                    {
                        Name = newTaskName,
                        CreatedAt = DateTime.Now
                    });
                }
            }
        }

        /*
         * タスクの締切日時を入力させる
         * ref DateTime outDueDate: 入力された日時を返す。
         * return: 締切日時が入力された場合はtrue、入力されなければfalseを返す。
         */
        public bool InputDueDate(ref DateTime outDueDate)
        {
            string format = "yyyy/MM/dd HH:mm";
            while (true)
            {
                Console.WriteLine("締切日時を入力しますか？(y/n)");
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
                {
                    Console.WriteLine("無効な入力です。'y' または 'n' を入力してください");
                    Console.WriteLine();
                    continue;
                }
                input = input.Trim().ToLower(); // 入力のスペースを取り除き小文字に変換
                if (input == "y" || input == "yes")
                {
                    // 締切日時を追加する
                    Console.WriteLine("締切日時を入力してください(yyyy/MM/dd HH:mm)");
                    Console.Write("> ");
                    // 指定したフォーマットでのみ受け付ける
                    input = Console.ReadLine();
                    if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDueDate))
                    {
                        break;
                    }
                    Console.WriteLine("入力形式が正しくありません");
                    Console.WriteLine();
                }
                else if (input == "n" || input == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("無効な入力です。'y' または 'n' を入力してください。");
                    Console.WriteLine();
                }
            }
            return true;
        }

        /*
         * タスク一覧を表示する
         * bool sortCompleted: falseならば順番通りに表示。
         *                     trueならば完了済みタスクを後ろくるように並べ替えて表示。
         * return: タスクがあればtrue、なければfalseを返す。
         */
        public bool ShowTasks(bool sortCompleted = false)
        {
            bool hasTask = taskList.Count > 0;
            int index = 0;
            if (hasTask)
            {
                IEnumerable<TaskItem> tasks = taskList;
                Console.WriteLine("--- タスク一覧 ----------");
                if (sortCompleted)
                {
                    tasks = taskList.OrderBy(task => task.IsCompleted);
                }
                foreach (var task in tasks)
                {
                    ++index;
                    string completedMark = completedMark = task.IsCompleted ? "[x]" : "[ ]";
                    Console.WriteLine($"{index}. {completedMark} {task.Name}");
                    Console.WriteLine($"作成日時: {task.CreatedAt:yyyy/MM/dd HH:mm}");
                    Console.WriteLine($"締切日時: {(task.DueDate == null ? "なし" : task.DueDate.Value.ToString("yyyy/MM/dd HH:mm"))}");
                    Console.WriteLine();
                }
                Console.WriteLine("-------------------------\r\n");
            }
            else
            {
                Console.WriteLine("タスクが登録されていません\r\n");
            }
            return hasTask;
        }

        /*
         * タスクを削除する
         */
        public void DeleteTask()
        {
            if (!ShowTasks())
            {
                return;
            }
            Console.WriteLine("削除するタスクの番号を入力してください");
            Console.Write("> ");
            if (int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                if (inputNumber >= 1 && inputNumber <= taskList.Count)
                {
                    string delTaskName = taskList[inputNumber - 1].Name;
                    taskList.RemoveAt(inputNumber - 1);
                    Console.WriteLine(delTaskName + "を削除しました\r\n");
                }
                else
                {
                    ShowErrMsg(ERR_TASK_NOT_FOUND);
                }
            }
            else
            {
                ShowErrMsg(ERR_TASK_NUMBER);
            }
        }

        /*
         * タスクを完了する
         */
        public void CompletedTask()
        {
            if (!ShowTasks())
            {
                return;
            }
            Console.WriteLine("完了するタスクの番号を入力してください");
            Console.Write("> ");
            if (int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                if (inputNumber >= 1 && inputNumber <= taskList.Count)
                {
                    string delTaskName = taskList[inputNumber - 1].Name;
                    taskList[inputNumber - 1].IsCompleted = true;
                    Console.WriteLine(delTaskName + "を完了しました\r\n");
                }
                else
                {
                    ShowErrMsg(ERR_TASK_NOT_FOUND);
                }
            }
            else
            {
                ShowErrMsg(ERR_TASK_NUMBER);
            }
        }

        /*
         * タスクを編集する
         */
        public void EditTask()
        {
            if (!ShowTasks())
            {
                return;
            }
            Console.WriteLine("編集するタスクの番号を入力してください");
            Console.Write("> ");
            if (int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                if (inputNumber >= 1 && inputNumber <= taskList.Count)
                {
                    Console.WriteLine("新しいタスク名を入力してください");
                    Console.Write("> ");
                    string? newTaskName = Console.ReadLine();
                    if (String.IsNullOrEmpty(newTaskName))
                    {
                        Console.WriteLine("error: タスク名の追加に失敗しました");
                    }
                    else
                    {
                        taskList[inputNumber - 1].Name = newTaskName;
                        Console.WriteLine("タスク名を " + newTaskName + " に変更しました\r\n");
                    }
                }
                else
                {
                    ShowErrMsg(ERR_TASK_NOT_FOUND);
                }
            }
            else
            {
                ShowErrMsg(ERR_TASK_NUMBER);
            }
        }

        /*
         * タスクの状態を外部ファイルに保存する
         */
        public void SaveTasks()
        {
            try
            {
                string json = JsonSerializer.Serialize(taskList);
                File.WriteAllText(TASK_FILE, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("タスクファイルの保存に失敗しました。");
                Console.WriteLine(ex.Message);
            }
        }

        /*
         * ファイルからタスクを読み込む
         */
        public void LoadTasks()
        {
            try
            {
                // タスクを保存したファイルがあれば読み込む
                if (File.Exists(TASK_FILE))
                {
                    string json = File.ReadAllText(TASK_FILE);
                    taskList = JsonSerializer.Deserialize<List<TaskItem>>(json)
                        ?? new List<TaskItem>();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("タスクファイルの読み込みに失敗しました。");
                Console.WriteLine(ex.Message);
            }
        }

        /*
         * エラーメッセージ表示
         */
        public static void ShowErrMsg(int errcode)
        {
            switch (errcode)
            {
                case ERR_MENU: Console.WriteLine("1～5 の数字を入力してください\r\n"); break;
                case ERR_TASK_NUMBER: Console.WriteLine("タスクの番号を入力してください\r\n"); break;
                case ERR_TASK_NOT_FOUND: Console.WriteLine("入力された番号のタスクがありません\r\n"); break;
                default: break;
            }
        }
    }
}
