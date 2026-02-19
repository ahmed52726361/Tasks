using System;
using System.Windows.Forms;

namespace Tasks
{
    public partial class frmAddTask : Form
    {
        public delegate void TaskSavedHandler(string taskTitle);
        public event TaskSavedHandler TaskSaved;

        public frmAddTask()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var taskDescription = txtdescription.Text?.Trim();

            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                MessageBox.Show("Task is empty, you should input a task");
                return;
            }

            TaskSaved?.Invoke(taskDescription);
            DialogResult = DialogResult.OK;
        }
    }
}
