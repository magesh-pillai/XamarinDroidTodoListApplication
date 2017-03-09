namespace XamarinDroidTodoListApplication.Data
{
    using System;

    public class TaskContract
    {
        // The authority of the content provider
        public const string AUTHORITY = "com.example.android.todolist";

        // The tasks directory
        public const string PATH_TASKS = "tasks";

        // The base uri for this contract
        public static Android.Net.Uri BASE_CONTENT_URL
        {
            get
            {
                return Android.Net.Uri.Parse("content://" + AUTHORITY);
            }
        }
 
        public static class TaskEntry 
        {
            // Task table and column names
            public const string TABLE_NAME = "tasks";
            public const string ID = "_id";
            public const string COLUMN_DESCRIPTION = "description";
            public const string COLUMN_PRIORITY = "priority";

            // The task base uri
            public static Android.Net.Uri CONTENT_URI
            {
                get
                {
                    return BASE_CONTENT_URL.BuildUpon().AppendPath(TaskContract.PATH_TASKS).Build();
                }
            }
        }
    }
}