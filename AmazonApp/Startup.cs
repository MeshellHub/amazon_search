using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmazonApp.Startup))]
namespace AmazonApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}