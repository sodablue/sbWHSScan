using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace sbWHSScan.ScanObjectModel.Messages
{
    [KnownType("KnownTypes")]
    public class RequestMessageBase
    {
        static Type[] KnownTypes()
        {
            return GetKnownTypesProvider.GetKnownTypes<RequestMessageBase>();
        }
    }

    [KnownType("KnownTypes")]
    public class ResponseMessageBase
    {
        static Type[] KnownTypes()
        {
            return GetKnownTypesProvider.GetKnownTypes<ResponseMessageBase>();
        }
    }

    public static class GetKnownTypesProvider
    {
        public static Type[] GetKnownTypes<T>() where T : class
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToArray();
        }
    }
}
