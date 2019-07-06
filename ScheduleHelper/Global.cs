using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ScheduleHelper
{
    static class Global
    {

        private static GradeSubjectSetting _gradeSubjectSetting;
        private static TeacherCollection _gradeTeacher;
        private static GradeClassMaster _gradeClassMaster;

        private static int _classCount;
        private static bool _dirty;
        private static bool _empty;

        static Global()
        {
            Random = new Random();
            _classCount = StaticSQLiteHelper.GetClassCount();
            //------------------------------设置运行参数-----------------------------------
            //运行参数

            PopulationSize = 100;    //种群规模

            TotalFitness = PopulationSize * (PopulationSize + 1) / 2;   //种群的总适应度之和.

            TotalEvolveTime = 1000;      //进化的总代数,进化到这么多代后退出

            MutateRate = 60;            //变异概率

            CrossRate = 10;             //交叉概率

            //--------------------------运行参数设置,请勿随意设置--------------------------------

                DayPerWeek = 6;                 //每周上课的天数

                LessonPerMorning = 1;

                LessonPerDay = 6;               //每天安排的节数

                LessonPerForenoon = 4;          //上午安排的节数

                LessonPerNight = 2;     //晚自习安排的节数

            //-------------------------------------------------------------------------------------
                if (!_empty)
                {
                    LoadGradTeachers();             //加载全年级教师信息
                    LoadGradeSubjectSetting();      //加载全年级的课程设置要求
                    LoadGradeClassMaster();         //加载全年级的班主任列表
                    _dirty = false;
                }
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        public static bool Empty
        {
            get { return _empty; }
            set { _empty = value; }
        }
        /// <summary>
        /// 设置或获取晚自习安排的节数
        /// </summary>
        public static int LessonPerNight
        {
            get;
            set;
        }
        /// <summary>
        ///获取班级数量,如果导入过数据,则会重新读取.
        /// </summary>
        public static int ClassCount
        {
            get 
            {
                if (_dirty)
                {
                    _classCount = StaticSQLiteHelper.GetClassCount();
                    _dirty = false; //清除标记
                }
                return _classCount;
            }
        
        }

        //---------------------以下两个参数由高级管理员设置,一般用户最好别调整--------------
        /// <summary>
        /// 设置或获取总进化代数
        /// </summary>
        public static int TotalEvolveTime
        { get; set; }
        /// <summary>
        /// 设置或获取种群规模
        /// </summary>
        public static int PopulationSize
        { get; set; }
        /// <summary>
        /// 设置或获取变异概率
        /// </summary>
        public static int MutateRate
        { get; private set; }
        /// <summary>
        /// 获取交叉概率
        /// </summary>
        public static int CrossRate
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取总适应度
        /// </summary>
        public static int TotalFitness
        {
            get;
            private set;
        }

        //---------------以下这三个参数,用户可以设置-----------------
        /// <summary>
        /// 周课时
        /// </summary>
        public static int DayPerWeek
        { get; set; }
        /// <summary>
        /// 日课时
        /// </summary>
        public static int LessonPerDay
        {get;set; }
        /// <summary>
        /// 上午的课时
        /// </summary>
        public static int LessonPerForenoon
        { get; set; }

        public static int LessonPerMorning
        { get; set; }



        /// <summary>
        /// 返回全年级的课程设置要求
        /// 对其使用索引后返回某班级的课程设置
        /// </summary>
        public static GradeSubjectSetting GradeSubjectSetting
        {
            get{
                if(_gradeSubjectSetting==null)
                {
                    LoadGradeSubjectSetting();
                }
                return _gradeSubjectSetting;
            }
        
        }


        /// <summary>
        /// 返回全年级的教师课表
        /// 对其使用索引后返回某教师的课表
        /// </summary>
        public static TeacherCollection GradeTeachers
        {
            get {

                return _gradeTeacher;
            }
        
        }

        /// <summary>
        /// 获取全年级班主任列表
        /// </summary>
        public static GradeClassMaster GradeClassMaster
        {
            get 
            {
                return _gradeClassMaster;
            }
        }

        /// <summary>
        /// 获取或设置全局随机产生器
        /// </summary>
        public static Random Random
        { get; set; }

        /// <summary>
        /// 强制对象变脏
        /// </summary>
        public static void Dirty()
        {
            _dirty = true;
        }
        /// <summary>
        /// 载入全年级教师信息
        /// </summary>
        public static void LoadGradTeachers()
        {
            DataTable dt = StaticSQLiteHelper.GetTeacherInformation();
            _gradeTeacher = new TeacherCollection(dt.Rows.Count);
            int ti;            
            string tn,sn,ssn;
            foreach (DataRow row in dt.Rows)
            { 
                ti=Convert.ToInt32(row[0]);
                tn=row[1].ToString();
                sn=row[2].ToString();
                ssn=row[3].ToString();
                _gradeTeacher.Add(ti, new Teacher(ti, tn, sn, ssn));
            }
        
        }

        

        /// <summary>
        /// 载入全年级课程设置
        /// </summary>
        public static void LoadGradeSubjectSetting()
        {
            DataTable dt;
            _gradeSubjectSetting = new GradeSubjectSetting(_classCount);
            for (int i = 0; i <= _classCount; i++)
            {
                dt = StaticSQLiteHelper.GetClassSubjectSetting(i);
                ClassSubjectSettingCollection cssc = new ClassSubjectSettingCollection(dt.Rows.Count);
                foreach (DataRow row in dt.Rows)
                {
                    cssc.Add(row[0].ToString(), new ClassSubjectSetting(row[0].ToString(), Convert.ToInt32(row[1]), Convert.ToInt32(row[2]), (bool)row[3],row.IsNull(3)?0:Convert.ToInt32(row[3])));
                }
                _gradeSubjectSetting.Add(i, cssc);
            }

           

        
        
        }

        /// <summary>
        /// 载入年级班主任
        /// </summary>
        public static void LoadGradeClassMaster()
        {
            _gradeClassMaster=new GradeClassMaster(ClassCount);
            DataTable dt = StaticSQLiteHelper.GetGradeClassMaster();
            foreach(DataRow row in dt.Rows)
            {
                _gradeClassMaster.Add(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]));
            }
        }

        /// <summary>
        /// 产生两个不同的值
        /// </summary>
        /// <param name="max">最大值(产生的结果只能在[0,max)之间)</param>
        /// <param name="first">out型第一个参数</param>
        /// <param name="second">out型第二个参数</param>
        public static void RandomGeneric2Value(int max,out int first,out int second)
        {
            first = Random.Next(max);
            second = Random.Next(max);
            while (first == second)
            {
                second = Random.Next(max);
            }
        }


    }




}
