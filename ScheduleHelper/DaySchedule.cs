using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class DaySchedule:IEnumerable<Lesson>
    {
        private List<Lesson> _lessons;
        private ClassSchedule _class;
        private int _dayOfWeek;
        private int _assginedCount;

        public DaySchedule(ClassSchedule owner, int lessonsPerDay = 7, int dayOfWeek = 6)
        {
            _class = owner;
            _dayOfWeek = dayOfWeek;
            _lessons = new List<Lesson>(lessonsPerDay);
            AddLessons(lessonsPerDay);
        }

     




        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other"></param>
        /// <param name="_class"></param>
        public DaySchedule(DaySchedule other, ClassSchedule _class=null)
        {
            _dayOfWeek = other._dayOfWeek;
            _assginedCount = other._assginedCount;
            _lessons = new List<Lesson>(other._lessons.Count);
            foreach (Lesson l in other._lessons)
            {
                _lessons.Add(new Lesson(l, this));
            }

            if (_class == null)
            {
                this._class = other._class;
                
            }
            else
            {
                this._class = _class;
            }
        
        }





        /// <summary>
        /// 添加一节课，节次自动生成。
        /// </summary>
        public void AddLesson()
        {
            int count = _lessons.Count;
            _lessons.Add(new Lesson(this,count));
        }
        /// <summary>
        /// 自动添加课程，直到指定的节数
        /// </summary>
        /// <param name="lessonCount"></param>
        public void AddLessons(int lessonCount)
        {
            int count = lessonCount - _lessons.Count;
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                AddLesson();
            }
        
        }

        /// <summary>
        /// 交换两节课
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Exchange2Lesson(int first, int second)
        {
            if (first == second)
            {
                return;
            }
            Teacher t = _lessons[first].Teacher;
            _lessons[first].Teacher = _lessons[second].Teacher;
            _lessons[second].Teacher = t;

        }
        /// <summary>
        /// 随机交换两节课
        /// </summary>
        public void RandomExchange2Lesson()
        {
            int first, second;
            Global.RandomGeneric2Value(Global.LessonPerDay, out first, out second);
            Exchange2Lesson(first, second);
        }



        /// <summary>
        /// 批量交换一段课程
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public void BatchExchangeLesson(int start, int length)
        { 
            
        
        
        }
        /// <summary>
        /// 将一段课程打乱
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public void Mix(int start, int length)
        { 
            
        
        }

        /// <summary>
        /// 清空所有课程
        /// </summary>
        public void ClearAllLesson()
        { 
            foreach(Lesson l in _lessons)
            {
                l.Teacher = null;
            }
            _assginedCount = 0;
        }



        /// <summary>
        /// 将一段课颠倒过来
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public void Reverse(int start, int length)
        {
            
        
        }


        /// <summary>
        /// 班级ID（从零开始）
        /// </summary>
        public int ClassID
        {
            get { return _class.ClassID; }
        }

        /// <summary>
        /// 返回从0开始的本周的第几天
        /// </summary>
        public int DayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; }
        }

        /// <summary>
        /// 当天的课是否已经安排满
        /// </summary>
        public bool Full
        {
            get {
                if (_assginedCount == _lessons.Count)
                {
                    return true;
                }
                else
                    return false;
            }
        
        }
        /// <summary>
        /// 已经安排的节数
        /// </summary>
        public int AssignedCount
        {
            get {

                return _assginedCount;
            }
        
        
        }

        /// <summary>
        /// 安排一节课
        /// </summary>
        /// <param name="section"></param>
        /// <param name="teacher"></param>
        /// <param name="canModify"></param>
        /// <returns>返回false表示安排失败，返回true为安排成功</returns>
        public bool AssignLesson(int section, Teacher teacher, bool canModify)
        {
            if (_lessons[section].CanModify == false)
            {
                return false;
            }
            _lessons[section].Teacher = teacher;
            _lessons[section].CanModify = canModify;
            _assginedCount++;
            return true;
        }

        /// <summary>
        /// 查询某个教师的课已经安排的节数
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int AssignedLessonCount(Teacher t)
        {
            int sum = 0;
            foreach (Lesson l in _lessons)
            {
                if (l.TeacherID == t.TeacherID)
                {
                    sum++;
                }
            }
            return sum;
        }
        /// <summary>
        /// 查询某个教师的课已经安排的节数
        /// </summary>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        public int AssignedLessonCount(int teacherID)
        {
            int sum = 0;
            foreach (Lesson l in _lessons)
            {
                if (l.TeacherID == teacherID)
                {
                    sum++;
                }
            }
            return sum;
        }


        /// <summary>
        /// 计算某节课(某教师)在当天安排了多少节.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public int SubjectAssignedCount(Teacher teacher)
        {
            int c = 0;
            foreach (Lesson l in _lessons)
            {
                if (l.Teacher == teacher)
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// 随机返回一节没有安排的课
        /// </summary>
        /// <returns></returns>
        public Lesson RandomReturnEmptyLesson()
        {
            Lesson result = null;
            int count=_lessons.Count;
            int maxIndex = count - 1;
            int s = Global.Random.Next(count);
            for (int i = 0; i < count; i++)
            {
                if (!_lessons[s].Assigned)
                {
                    result = _lessons[s];
                }
                if (s == maxIndex)
                {
                    s = 0;
                }
                else
                {
                    s++;
                }

            }
            return result;
        }
        /// <summary>
        /// 随机安排一节课
        /// </summary>
        /// <param name="t"></param>
        public void  RandomAssignLesson(Teacher t)
        {
            Lesson l = RandomReturnEmptyLesson();
            l.Teacher = t;
            _assginedCount++;
        }


        /// <summary>
        /// 实现迭代器
        /// </summary>
        /// <returns></returns>
        //public IEnumerator IEnumerable.GetEnumerator()
        //{
        //    foreach (Lesson l in _lessons)
        //    {
        //        yield return l;
        //    }
        //}
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="section">从零开始的节次</param>
        /// <returns></returns>
        public Lesson this[int section]
        {
            get {
                return _lessons[section];
            }
            set {
                _lessons[section] = value;
            }
        }

        public override int GetHashCode()
        {
            return _dayOfWeek;
        }




        public override bool Equals(object other)
        {
            if (other is DaySchedule)
            {
                DaySchedule ds = other as DaySchedule;
                return ds._dayOfWeek == _dayOfWeek;
            }
            else
            {
                return false;
            }
        }



        public IEnumerator<Lesson> GetEnumerator()
        {
            foreach (Lesson l in _lessons)
            {
                yield return l;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



    }
}
