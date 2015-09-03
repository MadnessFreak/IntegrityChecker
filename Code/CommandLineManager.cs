using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrityChecker
{
    public delegate void CommandActionCallback(string[] parameters);

    public class CommandLineManager
    {
        // Properties
        public Dictionary<string, CommandActionCallback> CommandList { get; set; }
        public bool IsCommandLineEmpty { get; private set; }

        // Constructor
        public CommandLineManager()
        {
            CommandList = new Dictionary<string, CommandActionCallback>();
        }

        // Methods
        public void Parse(string[] args)
        {
            IsCommandLineEmpty = args.Length < 1;
            foreach (string line in args)
            {
                if (!line.StartsWith("-")) throw new ArgumentException("Invalid command syntax");

                if (line.Contains(":"))
                {
                    var parts = line.Split(':');
                    var cmd = parts[0].Remove(0, 1);

                    Invoke(cmd, parts.Skip(1).ToArray());
                }
                else
                {
                    var cmd = line.Remove(0, 1);
                    Invoke(cmd, null);
                }
            }
        }

        public void Invoke(string command, string[] args)
        {
            if (CommandList.ContainsKey(command))
            {
                try
                {
                    CommandList[command].Invoke(args);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Command execution error - " + command + "\n" + ex.Message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid command execution - " + command);
            }
        }
    }
}
