using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopStatusChecker
{
    public partial class StoreCardControl : UserControl
    {
        // 外部から店舗名を設定・取得できるようにする
        public string StoreName
        {
            get { return lblStoreName.Text; }
            set { lblStoreName.Text = value; }
        }

        // 外部からSNS情報を設定・取得できるようにする
        public string SnsInfo
        {
            get { return lblSnsInfo.Text; }
            set { lblSnsInfo.Text = value; }
        }

        // 外部から住所を設定・取得できるようにする
        public string Address
        {
            get { return lblAddress.Text; }
            set { lblAddress.Text = value; }
        }

        // 外部からステータスの文字を設定できるようにする
        public string StatusText
        {
            get { return btnStatus.Text; }
            set { btnStatus.Text = value; }
        }

        //外部から店舗画像を設定できるようにするプロパティ
        public Image StoreImage
        {
            get { return picStoreImage.Image; }
            set { picStoreImage.Image = value; }
        }
        public StoreCardControl()
        {
            InitializeComponent();
        }

        private void StoreCardControl_Load(object sender, EventArgs e)
        {

        }
    }
}
