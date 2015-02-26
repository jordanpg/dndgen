using System;

namespace dndgen_console
{
	class Program
	{
		public static void Main(string[] args)
		{
			var dndrand = new DnDRandom();

			Console.WriteLine("Input a dice roll, e.g. 1d20+4, 5d6, etc... ");
			string input;
			//int total;
			while(true)
			{
				input = Console.ReadLine();
				var output = dndrand.parseRollExpanded(input);

				Console.WriteLine("End result: " + output[0]);
				Console.WriteLine("Rolls: " + output[1] + "\n");
			}
		}
	}
}
