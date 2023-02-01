using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scar
{
	class Spinner
	{
		public static void Show(Task asyncTask)
		{
			char[] spinChars = new char[] { '|', '/', '-', '\\' };
			int spinnerPosition = Console.CursorLeft;
			do
			{
				foreach (char spinChar in spinChars)
				{
					Console.CursorLeft = spinnerPosition;
					Console.Write(spinChar);
					Thread.Sleep(100);
				}
			} while (!asyncTask.IsCompleted);
			Console.WriteLine();
		}
	}
}