﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace RockLib.HealthChecks.AspNetCore
{
    /// <summary>
    /// Extensions for using RockLib.HealthChecks with ASP.NET Core middleware.
    /// </summary>
    public static class HealthCheckMiddlewareExtensions
    {
        /// <summary>
        /// Adds a terminal <see cref="HealthCheckMiddleware"/> to the application.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="healthCheckRunnerName">
        /// The name of the <see cref="IHealthCheckRunner"/> to use. If <see langword="null"/> or not provided,
        /// the default value of the <see cref="HealthCheck.GetRunner"/> method is used.
        /// </param>
        /// <param name="route">The route of the health endpoint.</param>
        /// <param name="indent">Whether to indent the JSON output.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseRockLibHealthChecks(this IApplicationBuilder builder,
            string healthCheckRunnerName = null, string route = "/health", bool indent = false)
            => builder.UseRockLibHealthChecks(HealthCheck.GetRunner(healthCheckRunnerName), route, indent);

        /// <summary>
        /// Adds a terminal <see cref="HealthCheckMiddleware"/> to the application.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="healthCheckRunner">
        /// The <see cref="IHealthCheckRunner"/> to use. If <see langword="null"/>,
        /// the value of the <see cref="HealthCheck.GetRunner"/> method is used.
        /// </param>
        /// <param name="route">The route of the health endpoint.</param>
        /// <param name="indent">Whether to indent the JSON output.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseRockLibHealthChecks(this IApplicationBuilder builder,
            IHealthCheckRunner healthCheckRunner, string route = "/health", bool indent = false)
        {
            healthCheckRunner = healthCheckRunner ?? HealthCheck.GetRunner();
            route = $"/{route.Trim('/')}";

            return builder
                .Map(new PathString(route), appBuilder =>
                {
                    appBuilder.UseMiddleware<HealthCheckMiddleware>(healthCheckRunner, indent);
                });
        }
    }
}
