using Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheSecondLevelCache
{
    public class Common
    {
        public static ISessionFactory BuildSessionFactory()
        {
            var sf = DomainMapping.Mapper.BuildSessionFactory();

            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "John";
                session.Update(p);
                tx.Commit();
            }


            return sf;
        }
    }
}
