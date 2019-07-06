using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class Gas
    {
        private List<Schedule> _population;//种群
        private List<Schedule> _newPopulation;//新一代的种群
        private int _populationSize;//种群的规模
        private int _evolveTimes; //目前已经进化的代数
        private int _bestIndex;   //最佳个体的索引
        private int _worstIndex;    //最差个体的索引
        private List<Require> _requestCollection; //需求集合
        private Schedule[] _selectedSchedule;   //选中的个体集合
        private int _fitnessSum, _fitnessAvg, _fitnessMax, _fitnessMin; //几个临时变量,适应度的和,平均值,最大值和最小值



        public event EvolvedHandler Evolved;

        /// <summary>
        /// 最佳个体
        /// </summary>
        public Schedule Best
        {
            get 
            {
                return _population[_bestIndex];
            }
        }
        /// <summary>
        /// 最差个体
        /// </summary>
        public Schedule Worst
        {
            get 
            {
                return _population[_worstIndex];
            }
        }

        public Gas()
        {
            _evolveTimes = 0;
            _populationSize = Global.PopulationSize;
            _population = new List<Schedule>(_populationSize);
            _newPopulation = new List<Schedule>(_populationSize);

            _requestCollection = new List<Require>();
            _selectedSchedule = new Schedule[_populationSize];
        }


        /*
         * 本函数只在遗传算法启动时执行一次,对性能影响不大,应该尽量使
         * 初始值更接近最终解.
         * 
        */
        /// <summary>
        /// 初始化种群,产生初始化课表.
        /// </summary>
        private void Initialize()
        {
            for (int i = 0; i < _populationSize; i++)
            {
                Schedule s = new Schedule(Global.ClassCount, Global.DayPerWeek, Global.LessonPerDay);

                    foreach (ClassSchedule cs in s)
                    {
                        cs.InitializeSchedule(Global.GradeSubjectSetting[cs.ClassID]);
                    }
                _population.Add(s);
            }
            
            //模拟载入需求
            LoadRequire();
        }


        /// <summary>
        /// 加载需求系统
        /// </summary>
        private void LoadRequire()
        {
            //班主任周一的第7节都不安排
            //_requestCollection.Add(new MasterRequire(-1, true, "", -1, 0, 6, false, 100));

            //班主任的周六的第7节都安排
           // _requestCollection.Add(new MasterRequire(-1, true, "", -1, 5, 6, true, 300));

            //尽量不冲突
            _requestCollection.Add(new ConflictRequire(20));

            //_requestCollection.Add(new )


            //将教师的课尽量安排在同一个半天
            _requestCollection.Add(new TogetherRequire(10));

            //保证一个教师的第四节不会太多
            _requestCollection.Add(new FairRequire(200));


            //英语老师课尽量安排在周六上午.
            _requestCollection.Add(new SubjectRequire("英语", new List<int>(1) { 5 }, true, 3));


            //所有教师都不上周五第七节课
            _requestCollection.Add(new TeacherRequire(-1, 4, 6, false, 10));
            _requestCollection.Add(new TeacherRequire(-1, 2, 6, false, 10));
            _requestCollection.Add(new TeacherRequire(-1, 0, 6, false, 10));

        }



        /// <summary>
        /// 进化一代
        /// </summary>
        private void Evolve()
        {
            _population.Clear();
            foreach (Schedule s in _newPopulation)
            {
                _population.Add(Mutate(s));
            }
            _newPopulation.Clear();
            _evolveTimes++;
            //如果有事件订阅者,以下事件才会触发
            if (Evolved != null)
            {
                Evolved(new EvolvedArgs(_evolveTimes, _fitnessMax, _fitnessMin, _fitnessSum, _fitnessAvg));
            }
        }

        /// <summary>
        /// 交叉算子,双亲交配并产生下一代种群
        /// </summary>
        private void Cross()
        {
            int first, second;
            for (int i = 0; i < _populationSize; i++)
            {
                Schedule s;
                //交叉概率80%
                if (Global.Random.Next(100) < Global.CrossRate)
                {
                    Global.RandomGeneric2Value(_populationSize, out first, out second);

                    //切分的标志,标志以前用第一个个体的基因,标志之后用第二个个体的基因.
                    int f = Global.Random.Next(Global.ClassCount);

                    s = new Schedule(Global.ClassCount);
                    for (int j = 0; j < f; j++)
                    {
                        s[j] = new ClassSchedule(_selectedSchedule[first],Global.DayPerWeek,Global.LessonPerDay);
                    }
                    for (int j = f; j < Global.ClassCount; j++)
                    {
                        s[j] = new ClassSchedule(_selectedSchedule[second],Global.DayPerWeek,Global.LessonPerDay);
                    }
                    s.Dirty();
                }
                else
                {
                    s = new Schedule(_selectedSchedule[Global.Random.Next(_populationSize)]);
                }
                //将新个体放入新种群
                _newPopulation.Add(s);
            }
        
        }

        /// <summary>
        /// 变异算子,内部调用交换算子实现
        /// 变异概率20%
        /// </summary>
        private Schedule Mutate(Schedule s)
        {
            int ci, d;

            //调换概率20%
            if (Global.Random.Next(100) < Global.MutateRate)
            {

                ci = Global.Random.Next(Global.ClassCount);
                s[ci].RandomExchange2Days();
                s.Dirty();
            }

            //微调概率20%
            if (Global.Random.Next(100) < Global.MutateRate)
            { 
                ci = Global.Random.Next(Global.ClassCount);
                d = Global.Random.Next(Global.DayPerWeek);
                s[ci][d].RandomExchange2Lesson();
                s.Dirty();
            }
            return s;
        }

        /// <summary>
        /// 整个遗传算法的框架
        /// </summary>
        public void Genetic()
        {
            //初始化种群
            Initialize();
            for (int i = 0; i < Global.TotalEvolveTime; i++)
            {
                //评估函数
                Evaluate();

                //选择算子
                Select();

                //交叉算子
                Cross();

                //进化一代
                Evolve();

            }

            //完成遗传算法的后续处理

            
        }
        /// <summary>
        /// 换位算子,将一段基因交换
        /// 调用Schedule下某个班级的交换随机两天
        /// </summary>
        private void Exchange(Schedule schedule)
        {
            int ci = Global.Random.Next(Global.ClassCount);
            int first, second;
            Global.RandomGeneric2Value(Global.DayPerWeek, out first, out second);
            schedule[ci].Exchange2Days(first, second);
            schedule.Dirty();
        }

        /// <summary>
        /// 换位算子,调动幅度更小,只是更换一天的两节课
        /// </summary>
        /// <param name="schedule"></param>
        private void TinyExchange(Schedule schedule)
        {
            int ci  = Global.Random.Next(Global.ClassCount);
            int d = Global.Random.Next(Global.DayPerWeek);
            int first,second;
            Global.RandomGeneric2Value(Global.LessonPerDay,out first,out second);
            schedule[ci][d].RandomExchange2Lesson();
        }




        /// <summary>
        /// 选择算子,控制个体基因被遗传到下一代的几率
        /// 适应度高的个体的基因被遗产到下一代的几率更大一些.
        /// </summary>
        private void Select()
        {
            //先计算种群的总适应度
            _population.Sort();
            int sum = Global.TotalFitness;

            //选中的个体才有机会交叉变异并遗传到下一代
            for (int i = 0; i < _populationSize; i++)
            {
                int s = Roulette(_populationSize, sum);
                _selectedSchedule[i] = _population[s];
            }
        }


        /// <summary>
        /// 评估函数,评估每个个体的适应度
        /// 
        /// </summary>
        /// <returns></returns>
        private void  Evaluate()
        {
            int fitness = 0;
            
            //---以下四个数据作为统计之用
            _fitnessAvg = 0;
            _fitnessSum = 0;
            _fitnessMax = int.MinValue;
            _fitnessMin = int.MaxValue;

            //_fitnessList.Clear();
            for(int i=0;i<_populationSize;i++)
            {
                fitness = 0;

               
                foreach (Require r in _requestCollection)
                {
                    fitness += r.GetWeight(_population[i]);
                }

                _population[i].Fitness = fitness;
                _fitnessSum += fitness;


                if (fitness > _fitnessMax)
                {
                    _fitnessMax = fitness;
                    _bestIndex = i;
                }

                if (fitness < _fitnessMin)
                {
                    _fitnessMin = fitness;
                    _worstIndex = i;
                }
            }
            _fitnessAvg = _fitnessSum / _populationSize;
        }



        /// <summary>
        /// 根据指定索引获取或设置Schedule对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Schedule this[int index]
        {
            get { return _population[index]; }
            set { _population[index] = value; }
        }

        /// <summary>
        /// 轮盘赌算法
        /// </summary>
        /// <param name="fitness">个体适应度列表</param>
        /// <param name="sum">种群总适应度</param>
        /// <returns>被选中个体的索引</returns>
        private int Roulette(IList<int> fitness, int sum)
        {
            if (sum < 0)
            {
                sum = 0;
            }
            int r = Global.Random.Next(sum);
            int _sum = 0;
            int index = 0;
            for (int i = 0; i < fitness.Count; i++)
            {
                _sum += fitness[i];
                if (_sum > r)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        /// <summary>
        /// 轮盘赌算法
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="sum">所有个体之和</param>
        /// <returns></returns>
        private int Roulette(int max,int sum)
        {
            int _sum = 0;
            int r = Global.Random.Next(sum) + 1;
            int index = 0;
            for (int i = 1; i <= max; i++)
            {
                _sum += i;
                if (_sum >= sum)
                {
                    index = i;
                    break;
                }
            }
            return index-1;
        }



    }
    /// <summary>
    /// 已进化一代,进化完一代后触发
    /// </summary>
    /// <param name="e"></param>
    public delegate void EvolvedHandler(EvolvedArgs e);
    public class EvolvedArgs : System.EventArgs
    {
        /// <summary>
        /// 获取当前所处的进化代数
        /// </summary>
        public int CurrentTimes { get; private set; }
        /// <summary>
        /// 获取种群中的最大适应度
        /// </summary>
        public int FitnessMax { get; private set; }
        /// <summary>
        /// 获取种群中的最小适应度
        /// </summary>
        public int FitnessMin { get; private set; }
        /// <summary>
        /// 获取种群的适应度之和
        /// </summary>
        public int FitnessSum { get; private set; }
        /// <summary>
        /// 获取种群的平均适应度
        /// </summary>

        public int FitnessAvg { get; private set; }

       // public bool Cancel { get; set; }

        public EvolvedArgs(int currentTimes, int fitnessMax, int fitnessMin, int fitnessSum, int fitnessAvg)
        {
            CurrentTimes = currentTimes;
            FitnessMax = fitnessMax;
            FitnessMin = fitnessMin;
            FitnessSum = fitnessSum;
            FitnessAvg = fitnessAvg;
        }
    }


}
