using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace ExpressionTemplator
{
    public class TemplateGenerator
    {
        

        public Func<IDictionary<string,string>, string> CompileTemplate(string template)
        {

            List<Node> nodes = new List<Node>();

            int firstChar = 0;
            int currentChar = 0;

            void AddTextIfNotEmpty(bool isTemplateParameter)
            {
                if (currentChar != firstChar)
                {
                    //TODO : Figure out if this is worth encapulating into a function
                    //Take the current text and use it to create a node
                    var stringArea = template.AsMemory(firstChar, currentChar - firstChar);
                    nodes.Add(new Node(stringArea, isTemplateParameter: isTemplateParameter));
                }
            }


            

            //This marks where the first character is in the current section of text
            //It will be shifted around after moving past a constant in the string

            while (currentChar < template.Length)
            {

                int sequenceLength = currentChar - firstChar;

                bool isDoubleBracketsOpening = template[currentChar] == '{' && template[currentChar + 1] == '{';
                bool isDoubleBracketsClosing = template[currentChar] == '}' && template[currentChar + 1] == '}';

                if (isDoubleBracketsOpening)
                {
                    //Bundle current string into a node to prepare for parsing the constant name
                    AddTextIfNotEmpty(isTemplateParameter: false);

                    //Skip past brackets {{
                    currentChar += 2;
                    firstChar = currentChar;
                }
                else if (isDoubleBracketsClosing)
                {
                    //Take the current text and use it to create a node
                    AddTextIfNotEmpty(isTemplateParameter: true);

                    //Skip past brackets }}
                    currentChar += 2;
                    firstChar = currentChar;
                }
                else
                {
                    //Go to next character
                    currentChar++;
                }
            }

            //Add the remaining text to a new node
            AddTextIfNotEmpty(isTemplateParameter: false);

            //Converting this to an array to speed up foreach loop
            // Reasoning : The template will be run more than once
            //      The template generator is only run once
            var nodeArray = nodes.ToArray();


            //Later on, optimize this to remove all loops
            //This can be done by constructing an expression tree and "baking" in the values
            // Ex:   "hello {{place}}!"  ------>   "Hello " + place + "!";
            return (data) =>
            {
                StringBuilder sb = new StringBuilder();

                foreach (var node in nodeArray)
                {
                    if (node.IsTemplateParameter)
                    {
                        //Variable, get value from data object
                        string variableValue = data[node.text.ToString()];

                        sb.Append(variableValue);
                    }
                    else
                    {
                        //Normal text, just append
                        sb.Append(node.text);
                    }
                }

                return sb.ToString();
            };
        }


    }
}
