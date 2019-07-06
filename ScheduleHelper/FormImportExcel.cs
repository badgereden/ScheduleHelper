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
    public partial class FormImportExcel : Form
    {
        private static bool finish = false;
        private DataTable _dtClassTeacher, _dtCourse;
        public FormImportExcel()
        {
            InitializeComponent();
        }

        private void btnBrowserTeacher_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel 97/2003(*.xls)|*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tbTeacher.Text = ofd.FileName;
                }
                else
                {
                    return;
                }
            }
            ReadClassTeacher(tbTeacher.Text);
        }


        private void ReadClassTeacher(string fileName)
        {
            ExcelReader er = new ExcelReader(fileName);
            _dtClassTeacher = er.FirstSheet2Table();
        }

        private void ReadCourse(string fileName)
        {
            ExcelReader er = new ExcelReader(fileName);
            _dtCourse = er.FirstSheet2Table();
        }
        private string GetCourseAssignSQLFromTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder(1000);
            string ct, sn;
            int wc;
            foreach (DataRow row in dt.Rows)
            {
                ct = row[0].ToString();
                sn = row[1].ToString();
                wc = (int)row[2];
                sb.AppendFormat("update class_course set 周课时={0} where 班级类型='{1}' and 学科名字='{2}';",wc,ct,sn);
            }

            return sb.ToString();
        
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtClassTeacher"></param>
        /// <returns></returns>
        private Dictionary<string,Teacher> GetTeacherInforFromTable(DataTable dtClassTeacher)
        {

            Dictionary<string,Teacher> dict = new Dictionary<string,Teacher>(100);
            int ti = 1;
            for (int c = 3; c < dtClassTeacher.Columns.Count; c++)
            {
                string subjectName = dtClassTeacher.Columns[c].ColumnName;
                string shortSubjectName = subjectName.Substring(0, 1);
                for (int r = 0; r < dtClassTeacher.Rows.Count; r++)
                {
                    //如果单元格为空,则跳过此行
                    if (dtClassTeacher.Rows[r].IsNull(dtClassTeacher.Columns[c]))
                    {
                        continue;
                    }
                    //如果教师已经插入,则跳过此行
                    string tn = dtClassTeacher.Rows[r][c].ToString();
                    if (dict.ContainsKey(tn))
                    {
                        continue;
                    }
                    else
                    {
                        dict.Add(tn, new Teacher(ti,tn,subjectName,shortSubjectName));

                        ti++;
                    }
                }
            }
            return dict;
        }
        
        private string Dictionary2Sql(Dictionary<string,Teacher> dict)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append("Insert into teacher(教师编号,教师姓名,学科名字,学科短名字) values");
            foreach (KeyValuePair<string, Teacher> kp in dict)
            {
                sb.AppendFormat("({0},'{1}','{2}','{3}'),", kp.Value.TeacherID, kp.Value.Name, kp.Value.SubjectName, kp.Value.ShortSubjectName);
            }
            return sb.ToString(0, sb.Length - 1);
        }

        private string GetTeacherSQLFromTable(DataTable dtClassTeacher)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.Append("Insert into class_course(班级,班级类型,学科名字,教师姓名,周课时) values");
            int ci;
            string ct,tn, sn;
            for (int r = 0; r <dtClassTeacher.Rows.Count ; r++)
            {
                ci=(int)dtClassTeacher.Rows[r][0];
                ct=dtClassTeacher.Rows[r][1].ToString();
                for (int c = 3; c < dtClassTeacher.Columns.Count; c++)
                {
                    if (dtClassTeacher.Rows[r].IsNull(dtClassTeacher.Columns[c]))
                    {
                        continue;
                    }
                    sn=dtClassTeacher.Columns[c].ColumnName;
                    tn = dtClassTeacher.Rows[r][c].ToString();
                    sb.AppendFormat("({0},'{1}','{2}','{3}',{4}),", ci, ct, sn, tn, 0);
                }
            }
            return sb.ToString(0, sb.Length - 1);

        }
        private string GetMasterSQLFromTable(DataTable dtClassTeacher)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.Append("update class_course set 班主任=0;");
            int ci;
            string tn;
            for (int r = 0; r < dtClassTeacher.Rows.Count; r++)
            {   
                ci=(int)dtClassTeacher.Rows[r][0];
                tn=dtClassTeacher.Rows[r][2].ToString();
                sb.AppendFormat("update class_course set 班主任=1 where 班级={0} and 教师姓名='{1}';",ci,tn);
            }
            return sb.ToString();
        
        }



        private void btnBrowserCourse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel 97/2003(*.xls)|*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tbCourse.Text = ofd.FileName;
                }
                else
                {
                    return;
                }
            }

            ReadCourse(tbCourse.Text);
        }

        private void Import()
        {
            string sql;
            sql = Dictionary2Sql(GetTeacherInforFromTable(_dtClassTeacher));
            StaticSQLiteHelper.ClearAndVacuumTable("teacher");
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            sql = GetTeacherSQLFromTable(_dtClassTeacher);
            StaticSQLiteHelper.ClearAndVacuumTable("class_course");
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            sql = GetCourseAssignSQLFromTable(_dtCourse);
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            sql = GetMasterSQLFromTable(_dtClassTeacher);
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            setupWeight();
            adjust();
            //Global.Dirty = true;
        }

        private void adjust()
        { 
            //设置副科和主科
            string sql = "update class_course set 副科=0;";
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            sql = "update class_course set 副科=1 where 学科名字 in ('政治','历史','地理','体育','音乐','信息')";
            //sql = "update class_course set 优先级=3 where 学科名字 in ('政治','历史','地理','体育','音乐','信息')";
            StaticSQLiteHelper.ExecuteNonQuery(sql);
            sql = "insert into teacher values(0,'无','自习',' ',null);";
            StaticSQLiteHelper.ExecuteNonQuery(sql);
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (finish)
            {
                this.Close();
                return;
            }
            else
            {
                Import();
                btnAccept.Text = "完成";
                finish = true;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void setupWeight()
        {
            //Global.Empty = true;
            StaticSQLiteHelper.ExecuteNonQuery(string.Format("update class_course set 优先级=1 where 周课时<{0};", Global.DayPerWeek));
            StaticSQLiteHelper.ExecuteNonQuery(string.Format("update class_course set 优先级=2 where 周课时={0};", Global.DayPerWeek));
            StaticSQLiteHelper.ExecuteNonQuery(string.Format("update class_course set 优先级=3 where 周课时>{0};", Global.DayPerWeek));
            //StaticSQLiteHelper.ExecuteNonQuery("update class_course set 优先级=0 where 学科名字='体育';");    
        }




    }
}
