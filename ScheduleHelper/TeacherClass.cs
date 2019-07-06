using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    /// <summary>
    /// 教师任教班级列表,索引器返回的该教师任教的某个班级ID
    /// </summary>
   public  class TeacherClass:IEnumerable<int>
    {
        private List<int> _classList;
        private int _teacherID;

        public TeacherClass(int teacherID)
        {
            _teacherID = teacherID;
            _classList = new List<int>(2);
        }
        /// <summary>
        /// 教师ID
        /// </summary>
        public int TeacherID
        {
            get { return _teacherID; }
            set { _teacherID = value; }
        }

        public void Add(int classID)
        {
            _classList.Add(classID);
        }
        public void Remove(int classID)
        {
            _classList.Remove(classID);
        }

        public bool Contains(int classID)
        {
            return _classList.Contains(classID);
        }

        public int ClassCount
        {
            get { return _classList.Count; }
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (int classID in _classList)
            {
                yield return classID;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 所有教师任教的班级列表,索引器返回的为指定教师的所有班级列表
    /// </summary>
   public  class TeacherClassCollection : IEnumerable<TeacherClass>
    {
        private Dictionary<int,TeacherClass> _teacherClassDictionary;

        public TeacherClassCollection(int capacity = 2)
        {
            _teacherClassDictionary = new Dictionary<int, TeacherClass>(capacity);
        }


        public TeacherClass this[int teacherID]
        {
            get { return _teacherClassDictionary[teacherID]; }
            set { _teacherClassDictionary[teacherID] = value; }
        }

        public void Add(int teacherID, TeacherClass tc)
        {
            _teacherClassDictionary.Add(teacherID,tc);
        }

        public void Add(int teacherID, int classID)
        {
            if (!_teacherClassDictionary.ContainsKey(teacherID))
            {
                _teacherClassDictionary.Add(teacherID, new TeacherClass(teacherID));
            }
            _teacherClassDictionary[teacherID].Add(classID);
        }


        public void Remove(int teacherID)
        {
            _teacherClassDictionary.Remove(teacherID);
        }

        public void Clear()
        {
            _teacherClassDictionary.Clear();
        }


        public IEnumerator<TeacherClass> GetEnumerator()
        {
            foreach (TeacherClass tc in _teacherClassDictionary.Values)
            {
                yield return tc;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
