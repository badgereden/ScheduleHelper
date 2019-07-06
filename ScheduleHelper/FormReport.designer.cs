namespace ScheduleHelper
{
    partial class FormReport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbTeacher = new System.Windows.Forms.CheckBox();
            this.ckbClass = new System.Windows.Forms.CheckBox();
            this.ckbGrade = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbContainNight = new System.Windows.Forms.CheckBox();
            this.chbGradeScheduleByTeacherName = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbGradeScheduleByTeacherName);
            this.groupBox1.Controls.Add(this.ckbTeacher);
            this.groupBox1.Controls.Add(this.ckbClass);
            this.groupBox1.Controls.Add(this.ckbGrade);
            this.groupBox1.Location = new System.Drawing.Point(22, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(736, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // ckbTeacher
            // 
            this.ckbTeacher.AutoSize = true;
            this.ckbTeacher.Checked = true;
            this.ckbTeacher.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbTeacher.Location = new System.Drawing.Point(579, 20);
            this.ckbTeacher.Name = "ckbTeacher";
            this.ckbTeacher.Size = new System.Drawing.Size(108, 16);
            this.ckbTeacher.TabIndex = 2;
            this.ckbTeacher.Text = "各学科教师课表";
            this.ckbTeacher.UseVisualStyleBackColor = true;
            // 
            // ckbClass
            // 
            this.ckbClass.AutoSize = true;
            this.ckbClass.Checked = true;
            this.ckbClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbClass.Location = new System.Drawing.Point(424, 20);
            this.ckbClass.Name = "ckbClass";
            this.ckbClass.Size = new System.Drawing.Size(84, 16);
            this.ckbClass.TabIndex = 1;
            this.ckbClass.Text = "各班级课表";
            this.ckbClass.UseVisualStyleBackColor = true;
            // 
            // ckbGrade
            // 
            this.ckbGrade.AutoSize = true;
            this.ckbGrade.Checked = true;
            this.ckbGrade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbGrade.Location = new System.Drawing.Point(20, 20);
            this.ckbGrade.Name = "ckbGrade";
            this.ckbGrade.Size = new System.Drawing.Size(84, 16);
            this.ckbGrade.TabIndex = 0;
            this.ckbGrade.Text = "全年级课表";
            this.ckbGrade.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowser);
            this.groupBox2.Controls.Add(this.tbFileName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(22, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(735, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(636, 16);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 2;
            this.btnBrowser.Text = "浏览(&B)...";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(88, 12);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.ReadOnly = true;
            this.tbFileName.Size = new System.Drawing.Size(529, 21);
            this.tbFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输出位置:";
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(216, 219);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(463, 219);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(80, 23);
            this.btnEnter.TabIndex = 3;
            this.btnEnter.Text = "确定";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbContainNight);
            this.groupBox3.Location = new System.Drawing.Point(22, 73);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(735, 52);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // chbContainNight
            // 
            this.chbContainNight.AutoSize = true;
            this.chbContainNight.Location = new System.Drawing.Point(20, 20);
            this.chbContainNight.Name = "chbContainNight";
            this.chbContainNight.Size = new System.Drawing.Size(84, 16);
            this.chbContainNight.TabIndex = 0;
            this.chbContainNight.Text = "附带晚自习";
            this.chbContainNight.UseVisualStyleBackColor = true;
            // 
            // chbGradeScheduleByTeacherName
            // 
            this.chbGradeScheduleByTeacherName.AutoSize = true;
            this.chbGradeScheduleByTeacherName.Checked = true;
            this.chbGradeScheduleByTeacherName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbGradeScheduleByTeacherName.Location = new System.Drawing.Point(182, 20);
            this.chbGradeScheduleByTeacherName.Name = "chbGradeScheduleByTeacherName";
            this.chbGradeScheduleByTeacherName.Size = new System.Drawing.Size(144, 16);
            this.chbGradeScheduleByTeacherName.TabIndex = 5;
            this.chbGradeScheduleByTeacherName.Text = "全年级课表[教师名字]";
            this.chbGradeScheduleByTeacherName.UseVisualStyleBackColor = true;
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 265);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormReport";
            this.Text = "导出课表";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbTeacher;
        private System.Windows.Forms.CheckBox ckbClass;
        private System.Windows.Forms.CheckBox ckbGrade;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.CheckBox chbGradeScheduleByTeacherName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbContainNight;
    }
}