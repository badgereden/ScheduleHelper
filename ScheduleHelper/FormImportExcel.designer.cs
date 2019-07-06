namespace ScheduleHelper
{
    partial class FormImportExcel
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowserCourse = new System.Windows.Forms.Button();
            this.tbCourse = new System.Windows.Forms.TextBox();
            this.btnBrowserTeacher = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTeacher = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowserCourse);
            this.groupBox2.Controls.Add(this.tbCourse);
            this.groupBox2.Controls.Add(this.btnBrowserTeacher);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbTeacher);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(642, 107);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnBrowserCourse
            // 
            this.btnBrowserCourse.Location = new System.Drawing.Point(539, 63);
            this.btnBrowserCourse.Name = "btnBrowserCourse";
            this.btnBrowserCourse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowserCourse.TabIndex = 2;
            this.btnBrowserCourse.Text = "浏览...";
            this.btnBrowserCourse.UseVisualStyleBackColor = true;
            this.btnBrowserCourse.Click += new System.EventHandler(this.btnBrowserCourse_Click);
            // 
            // tbCourse
            // 
            this.tbCourse.Location = new System.Drawing.Point(137, 63);
            this.tbCourse.Name = "tbCourse";
            this.tbCourse.Size = new System.Drawing.Size(383, 21);
            this.tbCourse.TabIndex = 1;
            // 
            // btnBrowserTeacher
            // 
            this.btnBrowserTeacher.Location = new System.Drawing.Point(539, 30);
            this.btnBrowserTeacher.Name = "btnBrowserTeacher";
            this.btnBrowserTeacher.Size = new System.Drawing.Size(75, 23);
            this.btnBrowserTeacher.TabIndex = 2;
            this.btnBrowserTeacher.Text = "浏览...";
            this.btnBrowserTeacher.UseVisualStyleBackColor = true;
            this.btnBrowserTeacher.Click += new System.EventHandler(this.btnBrowserTeacher_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "学科周课时设置:";
            // 
            // tbTeacher
            // 
            this.tbTeacher.Location = new System.Drawing.Point(137, 30);
            this.tbTeacher.Name = "tbTeacher";
            this.tbTeacher.Size = new System.Drawing.Size(383, 21);
            this.tbTeacher.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任课教师一览表:";
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(129, 145);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(428, 145);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "确定";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // FormImportExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 191);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.groupBox2);
            this.Name = "FormImportExcel";
            this.Text = "导入任课教师";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowserTeacher;
        private System.Windows.Forms.TextBox tbTeacher;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowserCourse;
        private System.Windows.Forms.TextBox tbCourse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnAccept;
    }
}