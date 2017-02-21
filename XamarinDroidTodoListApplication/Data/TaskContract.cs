namespace XamarinDroidTodoListApplication.Data
{
    using Android.Provider;

    public class TaskContract
    {
        public static class TaskEntry 
        {
            // Task table and column names
            public const string TABLE_NAME = "tasks";
            public const string ID = "_id";
            public const string COLUMN_DESCRIPTION = "description";
            public const string COLUMN_PRIORITY = "priority";
        }
    }
}