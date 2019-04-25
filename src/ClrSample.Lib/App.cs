using System;
using System.Collections.Generic;
using System.Reflection;

namespace ClrSample.Lib {
    public class App {

        static Dictionary<string, SampleBase> _samples = new Dictionary<string, SampleBase>();

        static App() {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types) {
                if (type.IsSubclassOf(typeof(SampleBase)) && !type.IsAbstract) {
                    SampleBase sample = (SampleBase)Activator.CreateInstance(type);
                    _samples.Add(sample.Command, sample);
                }
            }
        }

        public void Run() {

            while (true) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" ----------------------------------------------------------------------------------");
                Console.WriteLine("|   Clr Sample App Started, PRESS 'Q' to Exit,PRESS 'H' To Get All Sample's Help   |");
                Console.WriteLine(" ----------------------------------------------------------------------------------");
                Console.ResetColor();
                var commandWithArgs = Console.ReadLine();
                switch (commandWithArgs) {
                    case "Q":
                        Console.WriteLine("PRESS Any Key to Exit");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                    case "H":
                        PrintHelper();
                        break;
                    default:
                        var commands = commandWithArgs.Split(new char[] { ' ' });
                        if (commands.Length > 0) {
                            var command = commands[0];
                            if (_samples.ContainsKey(command)) {
                                var args = (string[])Array.CreateInstance(typeof(string), commands.Length - 1);
                                Array.ConstrainedCopy(commands, 1, args, 0, commands.Length - 1);
                                try {
                                    _samples[command].Invoke(args);
                                } catch (Exception ex) {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Invoke '{commandWithArgs}' Failed : {ex.Message}");
                                    Console.ResetColor();
                                }
                                break;
                            }
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Command Invalid : {commandWithArgs}");
                            Console.ResetColor();
                            PrintHelper();
                        }
                        break;
                }
            }
        }

        private void PrintHelper() {
            foreach (var sample in _samples.Values) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Usage For " + sample.Description + " :");
                Console.ResetColor();
                Console.WriteLine(sample.Usage);
            }
        }
    }
}
