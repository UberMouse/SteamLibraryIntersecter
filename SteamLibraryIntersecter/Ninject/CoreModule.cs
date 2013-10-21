using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using SteamLibraryIntersecter.Steam;

namespace SteamLibraryIntersecter.Ninject
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Community>().ToSelf().InSingletonScope();
            Bind<Store>().ToSelf().InSingletonScope();
        }
    }
}