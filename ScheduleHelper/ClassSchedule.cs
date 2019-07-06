using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class ClassSchedule:IEnumerable<DaySchedule>
    {
        private int _classID;
        private Schedule _schedule;
        private List<DaySchedule> _days;
        private ClassSubjectSettingCollection _classSubjectSettingCollection;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner">所属Schedule对象</param>
        /// <param name="daysPerWeek">每周的天数</param>
        /// <param name="lessonsPerDay">每天的节数</param>
        /// <param name="classID">班级ID</param>
        public ClassSchedule(Schedule owner,int daysPerWeek=6,int lessonsPerDay=7,int classID=0)
        {
            _schedule = owner;
            _classID = classID;
            _days = new List<DaySchedule>(daysPerWeek);
            AddDays(daysPerWeek,lessonsPerDay);
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other">源对象</param>
        /// <param name="owner">新对象属于的Schedule</param>
        public ClassSchedule(ClassSchedule other, Schedule owner = null)
        {
            this._days = new List<DaySchedule>(other._days.Count);
            foreach (DaySchedule d in other._days)
            {
                this._days.Add(new DaySchedule(d, this));
            }
            this._classID = other._classID;
            this._schedule = owner;
  
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <param name="owner">源对象</param>
        /// <returns></returns>
        public ClassSchedule DeepClone(Schedule owner = null)
        {
            ClassSchedule cs = new ClassSchedule(this, owner);
            return cs;   
        }


        /// <summary>
        /// 获取或设置班级ID
        /// </summary>
        public int ClassID
        {
            get { return _classID; }
            set { _classID = value; }
        }
        /// <summary>
        /// 获取所属Schedule对象的GUID
        /// </summary>
        public Guid GUID
        {
            get { return _schedule.GUID; }
        }

        /// <summary>
        /// 随机返回课未安排满的一个DaySchdule对象
        /// </summary>
        /// <returns></returns>
        public DaySchedule RandomReturnDay()
        {
            int count = _days.Count;

            int s= Global.Random.Next(_days.Count);
            int flag = 0;
            while (_days[s].Full)
            {
                s++;
                if (s >= _days.Count)
                {
                    s = 0;
                }
                flag++;
                if (flag > 20)
                {
                    throw new AssignFailedException("RandomReturnDay引发的异常,有可能进入死循环");
                }
            }

            return _days[s];
            
        
        }

        /// <summary>
        /// 随机返回一天可以插入指定课程的一天,有可能返回失败,如果返回为null则表示失败
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="maxCount">一天最多能安排的节数</param>
        /// <returns></returns>
        public DaySchedule RandomReturnValideDay(Teacher teacher,int maxCount)
        {
            int count = _days.Count;

            int s = Global.Random.Next(_days.Count);
            int flag = 0;
            while (_days[s].Full || _days[s].AssignedLessonCount(teacher.TeacherID)>=maxCount)
            {
                s++;
                if (s >= _days.Count)
                {
                    s = 0;
                }
                flag++;
                if (flag > 20)
                {
                    throw new SystemException("RandomReturnDay引发的异常,有可能进入死循环");
                }
            }

            return _days[s];
        }
        /// <summary>
        /// 随机返回一天可以插入指定课程的一天
        /// </summary>
        /// <param name="teacherID"></param>
        /// <param name="maxCount">一天最多能安排的节数</param>
        /// <returns></returns>
        public DaySchedule RandomReturnValideDay(int teacherID, int maxCount)
        {
            int count = _days.Count;
            int s = Global.Random.Next(_days.Count);
            int flag = 0;
            while (_days[s].Full || _days[s].AssignedLessonCount(teacherID) >= maxCount)
            {
                s++;
                if (s >= _days.Count)
                {
                    s = 0;
                }
                flag++;
                if (flag > 20)
                {
                    return null;
                }
            }

            return _days[s];

        }
        /// <summary>
        /// 添加一天
        /// </summary>
        public void AddDay(int lessonsPerDay=7)
        {
            int count = _days.Count;
            _days.Add(new DaySchedule(this, lessonsPerDay, count));
        }
        /// <summary>
        /// 添加到指定的天数
        /// </summary>
        /// <param name="dayCount"></param>
        public void AddDays(int dayCount,int lessonsPerDay=7)
        {
            int count = dayCount - _days.Count;
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                AddDay(lessonsPerDay);
            }
        
        }

        /// <summary>
        /// 交换两天的课
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Exchange2Days(int first, int second)
        {
            if (first == second)
            {
                return; 
            }
            DaySchedule temp = _days[first];
            _days[first] = _days[second];
            _days[second] = temp;

        }
        /// <summary>
        /// 随机交换两天的课程
        /// </summary>
        public void RandomExchange2Days()
        {
            int first, second;
            Global.RandomGeneric2Value(Global.DayPerWeek, out first, out second);
            Exchange2Days(first, second);
        }

        /// <summary>
        /// 将班级课表转换为DataTable对象返回
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string[] week = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                dt.Columns.Add(week[i]);
            }
            for (int s = 0; s < Global.LessonPerDay; s++)
            {
                System.Data.DataRow row = dt.NewRow();
                dt.Rows.Add(row);
            }

            foreach (DaySchedule d in this._days)
            {
                foreach (Lesson l in d)
                {
                    if (l.Teacher!=null)
                    {
                        dt.Rows[l.Section][l.DayOfWeek] = l.ShortSubjectName;
                    }
                }
            }

            return dt;
        
        
        }

        public override int GetHashCode()
        {
            return _classID;
        }

        public override bool Equals(object other)
        {
            if (other is ClassSchedule)
            {
                ClassSchedule cs = other as ClassSchedule;
                return cs._classID == _classID;
            }
            else
            {
                return false;
            }
        }

        public DaySchedule this[int dayOfWeek]
        {
            get { return _days[dayOfWeek]; }
            set { _days[dayOfWeek] = value; }
        }

        public IEnumerator<DaySchedule> GetEnumerator()
        {
            foreach (DaySchedule d in _days)
            {
                yield return d;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 初始化本班级课程
        /// </summary>
        public void InitializeSchedule(ClassSubjectSettingCollection classSubjectCollection)
        {
            _classSubjectSettingCollection = new ClassSubjectSettingCollection(classSubjectCollection);
            int _tryCount = 0;
            while ( !AssignLessons(_classSubjectSettingCollection))
            {
                _tryCount++;
                if (_tryCount >= 20)
                {
                    throw new AssignFailedException("已经安排了400次,但是都失败了");
                }
                //清空已经安排的课程，开始下一轮从新安排
                ClearAllLesson();
                ClearAsignHistory();

            }



        
        }

        /// <summary>
        /// 根据指定的ClassSubjectSettingCollection安排该班的所有课程
        /// </summary>
        /// <param name="cssc"></param>
        /// <returns></returns>
        public bool AssignLessons(ClassSubjectSettingCollection cssc)
        {
            bool error = false;
            foreach (ClassSubjectSetting css in cssc)
            {
                if (css.LessonsPerWeek >= Global.DayPerWeek)
                {
                    foreach (DaySchedule ds in _days)
                    {
                        ds.RandomAssignLesson(Global.GradeTeachers[css.TeacherID]);
                        css.Assigned++;
                    }
                }
            }
            //经过第一轮安排,基本上所有的课剩余的课时都会小于每周的天数.
            foreach (ClassSubjectSetting css in cssc)
            {
                int flag = 0;
                if (error)
                {
                    break;
                }
                while (css.LastCount > 0)
                {
                    //如果已经标记,说明已经尝试完了

                    DaySchedule ds = RandomReturnValideDay(css.TeacherID, css.MaxCountPerDay);
                    if (ds == null)
                    {
                        error = true;
                        break;
                    }
                    //如果返回的ds为null则表示查找失败,安排不进课了.
                    ds.RandomAssignLesson(Global.GradeTeachers[css.TeacherID]);
                    css.Assigned++;
                    flag++;
                    if (flag > 20)
                    {
                        //在这里打一标记,
                        error = true;
                        //throw new SystemException("可能进入死循环");
                    }
                }


            }

            return !error;

        }




        /// <summary>
        /// 将所有已经安排的课清空.回复到初始状态
        /// </summary>
        public void ClearAllLesson()
        { 
            foreach(DaySchedule d in _days)
            {
                d.ClearAllLesson();
            }
        
        }

        /// <summary>
        /// 重置ClassSubjectSettingCollection里的安排记录
        /// </summary>
        private void ClearAsignHistory()
        {
            foreach (ClassSubjectSetting css in _classSubjectSettingCollection)
            {
                css.Assigned = 0;
            }
        }
        /// <summary>
        /// 交换指定的两节课[层次不对,应该放弃使用该函数]
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        [Obsolete("该函数所在层次不对,请放弃使用该函数")]
        public bool Exchange2Lesson(Lesson first, Lesson second)
        {
            if ((first.DayOfWeek == second.DayOfWeek && first.Section == second.Section) || (first.TeacherID == second.TeacherID))
            {
                return false;
            }
            Lesson temp = first;
            first = second;
            second = temp;

            this._schedule.Dirty();
            return true;
        }


        /// <summary>
        /// 交换两节指定课程
        /// 指定的星期,节次
        /// </summary>
        /// <param name="firstDOW"></param>
        /// <param name="firstSection"></param>
        /// <param name="secondDOW"></param>
        /// <param name="secondSection"></param>
        /// <returns></returns>
        public bool Exchange2Lesson(int firstDOW, int firstSection, int secondDOW, int secondSection)
        {
            //如果传入的是同一节课,返回失败
            if (firstDOW == secondDOW && firstSection == secondSection)
            {
                return false;
            }
            //如果两节课时同一个教师的,则返回失败
            if (_days[firstDOW][firstSection].TeacherID == _days[secondDOW][secondSection].TeacherID)
            {
                return false;
            }
            




            Teacher temp = _days[firstDOW][firstSection].Teacher;
            _days[firstDOW][firstSection].Teacher = _days[secondDOW][secondSection].Teacher;
            _days[secondDOW][secondSection].Teacher = temp;


            this._schedule.Dirty();
            return true;


        }




  

    }

 


}
