using System;
using System.Drawing;

namespace ShopStatusChecker
{
    public class ShopInfo
    {
        public string Name { get; set; } = "";
        public string AddressText { get; set; } = "";
        public string Phone { get; set; } = ""; // 🌟 これを追加してエラーを解消！
        public string Sns { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DayOfWeek RegularHoliday { get; set; }

        public string Status { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        public Color StatusColor { get; set; }
    }
}