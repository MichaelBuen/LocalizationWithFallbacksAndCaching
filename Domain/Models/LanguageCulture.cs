using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DomainMapping")]

namespace Domain
{
    public class LanguageCulture
    {
        public string LanguageCultureCode { get; set; }

        public string NativeName { get; set; }
        public string EnglishName { get; set; }


        public LanguageCulture NeutralLanguage { get; set; }
    }
}
