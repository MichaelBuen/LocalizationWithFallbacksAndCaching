using Domain;
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
            Id(x => x.PersonId);
            Property(x => x.FirstName);
            Property(x => x.LastName);
        }
    }
}
