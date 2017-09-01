using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Buy.Startup))]
namespace Buy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
