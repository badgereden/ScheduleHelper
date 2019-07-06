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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void 生成课表GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGenerate fg = new FormGenerate();
            
                fg.MdiParent = this;
                fg.Show();
            
        }

        private void 查看课表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormViewSchedule fvs = new FormViewSchedule();
            
                fvs.MdiParent = this;
                fvs.Show();
            
        }

        private void 查询教师课表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormViewTeacherSchedule fvts = new FormViewTeacherSchedule();
                fvts.MdiParent = this;
                fvts.Show();
            
        }

        private void tsmiImport_Click(object sender, EventArgs e)
        {
            FormImportExcel fie = new FormImportExcel();
            fie.MdiParent = this;
            fie.Show();
        }

        private void 调整教师ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChangeTeacher fct = new FormChangeTeacher();
            fct.MdiParent = this;
            fct.Show();
        }

        private void tsmiExport_Click(object sender, EventArgs e)
        {
            FormReport fr = new FormReport();
            fr.MdiParent = this;
            fr.Show();
        }

        private void tsmiCheck_Click(object sender, EventArgs e)
        {
            FormCheck formCheck = new FormCheck();
            formCheck.Show();
        }
    }
}
