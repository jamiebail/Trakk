using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trakk.Startup))]
namespace Trakk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
