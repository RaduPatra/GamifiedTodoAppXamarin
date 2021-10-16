using System;

namespace XamarinTodoApp.Models
{
    public class TodoItem : IDataBaseItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public int Reward { get; set; }

        //creation date
    }
}
