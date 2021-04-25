using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValueDictionaryApp
{
    class App
    {
        //Created by: Douglas Bresee
       public void RunApp()
        {
            string userEnteredCommand = String.Empty; 
            string[] userEnteredValues = null;

            Commands commands = new Commands();

            Console.WriteLine("Multi Value Dictionary App");
            Console.WriteLine("Copyright 2021");
            Console.WriteLine(" ");


            //Main Loop
            do
            {
                Console.Write("> ");
                userEnteredValues = Console.ReadLine().Split(' ');


                userEnteredValues[0]  = userEnteredValues[0].ToUpper().Trim();

                if (userEnteredValues[0] == "EXIT")
                {
                    Console.WriteLine("Are you sure you want to Exit? Enter 'Yes' to Exit");
                    userEnteredCommand = Console.ReadLine().ToUpper().Trim();

                    if (userEnteredCommand == "YES")
                    { 
                        Environment.Exit(0);
                    }
                }

                DisplayOutput(commands.CommandProcessor(userEnteredValues)); 

            } while (true); 

        }

        public void DisplayOutput(List<string> outputList)
        {
            int count = 0;

            foreach(string lineToDisplay in outputList)
            {
                if (outputList.Count > 1)
                {
                    count++;
                    Console.WriteLine(count + ") " + lineToDisplay);
                }
                else
                {
                    Console.WriteLine( ") " + lineToDisplay);
                }
               

            }

        }
    }


}
