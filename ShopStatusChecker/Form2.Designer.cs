namespace ShopStatusChecker
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblMainTitle = new System.Windows.Forms.Label();
            this.lblLabel1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblLabel2 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblLabel3 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblLabel4 = new System.Windows.Forms.Label();
            this.txtSnsUrl = new System.Windows.Forms.TextBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMainTitle
            // 
            this.lblMainTitle.AutoSize = true;
            this.lblMainTitle.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMainTitle.ForeColor = System.Drawing.Color.FromArgb(0, 90, 190); // 🎨 標準的なエラーの出ない色指定に修正しました
            this.lblMainTitle.Location = new System.Drawing.Point(20, 15);
            this.lblMainTitle.Name = "lblMainTitle";
            this.lblMainTitle.Size = new System.Drawing.Size(145, 30);
            this.lblMainTitle.TabIndex = 0;
            this.lblMainTitle.Text = "新 規 店 舗 登 録";
            // 
            // lblLabel1
            // 
            this.lblLabel1.AutoSize = true;
            this.lblLabel1.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLabel1.Location = new System.Drawing.Point(22, 60);
            this.lblLabel1.Name = "lblLabel1";
            this.lblLabel1.Size = new System.Drawing.Size(39, 20);
            this.lblLabel1.TabIndex = 1;
            this.lblLabel1.Text = "店名";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.txtName.Location = new System.Drawing.Point(25, 85);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(340, 27);
            this.txtName.TabIndex = 2;
            // 
            // lblLabel2
            // 
            this.lblLabel2.AutoSize = true;
            this.lblLabel2.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLabel2.Location = new System.Drawing.Point(22, 125);
            this.lblLabel2.Name = "lblLabel2";
            this.lblLabel2.Size = new System.Drawing.Size(39, 20);
            this.lblLabel2.TabIndex = 3;
            this.lblLabel2.Text = "住所";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.txtAddress.Location = new System.Drawing.Point(25, 150);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(340, 27);
            this.txtAddress.TabIndex = 4;
            // 
            // lblLabel3
            // 
            this.lblLabel3.AutoSize = true;
            this.lblLabel3.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLabel3.Location = new System.Drawing.Point(22, 190);
            this.lblLabel3.Name = "lblLabel3";
            this.lblLabel3.Size = new System.Drawing.Size(69, 20);
            this.lblLabel3.TabIndex = 5;
            this.lblLabel3.Text = "電話番号";
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.txtPhone.Location = new System.Drawing.Point(25, 215);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(340, 27);
            this.txtPhone.TabIndex = 6;
            // 
            // lblLabel4
            // 
            this.lblLabel4.AutoSize = true;
            this.lblLabel4.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLabel4.Location = new System.Drawing.Point(22, 255);
            this.lblLabel4.Name = "lblLabel4";
            this.lblLabel4.Size = new System.Drawing.Size(217, 20);
            this.lblLabel4.TabIndex = 7;
            this.lblLabel4.Text = "情報元 SNS (Google Maps URLなど)";
            // 
            // txtSnsUrl
            // 
            this.txtSnsUrl.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.txtSnsUrl.Location = new System.Drawing.Point(25, 280);
            this.txtSnsUrl.Name = "txtSnsUrl";
            this.txtSnsUrl.Size = new System.Drawing.Size(340, 27);
            this.txtSnsUrl.TabIndex = 8;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Font = new System.Drawing.Font("Yu Gothic UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSelectImage.Location = new System.Drawing.Point(25, 325);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(120, 32);
            this.btnSelectImage.TabIndex = 9;
            this.btnSelectImage.Text = "🖼 画像の選択...";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(160, 325);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(75, 75);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 10;
            this.picPreview.TabStop = false;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Green; // 🎨 安全な標準色に修正しました
            this.btnRegister.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(70, 425);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(110, 40);
            this.btnRegister.TabIndex = 11;
            this.btnRegister.Text = "新規登録";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(210, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 40);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 491);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.txtSnsUrl);
            this.Controls.Add(this.lblLabel4);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblLabel3);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblLabel2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblLabel1);
            this.Controls.Add(this.lblMainTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "店舗の追加";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMainTitle;
        private System.Windows.Forms.Label lblLabel1;
        public System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblLabel2;
        public System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblLabel3;
        public System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblLabel4;
        public System.Windows.Forms.TextBox txtSnsUrl;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.PictureBox picPreview;
        public System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;
    }
}