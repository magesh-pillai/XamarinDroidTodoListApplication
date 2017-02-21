namespace XamarinDroidTodoListApplication.Data
{
    using System;
    using Android.Database.Sqlite;
    using Android.Content;

    public class TaskDbHelper : SQLiteOpenHelper
    {
        private const string DATABASE_NAME = "taskDb.db";

        private const int VERSION = 1;

        public TaskDbHelper(Context context) 
            : base(context, DATABASE_NAME, null, VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            const string CREATE_TABLE = 
                "CREATE TABLE " + TaskContract.TaskEntry.TABLE_NAME + " (" +
                                TaskContract.TaskEntry.ID + " INTEGER PRIMARY KEY, " +
                                TaskContract.TaskEntry.COLUMN_DESCRIPTION + " TEXT NOT NULL, " +
                                TaskContract.TaskEntry.COLUMN_PRIORITY + " INTEGER NOT NULL);";

            db.ExecSQL(CREATE_TABLE);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TaskContract.TaskEntry.TABLE_NAME);
            OnCreate(db);
        }
    }
}