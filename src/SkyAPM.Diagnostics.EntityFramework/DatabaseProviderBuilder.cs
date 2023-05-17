﻿using Microsoft.Extensions.DependencyInjection;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class DatabaseProviderBuilder
    {
        public IServiceCollection Services { get; }

        public DatabaseProviderBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
