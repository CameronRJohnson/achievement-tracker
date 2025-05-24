using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

class Program
{

    // Declare Variables
    static private List<Achievement> achievements = new List<Achievement>();
    static private readonly string saveFile = "achievements.json";

    // Main function to be ran at the beginning of the program
    static void Main(string[] args)
    {
        Console.Clear();
        Run();
    }

    // Run function that prompts the user
    static private void Run()
    {
        string selectedOption;

        while (true)
        {
            Console.Clear();
            Console.Write("Menu Options:\n"
                + "\n  1. Create New Achievement\n"
                + "  2. Complete Achievement\n"
                + "  3. Save Achievement\n"
                + "  4. Load Achievement\n"
                + "  5. Quit\n"
                + "\nSelect a choice from the menu: ");

            selectedOption = Console.ReadLine();

            switch (selectedOption)
            {
                case "1":
                    NewAchievement();
                    break;
                case "2":
                    CompleteAchievement();
                    break;
                case "3":
                    SaveAchievements();
                    break;
                case "4":
                    LoadAchievements();
                    break;
                case "5":
                    Console.Clear();
                    return;
            }
        }
    }

    // Creates a new achievement
    static private void NewAchievement() {

        // What does it want to be called
        Console.Clear();
        Console.Write("What do you want to call the new achievement? ");
        string name = Console.ReadLine();
        Console.WriteLine("");
        Achievement achievement = new Achievement(name);

        // While this is not blank continue to prompt the user
        while (true) {
            Console.Write("Enter a name for each goal you need to complete (leave it blank to finish): ");
            string goalName = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(goalName))
                break;
            achievement.Goals.Add(new Goal(goalName));
        }

        // Let the user know that it has been created
        achievements.Add(achievement);
        Console.WriteLine($"\nYou have created a new achievement called {name}");
        Console.ReadLine();
    }

    // Mark an achievement as complete
    static private void CompleteAchievement()
    {
        // Check to see if there are any achievements to begin with
        Console.Clear();
        if (achievements.Count == 0)
        {
            Console.Write("You have no achievements");
            Console.ReadLine();
            return;
        }

        // Write out all of the different goals
        Console.WriteLine("Current Achievements: \n");
        for (int i = 0; i < achievements.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {achievements[i].Title}");
        }

        // Ask the user which one to mark as complete
        Console.Write("\nPlease enter the number of the goal that you want to mark as completed: ");
        // Check to see if it is a valid number
        if (!int.TryParse(Console.ReadLine(), out int achIndex) || achIndex < 1 || achIndex > achievements.Count)
        {
            Console.WriteLine("The number you have selected does not exist.");
            Console.ReadLine();
            return;
        }
        Achievement selectedAchievement = achievements[achIndex - 1];
        
        // Check to see if the achievement has any goals
        if (selectedAchievement.Goals.Count == 0)
        {
            Console.WriteLine("You do not have any goals set for this achievement");
            Console.ReadLine();
            return;
        }

        // Write out the goals for the achievement
        Console.Clear();
        Console.WriteLine($"Goals for '{selectedAchievement.Title}':\n");
        for (int i = 0; i < selectedAchievement.Goals.Count; i++)
        {
            var goal = selectedAchievement.Goals[i];
            string status = goal.IsCompleted ? "[X]" : "[ ]";
            Console.WriteLine($"  {i + 1}. {goal.Description} {status}");
        }

        // Prompt the user to see which goal to mark as complete
        Console.Write("\nEnter the number of the goal to mark as completed: ");
        if (!int.TryParse(Console.ReadLine(), out int goalIndex) || goalIndex < 1 || goalIndex > selectedAchievement.Goals.Count)
        {
            Console.WriteLine("The number you have selected does not exist.");
            Console.ReadLine();
            return;
        }

        // Check to see if the goal is already completed
        var selectedGoal = selectedAchievement.Goals[goalIndex - 1];
        if (selectedGoal.IsCompleted)
        {
            Console.WriteLine("This goal is already marked as complete.");
        }
        // If not completed make it complete
        else
        {
            selectedGoal.IsCompleted = true;
            Console.WriteLine($"Goal '{selectedGoal.Description}' marked as complete.");
        }
        Console.Write("Press enter to go to the main menu.");
        Console.ReadLine();
    }

    static private void SaveAchievements()
    {
        // Check to see if any errors occur
        try
        {
            // Serialize the achievements and options so that it is writable
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(achievements, options);
            File.WriteAllText(saveFile, json);
            Console.Clear();
            Console.Write($"Achievements saved to '{saveFile}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving achievements: {ex.Message}\n");
        }
        Console.ReadLine();
    }

    static private void LoadAchievements()
    {
        // Check to see an error occurs
        try
        {
            // Check to see if the file exists
            if (!File.Exists(saveFile))
            {
                Console.WriteLine($"No file found ('{saveFile}').");
                Console.ReadLine();
                return;
            }

            // Load the serialized string into the file
            string json = File.ReadAllText(saveFile);
            var loaded = JsonSerializer.Deserialize<List<Achievement>>(json);
            Console.Clear();
            if (loaded != null)
            {
                achievements = loaded;
                Console.Write($"Achievements loaded.");
            }
            else
            {
                Console.WriteLine($"Failed to load achievements.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading achievements: {ex.Message}\n");
        }
        Console.ReadLine();
    }
}