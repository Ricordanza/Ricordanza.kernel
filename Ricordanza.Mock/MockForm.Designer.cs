namespace Ricordanza.Mock
{
    partial class MockForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MockForm));
            this.overlayIcon = new System.Windows.Forms.ImageList(this.components);
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // overlayIcon
            // 
            this.overlayIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("overlayIcon.ImageStream")));
            this.overlayIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.overlayIcon.Images.SetKeyName(0, "Ovl_Blue_LB.png");
            this.overlayIcon.Images.SetKeyName(1, "Ovl_Blue_LT.png");
            this.overlayIcon.Images.SetKeyName(2, "Ovl_Blue_RB.png");
            this.overlayIcon.Images.SetKeyName(3, "Ovl_Blue_RT.png");
            this.overlayIcon.Images.SetKeyName(4, "Ovl_Green_LB.png");
            this.overlayIcon.Images.SetKeyName(5, "Ovl_Green_LT.png");
            this.overlayIcon.Images.SetKeyName(6, "Ovl_Green_RB.png");
            this.overlayIcon.Images.SetKeyName(7, "Ovl_Green_RT.png");
            this.overlayIcon.Images.SetKeyName(8, "Ovl_Orange_LB.png");
            this.overlayIcon.Images.SetKeyName(9, "Ovl_Orange_LT.png");
            this.overlayIcon.Images.SetKeyName(10, "Ovl_Orange_RB.png");
            this.overlayIcon.Images.SetKeyName(11, "Ovl_Orange_RT.png");
            this.overlayIcon.Images.SetKeyName(12, "Ovl_Red_LB.png");
            this.overlayIcon.Images.SetKeyName(13, "Ovl_Red_LT.png");
            this.overlayIcon.Images.SetKeyName(14, "Ovl_Red_RB.png");
            this.overlayIcon.Images.SetKeyName(15, "Ovl_Red_RT.png");
            this.overlayIcon.Images.SetKeyName(16, "Ovl_Yellow_LB.png");
            this.overlayIcon.Images.SetKeyName(17, "Ovl_Yellow_LT.png");
            this.overlayIcon.Images.SetKeyName(18, "Ovl_Yellow_RB.png");
            this.overlayIcon.Images.SetKeyName(19, "Ovl_Yellow_RT.png");
            // 
            // iconList
            // 
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "Dashboard.png");
            this.iconList.Images.SetKeyName(1, "Gadget.png");
            this.iconList.Images.SetKeyName(2, "File.png");
            // 
            // MockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 503);
            this.Name = "MockForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList overlayIcon;
        private System.Windows.Forms.ImageList iconList;

    }
}

