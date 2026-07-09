namespace ShopStatusChecker
{
    partial class StoreCardControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            picStoreImage = new PictureBox();
            lblStoreName = new Label();
            lblSnsInfo = new Label();
            lblAddress = new Label();
            btnStatus = new Button();
            ((System.ComponentModel.ISupportInitialize)picStoreImage).BeginInit();
            SuspendLayout();
            // 
            // picStoreImage
            // 
            picStoreImage.Location = new Point(27, 30);
            picStoreImage.Name = "picStoreImage";
            picStoreImage.Size = new Size(125, 62);
            picStoreImage.SizeMode = PictureBoxSizeMode.Zoom;
            picStoreImage.TabIndex = 0;
            picStoreImage.TabStop = false;
            // 
            // lblStoreName
            // 
            lblStoreName.AutoSize = true;
            lblStoreName.Font = new Font("MS UI Gothic", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblStoreName.Location = new Point(176, 17);
            lblStoreName.Name = "lblStoreName";
            lblStoreName.Size = new Size(158, 23);
            lblStoreName.TabIndex = 1;
            lblStoreName.Text = "ラーメン　味よし";
            // 
            // lblSnsInfo
            // 
            lblSnsInfo.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblSnsInfo.Location = new Point(176, 40);
            lblSnsInfo.Name = "lblSnsInfo";
            lblSnsInfo.Size = new Size(400, 40);
            lblSnsInfo.TabIndex = 2;
            lblSnsInfo.Text = "最新SNS:本日11:30より元気に営業開始します！";
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblAddress.ForeColor = Color.IndianRed;
            lblAddress.Location = new Point(176, 80);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(362, 20);
            lblAddress.TabIndex = 3;
            lblAddress.Text = "住所:大分県大分市荷揚町4-1  電話番号:097-512-3412";
            // 
            // btnStatus
            // 
            btnStatus.BackColor = Color.CornflowerBlue;
            btnStatus.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnStatus.ForeColor = Color.White;
            btnStatus.Location = new Point(631, 30);
            btnStatus.Name = "btnStatus";
            btnStatus.Size = new Size(140, 50);
            btnStatus.TabIndex = 4;
            btnStatus.Text = "・営業中";
            btnStatus.UseVisualStyleBackColor = false;
            // 
            // StoreCardControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(btnStatus);
            Controls.Add(lblAddress);
            Controls.Add(lblSnsInfo);
            Controls.Add(lblStoreName);
            Controls.Add(picStoreImage);
            Margin = new Padding(10);
            Name = "StoreCardControl";
            Size = new Size(898, 118);
            Load += StoreCardControl_Load;
            ((System.ComponentModel.ISupportInitialize)picStoreImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picStoreImage;
        private Label lblStoreName;
        private Label lblSnsInfo;
        private Label lblAddress;
        private Button btnStatus;
    }
}
