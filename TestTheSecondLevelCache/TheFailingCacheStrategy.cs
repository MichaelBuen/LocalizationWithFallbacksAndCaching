
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using NHibernate.Linq;
using Domain;

namespace TestTheSecondLevelCache
{
    [TestClass]
    public class TheFailingCacheStrategy
    {

        [TestMethod]
        public void Fail_Caching_NoPagingOfDependentTable_IfStale_WhenReferencedTableIsUpdatedOutside_AndCacheRegionIsNotApplied()
        {



            var sf = Common.BuildSessionFactory();






            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "ZX-" + p.FirstName;
                session.Update(p);
                tx.Commit();


                sf.EvictQueries("Reference");
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].Person.FirstName);
                Assert.AreEqual("ZX-John", list[1].Person.FirstName);
            }

        }


        [TestMethod]
        public void Fail_Caching_NoPagingOfDependentTable_IfStale_WhenReferencedTableIsUpdatedOutside_AndQueryCacheNotEvicted()
        {



            var sf = Common.BuildSessionFactory();






            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).CacheRegion("Reference").Cacheable().ToList();
            }

            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "ZX-" + p.FirstName;
                session.Update(p);
                tx.Commit();


                
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).CacheRegion("Reference").Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].Person.FirstName);
                Assert.AreEqual("ZX-John", list[1].Person.FirstName);
            }

        }


        [TestMethod]
        public void Fail_Caching_NoPagingOfDependentTable_IfStale_WhenReferencedTableIsUpdatedOutside_AndQueryCacheNotEvicted_AndCacheRegionNotSpecificiedInQuery()
        {



            var sf = Common.BuildSessionFactory();






            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "ZX-" + p.FirstName;
                session.Update(p);
                tx.Commit();



            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                // this Order query is oblivous to stateles changes on Person entity, hence this will not requery the Order with new sorting, and then this will return stale cache
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].Person.FirstName);
                Assert.AreEqual("ZX-John", list[1].Person.FirstName);
            }

        }



    }
}
