using Autofac;
using Autofac.Integration.Mvc;
using ProSchool.Core.Configuration;
using ProSchool.Core.Data;
using ProSchool.Core.Fakes;
using ProSchool.Core.Infrastructure;
using ProSchool.Core.Infrastructure.DependencyManagement;
using ProSchool.Data;
using ProSchool.Services.Academics;
using ProSchool.Services.Finance;
using ProSchool.Services.Settings;
//using ProERP.Services.SubAssembly;
using ProSchool.Web.Framework.UI;
using System.Linq;
using System.Web;

namespace ProSchool.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, ProConfig config)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            builder.Register<IDbContext>(c => new ProSchoolContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<InfrastructureService>().InstancePerRequest();
            builder.RegisterType<InstitutionService>().InstancePerRequest();
            builder.RegisterType<FeeService>().InstancePerRequest();
            builder.RegisterType<StudentService>().InstancePerRequest();
            builder.RegisterType<InquiryService>().InstancePerRequest();
            builder.RegisterType<StudentProgramService>().InstancePerRequest();
            builder.RegisterType<StudentProgramInvoiceService>().InstancePerRequest(); 
                builder.RegisterType<SystemService>().InstancePerRequest();
        }
    }
}
