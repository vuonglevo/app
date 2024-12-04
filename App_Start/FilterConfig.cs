using System.Web;
using System.Web.Mvc;

namespace Trang_web_ban_dien_thoai
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
