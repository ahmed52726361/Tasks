using businessLayerOfTasks;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Tasks
{
    public partial class Form1 : Form
    {
        private readonly ITaskServices _taskServices;

        public Form1(ITaskServices service)
        {
            InitializeComponent();
            _taskServices = service;
        }

        private void RefreshDashboard()
        {
            var tasks = _taskServices.GetAllTasks();

            dataGridView1.DataSource = null;
            if (tasks != null && tasks.Count != 0)
            {
                dataGridView1.DataSource = tasks;
            }

            var progressPercent = _taskServices.GetTotalProgress() * 100;
            var progressValue = Math.Max(0, Math.Min(100, (int)progressPercent));
            lblProgress.Text = $"{progressPercent:0.##}%";
            prgBar.Minimum = 0;
            prgBar.Maximum = 100;
            prgBar.Value = progressValue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            using (var addTaskForm = new frmAddTask())
            {
                addTaskForm.TaskSaved += taskTitle =>
                {
                    _taskServices.AddTask(new clsTask(taskTitle));
                    RefreshDashboard();
                };

                addTaskForm.ShowDialog();
            }
        }

        private void btnCompleteTask_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(row => row.DataBoundItem as clsTask)
                .Where(task => task != null)
                .ToList()
                .ForEach(task => _taskServices.GetTaskDone(task));

            RefreshDashboard();
        }
    }
}
