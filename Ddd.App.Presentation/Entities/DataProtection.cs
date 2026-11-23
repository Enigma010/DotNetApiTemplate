using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.Presentation.Entities
{
    public class DataProtection
    {
        public const string SectionName = nameof(DataProtection);
        public string Directory { get; set; } = string.Empty;
    }
}
