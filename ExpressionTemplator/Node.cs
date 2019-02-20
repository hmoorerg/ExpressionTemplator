using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTemplator
{
    class Node
    {
        public ReadOnlyMemory<char> text;
        public bool IsTemplateParameter;

        public Node(ReadOnlyMemory<char> text, bool isTemplateParameter)
        {
            this.text = text;
            IsTemplateParameter = isTemplateParameter;
        }
    }
}
