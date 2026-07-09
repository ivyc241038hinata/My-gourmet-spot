using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopStatusChecker
{
    public partial class Form1 : Form
    {
        private Button btnRefresh;
        private Button btnAddShop;
        private Label lblTitle;
        private List<ShopInfo> storeList = new List<ShopInfo>();

        private readonly string jsonPath = Path.Combine(Application.StartupPath, "shops.json");
        private readonly string imageFolder = Path.Combine(Application.StartupPath, "images");

        public Form1()
        {
            InitializeComponent();

            if (!Directory.Exists(imageFolder)) { Directory.CreateDirectory(imageFolder); }

            lblTitle = new Label();
            lblTitle.Text = "「いま、営業中？」チェッカー";
            lblTitle.Font = new Font("Yu Gothic UI", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 25);
            this.Controls.Add(lblTitle);
            lblTitle.BringToFront();

            btnRefresh = new Button();
            btnRefresh.Text = "🔄 全店舗の情報を更新";
            btnRefresh.BackColor = Color.FromArgb(0, 90, 190);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold);
            btnRefresh.Size = new Size(200, 45);
            btnRefresh.Location = new Point(320, 20);
            btnRefresh.Click += new EventHandler(BtnRefresh_Click);
            this.Controls.Add(btnRefresh);
            btnRefresh.BringToFront();

            btnAddShop = new Button();
            btnAddShop.Text = "➕ 新規店舗登録";
            btnAddShop.BackColor = Color.FromArgb(40, 160, 80);
            btnAddShop.ForeColor = Color.White;
            btnAddShop.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold);
            btnAddShop.Size = new Size(160, 45);
            btnAddShop.Location = new Point(535, 20);
            btnAddShop.Click += new EventHandler(BtnAddShop_Click);
            this.Controls.Add(btnAddShop);
            btnAddShop.BringToFront();

            flowLayoutPanel1.Location = new Point(12, 95);
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void CheckShopStatus(ShopInfo shop)
        {
            if (shop.Status == "✖ 閉業")
            {
                shop.StatusColor = Color.FromArgb(210, 40, 40);
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan nowTime = now.TimeOfDay;
            DayOfWeek today = now.DayOfWeek;

            if (today == shop.RegularHoliday)
            {
                shop.Status = "✖ 定休日";
                shop.StatusColor = Color.FromArgb(210, 40, 40);
                return;
            }

            if (nowTime >= shop.OpenTime && nowTime <= shop.CloseTime)
            {
                shop.Status = "● 営業中";
                shop.StatusColor = Color.FromArgb(0, 90, 190);
            }
            else
            {
                shop.Status = "✖ 営業時間外";
                shop.StatusColor = Color.FromArgb(120, 120, 120);
            }
        }

        private void SaveShopsToData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(storeList, options);
                File.WriteAllText(jsonPath, jsonString, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("データの保存に失敗しました: " + ex.Message);
            }
        }

        private string SaveImageFile(Image img, string shopName)
        {
            if (img == null) return "";
            try
            {
                string safeName = string.Join("_", shopName.Split(Path.GetInvalidFileNameChars()));
                string fileName = $"{safeName}_{DateTime.Now.Ticks}.png";
                string destPath = Path.Combine(imageFolder, fileName);

                using (Bitmap bmp = new Bitmap(img))
                {
                    bmp.Save(destPath, System.Drawing.Imaging.ImageFormat.Png);
                }
                return destPath;
            }
            catch { return ""; }
        }

        private async void BtnAddShop_Click(object sender, EventArgs e)
        {
            Form2 registerForm = new Form2();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                string shopName = registerForm.GetShopName();
                string savedImagePath = SaveImageFile(registerForm.SelectedImage, shopName);

                ShopInfo newShop = new ShopInfo()
                {
                    Name = shopName,
                    AddressText = registerForm.GetAddress(),
                    Phone = registerForm.GetPhone(),
                    Sns = registerForm.GetSnsUrl(),
                    ImageUrl = savedImagePath,

                    OpenTime = new TimeSpan(11, 0, 0),
                    CloseTime = new TimeSpan(21, 0, 0),
                    RegularHoliday = DayOfWeek.Sunday,
                    Status = "✖ 閉業",
                    StatusColor = Color.FromArgb(210, 40, 40)
                };

                CheckShopStatus(newShop);
                storeList.Add(newShop);

                SaveShopsToData();
                await DisplayShopsAsync();
            }
        }

        private async void BtnEditShop_Click(object sender, EventArgs e)
        {
            Button btnEdit = (Button)sender;
            ShopInfo targetShop = (ShopInfo)btnEdit.Tag;

            Form2 editForm = new Form2();
            editForm.SetupEditMode();
            editForm.SetShopData(targetShop.Name, targetShop.AddressText, targetShop.Phone, targetShop.Sns);

            if (!string.IsNullOrEmpty(targetShop.ImageUrl) && File.Exists(targetShop.ImageUrl))
            {
                try
                {
                    using (var fs = new FileStream(targetShop.ImageUrl, FileMode.Open, FileAccess.Read))
                    {
                        editForm.SetPreviewImage(Image.FromStream(fs));
                    }
                }
                catch { }
            }

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                targetShop.Name = editForm.GetShopName();
                targetShop.AddressText = editForm.GetAddress();
                targetShop.Phone = editForm.GetPhone();
                targetShop.Sns = editForm.GetSnsUrl();

                if (editForm.SelectedImage != null)
                {
                    string savedImagePath = SaveImageFile(editForm.SelectedImage, targetShop.Name);
                    targetShop.ImageUrl = savedImagePath;
                }

                CheckShopStatus(targetShop);
                SaveShopsToData();
                await DisplayShopsAsync();
                MessageBox.Show($"{targetShop.Name} の情報を更新しました！", "編集完了");
            }
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            btnRefresh.Text = "⏳ 店舗の情報を更新中...";

            foreach (var shop in storeList)
            {
                CheckShopStatus(shop);
            }

            await DisplayShopsAsync();

            btnRefresh.Text = "🔄 全店舗の情報を更新";
            btnRefresh.Enabled = true;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(jsonPath))
            {
                try
                {
                    string jsonString = File.ReadAllText(jsonPath, Encoding.UTF8);
                    storeList = JsonSerializer.Deserialize<List<ShopInfo>>(jsonString);
                }
                catch
                {
                    storeList = null;
                }
            }

            if (storeList == null || storeList.Count == 0)
            {
                storeList = new List<ShopInfo>()
                {
                    new ShopInfo {
                        Name = "シャンティクレープ",
                        Sns = "https://www.instagram.com/shanti.crepe",
                        AddressText = "大分県大分市大字芳河原台...",
                        Phone = "097-560-2374",
                        OpenTime = new TimeSpan(11, 0, 0),
                        CloseTime = new TimeSpan(19, 0, 0),
                        RegularHoliday = DayOfWeek.Sunday,
                        Status = "✖ 閉業",
                        StatusColor = Color.FromArgb(210, 40, 40),
                        ImageUrl = ""
                    }
                };
                SaveShopsToData();
            }

            foreach (var shop in storeList)
            {
                CheckShopStatus(shop);
            }

            await DisplayShopsAsync();
        }

        private async Task DisplayShopsAsync()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var info in storeList)
            {
                StoreCardControl card = new StoreCardControl();

                // 1. 店名
                if (card.Controls.ContainsKey("lblStoreName"))
                {
                    var lblStoreName = card.Controls["lblStoreName"];
                    lblStoreName.Text = info.Name;

                    // ✏ 元の位置（店名の右横）に編集ボタンをきっちり作成
                    Button btnEdit = new Button();
                    btnEdit.Text = "✏ 編集";
                    btnEdit.BackColor = Color.FromArgb(230, 235, 245);
                    btnEdit.Font = new Font("Yu Gothic UI", 9, FontStyle.Bold);
                    btnEdit.Size = new Size(65, 28);
                    btnEdit.Location = new Point(lblStoreName.Right + 15, lblStoreName.Top - 2);
                    btnEdit.Tag = info;
                    btnEdit.Click += new EventHandler(BtnEditShop_Click);
                    card.Controls.Add(btnEdit);
                    btnEdit.BringToFront();
                }

                // 2. SNS URL
                if (card.Controls.ContainsKey("lblSnsInfo"))
                {
                    card.Controls["lblSnsInfo"].Text = "連動URL: " + info.Sns;
                }

                // 3. 住所（少し上にズラして、下の電話番号と絶対に重ならないように配置）
                if (card.Controls.ContainsKey("lblAddress"))
                {
                    var lblAddress = card.Controls["lblAddress"];
                    lblAddress.Text = info.AddressText.StartsWith("住所：") ? info.AddressText : "住所：" + info.AddressText;

                    // 🌟 住所を少し上に引っ張り上げます（Y座標を 70 に固定）
                    lblAddress.Location = new Point(180, 70);
                }

                // 4. 電話番号（住所の下に、完全に独立した安全な隙間をあけて追加）
                Label lblPhone = new Label();
                lblPhone.Text = "電話番号：" + (string.IsNullOrEmpty(info.Phone) ? "未登録" : info.Phone);
                lblPhone.Font = new Font("Yu Gothic UI", 9, FontStyle.Regular);
                lblPhone.ForeColor = Color.FromArgb(210, 40, 40);
                lblPhone.AutoSize = true;

                // 🌟 住所の下（Y座標 94）に固定配置。これで絶対に被りません！
                lblPhone.Location = new Point(180, 94);
                card.Controls.Add(lblPhone);
                lblPhone.BringToFront();

                // 5. 閉業ボタン
                if (card.Controls.ContainsKey("btnStatus"))
                {
                    var btnStatus = card.Controls["btnStatus"];
                    card.StatusText = info.Status;
                    btnStatus.Enabled = true;
                    btnStatus.BackColor = info.StatusColor;
                    btnStatus.ForeColor = Color.White;
                }

                // 画像
                if (!string.IsNullOrEmpty(info.ImageUrl) && File.Exists(info.ImageUrl))
                {
                    try
                    {
                        using (var fs = new FileStream(info.ImageUrl, FileMode.Open, FileAccess.Read))
                        {
                            card.StoreImage = Image.FromStream(fs);
                        }
                    }
                    catch { }
                }

                flowLayoutPanel1.Controls.Add(card);
            }

            await Task.CompletedTask;
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}