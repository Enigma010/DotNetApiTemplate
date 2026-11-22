using Ddd.App.Entities;

namespace Ddd.App.UnitTests.Entities
{
    public class VariableTests
    {
        [Fact]
        public void SubstituteVariable()
        {
            // Arrange
            var variable = new Variable(){
                Name = "TestVariable",
                Value = "TestValue"
            };
            var template = "This is a ${TestVariable} template.";
            // Act
            var result = variable.Substitute(template);
            // Assert
            Assert.Equal("This is a TestValue template.", result);
        }
        [Fact]
        public void SubstituteVariables()
        {
            // Arrange
            var variables = new List<Variable>()
            {
                new Variable() { Name = "Var1", Value = "Value1" },
                new Variable() { Name = "Var2", Value = "Value2" }
            };
            var template = "This is a ${Var1} and ${Var2} template.";
            // Act
            var result = variables.Substitute(template);
            // Assert
            Assert.Equal("This is a Value1 and Value2 template.", result);
        }
        [Fact]
        public void SubstituteVariableWithNullTemplate()
        {
            // Arrange
            var variable = new Variable()
            {
                Name = "TestVariable",
                Value = "TestValue"
            };
            string? template = null;
            // Act
            var result = variable.Substitute(template);
            // Assert
            Assert.Null(result);
        }
    }
}
