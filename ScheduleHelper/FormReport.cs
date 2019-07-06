using System;
using System.Data;
using System.Windows.Forms;

namespace ScheduleHelper
{
    public partial class FormReport : Form
    {
        private Schedule _schedule;
        private Schedule _nightSchedule;
        private  bool finish = false;
        public FormReport()
        {
            InitializeComponent();
            loadSchedule();
        }
        public FormReport(Schedule schedule,Schedule nightSchedule=null)
        {
            _schedule = schedule;
            _nightSchedule = nightSchedule;
        }
        
        public  void loadSchedule()
        {
            //DataTable dt = StaticSQLiteHelper.GetSchedule();
            _schedule = new Schedule(StaticSQLiteHelper.GetSchedule());
            _nightSchedule = new Schedule(StaticSQLiteHelper.GetNightSchedule());
        }


        public void ReportGradeSchedule(string fileName)
        {
            string[] week = new string[7] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            ExcelWriter ew = new ExcelWriter();
            ew.CreateSheet("全年级课表");
            int startColumn = 0;
            int startRow = 0;
            foreach (ClassSchedule cs in _schedule)
            {
                foreach (DaySchedule ds in cs)
                {
                    if (ds.DayOfWeek % 2 != 0)
                    {
                        startColumn = Global.ClassCount + 2;
                    }
                    else
                    {
                        startColumn = 0; 
                    }
                    startRow = ((ds.DayOfWeek) / 2) * (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight-1;
                    ew.Write(startRow, cs.ClassID + startColumn+1, cs.ClassID+1);
                    foreach (Lesson l in ds)
                    {
                        //写入课程名
                        ew.Write(startRow + l.Section+1, startColumn + cs.ClassID+1, l.ShortSubjectName);
                        //表格最左边加“节次”
                        if (cs.ClassID == 1)
                        {
                            ew.Write(startRow + l.Section + 1, startColumn, l.Section + 1);
                        }
                    }
                }
            }
            //如果包含晚自习
            if (chbContainNight.Checked)
            {
                foreach (ClassSchedule cs in _nightSchedule)
                {
                    foreach (DaySchedule ds in cs)
                    {

                        if (ds.DayOfWeek % 2!= 0)
                        {
                            startColumn = Global.ClassCount + 2;
                        }
                        else
                        {
                            startColumn = 0;
                        }
                        startRow = ((ds.DayOfWeek) / 2) * (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight-1;
                        ew.Write(startRow, cs.ClassID + startColumn + 1, cs.ClassID + 1);
                        foreach (Lesson l in ds)
                        {
                            if (l.Section >= Global.LessonPerNight)
                            {
                                break;
                            }
                            //写入课程名
                            ew.Write(startRow + l.Section +Global.LessonPerDay+ 1, startColumn + cs.ClassID + 1, l.ShortSubjectName);
                            //表格最左边加“节次”
                            if (cs.ClassID == 1)
                            {
                                ew.Write(startRow + l.Section+Global.LessonPerDay + 1, startColumn, l.Section + 1);
                            }
                        }
                    }
                }
            
            }




            //合并标题行
            ew.MergeCells(0, 0, 0, Global.ClassCount * 2 + 2);
            ew.MergeCells(1, Global.ClassCount + 1, Global.DayPerWeek / 2 * (Global.LessonPerDay + 2+ Global.LessonPerNight), Global.ClassCount + 1);
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                if (i % 2 == 0)
                {
                    startColumn = 0;
                }
                else
                {
                    startColumn = Global.ClassCount + 2;
                }
                startRow = (i / 2) * (Global.LessonPerDay + 2+ Global.LessonPerNight) + Global.LessonPerNight-2;
                ew.Write(startRow, startColumn, week[i]);
                ew.MergeCells(startRow, startColumn, startRow, startColumn + Global.ClassCount );
            }
            //显示边框
            for (int r = 0; r < Global.DayPerWeek / 2 * (Global.LessonPerDay + 2 + Global.LessonPerNight) +1; r++)
            {
                for (int c = 0; c < Global.ClassCount * 2 + 3; c++)
                {
                    ew[r, c].CellStyle = ew.CsCenterBorderAll;
                }
            
            
            }

            //输出课表的题头
            ew.Write(0, 0, string.Format("高三年级课表({0})", DateTime.Now.Date.ToString("yyyy年M月d日")));
            ew.CurrentSheet.PrintSetup.PaperSize = 9;
            ew.CurrentSheet.PrintSetup.Landscape = true;
            ew.SaveAs(fileName);
        
        }
        public void ReportGradeScheduleByTeacherName(string fileName)
        {
            string[] week = new string[7] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            ExcelWriter ew = new ExcelWriter();
            ew.CreateSheet("全年级课表-教师姓名");
            int startColumn = 0;
            int startRow = 0;
            foreach (ClassSchedule cs in _schedule)
            {
                foreach (DaySchedule ds in cs)
                {
                     startColumn = 0;
                    startRow = (ds.DayOfWeek) * (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight-1;

                    ew.Write(startRow, cs.ClassID + startColumn + 1, cs.ClassID + 1);
                    foreach (Lesson l in ds)
                    {
                         ew.Write(startRow + l.Section + 1, startColumn + cs.ClassID + 1, l.TeacherName);
                        //表格最左边加“节次”
                        if (cs.ClassID == 1)
                        {
                            ew.Write(startRow + l.Section + 1, startColumn, l.Section + 1);
                        }
                    }
                }
            }

            //如果包含晚自习
            if (chbContainNight.Checked)
            {
                foreach (ClassSchedule cs in _nightSchedule)
                {
                    foreach (DaySchedule ds in cs)
                    {
                        startColumn = 0;

                        startRow = (ds.DayOfWeek) * (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight - 1;
                        ew.Write(startRow, cs.ClassID + startColumn + 1, cs.ClassID + 1);
                        foreach (Lesson l in ds)
                        {
                            if (l.Section >= Global.LessonPerNight)
                            {
                                break;
                            }
                            //写入课程名
                            ew.Write(startRow + l.Section + Global.LessonPerDay + 1, startColumn + cs.ClassID + 1, l.TeacherName);
                            //表格最左边加“节次”
                            if (cs.ClassID == 1)
                            {
                                ew.Write(startRow + l.Section + Global.LessonPerDay + 1, startColumn, l.Section + 1);
                            }
                        }
                    }
                }

            }




            //合并标题行
            ew.MergeCells(0, 0, 0, Global.ClassCount);
            //ew.MergeCells(1, Global.ClassCount + 1, Global.DayPerWeek / 2 * (Global.LessonPerDay + 4), Global.ClassCount + 1);
            
           //显示星期几
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                startColumn = 0;
                startRow = i* (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight - 2;
                ew.Write(startRow, startColumn, week[i]);
                ew.MergeCells(startRow, startColumn, startRow, startColumn + Global.ClassCount);
            }
            //显示边框
            for (int r = 0; r < Global.DayPerWeek * (Global.LessonPerDay + 2 + Global.LessonPerNight) + Global.LessonPerNight-1; r++)
            {
                for (int c = 0; c < Global.ClassCount  + 1; c++)
                {
                    ew[r, c].CellStyle = ew.CsCenterBorderAll;
                }
            }

            //输出课表的题头
            ew.Write(0, 0, string.Format("高三年级课表({0})", DateTime.Now.Date.ToString("yyyy年M月d日")));
            ew.CurrentSheet.PrintSetup.PaperSize = 9;
            ew.SaveAs(fileName);

        }
        public void ReportClassSchedule(string fileName)
        {
            ExcelWriter ew = new ExcelWriter();
            string[] week = new string[7] { "星期一","星期二","星期三","星期四","星期五","星期六","星期天"};
            //插入白天课程
            foreach(ClassSchedule c in _schedule)
            {
                ew.CreateSheet((c.ClassID+1).ToString());
                
                foreach (DaySchedule d in c)
                {
                    ew.Write(0, d.DayOfWeek+1, week[d.DayOfWeek]);
                    foreach (Lesson l in d)
                    {
                        ew.Write(l.Section+1, d.DayOfWeek+1, l.ShortSubjectName);
                        if (d.DayOfWeek == 1)
                        {
                            ew.Write(l.Section+1, 0, l.Section+1);
                        }
                    }
                }
           
            }
            //插入晚自习
            if (chbContainNight.Checked)
            {
                foreach (ClassSchedule c in _nightSchedule)
                {

                    ew.ActiveSheet((c.ClassID+1).ToString());
                    foreach (DaySchedule d in c)
                    {
                        foreach (Lesson l in d)
                        {
                            if (l.Section >= Global.LessonPerNight)
                            {
                                break;
                            }
                            ew.Write(l.Section+Global.LessonPerDay + 1, d.DayOfWeek + 1, l.ShortSubjectName);
                            if (d.DayOfWeek == 1)
                            {
                                ew.Write(l.Section+Global.LessonPerDay + 1, 0, l.Section + 1);
                            }
                        }
                    }

                }
            }
            ew.CurrentSheet.PrintSetup.PaperSize = 9;
            ew.SaveAs(fileName);
        }

        public void ReportTeacherSchedule(string fileName)
        {
            string[] week = new string[7] { "星期一","星期二","星期三","星期四","星期五","星期六","星期天"};
            string[] subjects;
            using (DataTable dt = StaticSQLiteHelper.ExecuteQuery("select distinct(学科名字) from teacher;"))
            {
                subjects = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    subjects[i] = dt.Rows[i][0].ToString();
                }
            }
            DataTable dts;
             int ti, start_row, start_column;
            ExcelWriter ew = new ExcelWriter();
            for (int s = 0; s < subjects.Length; s++)
            {
                ew.CreateSheet(subjects[s]);
                dts = StaticSQLiteHelper.ExecuteQuery(string.Format("select 教师编号 from teacher where 学科名字='{0}' and  教师姓名 in (select 教师姓名 from class_course);", subjects[s]));
                for(int i=0;i<dts.Rows.Count;i++)
                {
                    ti=Convert.ToInt32(dts.Rows[i][0]);
                    if (ti == 0)
                    {
                        continue;
                    }
                    ew.Write(i * (Global.LessonPerDay + Global.LessonPerNight + 3), 0, Global.GradeTeachers[ti].Name);
                    start_row = i * (Global.LessonPerDay+Global.LessonPerNight+3) + 1;
                    start_column = 0;
                    for (int w = 0; w < Global.DayPerWeek; w++)
                    {
                        ew.Write(start_row, w+1, week[w]);
                    }

                    for (int sec = 1; sec <= Global.LessonPerDay+Global.LessonPerNight; sec++)
                    {
                        if (chbContainNight.Checked && sec>Global.LessonPerDay)
                        {
                            ew.Write(start_row + sec , 0,( sec-Global.LessonPerDay).ToString());
                        }
                        else
                        {
                            ew.Write(start_row + sec, 0, sec.ToString());
                        }
                    }
  
                   foreach (TeacherLesson tl in _schedule.TeacherSchedule[ti])
                   {
                      ew.Write(start_row + tl.Section+1, start_column + tl.DayOfWeek+1, tl.ClassNameListString);
                   }
                    //如果包含晚自习
                   if (chbContainNight.Checked && _nightSchedule.TeacherSchedule.ContainKey(ti))
                   {
                       foreach (TeacherLesson tl in _nightSchedule.TeacherSchedule[ti])
                       {
                           ew.Write(start_row + tl.Section+Global.LessonPerDay + 1, start_column + tl.DayOfWeek + 1, tl.ClassNameListString);
                       }
                   }
                }
            }
            ew.SaveAs(fileName);


        }


        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd=new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tbFileName.Text = fbd.SelectedPath;
                }
            
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (!finish)
            {
                btnEnter.Enabled = false;
                if (tbFileName.Text.Length == 0)
                {
                    MessageBox.Show("请设置文件输出路径!");
                }
                if (ckbGrade.Checked)
                {
                    ReportGradeSchedule(tbFileName.Text + "\\全年级课表.xls");
                }
                if (chbGradeScheduleByTeacherName.Checked)
                {
                    ReportGradeScheduleByTeacherName(tbFileName.Text + "\\全年级课表-教师姓名.xls");
                }

                if (ckbClass.Checked)
                {
                    ReportClassSchedule(tbFileName.Text + "\\班级课表.xls");
                }
                if (ckbTeacher.Checked)
                {
                    ReportTeacherSchedule(tbFileName.Text + "\\教师课表.xls");
                }

                btnEnter.Text = "完成";
                btnEnter.Enabled = true;
                finish = true;
            }
            else
            {
                this.Close();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}
