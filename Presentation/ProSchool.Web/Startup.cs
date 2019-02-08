using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProSchool.Web.Startup))]
namespace ProSchool.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
