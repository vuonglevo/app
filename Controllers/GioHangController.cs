using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trang_web_ban_dien_thoai.Models;

namespace Trang_web_ban_dien_thoai.Controllers
{
    public class GioHangController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();

            // GET: GioHang
            public ActionResult ThemGioHang(int ms, string url)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang sp = lstGioHang.Find(n => n.iMaSP == ms);
                if (sp == null)
                {
                    sp = new GioHang(ms);
                    lstGioHang.Add(sp);
                }
                else
                {
                    sp.iSoLuong++;
                }
                return Redirect(url);
            }

            public List<GioHang> LayGioHang()
            {
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang == null)
                {
                    lstGioHang = new List<GioHang>();
                    Session["GioHang"] = lstGioHang;
                }
                return lstGioHang;
            }

            private int TongSoLuong()
            {
                int iTongSoLuong = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
                }
                return iTongSoLuong;
            }

            private double TongTien()
            {
                double dTongTien = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)

                {
                    dTongTien = lstGioHang.Sum(n => n.dThanhTien);
                }
                return dTongTien;
            }
            public ActionResult GioHang()
            {
                List<GioHang> lstGioHang = LayGioHang();
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return View(lstGioHang);
            }

            public ActionResult GioHangPartial()
            {
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return PartialView();
            }
            public ActionResult XoaSPKhoiGioHang(int iMaSP)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
                if (sp != null)
                {
                    lstGioHang.RemoveAll(n => n.iMaSP == iMaSP);
                    if (lstGioHang.Count == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("GioHang");
            }
            public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
                if (sp != null)
                {
                    sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                }
                return RedirectToAction("GioHang");
            }
            public ActionResult XoaGioHang()
            {
                List<GioHang> lstGioHang = LayGioHang();
                lstGioHang.Clear();
                return RedirectToAction("Index", "Home");
            }

            [HttpGet]
            public ActionResult DatHang()
            {
                if (Session["KhachHang"] == null || Session["KhachHang"].ToString() == "")
                {
                    return RedirectToAction("DangNhap", "Account");
                }
                if (Session["GioHang"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return View(lstGioHang);
            }

            [HttpPost]
            public ActionResult DatHang(FormCollection f)
            {
                // Thêm đơn hàng
                DonHang ddh = new DonHang();
                KhachHang kh = (KhachHang)Session["KhachHang"];
                List<GioHang> lstGioHang = LayGioHang();
                ddh.idKH = kh.idKH;
                ddh.NgayDat = DateTime.Now;
                ddh.NgayGiao = DateTime.Now;
                ddh.TrangThaiDatHang = "Đặt hàng";
                ddh.TrangThaiThanhToan = false;
                db.DonHangs.Add(ddh);
                db.SaveChanges();
                foreach (var item in lstGioHang)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.idDonHang = ddh.idDonHang;
                    ctdh.MaSanPham = item.iMaSP;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.Gia = item.dDonGia;
                    db.ChiTietDonHangs.Add(ctdh);
                }
                db.SaveChanges();
                Session["GioHang"] = null; return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            public ActionResult XacNhanDonHang()
            {
                return View();
            }
        // GET: GioHang

        public ActionResult FailureView()
        {
            return View();
        }

        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            // lấy apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                // Tài nguyên đại diện cho Người trả tiền tài trợ cho Phương thức thanh toán thanh toán dưới dạng paypal
                // Id người thanh toán sẽ được trả về khi tiến hành thanh toán hoặc nhấp để thanh toán
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // phần này sẽ được thực thi đầu tiên vì PayerID không tồn tại
                    // nó được trả về bởi lệnh gọi hàm tạo của lớp thanh toán
                    // Tạo thanh toán
                    // baseURL là url mà paypal gửi lại dữ liệu. 
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    // ở đây chúng tôi đang tạo hướng dẫn lưu trữ PaymentID nhận được trong phiên
                    // sẽ được sử dụng trong quá trình thực hiện thanh toán 
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //Hàm CreatePayment cung cấp cho chúng ta url phê duyệt thanh toán
                    //người trả tiền được chuyển hướng để thanh toán tài khoản paypal 
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    // nhận các liên kết được trả về từ paypal để đáp lại lệnh gọi hàm Tạo
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // lưu URL chuyển hướng payapal tới nơi người dùng sẽ được chuyển hướng để thanh toán
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // lưu PaymentID trong hướng dẫn chính
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // Hàm này thực thi sau khi nhận được tất cả các tham số cho việc thanh toán
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    // Nếu thanh toán được thực hiện không thành công thì chúng tôi sẽ hiển thị thông báo thanh toán không thành công cho người dùng
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            //on successful payment, show success page to user.



            try
            {

                if (ModelState.IsValid)
                {
                    DonHang ddh = new DonHang();
                    KhachHang kh = (KhachHang)Session["KhachHang"];
                    List<GioHang> lstGioHang = LayGioHang();
                    using (ShopOnlineEntities db = new ShopOnlineEntities())
                    {
                        // Tạo đơn hàng mới
                        var order = new DonHang
                        {
                            idKH = kh.idKH,
                            NgayDat = DateTime.Now,
                            NgayGiao = DateTime.Now,
                            TrangThaiDatHang = "Đặt hàng",
                            TrangThaiThanhToan = true
                        };

                        // Lưu đơn hàng vào cơ sở dữ liệu
                        db.DonHangs.Add(order);
                        db.SaveChanges();



                        // Lấy giỏ hàng từ Session


                        // Lưu chi tiết đơn hàng từ giỏ hàng vào cơ sở dữ liệu
                        foreach (var cartItem in lstGioHang)
                        {
                            var orderDetail = new ChiTietDonHang
                            {

                                idDonHang = order.idDonHang,
                                MaSanPham = cartItem.iMaSP,
                                SoLuong = cartItem.iSoLuong,
                                Gia = cartItem.dDonGia,

                            };
                            db.ChiTietDonHangs.Add(orderDetail);

                            var product = db.SanPhams.Find(cartItem.iMaSP);
                            if (product != null)
                            {
                                product.Soluong -= cartItem.iSoLuong;
                            }
                        }

                        db.SaveChanges();
                        Session["GioHang"] = null;

                        RedirectToAction("SuccessView", "GioHang");
                    }
                }
            }
            catch
            {
                return Content("Xảy ra lỗi, mời bạn kiểm tra lại thông tin đã điền");

            }


            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            List<GioHang> lstGioHang = LayGioHang();
            //tạo danh sách vật phẩm và thêm các đối tượng vật phẩm vào đó
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var item in lstGioHang)
            {
                itemList.items.Add(new Item()
                {
                    name = item.sTenSP,
                    currency = "USD",
                    price = Math.Round((double)(item.dDonGia) / 24000, 2).ToString(),
                    quantity = item.iSoLuong.ToString(),
                    sku = item.iMaSP.ToString()
                });
            }
            // Thêm thông tin chi tiết về mặt hàng như tên, tiền tệ, giá cả, v.v. 

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Định cấu hình Url chuyển hướng tại đây với đối tượng RedirectUrls
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Thêm chi tiết về Thuế, vận chuyển và Tổng phụ
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = lstGioHang.Sum(x => Math.Round(x.dThanhTien / 24000, 2)).ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = lstGioHang.Sum(x => Math.Round(x.dThanhTien / 24000, 2)).ToString(), // Tổng số phải bằng tổng thuế, phí vận chuyển và tổng phụ.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Thêm mô tả về giao dịch
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Tạo hóa đơn số  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Tạo thanh toán bằng APIContext
            return this.payment.Create(apiContext);
        }

    }
}
