using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Standard.Startup))]
namespace Standard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
