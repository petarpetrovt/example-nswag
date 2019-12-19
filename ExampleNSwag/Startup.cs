using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag.AspNetCore;

namespace ExampleNSwag
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			var assembly = Assembly.GetExecutingAssembly();
			string assemblyFileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
			var assemblyVersion = Version.Parse(assemblyFileVersion);

			services.AddOpenApiDocument(options =>
			{
				options.DocumentName = $"{SchemaType.OpenApi3}";
				options.SchemaType = SchemaType.OpenApi3;

				options.Title = "ExampleNSwag";
				options.Description = "This specification contains an API for working with ExampleNSwag.";
				options.Version = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";

				options.DefaultDictionaryValueReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
				options.DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
			});

			app.UseOpenApi(options =>
			{
				options.DocumentName = $"{SchemaType.OpenApi3}";
				options.Path = $"/openapi/openapi.json";
			});

			app.UseSwaggerUi3(options =>
			{
				options.Path = $"/openapi";
				options.OperationsSorter = "alpha";

				var route = new SwaggerUi3Route($"ExampleNSwag (OpenAPI 3)", "/openapi/openapi.json");
				options.SwaggerRoutes.Add(route);

				// Hosting as an IIS virtual application
				options.TransformToExternalPath = (internalUiRoute, request) =>
				{
					if (internalUiRoute.StartsWith("/") && !internalUiRoute.StartsWith(request.PathBase))
					{
						return request.PathBase + internalUiRoute;
					}
					else
					{
						return internalUiRoute;
					}
				};
			});
		}
	}
}
