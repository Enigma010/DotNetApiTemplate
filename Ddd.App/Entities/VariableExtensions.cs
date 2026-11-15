namespace Ddd.App.Entities
{
    public static class VariableExtensions
    {
        public static string? Substitute(this IEnumerable<Variable> variables, string? template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return template;
            }
            foreach (var variable in variables)
            {
                template = variable.Substitute(template);
            }
            return template;
        }
        public static string? Substitute(this Variable variable, string? template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return template;
            }
            return template.Replace($"${{{variable.Name}}}", variable.Value ?? string.Empty);
        }
    }
}
