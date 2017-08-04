namespace 手机摄像头
{
    partial class PhoneVideoForm
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
            this.videopictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.videopictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // videopictureBox
            // 
            this.videopictureBox.BackColor = System.Drawing.Color.Black;
            this.videopictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videopictureBox.Location = new System.Drawing.Point(0, 0);
            this.videopictureBox.Name = "videopictureBox";
            this.videopictureBox.Size = new System.Drawing.Size(1064, 1053);
            this.videopictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.videopictureBox.TabIndex = 1;
            this.videopictureBox.TabStop = false;
            // 
            // PhoneVideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 1053);
            this.Controls.Add(this.videopictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PhoneVideoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JustImage";
            ((System.ComponentModel.ISupportInitialize)(this.videopictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox videopictureBox;
    }
}