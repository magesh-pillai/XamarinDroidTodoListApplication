namespace XamarinDroidTodoListApplication.Data
{
    using System;
    using Android.Content;
    using Android.Database;
    using AndroidNet = Android.Net;
    using Android.Database.Sqlite;

    [ContentProvider(new[] { "com.example.android.todolist" }, Exported = false)]
    public class TaskContentProvider : ContentProvider
    {
        public const int TASKS = 100;
        public const int TASK_WITH_ID = 101;


        private TaskDbHelper taskDbHelper;

        private static UriMatcher sUriMatcher;

        static TaskContentProvider()
        {
            sUriMatcher = buildUriMatcher();
        }

        private static UriMatcher buildUriMatcher()
        {
            UriMatcher uriMatcher = new UriMatcher(UriMatcher.NoMatch);
            uriMatcher.AddURI(TaskContract.AUTHORITY, TaskContract.PATH_TASKS, TASKS);
            uriMatcher.AddURI(TaskContract.AUTHORITY, TaskContract.PATH_TASKS + "/#", TASKS);
            return uriMatcher;
        }

        public override int Delete(AndroidNet.Uri uri, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }

        public override string GetType(AndroidNet.Uri uri)
        {
            throw new NotImplementedException();
        }

        public override AndroidNet.Uri Insert(AndroidNet.Uri uri, ContentValues values)
        {
            SQLiteDatabase db = taskDbHelper.WritableDatabase;

            int match = sUriMatcher.Match(uri);

            AndroidNet.Uri returnUri;

            switch(match)
            {
                case TASKS:
                    {
                        long id = db.Insert(TaskContract.TaskEntry.TABLE_NAME, null, values);
                        if (id > 0)
                        {
                            returnUri = ContentUris.WithAppendedId(TaskContract.TaskEntry.CONTENT_URI, id);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unknown uri: " + uri);
                        }
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown uri: " + uri);
                    }
            }

            // Notify the resolver if the uri has been changed
            this.Context.ContentResolver.NotifyChange(uri, null);

            return returnUri;
        }

        public override bool OnCreate()
        {
            taskDbHelper = new TaskDbHelper(this.Context);
            return true;
        }

        public override ICursor Query(AndroidNet.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public override int Update(AndroidNet.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }
    }
}