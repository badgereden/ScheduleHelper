using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class Teacher
    {
        private int _teacherID;
        private string _name;
        private string _subjectName;
        private string _shortSubjectName;
        public string Name
        {
            get { return _name; }
            //set { _name = value; }
        }


        public string SubjectName
        {
            get { return _subjectName; }
            //set { _subjectName = value; }
        }

        public int TeacherID
        {
            get { return _teacherID; }
            //set { _teacherID = value; }
        }
        public string ShortSubjectName
        {
            get { return _shortSubjectName; }
            //set { _shortSubjectName = value; }
        }

        public Teacher(int teacherID, string name, string subjectName, string shortSubjectName)
        {
            _teacherID = teacherID;
            _name = name;
            _subjectName = subjectName;
            _shortSubjectName = shortSubjectName;
        }



        public override bool Equals(object t)
        {
            if (t is Teacher)
            {
                Teacher tt = (t as Teacher);
                return this._teacherID == tt._teacherID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this._teacherID;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", _name, _teacherID);
        }
    }

    public class TeacherCollection : IEnumerable<Teacher>
    {
        private Dictionary<int, Teacher> _teachersDictionary;

        public TeacherCollection(int capacity)
        {
            _teachersDictionary = new Dictionary<int, Teacher>(capacity);
        }

        public Teacher this[int teacherID]
        {
            get { return _teachersDictionary[teacherID]; }
            set { _teachersDictionary[teacherID] = value; }
        }
        public void Add(int teacherID, Teacher teacher)
        {
            _teachersDictionary.Add(teacherID, teacher);
        }

        public void Add(int teacherID, string name, string subjectName, string shortSubjectName)
        {
            Teacher t = new Teacher(teacherID, name, subjectName, shortSubjectName);
            Add(teacherID, t);
        }

        public IEnumerator<Teacher> GetEnumerator()
        {
            foreach (Teacher t in _teachersDictionary.Values)
            {
                yield return t;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 班主任列表,创建之后不能再修改.
    /// </summary>
    public class ClassMaster
    {
        private int _classID, _teacherID;
        public int ClassID
        {
            get { return _classID; }
        }
        public int TeacherID
        {
            get { return _teacherID; }
        }
        public ClassMaster(int classID, int teacherID)
        {
            _classID = classID;
            _teacherID = teacherID;
        }
    }

    public class GradeClassMaster : IEnumerable<ClassMaster>
    {
        private Dictionary<int, ClassMaster> _classMasterDic;
        public GradeClassMaster(int capacity = 30)
        {
            _classMasterDic = new Dictionary<int, ClassMaster>(capacity);

        }
        public ClassMaster this[int classID]
        {
            get { return _classMasterDic[classID]; }
            set { _classMasterDic[classID] = value; }
        }
        public void Add(int classID, int teacherID)
        {
            _classMasterDic.Add(classID, new ClassMaster(classID, teacherID));
        }
        public void Remove(int classID)
        {
            _classMasterDic.Remove(classID);
        }
        public bool ContainsKey(int classID)
        {
            return _classMasterDic.ContainsKey(classID);
        }

        public IEnumerator<ClassMaster> GetEnumerator()
        {
            foreach (ClassMaster cm in _classMasterDic.Values)
            {
                yield return cm;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
