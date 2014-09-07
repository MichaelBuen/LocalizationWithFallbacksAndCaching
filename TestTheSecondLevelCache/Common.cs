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
        // This is not working though:
        // http://stackoverflow.com/questions/700043/mstest-executing-all-my-tests-simultaneously-breaks-tests-what-to-do
        // public static object LockObject = new object();


        // Just need to change the cache provider to HashtableCacheProvider on unit testing, it's more predictable http://stackoverflow.com/questions/4297934/why-is-nhibernate-cache-hashtablecacheprovider-not-intended-for-production-use#comment4666523_4298483
        // x.Provider<NHibernate.Cache.HashtableCacheProvider>();  




        public static ISessionFactory BuildSessionFactory()
        {
            var sf = DomainMapping.Mapper.BuildSessionFactory(useUnitTest: true);

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
