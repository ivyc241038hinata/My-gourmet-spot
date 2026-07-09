using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ShopStatusChecker
{
    public partial class Form2 : Form
    {
        public Image SelectedImage { get; private set; }

        private TextBox txtNameCtrl;
        private TextBox txtAddressCtrl;
        private TextBox txtPhoneCtrl;
        private TextBox txtSnsUrlCtrl;
        private PictureBox pbPreviewCtrl;

        public Form2()
        {
            InitializeComponent();

            // フォームが読み込まれたら、上から順にコントロールを完璧に整理する
            InitializeSmartMapping();
        }

        private void InitializeSmartMapping()
        {
            var textBoxes = new List<TextBox>();
            FindTextBoxesAndButtons(this, textBoxes);

            // 📐 画面の上から順番に並び替えることで、デザイナの名前がバグっていても1番目＝店名、2番目＝住所…と確定させる
            textBoxes.Sort((a, b) => a.Top.CompareTo(b.Top));

            if (textBoxes.Count >= 1) txtNameCtrl = textBoxes[0];
            if (textBoxes.Count >= 2) txtAddressCtrl = textBoxes[1];
            if (textBoxes.Count >= 3) txtPhoneCtrl = textBoxes[2];
            if (textBoxes.Count >= 4) txtSnsUrlCtrl = textBoxes[3];
        }

        private void FindTextBoxesAndButtons(Control container, List<TextBox> textBoxes)
        {
            foreach (Control c in container.Controls)
            {
                if (c is TextBox tb) textBoxes.Add(tb);
                if (c is PictureBox pb) pbPreviewCtrl = pb;

                // ボタンのクリックイベントをここで確実に上書き
                if (c is Button btn)
                {
                    if (btn.Text.Contains("選択"))
                    {
                        btn.Click -= btnSelectImage_Click;
                        btn.Click -= btnSelectImage_Click_1;
                        btn.Click += (s, e) => CustomSelectImage();
                    }
                    else if (btn.Text.Contains("登録") || btn.Text.Contains("更新") || btn.Text.Contains("確定"))
                    {
                        btn.Click -= btnRegister_Click;
                        btn.Click -= btnSubmit_Click;
                        btn.Click += (s, e) => CustomSubmit();
                    }
                    else if (btn.Text.Contains("キャンセル") || btn.Text.Contains("戻る"))
                    {
                        btn.Click -= btnCancel_Click;
                        btn.Click -= btnCancel_Click_1;
                        btn.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                    }
                }

                if (c.HasChildren) FindTextBoxesAndButtons(c, textBoxes);
            }
        }

        // 🌟 編集モードへの強制切り替え（タイトルやボタンの文字が戻る問題を修正）
        public void SetupEditMode()
        {
            this.Text = "店舗情報の編集";
            ChangeTextRecursive(this);
        }

        private void ChangeTextRecursive(Control container)
        {
            foreach (Control c in container.Controls)
            {
                // 「新規店舗登録」などの古いメインタイトルを見つけたら強制書き換え
                if (c is Label && (c.Text.Contains("新規") || c.Text.Contains("登録")) && c.Font.Size > 12)
                {
                    c.Text = "店 舗 情 報 の 編 集";
                }
                // ボタンの文字を「更新する」に強制変更
                if (c is Button && (c.Text.Contains("登録") || c.Text.Contains("新規")))
                {
                    c.Text = "更新する";
                    c.BackColor = Color.FromArgb(0, 90, 190); // ボタンの色も青に変える
                    c.ForeColor = Color.White;
                }

                if (c.HasChildren) ChangeTextRecursive(c);
            }
        }

        public void SetShopData(string name, string address, string phone, string sns)
        {
            // マッピングの安全を担保するため再実行
            InitializeSmartMapping();

            if (txtNameCtrl != null) txtNameCtrl.Text = name ?? "";
            if (txtAddressCtrl != null) txtAddressCtrl.Text = address ?? "";
            if (txtPhoneCtrl != null) txtPhoneCtrl.Text = phone ?? "";
            if (txtSnsUrlCtrl != null) txtSnsUrlCtrl.Text = sns ?? "";
        }

        public string GetShopName() => txtNameCtrl != null ? txtNameCtrl.Text : "";
        public string GetAddress() => txtAddressCtrl != null ? txtAddressCtrl.Text : "";
        public string GetPhone() => txtPhoneCtrl != null ? txtPhoneCtrl.Text : "";
        public string GetSnsUrl() => txtSnsUrlCtrl != null ? txtSnsUrl.Text : "";

        public void SetPreviewImage(Image img)
        {
            SelectedImage = img;
            if (pbPreviewCtrl != null)
            {
                pbPreviewCtrl.Image = img;
                pbPreviewCtrl.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void CustomSelectImage()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "画像ファイル(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var temp = Image.FromFile(ofd.FileName))
                    {
                        SetPreviewImage(new Bitmap(temp));
                    }
                }
            }
        }

        private void CustomSubmit()
        {
            if (string.IsNullOrWhiteSpace(GetShopName()))
            {
                MessageBox.Show("店名は必須入力です。", "エラー");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ゾンビコード対応のダミー
        private void btnSelectImage_Click(object sender, EventArgs e) => CustomSelectImage();
        private void btnSelectImage_Click_1(object sender, EventArgs e) => CustomSelectImage();
        private void btnSubmit_Click(object sender, EventArgs e) => CustomSubmit();
        private void btnRegister_Click(object sender, EventArgs e) => CustomSubmit();
        private void btnCancel_Click(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
        private void btnCancel_Click_1(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
        private void Form2_Load(object sender, EventArgs e) { }
    }
}