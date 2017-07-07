using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LexiconLMSPortal.Startup))]
namespace LexiconLMSPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
