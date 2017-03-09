namespace XamarinDroidTodoListApplicationTests
{
    using Android.App;
    using Android.Content;
    using Android.Runtime;
    using System;
    using System.Reflection;
    using Xamarin.Android.NUnitLite;

    [Instrumentation(Name = "app.tests.TestInstrumentation")]
    public class TestInstrumentation : TestSuiteInstrumentation
    {
        public TestInstrumentation(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        protected override void AddTests()
        {
            AddTest(Assembly.GetExecutingAssembly());
        }

        public Context GetTargetContext()
        {
            return null;
        }
    }
}