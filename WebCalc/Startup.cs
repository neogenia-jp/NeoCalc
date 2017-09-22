using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebCalc.Startup))]
namespace WebCalc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
