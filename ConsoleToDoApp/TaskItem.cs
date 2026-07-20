using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoApp
{
    public class TaskItem
    {
        public string Name { get; set; } = "";  // タスク名
        public bool IsCompleted { get; set; }   // タスク完了フラグ(true:完了, false:未完了)
        public DateTime CreatedAt { get; set; } // 作成日時
        public DateTime? DueDate {  get; set; } // 締切日時
    }
}
