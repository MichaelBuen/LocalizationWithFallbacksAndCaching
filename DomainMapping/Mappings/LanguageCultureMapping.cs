using Domain;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainMapping.Mappings
{
    class LanguageCultureMapping : ClassMapping<LanguageCulture>
    {
        public LanguageCultureMapping()
        {
            Id(x => x.LanguageCultureCode);
            Property(x => x.EnglishName);
            Property(x => x.NativeName);            
        }
    }
}
