using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(yEtiHotel.Startup))]
namespace yEtiHotel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
