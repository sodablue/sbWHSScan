using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Reflection;
using sbWHSScan.Provider.Handlers;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Provider
{
    public static class RequestProcessor
    {
        private static readonly IContainer Container = CreateContainer();

        public static ResponseMessageBase Handle(RequestMessageBase message)
        {
            Type handlerType = GetRequestHandlerTypeFor(message);
            var handler = (IMessageHandler)Container.Resolve(handlerType);
            return handler.Handle(message);
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IMessageHandler<>)).InstancePerDependency();
            return builder.Build();
        }

        private static Type GetRequestHandlerTypeFor(RequestMessageBase request)
        {
            // get a type reference to IRequestHandler<ThisSpecificRequestType>
            return typeof(IMessageHandler<>).MakeGenericType(request.GetType());
        }

    }
}
