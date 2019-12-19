using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ExampleNSwag
{
	public class Program
	{
		public static void Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(x => x.UseStartup<Startup>())
				.Build()
				.Run();
	}
}
