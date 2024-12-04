using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trang_web_ban_dien_thoai.Startup))]
namespace Trang_web_ban_dien_thoai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
