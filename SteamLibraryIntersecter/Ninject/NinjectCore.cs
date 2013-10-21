using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace SteamLibraryIntersecter.Ninject
{
    public class NinjectCore
    {
        public static readonly IKernel Kernel = new StandardKernel(new CoreModule());

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}