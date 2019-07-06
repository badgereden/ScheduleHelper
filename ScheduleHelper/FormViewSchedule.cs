using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduleHelper
{
    public partial class FormViewSchedule : Form
    {
        //所有的课表信息都缓存在该对象中
        private Schedule schedule;
        //晚自习课表
        private Schedule nightSchedule;

        private Schedule morningSchedule;
        //当前班级ID(从0开始)
        private int currentClassID;
        private OperationState operationState;  //当前操作状态
        private bool hold = false;  //是否持有一节课.
        private int holdCellRowIndex, holdCellColumnIndex; //持有的单元格行列号
        private int currentCellRowIndex, currentCellColumnIndex; //鼠标所在单元格的行列号
        private List<int> classList;
        private DataGridViewCellStyle lockedStyle;
        private ConflictInformation firstConflictInformation;
        //private int firstConflictClassID;   //第一个有冲突的班级ID
        private DataGridViewCellStyle styleForeColorRed;
        private bool night = false;
        private bool holdNight = false;
        private bool morning = false;
        private bool holdMoring = false;
        private int section;

        public FormViewSchedule()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            styleForeColorRed = new DataGridViewCellStyle();
            styleForeColorRed.ForeColor = Color.Red;
            schedule = new Schedule(StaticSQLiteHelper.GetSchedule());
            nightSchedule = new Schedule(StaticSQLiteHelper.GetNightSchedule());
            morningSchedule = new Schedule(StaticSQLiteHelper.GetMorningSchedule());
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TestConflict();

            lockedStyle = new DataGridViewCellStyle();
            lockedStyle.BackColor = dgv.DefaultCellStyle.SelectionBackColor;
            lockedStyle.ForeColor = dgv.DefaultCellStyle.SelectionForeColor;
            

            LoadClassList();

            adjustDataGridViewStyle();

            loadClassSchedule(0);

            rbExchange.Checked = true;

            loadHistory();


        }

        /// <summary>
        /// 调整DataGridView的样式和格式
        /// </summary>
        private void adjustDataGridViewStyle()
        {
            string[] weekDays = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
            for (int i = 0; i < Global.DayPerWeek; i++)
            {
                dgv.Columns.Add(weekDays[i], weekDays[i]);
                dgv.Columns[i].Width = 80;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //增加早自习
            for (int i = 0; i < Global.LessonPerMorning; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
                dgv.Rows[i].Height = 30;
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //增加白天课程
            for (int i = 0; i < Global.LessonPerDay; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i + Global.LessonPerMorning].HeaderCell.Value = (i + 1).ToString();
                dgv.Rows[i + Global.LessonPerMorning].Height = 30;
                dgv.Rows[i + Global.LessonPerMorning].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            //增加晚自习
            for (int i = 0; i < Global.LessonPerNight; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i + Global.LessonPerMorning + Global.LessonPerDay].HeaderCell.Value = (i + 1).ToString();
                dgv.Rows[i + Global.LessonPerMorning + Global.LessonPerDay].Height = 30;
                dgv.Rows[i + Global.LessonPerMorning + Global.LessonPerDay].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgv.RowHeadersWidth = 46;
            dgv.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 30;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 9, FontStyle.Bold);
            dgv.Rows[ Global.LessonPerMorning - 1].DividerHeight = 3;
            dgv.Rows[Global.LessonPerForenoon + Global.LessonPerMorning - 1].DividerHeight = 1;
            dgv.Rows[Global.LessonPerDay + Global.LessonPerMorning - 1].DividerHeight = 3;
        }




        /// <summary>
        /// 添加班级列表的链接
        /// </summary>
        public void LoadClassList()
        {
            classList = new List<int>();
            foreach (ClassSchedule cs in schedule)
            {
                classList.Add(cs.ClassID + 1);
            }
            foreach(int ci in classList)
            {
                LinkLabel ll = new LinkLabel();
                ll.Text = ci.ToString() + "班";
                ll.Size = new Size(30, 20);
                ll.Click += new EventHandler((obj, eve) =>
                {
                    currentClassID = ci-1;
                    loadClassSchedule(currentClassID);
                });
                ll.Visible = true;
                ll.Location = new Point(46 * ((ci-1)%12)+40, 20*((ci-1)/12)+30);
                groupBox1.Controls.Add(ll);
                
            }
        }



        /// <summary>
        /// 根据传入的班级ID载入班级课表,班级ID从0开始
        /// </summary>
        /// <param name="classID"></param>
        private void loadClassSchedule(int classID)
        {
            hold = false;

            Lesson l;
            for (int ri = 0; ri <  Global.LessonPerMorning; ri++)
            {
                for (int ci = 0; ci < Global.DayPerWeek; ci++)
                {
                    l = morningSchedule[currentClassID][ci][ri];
                    dgv.Rows[ri].Cells[ci].Value = l.ShortSubjectName;
                    dgv.Rows[ri].Cells[ci].ToolTipText = l.TeacherName;
                    //dgv.Rows[ri].Cells[ci].ContextMenuStrip = cmsMenu;
                    if (l.Conflict)
                    {
                        dgv.Rows[ri].Cells[ci].Style = styleForeColorRed;
                    }
                    else
                    {
                        dgv.Rows[ri].Cells[ci].Style = dgv.DefaultCellStyle;
                    }
                }
            }



            for (int ri = Global.LessonPerMorning; ri <Global.LessonPerDay+Global.LessonPerMorning; ri++)
            {
                for (int ci = 0; ci < Global.DayPerWeek; ci++)
                {
                    l = schedule[currentClassID][ci][ri-Global.LessonPerMorning];
                    dgv.Rows[ri].Cells[ci].Value = l.ShortSubjectName;
                    dgv.Rows[ri].Cells[ci].ToolTipText = l.TeacherName;
                    //dgv.Rows[ri].Cells[ci].ContextMenuStrip = cmsMenu;
                    if (l.Conflict)
                    {
                        dgv.Rows[ri].Cells[ci].Style = styleForeColorRed;
                    }
                    else
                    {
                        dgv.Rows[ri].Cells[ci].Style = dgv.DefaultCellStyle;
                    }
                }
            }

            for (int ri = 0; ri < Global.LessonPerNight; ri++)
            {
                for (int ci = 0; ci < Global.DayPerWeek; ci++)
                {
                    l = nightSchedule[currentClassID][ci][ri];
                    dgv.Rows[ri + Global.LessonPerMorning + Global.LessonPerDay].Cells[ci].Value = l.ShortSubjectName;
                    dgv.Rows[ri + Global.LessonPerMorning + Global.LessonPerDay].Cells[ci].ToolTipText = l.TeacherName;
                    if (l.Conflict)
                    {
                        dgv.Rows[ri].Cells[ci].Style = styleForeColorRed;
                    }
                    else
                    {
                        dgv.Rows[ri].Cells[ci].Style = dgv.DefaultCellStyle;
                    }
                }
            }

            loadContextMenuItems();
            this.Text = (classID + 1).ToString() + "班课表";
        }
 
        public enum OperationState
        { 
            None=0,
            ExchangeMode=1,
            ReforceMode=2
        }

        private void rbExchange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExchange.Checked)
            {
                operationState = OperationState.ExchangeMode;
            }
        }

        private void rbReforce_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReforce.Checked)
            {
                operationState = OperationState.ReforceMode;
            }
        }


        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex < Global.LessonPerMorning)
                {
                    morning = true;
                    night = false;
                    section = e.RowIndex;
                }
                else if (e.RowIndex < Global.LessonPerDay+Global.LessonPerMorning )
                {
                    morning = false;
                    night = false;
                    section = e.RowIndex-Global.LessonPerMorning;
                }
                else 
                {
                    night = true;
                    holdMoring = false;
                    section = e.RowIndex - Global.LessonPerDay-Global.LessonPerMorning;
                }


                if (operationState == OperationState.ExchangeMode)
                {
                    if (hold)
                    {
                        
                        //如果要交换的两节课为同一门课或者要交换的两节课一节是白天一节是晚自习,都返回失败
                        //if( (e.RowIndex == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex)|| (e.RowIndex -Global.LessonPerMorning == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex) || (e.RowIndex-Global.LessonPerDay-Global.LessonPerMorning == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex) ||(night!=holdNight)||(morning!=holdMoring))
                        if ((night != holdNight) || (morning != holdMoring)
                            ||(night &&e.RowIndex-Global.LessonPerMorning-Global.LessonPerDay == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex)
                            || (morning && e.RowIndex == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex)
                            || (!morning &&!night && e.RowIndex - Global.LessonPerMorning == holdCellRowIndex && e.ColumnIndex == holdCellColumnIndex)

                            )
                        {
                            setHoldState(false);
                            tslExchange.Text = "不能更换";
                        }
                        else
                        {
                            if (morning)
                            {
                                //交换缓存的课程表中的数据
                                morningSchedule[currentClassID].Exchange2Lesson(e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                                //更新数据库中数据
                                StaticSQLiteHelper.Exchange2MorningLesson(currentClassID, e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                            }
                            if (night)
                            {
                                //交换缓存的课程表中的数据
                                nightSchedule[currentClassID].Exchange2Lesson(e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                                //更新数据库中数据
                                StaticSQLiteHelper.Exchange2NightLesson(currentClassID, e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                            }
                            else
                            {
                                //交换缓存的课程表中的数据
                                schedule[currentClassID].Exchange2Lesson(e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                                //更新数据库中数据

                                ///有问题.....
                                StaticSQLiteHelper.Exchange2Lesson(currentClassID,e.ColumnIndex, section, holdCellColumnIndex, holdCellRowIndex);
                            }
                                //重新加载以刷新界面
                            loadClassSchedule(currentClassID);
                            loadHistory();
                            setHoldState(false);

                            TestConflict();
                        }
                    }
                    else
                    {
                        setHoldState(true);
                    }
                }
                else if (operationState == OperationState.ReforceMode)
                {
                    selectCell(e.RowIndex, e.ColumnIndex, true);
                    cmsMenu.Show(dgv, e.X + dgv.CurrentCell.Size.Width * e.ColumnIndex+dgv.RowHeadersWidth, e.Y + dgv.CurrentCell.Size.Height * e.RowIndex+dgv.ColumnHeadersHeight);
                 }

            }



        }

        private void TestConflict()
        {
            schedule.GenericTeacherSchedule();
            //返回第一个有冲突的班级ID
            //firstConflictClassID = this.schedule.ConflictClassID;
            firstConflictInformation = this.schedule.FirstConflictInformation;
            if (firstConflictInformation==null)
            {
                tslConflict.Text = "没有任何冲突课程";
            }
            else
            {
                tslConflict.Text = string.Format("{0}班{1}有冲突",firstConflictInformation.ConflictClassID+1,firstConflictInformation.SubjectName);
            }
        }

        /// <summary>
        /// 选中指定行列号的cell
        /// </summary>
        /// <param name="ri">行号（从0开始）</param>
        /// <param name="ci">列号（从0开始）</param>
        /// <param name="unselectOthers">是否反选其它cell</param>
        private void selectCell(int ri,int ci,bool unselectOthers)
        {
            if (unselectOthers)
            {
                unselectAll();
            }
            dgv.Rows[ri].Cells[ci].Selected = true;
        
        }

        /// <summary>
        /// 反选所有的cell
        /// </summary>
        private void unselectAll()
        {
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                cell.Selected = false;
            }
        
        }
        /// <summary>
        /// 重置指定单元格样式
        /// </summary>
        /// <param name="ri">行号（从0开始）</param>
        /// <param name="ci">列号（从0开始）</param>
        private void resetCellStyle(int ri, int ci)
        {
            dgv.Rows[ri].Cells[ci].Style = dgv.DefaultCellStyle;
        }

        /// <summary>
        /// 将指定单元格设置为选中的样式
        /// </summary>
        /// <param name="ri">行号（从0开始）</param>
        /// <param name="ci">列号（从0开始）</param>
        private void setLockedStyle(int ri, int ci)
        {
            dgv.Rows[ri].Cells[ci].Style = lockedStyle;
        }

        private void dgv_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            currentCellRowIndex = e.RowIndex;
            currentCellColumnIndex = e.ColumnIndex;
        }

        private void cmsMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            bool night = false;
            int section;
            string sn = e.ClickedItem.Text;
            int dow = currentCellColumnIndex;
            if (currentCellRowIndex < Global.LessonPerMorning)
            {
                morning = true;
                night = false;
                section = currentCellRowIndex;
            }
            else  if (currentCellRowIndex < Global.LessonPerDay+Global.LessonPerMorning)
            {
                morning = false;
                night = false;
                section = currentCellRowIndex - Global.LessonPerMorning;
            }
            else
            {
                night = true;
                morning = false;
                section = currentCellRowIndex - Global.LessonPerDay - Global.LessonPerMorning;
            }
            int ti = (int)e.ClickedItem.Tag;
            if (morning)
            {
                StaticSQLiteHelper.ReforceSetMorningLesson(currentClassID, dow, section, ti);
            }
            else if (night)
            {
                StaticSQLiteHelper.ReforceSetNightLesson(currentClassID, dow, section, ti);
            }
            else
            {
                StaticSQLiteHelper.ReforceSetLesson(currentClassID, dow, section, ti);
            }

            if (ti == 0)
            {
                dgv.Rows[currentCellRowIndex].Cells[currentCellColumnIndex].Value = "";
            }
            else
            {
                dgv.Rows[currentCellRowIndex].Cells[currentCellColumnIndex].Value = sn.Substring(0, 1);
            }

            loadHistory();

        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Lesson l;
            if (e.RowIndex < Global.LessonPerMorning)
            {
                l = morningSchedule[currentClassID][e.ColumnIndex][e.RowIndex];
            }
            else if (e.RowIndex < Global.LessonPerDay+Global.LessonPerMorning)
            {
                l = schedule[currentClassID][e.ColumnIndex][e.RowIndex-Global.LessonPerMorning];
            }
            else //if (e.RowIndex<Global.LessonPerMorning+Global.LessonPerDay+Global.LessonPerNight)
            {
                l = nightSchedule[currentClassID][e.ColumnIndex][e.RowIndex - Global.LessonPerDay-Global.LessonPerMorning];
            }
            int teacherID = l.TeacherID;
            string teacherName = l.TeacherName;
            if (teacherID == 0)
            {
                return;
            }
            FormViewTeacherSchedule fvts = new FormViewTeacherSchedule(teacherID,teacherName);
            fvts.ShowDialog();
        }

        private void loadContextMenuItems()
        {
            cmsMenu.Items.Clear();
            foreach (ClassSubjectSetting css in Global.GradeSubjectSetting[currentClassID])
            {
                var item=cmsMenu.Items.Add(css.SubjectName);
                item.Tag = css.TeacherID;
                if (css.Master)
                {
                    item.ForeColor = Color.Red;
                }
            }
            //增加"自习"选项
            var xitem = cmsMenu.Items.Add("自习");
            xitem.Tag = 0;
        }

        private void setHoldState(bool state)
        {
            if (state)
            {
                hold = true;
                //改变鼠标样式
                dgv.Cursor = Cursors.Hand;
                //缓存持有的课程坐标
                holdCellColumnIndex = currentCellColumnIndex;
                if (currentCellRowIndex < Global.LessonPerMorning)
                {
                    holdMoring = false;
                    holdCellRowIndex = currentCellRowIndex;
                    tslExchange.Text = dgv.Rows[currentCellRowIndex].Cells[currentCellColumnIndex].Value.ToString()+"[早]";

                }
                else if(currentCellRowIndex < Global.LessonPerDay+Global.LessonPerMorning)
                {
                    holdNight = false;
                    holdCellRowIndex = currentCellRowIndex-Global.LessonPerMorning;
                    tslExchange.Text = dgv.Rows[currentCellRowIndex].Cells[currentCellColumnIndex].Value.ToString();
                }
                else
                {
                    holdNight = true;
                    holdCellRowIndex = currentCellRowIndex - Global.LessonPerDay-Global.LessonPerMorning;
                    tslExchange.Text = dgv.Rows[currentCellRowIndex].Cells[currentCellColumnIndex].Value.ToString() + "[晚]";
                }
                selectCell(currentCellRowIndex, currentCellColumnIndex, true);
                //更新界面
                
            }
            else
            {
                hold = false;
                holdNight = false;
                unselectAll();
                dgv.Cursor = Cursors.Default;
                tslExchange.Text = "更换成功";
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            //直接根据选中的项的数量来还原
            string result = null;
            int n = clbHistory.CheckedItems.Count;
            for (int i = 0; i < n; i++)
            { 
                result=StaticSQLiteHelper.Recovery();
            }

            //string result=StaticSQLiteHelper.Recovery();
            if (result == null)
            {
                MessageBox.Show("已经没有可以撤销的操作！");
            }
            else
            {
                //MessageBox.Show(result);

                schedule = new Schedule(StaticSQLiteHelper.GetSchedule());
                nightSchedule = new Schedule(StaticSQLiteHelper.GetNightSchedule());
                loadClassSchedule(currentClassID);
                loadHistory();
            }

        }
        private void loadHistory()
        {
            clbHistory.Items.Clear();
            DataTable dt = StaticSQLiteHelper.ExecuteQuery("select time,description from history order by id desc");
            foreach (DataRow r in dt.Rows)
            {
                clbHistory.Items.Add(r[1]);
            }
            if(clbHistory.Items.Count!=0)
            {
                clbHistory.SetItemChecked(0, true);

            }
           
        }

        private void clbHistory_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            clbHistory.ItemCheck -= new ItemCheckEventHandler(clbHistory_ItemCheck);
            if (e.NewValue == CheckState.Unchecked)
            {
                e.NewValue = CheckState.Checked;
            }
                for (int i = 0; i < e.Index; i++)
                {
                    clbHistory.SetItemChecked(i, true);
                }
            
            for (int j = e.Index; j < clbHistory.Items.Count; j++)
            {
                clbHistory.SetItemChecked(j, false);
            }

            clbHistory.ItemCheck += new ItemCheckEventHandler(clbHistory_ItemCheck);

            //clbHistory.ClearSelected();
            //for (int i = 0; i < e.Index; i++)
            //{
            //    clbHistory.SetItemChecked(i, true);
            //}

        }


    }
}
