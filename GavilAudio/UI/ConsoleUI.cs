using GavilAudio.Audio;

namespace GavilAudio.UI;

public class ConsoleUI
{
    /// DRAWING METHODS
    public void ShowConsoleUI(int volume, List<AudioDevice> devices)
    {
        // HEADERR
        Console.Clear();
        Console.WriteLine(@" _______  _______  __   __  ___   ___      _______  __   __  ______   ___   _______ 
|       ||   _   ||  | |  ||   | |   |    |   _   ||  | |  ||      | |   | |       |
|    ___||  |_|  ||  |_|  ||   | |   |    |  |_|  ||  | |  ||  _    ||   | |   _   |
|   | __ |       ||       ||   | |   |    |       ||  |_|  || | |   ||   | |  | |  |
|   ||  ||       ||       ||   | |   |___ |       ||       || |_|   ||   | |  |_|  |
|   |_| ||   _   | |     | |   | |       ||   _   ||       ||       ||   | |       |
|_______||__| |__|  |___|  |___| |_______||__| |__||_______||______| |___| |_______|
");
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------------------------------");
        Console.WriteLine();
        // VOLUME
        Console.WriteLine($"Volume: {volume}%");
        Console.WriteLine();
        // DEVICES
        Console.WriteLine("Devices:");
        Console.WriteLine();

        int deviceNumber = 1;

        foreach (AudioDevice device in devices)
        {
            Console.Write($"{deviceNumber}. {device.DeviceFriendlyName} ");

            if (device.IsDefaultForMultimedia)
            {
                Console.Write(" (Default Multimedia)");
            }
            if (device.IsDefaultForCommunications)
            {
                Console.Write(" (Default Communications)");
            }
            Console.WriteLine();
            deviceNumber++;

        }
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------------------------------");
        Console.WriteLine();
        // COMMANDS
        Console.WriteLine(@"Commands:

- exit
- help
- volume <0-100>
- refresh
- device change <multimedia OR communications> <device number>");
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------------------------------");
        Console.WriteLine();
    }
    public void ShowLastInteraction(string userInput, string commandResult)
    {
        Console.WriteLine($"Last Command: {userInput}");
        Console.WriteLine();
        Console.WriteLine($"Result: {commandResult}");
        Console.WriteLine("------------------------------------------------------------------------------------");
        Console.WriteLine();
    }
    public string GetCommand()
    {
        Console.Write("Enter command: ");
        string? input = Console.ReadLine();
        return input ?? "";
    }


    /// MESSAGE METHODS
    public string ShowHelp()
    {
        return @"Breakdown of the commands:
1. exit: it will close the application.
2. help: displays current help.
3. volume <0-100>: Set the master volume with volume <0-100>. Example: volume 50.
4. refresh: it will refresh the whole UI, refreshing the stats
5. Type <device change> followed by <type of audio> (multimedia OR communications)
   followed by <device number> you will like to use from: 'All available Devices'.
   Example: device change multimedia 2";
    }
    public string GetErrorNoCommand()
    {
        return "No input detected. Type \"help\" to see available commands.";
    }
    public string GetErrorInvalidVolumeNumber()
    {
        return "Unable to change volume. Input a valid number. Example: volume 50.";
    }
    public string GetErrorInvalidCommand()
    {
        return "Invalid command. Type \"help\" to see available commands.";
    }
}