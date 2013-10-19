namespace Ricordanza.WinFormsUI.Forms
{
    partial class ProgressWindow
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
            this.msg = new System.Windows.Forms.Label();
            this.circularProgress = new Ricordanza.WinFormsUI.Forms.CircularProgressControl();
            this.SuspendLayout();
            // 
            // msg
            // 
            this.msg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msg.ForeColor = System.Drawing.Color.Silver;
            this.msg.Location = new System.Drawing.Point(0, 78);
            this.msg.Name = "msg";
            this.msg.Size = new System.Drawing.Size(160, 12);
            this.msg.TabIndex = 3;
            this.msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // circularProgress
            // 
            this.circularProgress.BackColor = System.Drawing.Color.Transparent;
            this.circularProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circularProgress.ForeColor = System.Drawing.Color.Blue;
            this.circularProgress.Interval = 80;
            this.circularProgress.Location = new System.Drawing.Point(0, 0);
            this.circularProgress.MinimumSize = new System.Drawing.Size(28, 28);
            this.circularProgress.Name = "circularProgress";
            this.circularProgress.Rotation = Ricordanza.WinFormsUI.Forms.CircularProgressControl.Direction.CLOCKWISE;
            this.circularProgress.Size = new System.Drawing.Size(160, 90);
            this.circularProgress.StartAngle = 270;
            this.circularProgress.TabIndex = 2;
            this.circularProgress.TickColor = System.Drawing.Color.Silver;
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(160, 90);
            this.ControlBox = false;
            this.Controls.Add(this.msg);
            this.Controls.Add(this.circularProgress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressWindow";
            this.Opacity = 0.6D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressWindow";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private CircularProgressControl circularProgress;
        private System.Windows.Forms.Label msg;
    }
}