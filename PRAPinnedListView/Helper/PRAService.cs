using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PRAPinnedListView
{
    /// <summary>
    /// A simple service container implementation, singleton only
    /// </summary>
    public static class PRAService
    {
        static readonly Dictionary<Type, Lazy<object>> Services = new Dictionary<Type, Lazy<object>>();

        /// <summary>
        /// Register the specified service with an instance
        /// </summary>
        public static void Register<T>(T service)
        {
            Services[typeof(T)] = new Lazy<object>(() => service);
        }

        /// <summary>
        /// Resolves the type, throwing an exception if not found
        /// </summary>
        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static object Resolve(Type type)
        {

            //Non-scoped services
            {
                Lazy<object> service;
                if (Services.TryGetValue(type, out service))
                {
                    return service.Value;
                }
                else
                {
                    throw new KeyNotFoundException(string.Format("Service not found for type '{0}'", type));
                }
            }
        }

        /// <summary>
        /// Mainly for testing, clears the entire container
        /// </summary>
        public static void Clear()
        {
            Services.Clear();
        }
    }
}