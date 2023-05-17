using System;
using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using SkyApm.Utilities.DependencyInjection;
using SkyAPM.Diagnostics.EntityFramework;

namespace SkyAPM
{
    public static class SkyWalkingBuilderExtensions
    {
        public static SkyApmExtensions AddEntityFramework(this SkyApmExtensions extensions, Action<DatabaseProviderBuilder> optionAction = null)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            extensions.Services.AddSingleton<ITracingDiagnosticProcessor, EntityFrameworkTracingDiagnosticProcessor>();
            extensions.Services.AddSingleton<IEntityFrameworkSegmentContextFactory, EntityFrameworkSegmentContextFactory>();
            // 
            extensions.Services.AddSingleton<IEntityFrameworkSpanMetadataProvider, SqlServerEntityFrameworkSpanMetadataProvider>();

            if (optionAction != null)
            {
                var databaseProviderBuilder = new DatabaseProviderBuilder(extensions.Services);
                optionAction(databaseProviderBuilder);
            }

            return extensions;
        }
    }
}
