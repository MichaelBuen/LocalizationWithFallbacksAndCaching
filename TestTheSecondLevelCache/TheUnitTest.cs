using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHibernate;

using NHibernate.Linq;
using Domain;

using System.Linq;

namespace TestTheSecondLevelCache
{
    [TestClass]
    public class TheUnitTest
    {

   

        [TestMethod]
        public void Test()
        {

        }

        [TestMethod]
        public void Test_Simplest_Caching()
        {
            
            var sf = Common.BuildSessionFactory();
            

            using(var session = sf.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                var list = session.Query<Person>().Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {

                var list = session.Query<Person>().Cacheable().ToList();                
            }
        }


        [TestMethod]
        public void Test_Caching_With_Same_OrderBy()
        {
            var sf = Common.BuildSessionFactory();

            // I don't know why this problem says the second query with just different OrderBy from the first query receives cached results of first query
            // http://stackoverflow.com/questions/10725241/nhibernate-retrieves-cached-query-results-even-though-the-order-by-clause-differ


            // it looks like a Criteria bug only, there's no problem in Linq

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1x");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }            
        }


        [TestMethod]
        public void Test_Caching_With_Different_OrderBy()
        {
            var sf = Common.BuildSessionFactory();

            // I don't know why this problem says the second query with just different OrderBy from the first query receives cached results of first query
            // http://stackoverflow.com/questions/10725241/nhibernate-retrieves-cached-query-results-even-though-the-order-by-clause-differ


            

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Cacheable().ToList();
            }
        }

        
        [TestMethod]
        public void Test_Evicting_Object()
        {
            var sf = Common.BuildSessionFactory();

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                sf.Evict(typeof(Person), 2);
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                // the Evict forces the cached query to re-fetch the Person #2                
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }            

        }


        [TestMethod]
        public void Test_Paging_IfStale_When_Updated()
        {
            var sf = Common.BuildSessionFactory();

            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "John";
                session.Update(p);
                tx.Commit();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Skip(0).Take(100).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var p = session.Load<Person>(1);
                p.FirstName = "ZX-" + p.FirstName;
                session.Save(p);
                tx.Commit();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                // the Evict forces the cached query to re-fetch the Person #2                
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Skip(0).Take(100).Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].FirstName);
            }

        }


        [TestMethod]
        public void Test_NoPaging_IfStale_When_Updated()
        {
            var sf = Common.BuildSessionFactory();

    

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var p = session.Load<Person>(1);
                p.FirstName = "ZX-" + p.FirstName;
                session.Save(p);
                tx.Commit();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                // the Evict forces the cached query to re-fetch the Person #1                
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].FirstName);
            }

        }



        [TestMethod]
        public void Test_NoPaging_IfStale_When_UpdatedOutside()
        {



            var sf = Common.BuildSessionFactory();




            using (var session = sf.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Stateless update");
                var p = session.Get<Person>(1);
                p.FirstName = "John";
                session.Update(p);
                tx.Commit();
            }
            

 

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Person>().OrderBy(x => x.FirstName).CacheRegion("Reference").Cacheable().ToList();
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
                var list = session.Query<Person>().OrderBy(x => x.FirstName).CacheRegion("Reference").Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].FirstName);
            }

        }

        


        [TestMethod]
        public void Test_NoPagingOfDependentTable_IfStale_WhenReferencedTableIsUpdatedOutside()
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


                sf.EvictQueries("Reference");
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 2");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).CacheRegion("Reference").Cacheable().ToList();
                Assert.AreEqual("Paul", list[0].Person.FirstName);                
            }

        }


        [TestMethod]
        public void Test_NoCaching_NoPagingOfDependentTable_IfStale_WhenReferencedTableIsUpdatedOutside()
        {



            var sf = Common.BuildSessionFactory();






            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Query 1");
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Fetch(x => x.Person).ToList();
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
                var list = session.Query<Order>().OrderBy(x => x.Person.FirstName).Fetch(x => x.Person).ToList();
                Assert.AreEqual("Paul", list[0].Person.FirstName);
                Assert.AreEqual("ZX-John", list[1].Person.FirstName);
            }

        }

  

    }


}
