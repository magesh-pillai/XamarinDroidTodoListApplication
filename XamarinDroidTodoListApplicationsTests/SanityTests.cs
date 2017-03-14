namespace XamarinDroidTodoListApplicationsTests
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    [TestFixture]
    public class SanityTests
    {
        private IApp app;

        [SetUp]
        public void Setup()
        {
            app = ConfigureApp.Android.ApkFile("../../../XamarinDroidTodoListApplication/bin/Debug/XamarinDroidTodoListApplication.XamarinDroidTodoListApplication.apk").StartApp();
        }

        //[Test]
        //public void AddingOneTask_Success()
        //{
        //    app.Repl();
        //}

        [Test]
        public void Add2TasksThenDeleteFirst_SecondShouldRemain_Success()
        {
            app.WaitForElement(x => x.Marked("content"), "Timed out waiting for starting screen to appear", TimeSpan.FromSeconds(3000));
            app.Tap(x => x.Id("fab"));
            app.EnterText(x => x.Id("editTextTaskDescription"), "task1");
            app.Tap(x => x.Id("radButton2"));
            app.Tap(x => x.Id("addButton"));
            app.Tap(x => x.Id("fab"));
            app.EnterText(x => x.Id("editTextTaskDescription"), "task2");
            app.Tap(x => x.Id("addButton"));
            app.SwipeLeftToRight();
            app.Screenshot("Swiped right");
            AppResult[] result = app.Query(x => x.Marked("content").Descendant().Text("task2"));
            Assert.IsTrue(result.Any(), "task2 was found.");
        }
    }
}
