using System;

namespace Common {
    /// <summary>
    /// </summary>
    public abstract class Runner {

        public void Run() {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"----------{this.GetType().Name} Run----------");
            Console.ResetColor();
            RunCore();
        }
        /// <summary>
        /// Run
        /// </summary>
        protected abstract void RunCore();
    }
}