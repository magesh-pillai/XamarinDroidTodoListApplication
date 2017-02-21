namespace XamarinDroidTodoListApplication
{
    using System;
    using Android.Support.V7.Widget;
    using Android.Views;
    using Android.Widget;
    using Android.Database;
    using Android.Content;
    using Android.Support.V4.Content;
    using Data;
    using Android.Graphics.Drawables;

    public class CustomCursorAdapter : RecyclerView.Adapter
    {
        private Context context;
        private ICursor cursor;

        public CustomCursorAdapter(Context context)
        {
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return this.cursor == null ? 0 : this.cursor.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // Indices for the _id, description, and priority columns
            int idIndex = this.cursor.GetColumnIndex(TaskContract.TaskEntry.ID);
            int descriptionIndex = this.cursor.GetColumnIndex(TaskContract.TaskEntry.COLUMN_DESCRIPTION);
            int priorityIndex = this.cursor.GetColumnIndex(TaskContract.TaskEntry.COLUMN_PRIORITY);

            this.cursor.MoveToPosition(position); // get to the right location in the cursor

            // Determine the values of the wanted data
            int id = this.cursor.GetInt(idIndex);
            string description = this.cursor.GetString(descriptionIndex);
            int priority = this.cursor.GetInt(priorityIndex);

            // Set values
            TaskViewHolder taskVH = holder as TaskViewHolder;
            taskVH.ItemView.Tag = id;
            taskVH.TaskDescriptionView.Text = description;

            // Programmatically set the text and color for the priority TextView
            string priorityString = "" + priority; // converts int to String
            taskVH.PriorityView.Text = priorityString;

            GradientDrawable priorityCircle = (GradientDrawable)taskVH.PriorityView.Background;
           
            // Get the appropriate background color based on the priority
            int priorityColor = this.GetPriorityColor(priority);
            priorityCircle.SetColor(priorityColor);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(this.context).Inflate(Resource.Layout.Task, parent, false);
            return new TaskViewHolder(view);
        }

        private int GetPriorityColor(int priority)
        {
            int priorityColor = 0;

            switch (priority)
            {
                case 1:
                    {
                        priorityColor = ContextCompat.GetColor(this.context, Resource.Color.materialRed);
                        break;
                    }
                case 2:
                    {
                        priorityColor = ContextCompat.GetColor(this.context, Resource.Color.materialOrange);
                        break;
                    }
                case 3:
                    {
                        priorityColor = ContextCompat.GetColor(this.context, Resource.Color.materialYellow);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return priorityColor;
        }


        // When data changes and a re-query occurs, this function 
        // swaps the old Cursor with a newly updated Cursor (Cursor c) 
        // that is passed in.
        public ICursor SwapCursor(ICursor c)
        {
            // check if this cursor is the same as the previous cursor
            if (this.cursor == c)
            {
                return null; // bc nothing has changed
            }

            ICursor temp = this.cursor;
            this.cursor = c; // new cursor value assigned

            //check if this is a valid cursor, then update the cursor
            if (c != null)
            {
                this.NotifyDataSetChanged();
            }

            return temp;
        }
    }

    public class TaskViewHolder : RecyclerView.ViewHolder
    {
        public TextView TaskDescriptionView { get; set; }
        public TextView PriorityView { get; set; }

        public TaskViewHolder(View itemView) : base(itemView)
        {
            this.TaskDescriptionView = itemView.FindViewById<TextView>(Resource.Id.taskDescription);
            this.PriorityView = itemView.FindViewById<TextView>(Resource.Id.priorityTextView);
        }
    }
}