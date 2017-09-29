using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Options;


namespace DocStat
{
    public class apistat
    {
		private static void Main(string[] args)
		{
			apistat a = new apistat();
			try
			{
				a.Run(args);
			}
			catch (Exception e)
			{
				
				Console.Error.WriteLine("apistat: {0}", e.Message);
				Environment.ExitCode = 1;
			}
		}

        internal Dictionary<string, ApiCommand> subcommands;

        private void Run (string[] args) {
            subcommands = new Dictionary<string, ApiCommand>() {
                {"internalize", new InternalizeCommand()  },
                {"remaining", new RemainingCommand() },
                {"obsolete", new ObsoleteCommand() },
                {"comparefix", new CompareFixCommand()},
                {"reportnew", new CompareReportCommand()},
                {"fixsummaries", new FixSummariesCommand()}
            };

            GetCommand(args.First()).Run(args);
        }
        internal ApiCommand GetCommand(string command)
        {
            ApiCommand a;
            if (!subcommands.TryGetValue(command, out a))
            {
                throw new Exception(String.Format("Unknown command: {0}.", command));
            }
            return a;
        }
	}

    public abstract class ApiCommand {
        
        public abstract void Run(IEnumerable<string> args);

		protected List<string> Parse(OptionSet p, IEnumerable<string> args,
				string command, string prototype, string description)
		{
			bool showHelp = false;
			p.Add("h|?|help",
					"Show this message and exit.",
					v => showHelp = v != null);

			List<string> extra = null;
			if (args != null)
			{
				extra = p.Parse(args.Skip(1));
			}
			if (args == null || showHelp)
			{
                Console.WriteLine("usage: mdoc {0} {1}",
						args == null ? command : args.First(), prototype);
				Console.WriteLine();
				Console.WriteLine(description);
				Console.WriteLine();
				Console.WriteLine("Available Options:");
				p.WriteOptionDescriptions(Console.Out);
				return null;
			}
			return extra;
		}
    }
}
