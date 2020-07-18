using System;
using System.Collections.Generic;

namespace HelloArduino
{
	public class Command
	{

		private static Dictionary<CommandComparisonMethod, Func<string, string, bool>> comparers = new Dictionary<CommandComparisonMethod, Func<string, string, bool>>()
		{
			{CommandComparisonMethod.StartWith,  StartWith},
			{CommandComparisonMethod.FullMessage,  Equal},
			{CommandComparisonMethod.Contains,  Contains},
		};

		public List<string> Names;
		public Action<string> Action;

		public Command(Action<string> action, params string[] names)
		{
			Action = action;
			Names = new List<string>(names);
		}

		public void Run(string args)
		{
			Action?.Invoke(args);
		}

		public bool ContainsName(string name, CommandComparisonMethod comparisonMethod)
		{
			for(int i = 0; i < Names.Count; ++i)
			{
				if (comparers[comparisonMethod].Invoke(Names[i], name))
				{
					return true;
				}
			}

			return false;
		}

		private static bool Equal(string commandName, string input)
		{
			return commandName.Equals(input, StringComparison.CurrentCultureIgnoreCase);
		}

		private static bool StartWith(string commandName, string input)
		{
			return commandName.StartsWith(input, StringComparison.CurrentCultureIgnoreCase);
		}

		private static bool Contains(string commandName, string input)
		{
			return commandName.Contains(input);
		}
	}
}
