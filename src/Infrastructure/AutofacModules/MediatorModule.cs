using Autofac;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Api.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        private readonly string[] _assemblyNames;

        public MediatorModule(string[] assemblyNames)
        {
            if(assemblyNames == null || assemblyNames.All(string.IsNullOrEmpty))
                throw new ArgumentNullException(nameof(assemblyNames));

            _assemblyNames = assemblyNames;
        }

        public MediatorModule(string assemblyName)
        {
            if(string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));

            _assemblyNames = new [] { assemblyName };
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var apiAssemblies = _assemblyNames.Select(Assembly.Load).ToArray();
            builder.RegisterAssemblyTypes(apiAssemblies).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(apiAssemblies).AsClosedTypesOf(typeof(INotificationHandler<>));
            builder.RegisterAssemblyTypes(apiAssemblies).Where(t => t.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out var o) ? o : null;
            });

        }
    }
}
