namespace XamarinDroidTodoListApplication.Data
{
    using System;
    using Android.Content;
    using Android.Database;
    using AndroidNet = Android.Net;

    [ContentProvider(new[] { "com.example.android.todolist" }, Exported = false)]
    public class TaskContentProvider : ContentProvider
    {
        private TaskDbHelper taskDbHelper;

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
            throw new NotImplementedException();
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