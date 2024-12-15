using System;
using System.Threading;

// Define a delegate for stopwatch events
public delegate void StopwatchEventHandler(string message);

class Stopwatch
{
    // Store the total time elapsed in seconds
    private int timeElapsed;
    public bool IsRunning { get; private set; }

    // Events to notify when the stopwatch starts, stops, or resets
    public event StopwatchEventHandler? OnStarted;
    public event StopwatchEventHandler? OnStopped;
    public event StopwatchEventHandler? OnReset;

    // Method to start the stopwatch
    public void Start()
    {
        // Check if the stopwatch is already running
        if (!IsRunning)
        {
            IsRunning = true; // Set the state to running
            OnStarted?.Invoke("Stopwatch Started!"); // Notify that the stopwatch has started
        }
    }

    // Method to stop the stopwatch
    public void Stop()
    {
        // Check if the stopwatch is currently running
        if (IsRunning)
        {
            IsRunning = false; // Set the state to not running
            // No output displayed when stopped
        }
    }

    // Method to reset the stopwatch
    public void Reset()
    {
        timeElapsed = 0; // Reset the elapsed time to zero
        OnReset?.Invoke("Stopwatch Reset!"); // Notify that the stopwatch has been reset
    }

    // Method to update the elapsed time every second
    public void Tick()
    {
        if (IsRunning)
        {
            timeElapsed++; // Increase the elapsed time by one second
            Console.Write($"\rTime Elapsed: {FormatTime(timeElapsed)}"); // Display the formatted time on the same line
        }
    }

    // Method to format the elapsed time into hours, minutes, and seconds
    private string FormatTime(int totalSeconds)
    {
        int hours = totalSeconds / 3600; // Calculate hours
        int minutes = (totalSeconds % 3600) / 60; // Calculate minutes
        int seconds = totalSeconds % 60; // Calculate remaining seconds
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}"; // Return formatted time as HH:MM:SS
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events to handle messages
        stopwatch.OnStarted += MessageHandler;
        stopwatch.OnStopped += MessageHandler;
        stopwatch.OnReset += MessageHandler;

        // Main loop for user interaction
        while (true)
        {
            // Display the prompt on a new line
            Console.WriteLine("\nEnter S to Start, T to Stop, R to Reset, Q to Quit:");
            string? input = Console.ReadLine()?.ToUpper(); // Read user input and convert to uppercase

            // Check for empty input
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input! Try again."); // Notify invalid input
                continue;
            }

            // Handle user input
            switch (input)
            {
                case "S": // Start the stopwatch
                    stopwatch.Start();
                    while (stopwatch.IsRunning) // Continue until the stopwatch is stopped
                    {
                        Thread.Sleep(1000); // Wait for one second
                        stopwatch.Tick(); // Update the elapsed time

                        // Check if a key has been pressed
                        if (Console.KeyAvailable)
                        {
                            input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                            if (input == "T") // Stop the stopwatch
                                stopwatch.Stop();
                            else if (input == "R") // Reset the stopwatch
                                stopwatch.Reset();
                            else if (input == "Q") // Quit the application
                                return;
                        }
                    }
                    break;
                case "R": // Reset the stopwatch
                    stopwatch.Reset();
                    break;
                case "Q": // Quit the application
                    return;
                default: // Handle invalid input
                    Console.WriteLine("Invalid input! Try again.");
                    break;
            }
        }
    }

    // Event handler method to display messages
    static void MessageHandler(string message)
    {
        Console.WriteLine(message); // Print the message to the console
    }
}