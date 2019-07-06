using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduleHelper
{
    public partial class FormGenerate : Form
    {
        Gas gas;
        Schedule schedule;
        TeacherSchedule ts;
        private bool loaded;
        public FormGenerate()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gas = new Gas();
            gas.Evolved += gas_Evolved;
            gas.Genetic();
            schedule = gas.Best;
            //this.dataGridView1.DataSource = gas.Best.TeacherSchedule[59].GetTeacherLessonTable();
            ts = schedule.TeacherSchedule;
            //ts = gas.Worst.TeacherSchedule;
            loaded = true;
            //this.textBox1.AppendText(gas.Best.TeacherSchedule[]

            this.button1.Text = "完成";
        }

        void gas_Evolved(EvolvedArgs e)
        {
            this.textBox1.AppendText(string.Format("{0}代,最大值:{1},最小值{2},平均值:{3}\r\n", e.CurrentTimes, e.FitnessMax, e.FitnessMin, e.FitnessAvg));
            Application.DoEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cbTeachers.DataSource = StaticSQLiteHelper.GetTeacherInformation();
            this.cbTeachers.DisplayMember = "教师姓名";
            this.cbTeachers.ValueMember = "教师编号";
            //this.cbTeachers.SelectedIndex = 0;

        }

        private void cbTeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                int ti = Convert.ToInt32(cbTeachers.SelectedValue);
                //int ti = Convert.ToInt32((cbTeachers.SelectedItem as DataRowView).Row[0]);
                dataGridView1.DataSource = ts[ti].ToDataTable();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StaticSQLiteHelper.ClearAndVacuumTable("schedule");
            int rows = StaticSQLiteHelper.BatchExecuteNonQuery(schedule.ToSQLStringList("schedule"));
            MessageBox.Show(string.Format("影响的行数为:{0}", rows), "执行完成");
        }

      
    }
}
