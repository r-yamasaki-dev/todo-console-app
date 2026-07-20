using ConsoleToDoApp;

class Program
{
    // メニュー番号
    const int MENU_ADD = 1;
    const int MENU_SHOW = 2;
    const int MENU_REMOVE = 3;
    const int MENU_COMPLETED = 4;
    const int MENU_EDIT = 5;
    const int MENU_EXIT = 6;

    /*
     * メニューを表示する
     */
    private static void ShowMenu()
    {
        Console.WriteLine("★ ToDoアプリ ★");
        Console.WriteLine("1. タスク追加");
        Console.WriteLine("2. タスク一覧");
        Console.WriteLine("3. タスク削除");
        Console.WriteLine("4. タスク完了");
        Console.WriteLine("5. タスク編集");
        Console.WriteLine("6. 終了");
        Console.Write("> ");
    }

    static void Main(string[] args)
    {
        TodoApp todoApp = new TodoApp();
        int menuInput = 0;

        // タスクを保存したファイルがあれば読み込む
        todoApp.LoadTasks();

        // メイン部分
        while (menuInput != MENU_EXIT)
        {
            //メニュー表示
            ShowMenu();
            if (int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                menuInput = inputNumber;
                switch (menuInput)
                {
                    //タスク追加
                    case MENU_ADD: todoApp.AddTask(); break;
                    //タスク一覧
                    case MENU_SHOW: todoApp.ShowTasks(true); break;
                    //タスク削除
                    case MENU_REMOVE: todoApp.DeleteTask(); break;
                    //タスク完了
                    case MENU_COMPLETED: todoApp.CompletedTask(); break;
                    //タスク編集
                    case MENU_EDIT: todoApp.EditTask(); break;
                    //終了
                    case MENU_EXIT: break;
                    //該当なし
                    default: TodoApp.ShowErrMsg(TodoApp.ERR_MENU); break;
                }
            }
            else
            {
                TodoApp.ShowErrMsg(TodoApp.ERR_MENU);
            }
        }

        // タスクリストを保存する。
        todoApp.SaveTasks();
    }
}
