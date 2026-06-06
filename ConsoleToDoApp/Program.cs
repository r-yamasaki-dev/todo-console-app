class Program
{
    const int MENU_ADD = 1;
    const int MENU_SHOW = 2;
    const int MENU_REMOVE = 3;
    const int MENU_EXIT = 4;


    static List<string> taskList = new();
    
    /*
     * メニューを表示する
     */
    static void ShowMenu()
    {
        Console.WriteLine("★ ToDoアプリ ★");
        Console.WriteLine("1. タスク追加");
        Console.WriteLine("2. タスク一覧");
        Console.WriteLine("3. タスク削除");
        Console.WriteLine("4. 終了");
    }

    /*
     * タスクを追加する
     */
    static void AddTask()
    {
        Console.WriteLine("タスク名を入力してください");
        string? newTaskName = Console.ReadLine();
        if (String.IsNullOrEmpty(newTaskName))
        {
            Console.WriteLine("error: タスク名の追加に失敗しました");
        }
        else
        {
            taskList.Add(newTaskName);
        }
    }

    /*
     * タスク一覧を表示する
     * return: タスクがあればtrue、なければfalseを返す。
     */
    static bool ShowTasks()
    {
        bool hasTask = taskList.Count > 0;
        int index = 0;
        if (hasTask)
        {
            Console.WriteLine("--- タスク一覧 ----------");
            foreach (var task in taskList)
            {
                ++index;
                Console.WriteLine(index + ". " + task);
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
    static void DeleteTask()
    {
        bool delFlag = false;
        if (!ShowTasks())
        {
            return;
        }
        Console.WriteLine("削除する番号を入力してください");
        if (int.TryParse(Console.ReadLine(), out int inputNumber))
        {
            int count = 0;
            foreach(var task in taskList)
            {
                ++count;
                if (count == inputNumber)
                {
                    string delTaskName = task;
                    taskList.RemoveAt(count-1);
                    delFlag = true;
                    Console.WriteLine(delTaskName + "を削除しました\r\n");
                    break;
                }
            }
            if (!delFlag)
            {
                ShowErrMsg(3);
            }
        }
        else
        {
            ShowErrMsg(2);
        }
    }

    /*
     * エラーメッセージ表示
     */
    static void ShowErrMsg(int errcode)
    {
        switch (errcode)
        {
            case MENU_ADD:    Console.WriteLine("1～3 の数字を入力してください\r\n"); break;
            case MENU_SHOW:   Console.WriteLine("タスクの番号を入力してください\r\n"); break;
            case MENU_REMOVE: Console.WriteLine("入力された番号のタスクがありません\r\n"); break;
            default: break;
        }
    }

    static void Main(string[] args)
    {
        int menuInput = 0;
        while (menuInput != 4)
        {
            //メニュー表示
            ShowMenu();
            if (int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                menuInput = inputNumber;
                switch (menuInput)
                {
                    //タスク追加
                    case 1: AddTask(); break;
                    //タスク一覧
                    case 2: ShowTasks(); break;
                    //タスク削除
                    case 3: DeleteTask(); break;
                    //該当なし
                    default: ShowErrMsg(1); break;
                }
            }
            else
            {
                ShowErrMsg(1);
            }
        }
    }
}
