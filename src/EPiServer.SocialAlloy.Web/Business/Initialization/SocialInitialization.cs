﻿using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.User;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using System;

namespace EPiServer.SocialAlloy.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class SocialInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

        }

        private static void ConfigureContainer(ConfigurationExpression configuration)
        {
            configuration.For<IUserService>().Use<UserService>();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}