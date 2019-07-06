namespace ScheduleHelper
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExport = new System.Windows.Forms.ToolStripMenuItem();
            this.管理课表MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiViewSchdule = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiViewTeacherSchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneriate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.基础数据管理BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImportNightWork = new System.Windows.Forms.ToolStripMenuItem();
            this.课时统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.管理课表MToolStripMenuItem,
            this.基础数据管理BToolStripMenuItem,
            this.课时统计ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1044, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiImport,
            this.tsmiExport});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // tsmiImport
            // 
            this.tsmiImport.Name = "tsmiImport";
            this.tsmiImport.Size = new System.Drawing.Size(181, 26);
            this.tsmiImport.Text = "导入(&I)";
            this.tsmiImport.Click += new System.EventHandler(this.tsmiImport_Click);
            // 
            // tsmiExport
            // 
            this.tsmiExport.Name = "tsmiExport";
            this.tsmiExport.Size = new System.Drawing.Size(181, 26);
            this.tsmiExport.Text = "导出(&E)";
            this.tsmiExport.Click += new System.EventHandler(this.tsmiExport_Click);
            // 
            // 管理课表MToolStripMenuItem
            // 
            this.管理课表MToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiViewSchdule,
            this.tsmiViewTeacherSchedule,
            this.tsmiGeneriate,
            this.tsmiCheck});
            this.管理课表MToolStripMenuItem.Name = "管理课表MToolStripMenuItem";
            this.管理课表MToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.管理课表MToolStripMenuItem.Text = "管理课表(&M)";
            // 
            // tsmiViewSchdule
            // 
            this.tsmiViewSchdule.Name = "tsmiViewSchdule";
            this.tsmiViewSchdule.Size = new System.Drawing.Size(181, 26);
            this.tsmiViewSchdule.Text = "查看课表";
            this.tsmiViewSchdule.Click += new System.EventHandler(this.查看课表ToolStripMenuItem_Click);
            // 
            // tsmiViewTeacherSchedule
            // 
            this.tsmiViewTeacherSchedule.Name = "tsmiViewTeacherSchedule";
            this.tsmiViewTeacherSchedule.Size = new System.Drawing.Size(181, 26);
            this.tsmiViewTeacherSchedule.Text = "查询教师课表";
            this.tsmiViewTeacherSchedule.Click += new System.EventHandler(this.查询教师课表ToolStripMenuItem_Click);
            // 
            // tsmiGeneriate
            // 
            this.tsmiGeneriate.Name = "tsmiGeneriate";
            this.tsmiGeneriate.Size = new System.Drawing.Size(181, 26);
            this.tsmiGeneriate.Text = "生成课表(&G)";
            this.tsmiGeneriate.Click += new System.EventHandler(this.生成课表GToolStripMenuItem_Click);
            // 
            // tsmiCheck
            // 
            this.tsmiCheck.Name = "tsmiCheck";
            this.tsmiCheck.Size = new System.Drawing.Size(181, 26);
            this.tsmiCheck.Text = "检查课表";
            this.tsmiCheck.Click += new System.EventHandler(this.tsmiCheck_Click);
            // 
            // 基础数据管理BToolStripMenuItem
            // 
            this.基础数据管理BToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChangeTeacher,
            this.tsmiImportNightWork});
            this.基础数据管理BToolStripMenuItem.Name = "基础数据管理BToolStripMenuItem";
            this.基础数据管理BToolStripMenuItem.Size = new System.Drawing.Size(130, 24);
            this.基础数据管理BToolStripMenuItem.Text = "基础数据管理(&B)";
            // 
            // tsmiChangeTeacher
            // 
            this.tsmiChangeTeacher.Name = "tsmiChangeTeacher";
            this.tsmiChangeTeacher.Size = new System.Drawing.Size(181, 26);
            this.tsmiChangeTeacher.Text = "调整教师";
            this.tsmiChangeTeacher.Click += new System.EventHandler(this.调整教师ToolStripMenuItem_Click);
            // 
            // tsmiImportNightWork
            // 
            this.tsmiImportNightWork.Name = "tsmiImportNightWork";
            this.tsmiImportNightWork.Size = new System.Drawing.Size(181, 26);
            this.tsmiImportNightWork.Text = "导入晚自习";
            // 
            // 课时统计ToolStripMenuItem
            // 
            this.课时统计ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置参数ToolStripMenuItem});
            this.课时统计ToolStripMenuItem.Name = "课时统计ToolStripMenuItem";
            this.课时统计ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.课时统计ToolStripMenuItem.Text = "课时统计";
            // 
            // 设置参数ToolStripMenuItem
            // 
            this.设置参数ToolStripMenuItem.Name = "设置参数ToolStripMenuItem";
            this.设置参数ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.设置参数ToolStripMenuItem.Text = "生成课时统计";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 551);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiImport;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport;
        private System.Windows.Forms.ToolStripMenuItem 管理课表MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiViewSchdule;
        private System.Windows.Forms.ToolStripMenuItem 基础数据管理BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneriate;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeTeacher;
        private System.Windows.Forms.ToolStripMenuItem tsmiViewTeacherSchedule;
        private System.Windows.Forms.ToolStripMenuItem tsmiImportNightWork;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheck;
        private System.Windows.Forms.ToolStripMenuItem 课时统计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置参数ToolStripMenuItem;
    }
}