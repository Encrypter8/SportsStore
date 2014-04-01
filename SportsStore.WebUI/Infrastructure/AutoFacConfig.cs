using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using System.Configuration;

namespace SportsStore.WebUI.Infrastructure
{
	public static class AutoFacConfig
	{
		public static void Config()
		{
			// New instance of Container Builder
			var builder = new ContainerBuilder();

			// Register Repositories
			// We do this off convention
			builder.RegisterAssemblyTypes(typeof(EFProductRepository).Assembly)
				.PropertiesAutowired()
				.Where(t => t.Name.EndsWith("Repository"))
				.Where(t => !t.Name.StartsWith("Mock"))
				.AsImplementedInterfaces()
				.InstancePerHttpRequest();

			// Register Repository overrides
			// (Great for when you want to use MockRepository
			//builder.RegisterType<MockProductRepository>().As<IProductRepository>()
			//	.InstancePerHttpRequest();

			// Register Providers
			// also with convention
			builder.RegisterAssemblyTypes(typeof(FormsAuthProvider).Assembly)
				.PropertiesAutowired()
				.Where(t => t.Name.EndsWith("Provider"))
				.AsImplementedInterfaces()
				.InstancePerHttpRequest();


			// Register Order Processor
			// because we have Constructor Arguments with this one we cannot use convention
			EmailSettings emailSettings = new EmailSettings()
			{
				WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
			};

			builder.RegisterType<EmailOrderProcessor>().As<IOrderProcessor>().WithParameter("emailSettings", emailSettings);

			// Register Controllers
			builder.RegisterControllers(Assembly.GetExecutingAssembly())
				.PropertiesAutowired();

			// Build Container
			var container = builder.Build();

			// Set Resolver
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}