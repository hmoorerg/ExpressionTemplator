using ExpressionTemplator;
using System;
using System.Collections.Generic;
using Xunit;

namespace TemplateTests
{
    public class TemplateGeneratorShould
    {
        private readonly TemplateGenerator _templateGenerator;

        public TemplateGeneratorShould()
        {
            this._templateGenerator = new TemplateGenerator();
        }


        [Fact]
        public void ReturnUnchangedNormalString()
        {
            string input = "this is a normal string without template values";

            var template = _templateGenerator.CompileTemplate(input);

            Assert.True(template(null) == input);
        }

        [Fact]
        public void CreateExceptionOnNullData()
        {
            string input = "this string has {{adjective}} values";

            var template = _templateGenerator.CompileTemplate(input);


            Dictionary<string, string> data = new Dictionary<string, string>
            {
                //This variable is left out intentionally
                //["adjective"] = "special"
            };

            Assert.Throws<KeyNotFoundException>(() => template(data));
        }


        [Fact]
        public void InsertTemplateValues()
        {
            string input = "this string has {{adjective}} values";

            var template = _templateGenerator.CompileTemplate(input);


            Dictionary<string, string> data = new Dictionary<string, string>
            {
                ["adjective"] = "special"
            };

            string output = template(data);
            string expectedOutput = "this string has special values";

            Assert.True(output == expectedOutput);
        }
    }
}
