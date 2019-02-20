using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTemplator
{
    class Node
    {
        public string text;
        public bool IsTemplateParameter;

        public Node(string text, bool isTemplateParameter)
        {
            this.text = text;
            IsTemplateParameter = isTemplateParameter;
        }
    }
}
