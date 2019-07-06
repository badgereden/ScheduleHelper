using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class Lesson
    {
        private int _section;//节次
        private bool _forenoon;//是否上午的课
        private Teacher _teacher;//教师
        private bool _canModify;//能否修改
        private bool _conflict;//是否冲突
        private DaySchedule _day;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner">所属的Day对象,类型Day</param>
        /// <param name="section">节次（从零开始）,类型int</param>
        /// <param name="teacher">教师，类型Teacher</param>
        public Lesson(DaySchedule owner, int section = 0, Teacher teacher = null)
        {
            _day = owner;
            _section = section;
            _teacher = teacher;
            _canModify = true;
        }





        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other"></param>
        /// <param name="owner"></param>
        public Lesson(Lesson other, DaySchedule owner = null)
        {
            _section = other._section;
            _forenoon = other._forenoon;
            _teacher = other._teacher;
            if (owner == null)
            {
                _day = other._day;
            }
            else
            {
                _day = owner;
            }

        }

        public string ToSQLString(string destinationTable, int rowid)
        {
            StringBuilder sb = new StringBuilder(20);
            sb.Append("INSERT INTO ");
            sb.Append(destinationTable);
            sb.Append("(id,class_id,day_of_week,section,teacher_id) values ");
            sb.Append("(");
            sb.Append(rowid);
            sb.Append(",");
            sb.Append(this.ClassID);
            sb.Append(",");
            sb.Append(this.DayOfWeek);
            sb.Append(",");
            sb.Append(this.Section);
            sb.Append(",");
            sb.Append(this.TeacherID);
            sb.Append(");");
            return sb.ToString();
        }




        public DaySchedule Day
        {
            get
            {
                return _day;
            }
            set
            {
                _day = value;
            }
        }

        /// <summary>
        /// 获取从0开始的班级ID
        /// </summary>
        public int ClassID
        {
            get { return _day.ClassID; }
        }


        /// <summary>
        /// 获取从0开始的星期几
        /// </summary>
        public int DayOfWeek
        {
            get { return _day.DayOfWeek; }
            //set { _dayOfWeek = value; }
        }
        /// <summary>
        /// 获取从0开始的节次
        /// </summary>
        public int Section
        {
            get { return _section; }
        }
        /// <summary>
        /// 设置或获取教师
        /// </summary>
        public Teacher Teacher
        {
            get { return _teacher; }
            set
            {
                _teacher = value;
            }
        }
        /// <summary>
        /// 获取是否上午的课
        /// </summary>
        public bool Forenoon
        {
            get
            {
                return _section < Global.LessonPerForenoon;
               }
        }
        /// <summary>
        /// 获取课程名称
        /// </summary>
        public string SubjectName
        {
            get
            {
                return _teacher == null ? "" : _teacher.SubjectName;
            }

        }
        /// <summary>
        /// 获取课程名称的简称
        /// </summary>
        public string ShortSubjectName
        {
            get
            {
                return _teacher == null ? "" : _teacher.ShortSubjectName;
            }
        }
        /// <summary>
        /// 获取教师姓名
        /// </summary>
        public string TeacherName
        {
            get
            {
                return _teacher == null ? "" : _teacher.Name;
            }
        }
        /// <summary>
        /// 获取教师ID
        /// </summary>
        public int TeacherID
        {
            get
            {
                return _teacher == null ? 0 : _teacher.TeacherID;
            }
        }
        /// <summary>
        /// 获取是否已安排课
        /// </summary>
        public bool Assigned
        {
            get
            {
                return _teacher != null;
            }
        }
        /// <summary>
        /// 设置或获取能否修改
        /// </summary>
        public bool CanModify
        {
            get { return _canModify; }
            set { _canModify = value; }
        }

        public bool Conflict
        {
            get { return _conflict; }
            set { _conflict = value; }
        }

    }
}
