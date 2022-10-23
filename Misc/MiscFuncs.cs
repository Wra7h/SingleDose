using System;

namespace SingleDose.Misc
{
    class MiscFuncs
    {
        public static bool ConfirmExit()
        {
            SDConsole.WriteWarning("Exit program? (Y/N)");
            string UserInput = null;
            do
            {
                Console.Write("       > ");
                UserInput = Console.ReadLine();
                SDConsole.iConsoleLineNum++;
            } while (!UserInput.StartsWith("y", StringComparison.OrdinalIgnoreCase) && !UserInput.StartsWith("n", StringComparison.OrdinalIgnoreCase));

            if (UserInput.StartsWith("Y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
