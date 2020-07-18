using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.IO.Ports;
using TwitchLib.Client;
using System.Configuration;
using System.IO;

namespace HelloArduino
{
	public class Application
	{
		private static string[] commandNamesSplitSymbols = new string[] { "," };

		private SerialPort port;
		private TwitchClient client;
		private List<Command> commands;
		private CommandComparisonMethod comparisonMethod;

		public Application()
		{
			InitPort();
			InitTwitch();
			InitCommands();
			comparisonMethod = GetComparisonMethod();
		}

		private void InitTwitch()
		{
			var name = ConfigurationManager.AppSettings["twitchName"];
			var token = ConfigurationManager.AppSettings["twitchToken"];
			var channel = ConfigurationManager.AppSettings["twitchChannel"];

			var cred = new TwitchLib.Client.Models.ConnectionCredentials(name, token);
			client = new TwitchClient(protocol: TwitchLib.Client.Enums.ClientProtocol.TCP);
			client.Initialize(cred, channel);
			client.OnConnected += TwitchConnected;
			client.OnJoinedChannel += TwitchJoinedChannel;
			client.OnMessageReceived += TwitchMessageReceived;
		}

		private void TwitchConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
		{
			Console.WriteLine("Connected to twitch");
		}

		private void TwitchJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
		{
			Console.WriteLine($"Joined to {e.Channel}");
		}

		private void TwitchMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
		{
			var firstWord = e.ChatMessage.Message.GetFirstWord();

			for(int i = 0; i < commands.Count; ++i)
			{
				if(commands[i].ContainsName(firstWord, comparisonMethod))
				{
					Console.WriteLine($"{e.ChatMessage.Username} sent {firstWord}");
					commands[i].Run(e.ChatMessage.Message);
				}
			}
		}

		private void On(string args)
		{
			port.WriteLine("n");
		}

		private void Off(string args)
		{
			port.WriteLine("f");
		}

		private void Switch(string args)
		{
			port.WriteLine("s");
		}

		private void InitPort()
		{
			var names = SerialPort.GetPortNames();

			if(names.Length == 0)
			{
				Console.WriteLine("Ports not exist");
				return;
			}

			var targetPort = ConfigurationManager.AppSettings["port"];
			if (names.Contains(targetPort))
			{
				port = new SerialPort(targetPort, 9600);
				return;
			}

			Console.WriteLine($"Port {targetPort} not found");
		}


		public async Task Run()
		{
			try
			{
				client?.Connect();
				port?.Open();
			}
			catch (Exception ex)
			{
				var stamp = DateTime.Now.ToString("HHmmss");
				var logName = $"err{stamp}.txt";
				File.WriteAllText(logName, ex.ToString());
				Console.WriteLine($"Error. For more details check {logName} file");

			}

			await Task.Yield();
		}

		private void InitCommands()
		{
			commands = new List<Command>();
			CreateCommand(ConfigurationManager.AppSettings["commandOn"], On);
			CreateCommand(ConfigurationManager.AppSettings["commandOff"], Off);
			CreateCommand(ConfigurationManager.AppSettings["commandSwitch"], Switch);
		}

		private void CreateCommand(string variants, Action<string> executer)
		{
			var names = variants.Split(commandNamesSplitSymbols, StringSplitOptions.RemoveEmptyEntries);
			var cmd = new Command(executer, names);
			commands.Add(cmd);
		}

		private CommandComparisonMethod GetComparisonMethod()
		{
			var value = ConfigurationManager.AppSettings["comparison"];
			var result = (CommandComparisonMethod )Enum.Parse(typeof(CommandComparisonMethod), value, true);
			return result;
		}
	}
}
