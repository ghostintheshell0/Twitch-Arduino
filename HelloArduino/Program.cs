using System;
using System.Threading.Tasks;

namespace HelloArduino
{
	class Program
	{
		static void Main(string[] args)
		{
			MainAsync().Wait();
		}

		static async Task MainAsync()
		{
			var app = new Application();
			await app.Run();
			Console.WriteLine("Press enter for exit");
			Console.ReadLine();
		}
	}
}
