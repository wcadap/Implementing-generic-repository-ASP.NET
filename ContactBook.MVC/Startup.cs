using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContactBook.MVC.Startup))]
namespace ContactBook.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
