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
    public partial class FormViewTeacherSchedule : Form
    {
        private DataTable dtSubject, dtTeacher;
        private List<string> subjects;
        Schedule schedule;
        Schedule nightSchedule;
        //TeacherSchedule teacherSchedule;
        string teacherName;
        int _teacherID=1;
        public FormViewTeacherSchedule()
        {
            InitializeComponent();
            Initialize(true);
        }
        public FormViewTeacherSchedule(int teacherID,string teacherName)
        {
            InitializeComponent();
            _teacherID = teacherID;
            Initialize(false);
            this.Text=string.Format("{0}老师的课表",teacherName);
            cbSubject.Enabled=false;
            cbTeacher.Enabled=false;
        }

        public void Initialize(bool enabled)
        {
            dtSubject = StaticSQLiteHelper.GetSubjectList();
            subjects = new List<string>();
            schedule = new Schedule(StaticSQLiteHelper.GetSchedule());
            nightSchedule = new Schedule(StaticSQLiteHelper.GetNightSchedule());
            AdjustDataGridView();
            UpdateDataGridView();
            if (enabled)
            {
                foreach (DataRow r in dtSubject.Rows)
                {
                    subjects.Add(r[0].ToString());
                }
                cbSubject.DataSource = subjects;
            }
        }

        private void AdjustDataGridView()
        {
            string[] week = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                dgv.Columns.Add(week[i], week[i]);
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //白天课程
            for (int i = 0; i < Global.LessonPerDay; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i].HeaderCell.Value =( i+1).ToString();
                dgv.Rows[i].Height = 30;
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            //晚自习
            for (int i = 0; i < Global.LessonPerNight; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i+Global.LessonPerDay].HeaderCell.Value = (i+1).ToString();
                dgv.Rows[i+Global.LessonPerDay].Height = 30;
                dgv.Rows[i+Global.LessonPerDay].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgv.Rows[Global.LessonPerForenoon - 1].DividerHeight = 3;
            dgv.Rows[Global.LessonPerDay - 1].DividerHeight = 3;
        }

        private void UpdateDataGridView()
        {
            //情况表格内容
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Value = null;
                }
            }

            foreach (TeacherLesson tl in schedule.TeacherSchedule[_teacherID])
            {
                dgv.Rows[tl.Section].Cells[tl.DayOfWeek].Value = tl.ClassNameListString;
            }

            if (nightSchedule.TeacherSchedule.ContainKey(_teacherID))
            {
                foreach (TeacherLesson tl in nightSchedule.TeacherSchedule[_teacherID])
                {
                    if (tl.Section <= Global.LessonPerNight)
                    {
                        dgv.Rows[tl.Section+Global.LessonPerDay].Cells[tl.DayOfWeek].Value = tl.ClassNameListString;
                    }
                }
            }
        }



        private void cbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {

                teacherName = cbSubject.SelectedValue.ToString();
                dtTeacher = StaticSQLiteHelper.GetSubjectTeacher(teacherName);
                cbTeacher.DisplayMember = "教师姓名";
                cbTeacher.ValueMember = "教师编号";
                cbTeacher.DataSource = dtTeacher;
                cbTeacher.SelectedIndex = 0;

        }

        private void cbTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
                
                _teacherID = Convert.ToInt32(cbTeacher.SelectedValue);
                UpdateDataGridView();
                //this.dataGridView1.Invalidate();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
