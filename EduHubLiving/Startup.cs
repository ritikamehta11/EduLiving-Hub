using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EduHubLiving.Startup))]
namespace EduHubLiving
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
