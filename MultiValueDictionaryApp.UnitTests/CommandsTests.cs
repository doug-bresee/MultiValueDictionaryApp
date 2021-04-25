using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValueDictionaryApp.UnitTests
{
    [TestFixture]
    class CommandsTests
    {

        private Commands _commands;

        [SetUp]
        public void SetUp()
        {
            _commands = new Commands();
        }

        [Test]
        public void AddCommand_AddNewItem_ReturnsAddedMessage()
        {
            List<string> result;

            result = _commands.CommandProcessor(new[] { "ADD", "foo", "bar" }); 
            Assert.That(result[0], Is.EqualTo(_commands.addedString));

        }

        [Test]
        public void AddCommand_AddNewItemWithNoKey_ReturnsError()
        {
            List<string> result;

            result = _commands.CommandProcessor(new[] { "ADD" }); 
            Assert.That(result[0], Is.EqualTo(_commands.keyRequiredString));

        }

        [Test]
        public void AddCommand_AddNewItemAlreayExists_ReturnsError()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            result = _commands.CommandProcessor(new[] { "ADD", "foo", "bar" }); 
            Assert.That(result[0], Is.EqualTo(_commands.valueExistsString));

        }


        [Test]
        public void MembersCommand_UseValidKey_ReturnsMembers()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "foo", "baz" });

            result = _commands.CommandProcessor(new[] { "MEMBERS", "foo" });

            Assert.That(result[0], Is.EqualTo("bar"));
            Assert.That(result[1], Is.EqualTo("baz"));
        }



        [Test]
        public void MembersCommand_UseInValidKey_ReturnsError()
        {
            List<string> result; 

            result = _commands.CommandProcessor(new[] { "MEMBERS", "bad" }); 
            Assert.That(result[0], Is.EqualTo(_commands.keyDoesNotExistString)); 
        }


        [Test]
        public void KeysCommand_UseValidKeys_ReturnsKeys()
        { 
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "baz", "bang" });

            result = _commands.CommandProcessor(new[] {"KEYS" });

            Assert.That(result[0], Is.EqualTo("foo"));
            Assert.That(result[1], Is.EqualTo("baz"));
        }



        [Test]
        public void KeysCommand_NoKey_ReturnsError()
        {
            List<string> result; 

            result = _commands.CommandProcessor(new[] { "KEYS" }); 
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString)); 
        }

        [Test]
        public void RemoveCommand_ValidKey_ReturnsRemovedMessage()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            result = _commands.CommandProcessor(new[] { "REMOVE", "foo", "bar" }); 
            Assert.That(result[0], Is.EqualTo(_commands.removedString));
        }

        [Test]
        public void RemoveCommand_InValidKey_ReturnsError()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            _commands.CommandProcessor(new[] { "REMOVE", "foo", "bar" });

            result = _commands.CommandProcessor(new[] { "REMOVE", "foo", "bar" });  
            Assert.That(result[0], Is.EqualTo(_commands.keyDoesNotExistString));

            result = _commands.CommandProcessor(new[] { "KEYS"}); 
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString));
        }

        [Test]
        public void RemoveCommand_ValidKeyInValidValue_ReturnsError()
        {
            List<string> result; 
            
            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            result =  _commands.CommandProcessor(new[] { "REMOVE", "foo", "baz" });  
            Assert.That(result[0], Is.EqualTo(_commands.valueDoesNotExistString));
        }


        [Test]
        public void RemoveAllCommand_ValidKey_ReturnsRemovedMessage()
        {
            List<string> result; 

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "foo", "baz" });

            result = _commands.CommandProcessor(new[] { "KEYS" }); 
            Assert.That(result[0], Is.EqualTo("foo"));

            result = _commands.CommandProcessor(new[] { "REMOVEALL", "foo" }); 
            Assert.That(result[0], Is.EqualTo(_commands.removedString));

            result = _commands.CommandProcessor(new[] { "KEYS" });
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString));
        }


        [Test]
        public void RemoveAllCommand_InValidKey_ReturnsError()
        {
            List<string> result; 

            result = _commands.CommandProcessor(new[] { "KEYS" });
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString));

            result = _commands.CommandProcessor(new[] { "REMOVEALL", "foo" });
            Assert.That(result[0], Is.EqualTo(_commands.keyDoesNotExistString));

        }

        [Test]
        public void ClearAllCommand_ValidKey_ReturnsClearedMessage()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "baz", "bang" });

            result = _commands.CommandProcessor(new[] { "KEYS" }); 
            Assert.That(result[0], Is.EqualTo("foo"));
            Assert.That(result[1], Is.EqualTo("baz"));

            result = _commands.CommandProcessor(new[] { "CLEAR" });
            Assert.That(result[0], Is.EqualTo(_commands.clearedString));

            result = _commands.CommandProcessor(new[] { "KEYS" });
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString));
        }


        [Test]
        public void KeyExistsCommand_ValidKey_ReturnsTrue()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" }); 

            result = _commands.CommandProcessor(new[] { "KEYEXISTS", "foo" });  
            Assert.That(result[0], Is.EqualTo(_commands.trueString)); 

        }

        [Test]
        public void KeyExistsCommand_InValidKey_ReturnsTrue()
        {
            List<string> result; 

            result = _commands.CommandProcessor(new[] { "KEYEXISTS", "foo" });
            Assert.That(result[0], Is.EqualTo(_commands.falseString)); 
        }

        [Test]
        public void ValueExistsCommand_ValidValue_ReturnsTrue()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            result = _commands.CommandProcessor(new[] { "VALUEEXISTS", "foo", "bar" });
            Assert.That(result[0], Is.EqualTo(_commands.trueString));

        }

        [Test]
        public void ValueExistsCommand_InValidValue_ReturnsFalse()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });

            result = _commands.CommandProcessor(new[] { "VALUEEXISTS", "foo", "baz" });
            Assert.That(result[0], Is.EqualTo(_commands.falseString));

        }


        [Test]
        public void AllMembersCommand_ValidValues_ReturnsMembers()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "foo", "baz" });
            _commands.CommandProcessor(new[] { "ADD", "bang", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "bang", "baz" });


            result = _commands.CommandProcessor(new[] { "ALLMEMBERS"});
            Assert.That(result[0], Is.EqualTo("bar"));
            Assert.That(result[1], Is.EqualTo("baz"));
            Assert.That(result[2], Is.EqualTo("bar"));
            Assert.That(result[3], Is.EqualTo("baz"));

        }

        [Test]
        public void ItemsCommand_ValidValuesAndKeys_ReturnsKeysAndValues()
        {
            List<string> result;

            _commands.CommandProcessor(new[] { "ADD", "foo", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "foo", "baz" });
            _commands.CommandProcessor(new[] { "ADD", "bang", "bar" });
            _commands.CommandProcessor(new[] { "ADD", "bang", "baz" });


            result = _commands.CommandProcessor(new[] { "ITEMS" });
            Assert.That(result[0], Is.EqualTo("foo: bar"));
            Assert.That(result[1], Is.EqualTo("foo: baz"));
            Assert.That(result[2], Is.EqualTo("bang: bar"));
            Assert.That(result[3], Is.EqualTo("bang: baz"));

        }

        [Test]
        public void ItemsCommand_EmptyDictionary_ReturnsEmptySetMessge()
        {
            List<string> result; 

            result = _commands.CommandProcessor(new[] { "ITEMS" });
            Assert.That(result[0], Is.EqualTo(_commands.emptySetString)); 
        }
    } 
}
