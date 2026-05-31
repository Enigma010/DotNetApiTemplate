using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.Core
{
    public class EnvConfig
    {
        public EnvConfig(string path)
        {
            Path = path;
            if(!File.Exists(path))
            {
                throw new FileNotFoundException($"Environment configuration file not found: {path}");
            }
        }
        public string Path { get; private set; }  = string.Empty;
        public void SetEnvironmentVariables()
        {
            File.ReadAllLines(Path).ToList().ForEach(line =>
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("#"))
                {
                    string[] parts = trimmedLine.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        string variable = parts[0].Trim();
                        string value = parts[1].Trim();
                        Environment.SetEnvironmentVariable(variable, value);
                    }
                }
            });
        }
    }
}
