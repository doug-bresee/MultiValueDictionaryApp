using System;
using System.Collections.Generic; 
namespace MultiValueDictionaryApp
{
    class App
    {
        //Created by: Douglas Bresee

        Commands commands;
        public App()
        {
           commands = new Commands(); 
        }
       public void RunApp()
        {
            try
            {
                string userEnteredCommand = String.Empty;
                string[] userEnteredValues = null;

                Console.WriteLine("Multi Value Dictionary App");
                Console.WriteLine("Copyright 2021");
                Console.WriteLine("Created by Douglas Bresee");
                Console.WriteLine("");

                //Main Loop
                do
                {
                    Console.Write("> ");
                    userEnteredValues = Console.ReadLine().Split(' ');
                    userEnteredValues[0] = userEnteredValues[0].ToUpper().Trim();

                    if (userEnteredValues[0] == "EXIT")
                    {
                        Console.WriteLine("Are you sure you want to Exit? Enter 'Yes' to Exit, 'No' to continue.");
                        userEnteredCommand = Console.ReadLine().ToUpper().Trim();

                        if (userEnteredCommand == "YES")
                        {
                            Environment.Exit(0);
                        }
                        if (userEnteredCommand == "NO")
                        {
                            //prompt for next command
                            continue;
                        }
                    }

                    DisplayOutput(commands.CommandProcessor(userEnteredValues));

                } while (true);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception in runApp()");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

        public void DisplayOutput(List<string> outputList)
        {
            int count = 0;

            foreach(string lineToDisplay in outputList)
            { 
                if(lineToDisplay.Contains( ")" ))
                {
                    Console.WriteLine(lineToDisplay); 
                }
                else
                { 
                    count++;
                    Console.WriteLine(count + ") " + lineToDisplay);
                } 
            } 
        }
    }


}
