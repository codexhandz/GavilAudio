using GavilAudio.Audio;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GavilAudio.UI;

public class ConsoleUI
{
    public ConsoleUI()
    {
        try
        {
            // Slightly tighter window to match the compact aesthetic
            Console.SetWindowSize(90, 35);
        }
        catch { }
    }

    /// <summary>
    /// Helper method to draw a sleek solid volume bar using Unicode blocks
    /// </summary>
    private string CreateVolumeBar(float volumePercentage)
    {
        int maxBars = 20;
        int activeBars = (int)Math.Round((volumePercentage / 100f) * maxBars);

        string filled = new string('█', activeBars);
        string empty = new string('░', maxBars - activeBars);

        return $"[{filled}{empty}] {volumePercentage,3:0}%";
    }

    public void ShowConsoleUI(int volume, List<AudioDevice> devices, List<(string ProcessName, float Volume)> sessions)
    {
        Console.Clear();

        // NEON HEADER
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@"
  ___  ___  _   _ ___ _       _  _ _ ___ ___ ___  
 / __|/ _ \| \ / |_ _| |     / \| | |   \_ _/ _ \ 
| (_ |  _  |\ V / | || |__  / _ \ U | |) | | (_) |
 \___|_| |_| \_/ |___|____|/_/ \_\___/___/___\___/");

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("=======================================================================");

        // MASTER VOLUME
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" MASTER VOLUME: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(CreateVolumeBar(volume));

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("-----------------------------------------------------------------------");

        // DEVICES
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" [ OUTPUT DEVICES ]");

        int deviceNumber = 1;
        foreach (AudioDevice device in devices)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"  {deviceNumber}. {device.DeviceFriendlyName}");

            if (device.IsDefaultForMultimedia || device.IsDefaultForCommunications)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string tags = "";
                if (device.IsDefaultForMultimedia) tags += " [Media]";
                if (device.IsDefaultForCommunications) tags += " [Comms]";
                Console.Write(tags);
            }
            Console.WriteLine();
            deviceNumber++;
        }

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("-----------------------------------------------------------------------");

        // ACTIVE SESSIONS
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" [ ACTIVE SESSIONS ]");

        if (sessions.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  > No active audio streams detected");
        }
        else
        {
            foreach (var s in sessions)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"  > {s.ProcessName.PadRight(20)} ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(CreateVolumeBar(s.Volume));
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("=======================================================================");

        // COMPACT COMMANDS CHEAT SHEET
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(" CMDs: [volume <0-100>] [session <name> volume <0-100>] [refresh]");
        Console.WriteLine("       [device change <type> <#>] [help] [exit]");

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("=======================================================================\n");
        Console.ResetColor();
    }

    public void ShowLastInteraction(string userInput, string commandResult)
    {
        if (!string.IsNullOrEmpty(userInput))
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" INPUT  > ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(userInput);

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(" RESULT > ");

            if (commandResult.StartsWith("Error") || commandResult.StartsWith("Unable") || commandResult.StartsWith("Invalid"))
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(commandResult);

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("-----------------------------------------------------------------------\n");
            Console.ResetColor();
        }
    }

    public string? GetCommand(ref string currentInputBuffer, int timeoutMs = 5000)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(" GavilAudio ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"> {currentInputBuffer}");

        var sw = System.Diagnostics.Stopwatch.StartNew();

        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    string result = currentInputBuffer;
                    currentInputBuffer = "";
                    Console.WriteLine();
                    Console.ResetColor();
                    return result;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (currentInputBuffer.Length > 0)
                    {
                        currentInputBuffer = currentInputBuffer.Substring(0, currentInputBuffer.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    currentInputBuffer += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Thread.Sleep(50);
        }

        Console.ResetColor();
        return null;
    }

    /// MESSAGE METHODS
    public string ShowHelp()
    {
        return @"--- GAVIL AUDIO HELP ---
1. exit: Closes the application.
2. help: Displays this detailed help menu.
3. refresh: Updates the UI and active audio stats.
4. volume <0-100>: Sets the master volume. 
   Example: 'volume 50'
5. session <name> volume <0-100>: Sets the volume of a specific running application. 
   Example: 'session chrome volume 50'
6. device change <role> <number>: Changes the default Windows audio device.
   Roles: 'multimedia' or 'communications'
   Number: The ID listed in the Output Devices section.
   Example: 'device change multimedia 2'";
    }

    public string GetErrorNoCommand()
    {
        return "No input detected. Type 'help' to see available commands.";
    }

    public string GetErrorInvalidVolumeNumber()
    {
        return "Unable to change volume. Input a valid number (e.g., 'volume 50').";
    }

    public string GetErrorInvalidCommand()
    {
        return "Invalid command. Type 'help' to see available commands.";
    }
}