using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Models
{
    public class HTCTicketTransferModel
    {
        public string guid { get; set; }
        public string code { get; set; }
        public string danh_muc { get; set; }
        public string tinh_su_co { get; set; }
        public string cid { get; set; }
        public string thoi_gian_bat_dau { get; set; }
        public string thoi_gian_ket_thuc { get; set; }
        public string nguyen_nhan_so_bo { get; set; }
        public string noi_dung_kn { get; set; }
        public string so_hop_dong { get; set; }
        public string ten_khach_hang { get; set; }
        public string ma_kh { get; set; }
        public string sdt { get; set; }
        public string ma_can_ho { get; set; }
        public string ngay_hop_dong { get; set; }
        public string ngay_ban_giao { get; set; }
        public string goi_cuoc { get; set; }
        public string tien_cuoc { get; set; }
        public string thue_ip { get; set; }
        public string note { get; set; }
        public int status { get; set; }
    }
}
