using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopolkoSeminar.Startup))]
namespace TopolkoSeminar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
