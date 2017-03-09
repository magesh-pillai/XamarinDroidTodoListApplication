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
            uriMatcher.AddURI(TaskContract.AUTHORITY, TaskContract.PATH_TASKS + "/#", TASK_WITH_ID);
            return uriMatcher;
        }

        public override int Delete(AndroidNet.Uri uri, string selection, string[] selectionArgs)
        {
            SQLiteDatabase db = this.taskDbHelper.WritableDatabase;

            int match = sUriMatcher.Match(uri);

            int deleted = 0;

            switch (match)
            {
                case TASKS:
                    {
                        // Dangerous
                        deleted = db.Delete(TaskContract.TaskEntry.TABLE_NAME,
                            null,
                            null);
                        break;
                    }
                case TASK_WITH_ID:
                    {
                        // Get the id from the URI
                        string id = uri.PathSegments[1];

                        // Selection is the _ID column = ?
                        // SelectionArgs is the arg values
                        string mDeletion = TaskContract.TaskEntry.ID + " = ?";
                        string[] mDeletionArgs = new string[] { id };

                        deleted = db.Delete(TaskContract.TaskEntry.TABLE_NAME,
                            mDeletion,
                            mDeletionArgs);

                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown uri: " + uri);
                    }
            }

            this.Context.ContentResolver.NotifyChange(uri, null);

            return deleted;
        }

        public override string GetType(AndroidNet.Uri uri)
        {
            int match = sUriMatcher.Match(uri);

            switch (match)
            {
                case TASKS:
                    {
                        // directory
                        return "vnd.android.cursor.dir" + "/" + TaskContract.AUTHORITY + "/" + TaskContract.PATH_TASKS;
                    }
                case TASK_WITH_ID:
                    {
                        // single item type
                        return "vnd.android.cursor.item" + "/" + TaskContract.AUTHORITY + "/" + TaskContract.PATH_TASKS;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown uri: " + uri);
                    }
            }
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
            SQLiteDatabase db = this.taskDbHelper.ReadableDatabase;

            int match = sUriMatcher.Match(uri);

            ICursor retCursor;

            switch (match)
            {
                case TASKS:
                    {
                        retCursor = db.Query(TaskContract.TaskEntry.TABLE_NAME,
                            projection,
                            selection,
                            selectionArgs,
                            null,
                            null,
                            sortOrder);
                        break;
                    }
                case TASK_WITH_ID:
                    {
                        // Get the id from the URI
                        string id = uri.PathSegments[1];

                        // Selection is the _ID column = ?
                        // SelectionArgs is the arg values
                        string mSelection = TaskContract.TaskEntry.ID + " = ?";
                        string[] mSelectionArgs = new string[] { id };

                        retCursor = db.Query(TaskContract.TaskEntry.TABLE_NAME,
                            projection,
                            mSelection,
                            mSelectionArgs,
                            null,
                            null,
                            sortOrder);

                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown uri: " + uri);
                    }
            }

            retCursor.SetNotificationUri(this.Context.ContentResolver, uri);

            return retCursor;
        }

        public override int Update(AndroidNet.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            int tasksUpdated;

            int match = sUriMatcher.Match(uri);

            switch (match)
            {
                case TASK_WITH_ID:
                    {
                        string id = uri.PathSegments[1];
                        tasksUpdated = this.taskDbHelper.WritableDatabase.Update(TaskContract.TaskEntry.TABLE_NAME,
                            values,
                            TaskContract.TaskEntry.ID + " = ?",
                            new string[] { id });
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Unknown uri: " + uri);
                    }
            }

            return tasksUpdated;
        }
    }
}