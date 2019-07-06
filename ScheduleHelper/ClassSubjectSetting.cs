using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class ClassSubjectSetting
    {
        /// <summary>
        /// 学科名字
        /// </summary>
        public string SubjectName { get; set; }
        //public Teacher Teacher { get; set; }
        /// <summary>
        /// 教师姓名
        /// </summary>
        public int TeacherID { get; set; }
        /// <summary>
        /// 周课时
        /// </summary>
        public int LessonsPerWeek { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 是否班主任
        /// </summary>
        public bool Master { get; set; }
        /// <summary>
        /// 是否副科
        /// </summary>
        public bool Minor { get; set; }

        public int Assigned{get;set;}

        public int MaxCountPerDay
        {
            get 
            {
                if (LessonsPerWeek> Global.DayPerWeek)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        
        }


        public int LastCount{
            get
            {
                return LessonsPerWeek-Assigned;
            }
        }

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="subjectName">学科名字</param>
        /// <param name="teacherID">教师编号</param>
        /// <param name="lessonsPerWeek">周课时</param>
        /// <param name="master">是否班主任</param>
        /// <param name="level">优先级</param>
        /// <param name="minor">是否副科</param>
        /// <param name="assigned">已安排节数</param>
        public ClassSubjectSetting(string subjectName, int teacherID = 0, int lessonsPerWeek = 6, bool master = false, int level = 1, bool minor = false, int assigned = 0)
        {
            SubjectName = subjectName;
            TeacherID = teacherID;
            LessonsPerWeek = lessonsPerWeek;
            level = Level;
            Master = master;
            Minor = minor;
            Assigned = assigned;
        }

        public ClassSubjectSetting DeepClone()
        {
            ClassSubjectSetting css = new ClassSubjectSetting(SubjectName, TeacherID, LessonsPerWeek, Master, Level, Minor, Assigned);
            return css;
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other"></param>
        public ClassSubjectSetting(ClassSubjectSetting other)
        {
            this.SubjectName = other.SubjectName;
            this.TeacherID = other.TeacherID;
            this.LessonsPerWeek = other.LessonsPerWeek;
            this.Master = other.Master;
            this.Level = other.Level;
            this.Minor = other.Minor;
            this.Assigned = other.Assigned;
        }


    }

 
    public class ClassSubjectSettingCollection : IEnumerable<ClassSubjectSetting>
    {
        private Dictionary<string, ClassSubjectSetting> _classSubjectSettings;

        public ClassSubjectSettingCollection(int capacity=10)
        {
            _classSubjectSettings = new Dictionary<string, ClassSubjectSetting>(capacity);
        }

        public ClassSubjectSettingCollection DeepClone()
        {
            ClassSubjectSettingCollection cssc = new ClassSubjectSettingCollection(_classSubjectSettings.Count);
            foreach (KeyValuePair<string, ClassSubjectSetting> kvp in _classSubjectSettings)
            {
                cssc._classSubjectSettings.Add(kvp.Key, new ClassSubjectSetting(kvp.Value));
            }
            return cssc;
        }

        public ClassSubjectSettingCollection(ClassSubjectSettingCollection p)
        {
            this._classSubjectSettings = new Dictionary<string, ClassSubjectSetting>(p._classSubjectSettings.Count);
            foreach (KeyValuePair<string, ClassSubjectSetting> kvp in p._classSubjectSettings)
            {
                this._classSubjectSettings.Add(kvp.Key, new ClassSubjectSetting(kvp.Value));
            }
        }



        public ClassSubjectSetting this[string subjectName]
        {
            get { return _classSubjectSettings[subjectName]; }
            set { _classSubjectSettings[subjectName] = value; }
        }





        public void Add(string key, ClassSubjectSetting value)
        {
            _classSubjectSettings.Add(key, value);
        }

        public void Remove(string key)
        {
            _classSubjectSettings.Remove(key);
        }

        public bool ContainKey(string key)
        {
            return _classSubjectSettings.ContainsKey(key);
        
        }
        public bool TryGetValue(string key, out ClassSubjectSetting value)
        {
            return _classSubjectSettings.TryGetValue(key, out value);
        }
        public void Clear()
        {
            _classSubjectSettings.Clear();
        }
        public IEnumerator<ClassSubjectSetting> GetEnumerator()
        {
            foreach (ClassSubjectSetting css in _classSubjectSettings.Values)
            {
                yield return css;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }

    public class GradeSubjectSetting : IEnumerable<ClassSubjectSettingCollection>
    {
        private Dictionary<int,ClassSubjectSettingCollection> _classSubjectSettingCollections;
        public ClassSubjectSettingCollection this[int classID]
        {
            get { return _classSubjectSettingCollections[classID]; }
            set { _classSubjectSettingCollections[classID] = value; }
        }

        public GradeSubjectSetting(int capacity = 25)
        {
            _classSubjectSettingCollections = new Dictionary<int, ClassSubjectSettingCollection>(capacity);
        }


        public GradeSubjectSetting DeepClone()
        {
            GradeSubjectSetting gss= new GradeSubjectSetting(_classSubjectSettingCollections.Count);
            foreach (KeyValuePair<int, ClassSubjectSettingCollection> kvp in _classSubjectSettingCollections)
            {
                gss._classSubjectSettingCollections.Add(kvp.Key, kvp.Value);
            }
            return gss;
        }

        public void Add(int classID,ClassSubjectSettingCollection classSubjectSettingCollection)
        {
            _classSubjectSettingCollections.Add(classID, classSubjectSettingCollection);
        }

        public void Remove(int classID)
        {
            _classSubjectSettingCollections.Remove(classID);
        
        }
        public void Clear()
        {
            _classSubjectSettingCollections.Clear();
        }

        public bool ContainKey(int classID)
        {
            return _classSubjectSettingCollections.ContainsKey(classID);
        
        }

        public bool TryGetValue(int classID, out ClassSubjectSettingCollection value)
        {
            return _classSubjectSettingCollections.TryGetValue(classID, out value);
        }



        public IEnumerator<ClassSubjectSettingCollection> GetEnumerator()
        {
            foreach (ClassSubjectSettingCollection cssc in _classSubjectSettingCollections.Values)
            {
                yield return cssc;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    

}
