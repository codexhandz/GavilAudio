namespace GavilAudio.UI;

public class ConsoleUI
{
    public void ShowHeader()
    {
        Console.Clear();

        Console.WriteLine("=================================");
        Console.WriteLine("          GavilAudio");
        Console.WriteLine("=================================");
        Console.WriteLine();
    }
    public void ShowCurrentStatus(string currentDevice, int volume)
    {
        Console.WriteLine($"Current Master Volume: {volume}%");
        Console.WriteLine();
        Console.WriteLine($"Current Device: {currentDevice}");
        Console.WriteLine();
    }
    public string GetVolumeInput()
    {
        Console.Write($"Enter new volume (0-100):");
        string input = Console.ReadLine();
        return input;
    }
}