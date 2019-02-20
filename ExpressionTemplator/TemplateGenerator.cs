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

            StringBuilder currentSection = new StringBuilder();
            int i = 0;
            while (i < template.Length)
            {

                bool isDoubleBracketsOpening = template[i] == '{' && template[i+1] == '{';
                bool isDoubleBracketsClosing = template[i] == '}' && template[i + 1] == '}';

                if (isDoubleBracketsOpening)
                {
                    //Finish up current string, clear the buffer
                    nodes.Add(new Node(currentSection.ToString(), false));
                    currentSection.Clear();

                    //Skip past brackets
                    i += 2;
                }
                else if (isDoubleBracketsClosing)
                {

                    //Take the current text and use it to create a node
                    nodes.Add(new Node(currentSection.ToString(), true));
                    currentSection.Clear();

                    //Skip past brackets
                    i += 2;
                }
                else
                {
                    //Add current character to the buffer
                    currentSection.Append(template[i]);
                    
                    //Go to next character
                    i += 1;
                }


            }

            //Add the remaining text to a new node
            nodes.Add(new Node(currentSection.ToString(),isTemplateParameter: false));



            return (data) =>
            {
                StringBuilder sb = new StringBuilder();

                foreach (var node in nodes)
                {
                    if (node.IsTemplateParameter)
                    {
                        //Variable, get value from data object
                        string variableValue = data[node.text];

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
