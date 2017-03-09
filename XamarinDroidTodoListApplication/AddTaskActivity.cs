namespace XamarinDroidTodoListApplication
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Support.V7.App;
    using Android.Views;
    using Android.Widget;
    using Data;
    using Java.Interop;

    [Activity(Label = "AddTaskActivity")]
    public class AddTaskActivity : AppCompatActivity
    {
        // The task's selected priority
        private int priority;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.AddTask);

            // Initialize to highest priority by default 
            this.FindRadioButton(Resource.Id.radButton1).Checked = true;
            this.priority = 1;
        }

        [Export("OnClickAddTask")]
        public void OnClickAddTask(View view)
        {
            string input = this.FindViewById<EditText>(Resource.Id.editTextTaskDescription).Text;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            ContentValues contentValues = new ContentValues();
            contentValues.Put(TaskContract.TaskEntry.COLUMN_DESCRIPTION, input);
            contentValues.Put(TaskContract.TaskEntry.COLUMN_PRIORITY, this.priority);

            Android.Net.Uri uri = this.ContentResolver.Insert(TaskContract.TaskEntry.CONTENT_URI, contentValues);

            if (uri != null)
            {
                Toast.MakeText(this.BaseContext, uri.ToString(), ToastLength.Long).Show();
            }

            Finish();
        } 

        [Export("OnPrioritySelected")]
        public void OnPrioritySelected(View view)
        {
            if (this.FindRadioButton(Resource.Id.radButton1).Checked)
            { 
                this.priority = 1;
            }
            else if (this.FindRadioButton(Resource.Id.radButton2).Checked)
            {
                this.priority = 2;
            }
            else if (this.FindRadioButton(Resource.Id.radButton3).Checked)
            {
                this.priority = 3;
            }
        }

        private RadioButton FindRadioButton(int id)
        {
            return this.FindViewById<RadioButton>(id);
        }
    }
}
