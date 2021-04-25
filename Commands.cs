using System;
using System.Collections.Generic;
using System.Linq; 

namespace MultiValueDictionaryApp
{

    public class Commands
    {
        //Created by: Douglas Bresee

        private Dictionary<string, List<string>> commandsDictionary { get; set;}

        //hard coded values all in one place
        public string trueString = ") true";
        public string falseString = ") false";
        public string emptySetString = "(empty set)";
        public string removedString = ") Removed";
        public string valueDoesNotExistString = ") ERROR, value does not exist.";
        public string keyDoesNotExistString = ") ERROR, key does not exist.";
        public string keyRequiredString = ") ERROR, Key is required for ADD.";
        public string valueExistsString = ") ERROR, value already exists.";
        public string addedString = ") Added";
        public string clearedString = ") Cleared";
        public string commandNotValidString = ") Command is not valid.";


        public Commands()
        {
            commandsDictionary = new Dictionary<string, List<string>>(); 
        }

        public List<string> CommandProcessor(string[] userEnteredValues)
        {
            string userEnteredCommand = string.Empty;
            List<string> commandResult = new List<string>(); 
            string userEnteredKey = string.Empty;
            string userEnteredValue = string.Empty;

            try
            {  
                if (userEnteredValues.Length > 0)
                {
                    userEnteredCommand = userEnteredValues[0];
                }

                if (userEnteredValues.Length > 1)
                {
                    userEnteredKey = userEnteredValues[1];
                }

                if (userEnteredValues.Length > 2)
                {
                    userEnteredValue = userEnteredValues[2];
                }

                switch (userEnteredCommand)
                {
                    case "ADD":
                        commandResult = AddCommand(userEnteredKey, userEnteredValue);
                        break;

                    case "KEYS":
                        commandResult = KeysCommand();
                        break;

                    case "MEMBERS":
                        commandResult = MembersCommand(userEnteredKey);
                        break;

                    case "REMOVE":
                        commandResult = RemoveCommand(userEnteredKey, userEnteredValue);
                        break;

                    case "REMOVEALL":
                        commandResult = RemoveAllCommand(userEnteredKey);
                        break;

                    case "CLEAR":
                        commandResult = ClearCommand();
                        break;

                    case "KEYEXISTS":
                        commandResult = KeyExistsCommand(userEnteredKey);
                        break;

                    case "VALUEEXISTS":
                        commandResult = ValueExistsCommand(userEnteredKey, userEnteredValue);
                        break;

                    case "ALLMEMBERS":
                        commandResult = AllMembersCommand(); 
                        break;

                    case "ITEMS":
                        commandResult = ItemsCommand();
                        break;

                    default:
                        commandResult.Add(commandNotValidString);
                        break;
                }
            }
            catch(Exception ex)
            {  
                Console.WriteLine("Exception in CommandProcessor()");
                Console.WriteLine("Command: " + userEnteredCommand);
                Console.WriteLine(ex.Message); 
                Console.ReadLine();
            } 

            return commandResult;

        }


        public List<string> AddCommand(string userEnteredKey, string userEnteredValue)
        {
            List<string> returnList = new List<string>();

            if (String.IsNullOrEmpty(userEnteredKey))
            {
                returnList.Add(keyRequiredString);
            }
            else if ((KeyExists(userEnteredKey)[0] == trueString) &&
                     (ValueExistsCommand(userEnteredKey, userEnteredValue)[0] == trueString))
            {
                //The Dictionary contains the key and the value List already contains the value 

                returnList.Add(valueExistsString);
            }
            else if (KeyExists(userEnteredKey)[0] == trueString)
            {
                //The Dictionary contains the key, but the value List does not contain the value

                commandsDictionary[userEnteredKey].Add(userEnteredValue);
                returnList.Add(addedString);
            }
            else
            {
                //completely new entry for dictionary

                List<string> valuesList = new List<string>();

                valuesList.Add(userEnteredValue);
                commandsDictionary.Add(userEnteredKey, valuesList);
                returnList.Add(addedString);

            }

            return returnList;

        }

        public List<string> KeysCommand()
        {
            List<string> keysList = new List<string>(commandsDictionary.Keys);
            List<string> returnList = new List<string>();

            if (keysList.Count > 0)
            {
                returnList = keysList;
            }
            else
            {
                returnList.Add(emptySetString);
            }

            return returnList;
        }

        public List<string> MembersCommand(string userEnteredKey)
        {
            List<string> membersList = new List<string>();
            List<string> returnList = new List<string>();

            if (commandsDictionary.TryGetValue(userEnteredKey, out membersList))
            {
                returnList = membersList;
            }
            else
            {
                returnList.Add(keyDoesNotExistString);
            }

            return returnList;
        }

        public List<string> RemoveCommand(string userEnteredKey, string userEnteredValue)
        { 
           List<string> returnList = new List<string>();

           if (KeyExists(userEnteredKey)[0] == trueString)
            {
                if (ValueExistsCommand(userEnteredKey, userEnteredValue)[0] == trueString)
                {
                    commandsDictionary[userEnteredKey].Remove(userEnteredValue);
                    if (commandsDictionary[userEnteredKey].Count == 0)
                    {
                        commandsDictionary.Remove(userEnteredKey);
                    }
                    returnList.Add(removedString);
                }
                else
                {
                    returnList.Add(valueDoesNotExistString);
                }

            }
            else
            {
                returnList.Add(keyDoesNotExistString);
            }

            return returnList;
        }

        public List<string> RemoveAllCommand(string userEnteredKey)
        {
            List<string> returnList = new List<string>();

            if (KeyExists(userEnteredKey)[0] == trueString)
            {
                commandsDictionary.Remove(userEnteredKey);
                returnList.Add(removedString);
            }
            else
            {
                returnList.Add(keyDoesNotExistString);
            }

            return returnList;
        }

        public List<string> ClearCommand()
        {
            List<string> returnList = new List<string>();

            commandsDictionary.Clear();
            returnList.Add(clearedString);

            return returnList;
        }


        public List<string>  AllMembersCommand()
        { 
            List<string> returnList = new List<string>();

            if (KeysCommand()[0] == emptySetString)
            {
                returnList.Add(emptySetString);
            }
            else
            {
                foreach(var valuesList in commandsDictionary.Values.ToList())
                {
                    foreach (var value in valuesList)
                    {
                        returnList.Add(value);
                    } 
                } 
            }

            return returnList;
        }

        public List<string> ItemsCommand()
        {
            List<string> returnList = new List<string>();

            if (KeysCommand()[0] == emptySetString)
            {
                returnList.Add(emptySetString);
            }
            else
            {
                foreach (var key in commandsDictionary.Keys.ToList())
                { 
                    foreach (var value in commandsDictionary[key])
                    {
                        returnList.Add(key + ": " + value);
                    }  
                }
            }

            return returnList;
        }


        public List<string> KeyExistsCommand(string userEnteredKey)
        {
            List<string> returnList = new List<string>();

            returnList.Add(KeyExists(userEnteredKey)[0]);
            return returnList;
        }

        public List<string> ValueExistsCommand(string userEnteredKey, string userEnteredValue)
        {
            List<string> returnList = new List<string>(); 

            if (KeyExists(userEnteredKey)[0] == trueString)
            {
                returnList.Add(commandsDictionary[userEnteredKey].Contains(userEnteredValue)  ? trueString : falseString) ;
            }
            else
            {
                returnList.Add(falseString); 
            } 

            return returnList;

        }

        public List<string> KeyExists(string userEnteredKey)
        {
            List<string> returnList = new List<string>();

            returnList.Add(commandsDictionary.ContainsKey(userEnteredKey)  ?  trueString  :  falseString); 

            return returnList;
        }


    }
}
