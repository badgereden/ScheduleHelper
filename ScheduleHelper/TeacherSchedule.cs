using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ScheduleHelper
{
    /// <summary>
    /// 所有教师的总课表
    /// </summary>
    public class TeacherSchedule : IEnumerable<TeacherLessonCollection>
    {
        private Dictionary<int, TeacherLessonCollection> _teacherLessonCollectionDictionary;

        public TeacherSchedule(int capacity = 12)
        {
            _teacherLessonCollectionDictionary = new Dictionary<int, TeacherLessonCollection>(capacity);
        }


        public TeacherLessonCollection this[int teacherID]
        {
            get
            {
                if (_teacherLessonCollectionDictionary.ContainsKey(teacherID))
                {
                    return _teacherLessonCollectionDictionary[teacherID];
                }
                else
                {
                    return null;
                }
            }


            set { _teacherLessonCollectionDictionary[teacherID] = value; }
        }


        /// <summary>
        /// 增加一个课时
        /// </summary>
        /// <param name="teacherID">教师ID</param>
        /// <param name="dayOfWeek">从零开始的星期</param>
        /// <param name="section">从零开始的节次</param>
        /// <param name="classID">班级ID</param>
        public bool Add(int teacherID, int dayOfWeek, int section, int classID)
        {
            if (!_teacherLessonCollectionDictionary.ContainsKey(teacherID))
            {
                _teacherLessonCollectionDictionary.Add(teacherID, new TeacherLessonCollection(teacherID));
            }

            return _teacherLessonCollectionDictionary[teacherID].Add(dayOfWeek, section, classID);
        }

        public void Remove(int teacherID)
        {
            _teacherLessonCollectionDictionary.Remove(teacherID);
        }

        public bool ContainKey(int teacherID)
        {
            return _teacherLessonCollectionDictionary.ContainsKey(teacherID);
        }

        public void Clear()
        {
            _teacherLessonCollectionDictionary.Clear();
        }


        public IEnumerator<TeacherLessonCollection> GetEnumerator()
        {
            foreach (TeacherLessonCollection tlc in _teacherLessonCollectionDictionary.Values)
            {
                yield return tlc;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 一个教师的所有课时的课表
    /// </summary>
    public class TeacherLessonCollection : IEnumerable<TeacherLesson>
    {
        private int _teacherID;
        public int TeacherID
        {
            get { return _teacherID; }
            set { _teacherID = value; }
        }

        public TeacherLessonCollection(int teacherID)
        {
            _teacherID = teacherID;
            _teacherLessonDictionary = new Dictionary<int, TeacherLesson>(12);
        }


        private Dictionary<int, TeacherLesson> _teacherLessonDictionary;

        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            string[] week = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                dt.Columns.Add(week[i]);
            }
            for (int s = 0; s < Global.LessonPerDay; s++)
            {
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
            }
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                dt.Rows[tl.Section][tl.DayOfWeek] = tl.ClassListString();
            }

            return dt;
        }

        public DataTable ToDataTableByClassName()
        {
            DataTable dt = new DataTable();
            string[] week = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                dt.Columns.Add(week[i]);
            }
            for (int s = 0; s < Global.LessonPerDay; s++)
            {
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
            }
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                dt.Rows[tl.Section][tl.DayOfWeek] = tl.ClassNameListString;
            }

            return dt;

        }


        /// <summary>
        /// 返回本教师课程在同一个半天的天数
        /// </summary>
        /// <returns></returns>
        public int GetTogetherCount()
        {
            bool[] forenoon = new bool[Global.DayPerWeek];
            bool[] afternoon = new bool[Global.DayPerWeek];
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                if (tl.Section < Global.LessonPerForenoon)
                {
                    forenoon[tl.DayOfWeek] = true;
                }
                else
                {
                    afternoon[tl.DayOfWeek] = true;
                }

            }
            int sum = 0;
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                if (!(forenoon[i] && afternoon[i]))
                {
                    sum++;
                }

            }

            return sum;
        }

        public int Get4SectionCount()
        {
            int sum = 0;
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                if (tl.Section == 3)
                {
                    sum++;
                }
            }
            return sum;
        }

        /// <summary>
        /// 返回指定星期和节次的教师课程,通常只有一节,但也可能是更多
        /// 多节意味着课程有冲突
        /// </summary>
        /// <param name="dayOfWeek">星期</param>
        /// <param name="section">节次</param>
        /// <returns></returns>
        public TeacherLesson this[int dayOfWeek, int section]
        {
            get { return GetTeacherLesson(dayOfWeek, section); }
            //set { _teacherLessonDictionary[dayOfWeek * Global.LessonPerDay + section] = value; }
        }


        private TeacherLesson GetTeacherLesson(int dayOfWeek, int section)
        {
            return GetPeriodLesson(dayOfWeek * Global.LessonPerDay + section);
        }

        /// <summary>
        /// 返回指定时间段的课程
        /// 时间段=星期*每天的天数+节次
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public TeacherLesson this[int period]
        {
            get
            {
                return GetPeriodLesson(period);
            }
            //set { _teacherLessonDictionary[period] = value; }
        }

        private TeacherLesson GetPeriodLesson(int period)
        {
            if (_teacherLessonDictionary.ContainsKey(period))
            {
                return _teacherLessonDictionary[period];
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// 返回指定教师的某一天的所有课程
        /// </summary>
        /// <param name="dayOfWeek">从零开始的星期</param>
        /// <returns></returns>
        public TeacherLessonCollection GetDOWLessons(int dayOfWeek)
        {
            TeacherLessonCollection tlc = new TeacherLessonCollection(_teacherID);
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                if (tl.DayOfWeek == dayOfWeek)
                {
                    tlc.Add(dayOfWeek, tl.Section, tl);
                }

            }

            return tlc;
        }

        /// <summary>
        /// 增加一个班级课时
        /// </summary>
        /// <param name="dayOfWeek">从零开始的星期</param>
        /// <param name="section">从零开始的节次</param>
        /// <param name="classID">班级ID</param>
        public bool Add(int dayOfWeek, int section, int classID)
        {
            int period = dayOfWeek * Global.LessonPerDay + section;
            //如果没有关键字,则新插入该关键字
            if (!_teacherLessonDictionary.ContainsKey(period))
            {
                _teacherLessonDictionary.Add(period, new TeacherLesson(dayOfWeek, section));
            }
            //在指定的时间上插入一节课的班级ID

            return _teacherLessonDictionary[period].Add(classID);
        }

        /// <summary>
        /// 增加一个教师的课时(集合)
        /// </summary>
        /// <param name="dayOfWeek">从零开始的星期</param>
        /// <param name="section">从零开始的节次</param>
        /// <param name="tl">教师的一个课时(集合,可能有多个班级ID)</param>
        public bool Add(int dayOfWeek, int section, TeacherLesson tl)
        {
            bool conflict = false;
            foreach (int ci in tl)
            {
                conflict = Add(dayOfWeek, section, ci);
            }
            return conflict;
        }

        /// <summary>
        /// 教师的总节数（如果冲突可能返回值小于预期值）
        /// </summary>
        public int SectionCount
        {
            get
            {
                return _teacherLessonDictionary.Count;
            }
        }
        /// <summary>
        /// 返回冲突的节数
        /// </summary>
        /// <returns></returns>
        public int GetConflictCount()
        {
            int sum = 0;
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                if (tl.Count > 1)
                {
                    sum++;
                }

            }
            return sum;
        }

        public IEnumerator<TeacherLesson> GetEnumerator()
        {
            foreach (TeacherLesson tl in _teacherLessonDictionary.Values)
            {
                yield return tl;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
    /// <summary>
    /// 一个教师的一个课时的班级(有冲突时可能有两个班级号)
    /// </summary>
    public class TeacherLesson : IEnumerable<int>
    {
        /// <summary>
        /// 从零开始的星期
        /// </summary>
        public int DayOfWeek { get; set; }
        /// <summary>
        /// 从零开始的节次
        /// </summary>
        public int Section { get; set; }

        public List<int> ClassList
        {
            get { return _classList; }
        }

        private List<int> _classList;

        public TeacherLesson(int dayOfWeek, int section)
        {
            DayOfWeek = dayOfWeek;
            Section = section;
            _classList = new List<int>(2);
        }

        /// <summary>
        /// 返回一个课时要上的班级数
        /// 正常值应该是1，返回值大于1代表有冲突。
        /// </summary>
        public int Count
        {
            get { return _classList.Count; }
        }
        /// <summary>
        /// 增加一个班级的课时
        /// </summary>
        /// <param name="classID"></param>
        public bool Add(int classID)
        {
            _classList.Add(classID);
            if (_classList.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void Clear()
        {
            _classList.Clear();
        }
        public void Remove(int classID)
        {
            _classList.Remove(classID);
        }

        /// <summary>
        /// 返回教师班级列表的字符串版本
        /// 注意:班级名字!=班级ID,班级ID从0开始.
        /// </summary>
        /// <returns></returns>
        public string ClassListString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int ci in _classList)
            {
                sb.Append(ci);
                sb.Append(',');
            }
            return sb.ToString(0, sb.Length - 1);
        }

        public bool Forenoon
        {
            get {
                if (this.Section < Global.LessonPerForenoon)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }




        /// <summary>
        /// 返回教师班级名字列表的字符串版本
        /// 注意:班级名字!=班级ID,班级名字从1开始.
        /// </summary>
        /// <returns></returns>
        public string ClassNameListString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (int ci in _classList)
                {
                    sb.Append(ci + 1);
                    sb.Append(',');
                }
                return sb.ToString(0, sb.Length - 1);
            }
        }


        public IEnumerator<int> GetEnumerator()
        {
            foreach (int ci in _classList)
            {
                yield return ci;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
