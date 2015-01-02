using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GalleryBlog.Startup))]
namespace GalleryBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
