namespace XamarinDroidTodoListApplicationTests
{
    using System;
    using NUnit.Framework;
    using XamarinDroidTodoListApplication.Data;
    using Android.Database.Sqlite;
    using Android.Content;

    [TestFixture]
    public class TestsSample
    {
        private Context GetContextStub
        {
            get
            {
                return null;
            }
        }

        [SetUp]
        public void Setup()
        {
            /* Use TaskDbHelper to get access to a writable database */
            TaskDbHelper dbHelper = new TaskDbHelper(GetContextStub);
            SQLiteDatabase database = dbHelper.WritableDatabase;
            database.Delete(TaskContract.TaskEntry.TABLE_NAME, null, null);
        }


        [TearDown]
        public void Tear() { }

        [Test]
        public void Pass()
        {
            Console.WriteLine("test1");
            Assert.True(true);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }

        [Test]
        public void Inconclusive()
        {
            Assert.Inconclusive("Inconclusive");
        }
    }
}