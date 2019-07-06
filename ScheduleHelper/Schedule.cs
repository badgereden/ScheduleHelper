using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ScheduleHelper
{
    public class Schedule:IEnumerable<ClassSchedule>,IComparable<Schedule>
    {
        //私有变量
        private bool _dirty;  //对象修改标志
        private Guid _guid;  
        private List<ClassSchedule> _classes;  //班级列表
        private TeacherSchedule _teacherSchedule;  //存储所有教师的课表
        //private int _conflictClassID = -1;//返回有冲突的班级ID
        private ConflictInformation _conflictInformation = null;
        private int _fitness;


        /// <summary>
        /// 返回有课程冲突的班级ID
        /// </summary>
        //public int ConflictClassID
        //{
        //    get { return _conflictClassID; }
        //    //set { _conflictClassID = value; }
        //}

        public ConflictInformation FirstConflictInformation
        {
            get { return _conflictInformation; }
        }
   

        public Guid GUID
        {
            get { return _guid; }
        }

        /// <summary>
        /// 获取所有教师的课表
        /// </summary>
        public TeacherSchedule TeacherSchedule
        {
            get {
                if (_dirty)
                {
                    GenericTeacherSchedule();
                    _dirty = false;
                }

                return _teacherSchedule;
            
            }
        
        }

        public int Fitness
        {
            get { return _fitness; }
            set { _fitness = value; }
        }



        public Schedule(int classCount=30,int daysPerWeek=6,int lessonsPerDay=7)
        {
            _guid = Guid.NewGuid();
            _teacherSchedule = new ScheduleHelper.TeacherSchedule();
            _classes = new List<ClassSchedule>(classCount);
            AddClasses(classCount, daysPerWeek, lessonsPerDay);
        }

        public void Dirty()
        {
            _dirty = true;
        }



        public Schedule(Schedule other)
        {
            this._guid = other._guid;
            _teacherSchedule = new ScheduleHelper.TeacherSchedule();



            _classes = new List<ClassSchedule>(other._classes.Count);
            foreach (ClassSchedule cs in other._classes)
            {
                _classes.Add(new ClassSchedule(cs, this));
            }
            
        }

        /// <summary>
        /// 根据数据表产生Schedule对象
        /// </summary>
        /// <param name="t"></param>
        public Schedule(DataTable t):this(Global.ClassCount, Global.DayPerWeek, Global.LessonPerDay)
        {
            //根据返回的DataTable对象设置课程
            int ci, dow, section, ti;
            foreach (DataRow row in t.Rows)
            {
                ci = Convert.ToInt32(row[0]);
                dow = Convert.ToInt32(row[1]);
                section = Convert.ToInt32(row[2]);
                if (row.IsNull(3))
                {
                    this._classes[ci][dow][section].Teacher = null;
                }
                else
                {
                    ti = Convert.ToInt32(row[3]);
                    this._classes[ci][dow][section].Teacher = Global.GradeTeachers[ti];
                }
            }
            _dirty = true;
        }

        public List<string> ToSQLStringList(string destinationTable)
        {
            List<string> _sqlList = new List<string>(1000);
            int i=0;
            foreach (ClassSchedule c in this._classes)
            {
                foreach (DaySchedule d in c)
                {
                    foreach (Lesson l in d)
                    {
                        _sqlList.Add(l.ToSQLString(destinationTable, i));
                        i++;
                    }
                }
            }
            return _sqlList;
        }



        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "schedule";
            dt.Columns.Add(new DataColumn("id", typeof(int)));
            dt.Columns.Add(new DataColumn("class_id", typeof(int)));
            dt.Columns.Add(new DataColumn("day_of_week", typeof(int)));
            dt.Columns.Add(new DataColumn("section", typeof(int)));
            dt.Columns.Add(new DataColumn("teacher_id", typeof(int)));
            int i = 0;
            foreach (ClassSchedule c in this._classes)
            {
                foreach (DaySchedule d in c)
                {
                    foreach (Lesson l in d)
                    {
                        if (l.Teacher == null)
                        {
                            dt.Rows.Add(new object[] { i, c.ClassID, d.DayOfWeek, l.Section, null });
                        }
                        else
                        {
                            dt.Rows.Add(new object[]{i,c.ClassID,d.DayOfWeek,l.Section,l.TeacherID});
                        }


                        i++;
                    }
                }
            }
            return dt;
        
        
        }


        public void AddClass(int daysPerWeek=6,int lessonsPerDay=7)
        {
            int count = _classes.Count;
            _classes.Add(new ClassSchedule(this,daysPerWeek,lessonsPerDay,count));
        }

        public void AddClasses( int classCount,int daysPerWeek = 7, int lessonsPerDay = 7)
        {
            int count = classCount - _classes.Count;
            if (count <= 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    AddClass(daysPerWeek, lessonsPerDay);
                }
            }
        
        
        }


         /// <summary>
        /// 交换两个schedule中相同班级
        /// </summary>
        /// <param name="other">另一个schedule,类型Schedule</param>
        /// <param name="classID">班级ID(从零开始)</param>
        public void Exchange2Class(Schedule other, int classID)
        {
            ClassSchedule temp = other._classes[classID];
            other._classes[classID] = _classes[classID];
            _classes[classID] = temp;
            Dirty();
        }

        public void Exchange2Day(int classID, int first, int second)
        {
            _classes[classID].Exchange2Days(first, second);
            Dirty();
        }


        /// <summary>
        /// 批量交换若干班级
        /// </summary>
        /// <param name="other"></param>
        /// <param name="startClassID"></param>
        /// <param name="count"></param>
        public void BatchExchangeClasses(Schedule other, int startClassID, int count)
        {
            int max = startClassID + count > _classes.Count ? _classes.Count : startClassID + count;
            for (int i = startClassID; i < max; i++)
            {
                Exchange2Class(other, i);
            }
            
        }

        /// <summary>
        /// 生成所有教师的课表
        /// </summary>
        /// <returns></returns>
        public void  GenericTeacherSchedule()
        {
            _conflictInformation = null;
            _teacherSchedule.Clear();
            foreach (ClassSchedule cs in this._classes)
            {
                foreach (DaySchedule ds in cs)
                { 
                    foreach(Lesson l in ds)
                    {
                          if(l!=null && l.TeacherID!=0)
                        {
                            if (_teacherSchedule.Add(l.TeacherID, l.DayOfWeek, l.Section, l.ClassID))
                            {
                                //_conflictClassID = l.ClassID;

                                _conflictInformation = new ConflictInformation(l.ClassID,l.SubjectName);

                                l.Conflict = true;
                            }
                            else
                            {
                                l.Conflict = false;
                            }

                        }
                    }
                }
            }
        }



        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public ClassSchedule this[int classID]
        {
            get { return _classes[classID]; }
            set { _classes[classID] = value; }
        }


        
        public IEnumerator<ClassSchedule> GetEnumerator()
        {
            foreach (ClassSchedule cs in _classes)
            {
                yield return cs;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CompareTo(Schedule other)
        {
            if (this.Fitness > other.Fitness)
            {
                return 1;
            }
            else if (this.Fitness == other.Fitness)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

     

    }




}
