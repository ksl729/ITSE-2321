namespace Lab1_KLeach;
/*This program will allow the user to select options,
overwrite the current options, display all task and
close the application.*/

internal class Program
{
    static void Main(string[] args)
    {
        // This array prevents the user from entering >100 tasks.
        string[] tasks = new string[100];
        int taskCount = 0; // Tracks how many tasks have been added so far

        bool running = true;

        while (running)
        {
            // Display menu
            Console.WriteLine("- Main Menu - ");
            Console.WriteLine("Please select one of the options below:");
            string[] mainMenu = { "Create a task", "Overwrite a task", "Display task list", "Exit application" };

            // This assigns numbers to the options in the menu
            for (int i = 0; i < mainMenu.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, mainMenu[i]);
            }

            // Read user's input
            string userInput = Console.ReadLine();

            // Confirms user's selection
            Console.WriteLine("You have selected option: {0}", userInput);

            // If user selects option 1, user enter's text for their newly created task
            if (userInput == "1")
            {
                if (taskCount >= 100)
                {
                    Console.WriteLine("Error: Task list is full. Cannot add more than 100 tasks.");
                }
                else
                {
                    Console.Write("Enter task text: ");
                    string task = Console.ReadLine();

                    // If user selects enter without a value, the error below will display
                    if (string.IsNullOrWhiteSpace(task))
                    {
                        Console.WriteLine("Error: Task cannot be blank.");
                    }
                    else
                    {
                        tasks[taskCount] = task;
                        taskCount++;
                        // Confirms user's input by displaying text
                        Console.WriteLine($"Task Created: {task}");
                    }
                }
            }
            /* If user selects option 2, user can overwite a task
            currently on the list.*/
            else if (userInput == "2")
            {
                if (taskCount == 0)
                {
                    Console.WriteLine("Error: There are no tasks to overwrite.");
                }
                else
                {
                    // User needs to enter the number of the task they want to overwrite
                    Console.Write($"Enter task number to overwrite (1-{taskCount}): ");
                    // Reads user's input
                    string index = Console.ReadLine();

                    int indexNumber;
                    bool validIndex = int.TryParse(index, out indexNumber);

                    if (!validIndex || indexNumber < 1 || indexNumber > taskCount)
                    {
                        Console.WriteLine("Error: Invalid index entered.");
                    }
                    else
                    {
                        //Prompts user to enter new task text
                        Console.Write("Enter new task text: ");
                        //Reads input
                        string newTask = Console.ReadLine();

                        // If user selects enter without a value, the error below will display
                        if (string.IsNullOrWhiteSpace(newTask))
                        {
                            Console.WriteLine("Error: Task cannot be blank.");
                        }
                        else
                        {
                            tasks[indexNumber - 1] = newTask;
                            // Displays confirmation of the overwritten task and new task
                            Console.WriteLine($"Task at {indexNumber} was overwritten with: {newTask}");
                        }
                    }
                }
            }
            else if (userInput == "3")
            {
                // Displays the task list
                Console.WriteLine("Task List:");

                bool hasTasks = false;

                for (int i = 0; i < tasks.Length; i++)
                {
                    // Avoids displaying empty task in the array
                    if (!string.IsNullOrWhiteSpace(tasks[i]))
                    {
                        Console.WriteLine($"{i + 1}. {tasks[i]}");
                        hasTasks = true;
                    }
                }
                // If user has not added any tasks to the list(array), the error below will display
                if (!hasTasks)
                {
                    Console.WriteLine("No tasks have been added yet.");
                }
            }
            else if (userInput == "4")
            {
                // Closes application
                Console.WriteLine("Closing application...");
                running = false;
            }
            else
            {
                Console.WriteLine("Invalid option. Please select 1, 2, 3, or 4.");
            }

            Console.WriteLine(); // Blank line for readability between loops
        }
    }
}