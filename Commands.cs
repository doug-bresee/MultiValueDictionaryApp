using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValueDictionaryApp
{
    class Commands
    {
        //Created by: Douglas Bresee

        Dictionary<string, List<string>> commandsDictionary = null;
        public Commands()
        {
            commandsDictionary = new Dictionary<string, List<string>>();

        }

        public List<string> CommandProcessor(string[] userEnteredValues)
        {
            List<string> commandResult = new List<string>();
            string userEnteredCommand = string.Empty;
            string userEnteredKey = string.Empty;
            string userEnteredValue = string.Empty;

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
                    commandResult.Add("Command is not valid.");
                    break;
            }

            return commandResult;
        }


        public List<string> AddCommand(string userEnteredKey, string userEnteredValue)
        {
            List<string> result = new List<string>();

            if (String.IsNullOrEmpty(userEnteredKey))
            {
                result.Add("ERROR, Key is required for ADD.");
            }
            else if (commandsDictionary.ContainsKey(userEnteredKey) &&
                     commandsDictionary[userEnteredKey].Contains(userEnteredValue))
            {
                //The Dictionary contains the key and the value List already contains the value 

                result.Add("ERROR, value already exists.");
            }
            else if (commandsDictionary.ContainsKey(userEnteredKey))
            {
                //The Dictionary contains the key, but the value List does not contain the value

                commandsDictionary[userEnteredKey].Add(userEnteredValue);
                result.Add("Added");
            }
            else
            {
                //completely new entry for dictionary

                List<string> valuesList = new List<string>();

                valuesList.Add(userEnteredValue);

                commandsDictionary.Add(userEnteredKey, valuesList);
                result.Add("Added");

            }

            return result;

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
                returnList.Add("empty set");
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
                returnList.Add("ERROR, key does not exist.");
            }

            return returnList;
        }

        public List<string> RemoveCommand(string userEnteredKey, string userEnteredValue)
        { 
           List<string> returnList = new List<string>();

           if (KeyExists(userEnteredKey)[0] == "true")
            {
                if (commandsDictionary[userEnteredKey].Contains(userEnteredValue))
                {
                    commandsDictionary[userEnteredKey].Remove(userEnteredValue);
                    if (commandsDictionary[userEnteredKey].Count == 0)
                    {
                        commandsDictionary.Remove(userEnteredKey);
                    }
                    returnList.Add("Removed");
                }
                else
                {
                    returnList.Add("ERROR, value does not exist");
                }

            }
            else
            {
                returnList.Add("ERROR, key does not exist.");
            }

            return returnList;
        }

        public List<string> RemoveAllCommand(string userEnteredKey)
        {
            List<string> returnList = new List<string>();

            if (KeyExists(userEnteredKey)[0] == "true")
            {
                commandsDictionary.Remove(userEnteredKey);
                returnList.Add("Removed");
            }
            else
            {
                returnList.Add("ERROR, key does not exist.");
            }

            return returnList;
        }

        public List<string> ClearCommand()
        {
            List<string> returnList = new List<string>();

            commandsDictionary.Clear();
            returnList.Add("Cleared");
            return returnList;
        }


        public List<string>  AllMembersCommand()
        { 
            List<string> returnList = new List<string>();

            if (KeysCommand()[0] == "empty set")
            {
                returnList.Add("empty set");
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

            if (KeysCommand()[0] == "empty set")
            {
                returnList.Add("empty set");
            }
            else
            {
                foreach (var key  in commandsDictionary.Keys.ToList())
                { 
                    foreach (var value in commandsDictionary[key])
                    {
                        returnList.Add(key + ": " + value);
                    }  
                }
            }

            return returnList;
        }



        public List<string> OutputMembersOrItems(string outputType)
        {
            List<string> returnList = new List<string>();

            if (KeysCommand()[0] == "empty set")
            {
                returnList.Add("empty set");
            }
            else
            {
                foreach (var valuesList in commandsDictionary.Values.ToList())
                {
                    foreach (var value in valuesList)
                    {
                        returnList.Add(value);
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

            if (KeyExists(userEnteredKey)[0] == "true")
            {
                returnList.Add(commandsDictionary[userEnteredKey].Contains(userEnteredValue)  ? "true"  : "false") ;
            }
            else
            {
                returnList.Add("false"); 
            } 

            return returnList;

        }

        public List<string> KeyExists(string userEnteredKey)
        {
            List<string> returnList = new List<string>();

            returnList.Add(commandsDictionary.ContainsKey(userEnteredKey)  ? "true" : "false"); 

            return returnList;
        }


    }
}
