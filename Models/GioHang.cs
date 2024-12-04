using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trang_web_ban_dien_thoai.Models
{
    
    public class GioHang
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int ms)
        {
            iMaSP = ms;
            SanPham s = db.SanPhams.Single(n => n.MaSanPham == iMaSP);

            sTenSP = s.TenSanPham;
            sAnhBia = s.HinhChinh;
            dDonGia = double.Parse(s.Gia.ToString());
            iSoLuong = 1;
        }

    }
}
