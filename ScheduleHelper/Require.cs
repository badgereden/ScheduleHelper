using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    /// <summary>
    /// 规则系统
    /// 如果某个参数为零,意味着所有的对象都适用.
    /// </summary>
    public abstract class Require
    {
        public int Weight { get; set; }
        public abstract int GetWeight(Schedule s);

    }

    /// <summary>
    /// 针对教师的需求设置
    /// </summary>
    public class TeacherRequire : Require
    {
        public int TeacherID { get; set; }
        public int DayOfWeek { get; set; }

        public int Section { get; set; }

        public bool Predication { get; set; }





        public TeacherRequire(int teacherID=-1,int dayOfWeek=-1, int section=-1, bool predication=true,int weight=10)
        {
            TeacherID = teacherID;
            DayOfWeek = dayOfWeek;
            Section = section;
            Predication = predication;
            Weight = weight;
        }



     

        public override int GetWeight(Schedule s)
        {
            int sum = 0; 
            TeacherSchedule ts = s.TeacherSchedule;
            //对所有教师都成立的规则
            if (TeacherID == -1)
            {
                foreach (TeacherLessonCollection tlc in ts)
                {
                    sum += getCount(tlc, DayOfWeek, Section);

                }
            }
            else
            {
                TeacherLessonCollection tlc = ts[TeacherID];
                sum = getCount(tlc, DayOfWeek, Section);
            }



            return sum * Weight;   
        
        }

        private bool weekCondition(int dayOfWeek,int presetDayOfWeek)
        {
            if (presetDayOfWeek == -1)
            {
                return true;
            }

            return presetDayOfWeek == dayOfWeek;
        }

        private bool sectionCondition(int section, int presetSection)
        {
            if (presetSection == -1)
            {
                return true;
            }
            return section == presetSection;
        }

 
        private int getCount(TeacherLessonCollection tlc, int presetDayOfWeek, int presetSection)
        {
            int sum = 0;
            foreach(TeacherLesson tl in tlc )
            {
                if ((weekCondition(tl.DayOfWeek, presetDayOfWeek) && sectionCondition(tl.Section, presetSection))==Predication)
                {
                    sum++;
                }
            
            }
            return sum;
        
        }


    }


    /// <summary>
    /// 针对班主任的需求
    /// </summary>
    public class MasterRequire : Require
    { 
        public int TeacherID { get; set; }
        public bool Master { get; set; }

        public string SubjectName { get; set; }
        public int ClassID { get; set; }
        public int DayOfWeek { get; set; }

        public int Section { get; set; }

        public bool Predication { get; set; }

        public MasterRequire(int teacherID=-1,bool master=false,string subjectName="", int classID=-1, int dayOfWeek=-1, int section=-1, bool predication=true,int weight=3)
        {
            TeacherID = teacherID;
            Master = master;
            SubjectName = subjectName;
            ClassID = classID;
            DayOfWeek = dayOfWeek;
            Section = section;
            Predication = predication;
            Weight = weight;
        }
         public override int GetWeight(Schedule s)
        {
             int sum=0;
             foreach (ClassSchedule cs in s)
             {
                 if ((cs[DayOfWeek][Section].TeacherID == Global.GradeClassMaster[cs.ClassID].TeacherID)==Predication)
                 {
                     sum++;
                 }
             }

            return sum * Weight;
        }
    
    }

    /// <summary>
    /// 班级课时要求,可以设置
    /// 1.指定班级某天的某节课尽量为(或尽量不为)某门课程
    /// 比如周六的第7节课,都为班主任的课
    /// </summary>
    public class ClassReuqire : Require
    { 
        public int ClassID{ get;set;}
        public int DayOfWeek { get; set; }
        public int Section { get; set; }

        public bool Predicate { get; set; }

        //public int TeacherID { get; set; }
        public string ShortSubjectName { get; set; }
        public override int GetWeight(Schedule s)
        {
            if ((s[ClassID][DayOfWeek][Section].ShortSubjectName == ShortSubjectName)== Predicate)
            {
                return Weight;
            }
            else
            {
                return 0;
            }
        }



    }

    /// <summary>
    /// 保证教师的课尽量不冲突
    /// 这个为基本需求,不需要对教师附加该要求
    /// </summary>
    public class ConflictRequire : Require
    {
        public override int GetWeight(Schedule schedule)
        {
            TeacherSchedule ts = schedule.TeacherSchedule;
            int sum = 0;
            foreach (TeacherLessonCollection tc in ts)
            {
                foreach (TeacherLesson tl in tc)
                {
                    if (tl.Count > 1)
                    {
                        sum -= Weight*5;
                    }
                    else
                    {
                        sum += Weight;
                    
                    }
                }
            }
            return sum;
        }
        public ConflictRequire(int weight)
        {
            Weight = weight;
        }
    }
    /// <summary>
    /// 尽量将教师的课程安排在同一个半天
    /// </summary>
    public class TogetherRequire : Require
    {
        public TogetherRequire(int weight)
        {
            Weight = weight;
        
        }
        public override int GetWeight(Schedule s)
        {
            TeacherSchedule ts = s.TeacherSchedule;
            int sum = 0;
            foreach (TeacherLessonCollection tlc in ts)
            {
                sum += tlc.GetTogetherCount();
            
            }
            return sum * Weight;
        }
    
    }

    /// <summary>
    /// 保证教师的上午第四节不会特别多.
    /// </summary>
    public class FairRequire : Require
    {
        public FairRequire(int weight)
        {
            Weight = weight;
        
        }

        public override int GetWeight(Schedule s)
        {
            TeacherSchedule ts = s.TeacherSchedule;
            int sum = 0;
            foreach (TeacherLessonCollection tlc in ts)
            {
                if (tlc.Get4SectionCount() > 3)
                {
                    sum++;
                }
            
            }
            return -sum * Weight;
        }
    }


    /// <summary>
    /// 保证某学科的课集中安排在某个半天
    /// </summary>
    public class SubjectRequire : Require
    {
        int sum = 0;

        /// <summary>
        /// 学科名字
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 星期
        /// </summary>
        public int Week { get; set; }

        public List<int> Weeks { get; set; }
        /// <summary>
        /// 是不是安排在上午
        /// </summary>
        public bool Forenoon { get; set; }

        public SubjectRequire(string subjectName, List<int>weeks, bool forenoon, int weight)
        {
            Weight = weight;
            SubjectName = subjectName;
            Weeks = weeks;
            Forenoon = forenoon;
        }

        public override int GetWeight(Schedule s)
        {
            TeacherSchedule ts = s.TeacherSchedule;
            foreach(TeacherLessonCollection tlc in ts)
            {
                if (Global.GradeTeachers[tlc.TeacherID].SubjectName==SubjectName)
                {
                    foreach (TeacherLesson tl in tlc)
                    {
                        if (Weeks.Contains(tl.DayOfWeek) && tl.Forenoon)
                        {
                            sum += Weight;
                        }
                }

                }
            }

            return sum;
        }

    }
}
