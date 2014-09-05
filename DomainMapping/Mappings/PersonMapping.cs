using Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainMapping.Mappings
{
    public class PersonMapping : ClassMapping<Person>
    {
        public PersonMapping()
        {
            Cache(x => 
            {
                x.Usage(CacheUsage.ReadWrite);
                x.Region("Person");
            });

            Id(x => x.PersonId);
            Property(x => x.FirstName);
            Property(x => x.LastName);
        }
    }
}
