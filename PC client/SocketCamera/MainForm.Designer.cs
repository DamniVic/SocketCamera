namespace SocketCamera
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.停止服务器button = new System.Windows.Forms.Button();
            this.开启服务器button = new System.Windows.Forms.Button();
            this.porttextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iptextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.phoneslistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(341, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(129, 17);
            this.toolStripStatusLabel1.Text = "powerd by DamniVic";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.停止服务器button);
            this.groupBox1.Controls.Add(this.开启服务器button);
            this.groupBox1.Controls.Add(this.porttextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.iptextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 80);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置：";
            // 
            // 停止服务器button
            // 
            this.停止服务器button.Enabled = false;
            this.停止服务器button.Location = new System.Drawing.Point(106, 45);
            this.停止服务器button.Name = "停止服务器button";
            this.停止服务器button.Size = new System.Drawing.Size(75, 23);
            this.停止服务器button.TabIndex = 5;
            this.停止服务器button.Text = "停止服务器";
            this.停止服务器button.UseVisualStyleBackColor = true;
            this.停止服务器button.Click += new System.EventHandler(this.停止服务器button_Click);
            // 
            // 开启服务器button
            // 
            this.开启服务器button.Location = new System.Drawing.Point(25, 45);
            this.开启服务器button.Name = "开启服务器button";
            this.开启服务器button.Size = new System.Drawing.Size(75, 23);
            this.开启服务器button.TabIndex = 4;
            this.开启服务器button.Text = "开启服务器";
            this.开启服务器button.UseVisualStyleBackColor = true;
            this.开启服务器button.Click += new System.EventHandler(this.开启服务器button_Click);
            // 
            // porttextBox
            // 
            this.porttextBox.Location = new System.Drawing.Point(219, 18);
            this.porttextBox.Name = "porttextBox";
            this.porttextBox.Size = new System.Drawing.Size(36, 21);
            this.porttextBox.TabIndex = 3;
            this.porttextBox.Text = "9635";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // iptextBox
            // 
            this.iptextBox.Location = new System.Drawing.Point(72, 18);
            this.iptextBox.Name = "iptextBox";
            this.iptextBox.Size = new System.Drawing.Size(107, 21);
            this.iptextBox.TabIndex = 1;
            this.iptextBox.Text = "172.16.60.14";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "本机IP：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.phoneslistView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(341, 310);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "remote devices：";
            // 
            // phoneslistView
            // 
            this.phoneslistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.phoneslistView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoneslistView.Location = new System.Drawing.Point(3, 17);
            this.phoneslistView.Name = "phoneslistView";
            this.phoneslistView.Size = new System.Drawing.Size(335, 290);
            this.phoneslistView.TabIndex = 0;
            this.phoneslistView.UseCompatibleStateImageBehavior = false;
            this.phoneslistView.View = System.Windows.Forms.View.Details;
            this.phoneslistView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.phoneslistView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 51;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "用户名";
            this.columnHeader2.Width = 77;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "IP地址";
            this.columnHeader3.Width = 112;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "登入时间";
            this.columnHeader4.Width = 140;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 412);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RealTime Image";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox porttextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox iptextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 开启服务器button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView phoneslistView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button 停止服务器button;
    }
}

