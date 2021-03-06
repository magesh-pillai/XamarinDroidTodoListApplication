﻿namespace XamarinDroidTodoListApplication
{
    using Android.App;
    using Android.Content;
    using Android.Database;
    using Android.OS;
    using Android.Support.Design.Widget;
    using Android.Support.V7.App;
    using Android.Support.V7.Widget;
    using Android.Support.V7.Widget.Helper;
    using Android.Util;
    using Data;
    using System;

    [Activity(Label = "XamarinDroidTodoListApplication", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity, LoaderManager.ILoaderCallbacks
    {
        private CustomCursorAdapter adapter;
        private RecyclerView recyclerView;

        internal static int TASK_LOADER_ID
        {
            get
            {
                return 0;
            }
        }

        internal static string TAG
        {
            get
            {
                return typeof(MainActivity).Name;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.SetContentView(Resource.Layout.Main);

            // Get the recyclerview
            this.recyclerView = this.FindViewById<RecyclerView>(Resource.Id.recyclerViewTasks);

            // Set the layout manager of the recycler view
            this.recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            // Attach adapter for recycler view
            this.adapter = new CustomCursorAdapter(this);
            this.recyclerView.SetAdapter(this.adapter);

            // Add a touch helper to the recycler view for user swipe deletion
            (new ItemTouchHelper(new TouchHelperImpl(this))).AttachToRecyclerView(this.recyclerView);

            // Add listener to the floating action button
            FloatingActionButton fltButton = this.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fltButton.Click += (object sender, EventArgs e) =>
            {
                // Create a new intent to start an AddTaskActivity
                Intent addTaskIntent = new Intent(this, typeof(AddTaskActivity));
                this.StartActivity(addTaskIntent);
            };

            this.LoaderManager.InitLoader(TASK_LOADER_ID, null, this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.LoaderManager.RestartLoader(TASK_LOADER_ID, null, this);
        }

        public Loader OnCreateLoader(int id, Bundle args)
        {
            return new TaskLoaderAsync(this);
        }

        public void OnLoaderReset(Loader loader)
        {
            this.adapter.SwapCursor(null);
        }

        public void OnLoadFinished(Loader loader, Java.Lang.Object data)
        {
            this.adapter.SwapCursor(data as ICursor);
        }

    }

    internal class TouchHelperImpl : ItemTouchHelper.SimpleCallback
    {
        Activity activity;

        internal TouchHelperImpl(Activity actv) : base(0, ItemTouchHelper.Left | ItemTouchHelper.Right)
        {
            this.activity = actv;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            throw new NotImplementedException();
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            long id = (long)viewHolder.ItemView.Tag;

            Android.Net.Uri uri = TaskContract.TaskEntry.CONTENT_URI;
            uri = uri.BuildUpon().AppendPath(id + "").Build();

            this.activity.ContentResolver.Delete(uri, null, null);
            this.activity.LoaderManager.RestartLoader(MainActivity.TASK_LOADER_ID, null, (LoaderManager.ILoaderCallbacks)this.activity);
        }
    }

    internal class TaskLoaderAsync : AsyncTaskLoader
    {
        // The cursor that will hold all the task data
        private ICursor data;

        internal TaskLoaderAsync(Context context) : base(context)
        {
        }

        protected override void OnStartLoading()
        {
            base.OnStartLoading();

            if (this.data != null)
            {
                this.DeliverResult(data);
            }
            else
            {
                this.ForceLoad();
            }
        }

        internal void DeliverResult(ICursor cursor)
        {
            this.data = cursor;
            base.DeliverResult(cursor as Java.Lang.Object);
        }

        public override Java.Lang.Object LoadInBackground()
        {
            try
            {
                return (Java.Lang.Object)this.Context.ContentResolver.Query(TaskContract.TaskEntry.CONTENT_URI,
                    null,
                    null,
                    null,
                    TaskContract.TaskEntry.COLUMN_PRIORITY);
            }
            catch (Exception e)
            {
                Log.Error(MainActivity.TAG, "Failed to asynchronously load data.\n" + e.StackTrace);
                return null;
            }
        }
    }
}

