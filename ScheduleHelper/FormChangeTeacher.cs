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
    public partial class FormChangeTeacher : Form
    {

        private int oti, nti;   //原教师编号,新教师的编号
        private string otn, ntn;//原教师姓名,新教师姓名
        private int ci;//班级号,从0开始
        private int cn;//班级名,从1开始
        private string sn;//学科名字;
        //private string osn="";//上次选择的学科名字
        public FormChangeTeacher()
        {
            InitializeComponent();
            CustomInitialize();
        }

        /// <summary>
        /// 生成班级列表
        /// </summary>
        private void CustomInitialize()
        {
            loadSubjectList();

        
        }

        private void loadSubjectList()
        {
            //_subjectList = new List<string>();
            string sql = "select distinct(学科名字) as 学科 from class_course order by  学科";
            DataTable dt = StaticSQLiteHelper.ExecuteQuery(sql);
            cbSubject.SelectedIndexChanged -= new EventHandler(cbSubject_SelectedIndexChanged);
            cbSubject.ValueMember = "学科";
            cbSubject.DisplayMember = "学科";
            cbSubject.DataSource = dt;
            cbSubject.SelectedIndexChanged += new EventHandler(cbSubject_SelectedIndexChanged);
        }


   
        private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubject.SelectedItem == null)
            {
                return;
            }
            cn = Convert.ToInt32(cbClass.SelectedValue);
            ci = cn - 1;
            string sql = "select 教师姓名 from class_course where 班级=" + ci.ToString() + " and 学科名字='" + sn + "'";
            otn = StaticSQLiteHelper.ExecuteScalar(sql).ToString();
            sql = "select distinct(教师姓名) as 教师姓名 from teacher where 教师姓名<>'" + otn + "' and  学科名字='" + sn + "'";
            DataTable dt = StaticSQLiteHelper.ExecuteQuery(sql);
            cbTeacherName.BeginUpdate();
            cbTeacherName.ValueMember = "教师姓名";
            cbTeacherName.DisplayMember = "教师姓名";
            cbTeacherName.DataSource = dt;
            cbTeacherName.EndUpdate();
        }

        private void cbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubject.SelectedItem == null)
            {
                return;
            }

            sn = cbSubject.Text;
            string sql = string.Format("select distinct(班级)+1 as 班级 from class_course where 学科名字='{0}' order by 班级;", sn);
            DataTable dt = StaticSQLiteHelper.ExecuteQuery(sql);
            cbClass.SelectedIndexChanged -= new EventHandler(cbClass_SelectedIndexChanged);
            cbClass.BeginUpdate();
            cbClass.ValueMember = "班级";
            cbClass.DisplayMember = "班级";
            cbClass.DataSource = dt;
            cbClass.SelectedIndex = 0;
            cbClass.EndUpdate();
            cbClass.SelectedIndexChanged += new EventHandler(cbClass_SelectedIndexChanged);
        }


        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //ntn = cbTeacherName.SelectedItem.ToString();
            ChangeTeacher();
            MessageBox.Show(string.Format("成功将{0}班的{1}教师由{2}修改为{3}", cn, sn, otn, ntn));
            //this.Close();
        }

        private void ChangeTeacher()
        {
            string sql = string.Format("select 教师编号 from teacher where 教师姓名='{0}'", otn);
            oti = Convert.ToInt32(StaticSQLiteHelper.ExecuteScalar(sql));
            sql = string.Format("select 教师编号 from teacher where 教师姓名='{0}';", ntn);
            nti = Convert.ToInt32(StaticSQLiteHelper.ExecuteScalar(sql));
            //更新课程设置
            sql = string.Format("update class_course set 教师姓名='{0}' where 班级={1} and 学科名字='{2}'",ntn,ci,sn);
            StaticSQLiteHelper.ExecuteNonQuery(sql);

            //更新白天课程
            sql = string.Format("update schedule set teacher_id={0} where class_id={1} and teacher_id={2}", nti, ci, oti);
            StaticSQLiteHelper.ExecuteNonQuery(sql);

            //更新晚自习
            sql=string.Format("update night set teacher_id={0} where class_id={1} and teacher_id={2}", nti, ci, oti);
            StaticSQLiteHelper.ExecuteNonQuery(sql);

            //更新早自习
            sql = string.Format("update morning set teacher_id={0} where class_id={1} and teacher_id={2}", nti, ci, oti);
            StaticSQLiteHelper.ExecuteNonQuery(sql);



        }

        private void cbTeacherName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ntn = cbTeacherName.Text;
        }





    }
}
