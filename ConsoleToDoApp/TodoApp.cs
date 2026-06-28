using System;
using System.Collections.Generic;
using System.Text;

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
            string? newTaskName = Console.ReadLine();
            if (String.IsNullOrEmpty(newTaskName))
            {
                Console.WriteLine("error: タスク名の追加に失敗しました");
            }
            else
            {
                taskList.Add(new TaskItem { Name = newTaskName });
            }
        }

        /*
         * タスク一覧を表示する
         * return: タスクがあればtrue、なければfalseを返す。
         */
        public bool ShowTasks()
        {
            bool hasTask = taskList.Count > 0;
            int index = 0;
            if (hasTask)
            {
                Console.WriteLine("--- タスク一覧 ----------");
                foreach (var task in taskList)
                {
                    ++index;
                    Console.WriteLine(index + ". " + task.Name);
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
            bool delFlag = false;
            if (!ShowTasks())
            {
                return;
            }
            Console.WriteLine("削除する番号を入力してください");
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
         * タスクの状態を外部ファイルに保存する
         */
        public void SaveTasks()
        {
            try
            {
                File.WriteAllLines(TASK_FILE, taskList.Select(x => x.Name));
            }
            catch(Exception ex)
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
                    taskList = File.ReadAllLines(TASK_FILE)
                        .Select(x => new TaskItem { Name = x })
                        .ToList();
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
                case ERR_MENU: Console.WriteLine("1～4 の数字を入力してください\r\n"); break;
                case ERR_TASK_NUMBER: Console.WriteLine("タスクの番号を入力してください\r\n"); break;
                case ERR_TASK_NOT_FOUND: Console.WriteLine("入力された番号のタスクがありません\r\n"); break;
                default: break;
            }
        }
    }
}
