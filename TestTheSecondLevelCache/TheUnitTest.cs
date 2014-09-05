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
        public void Test_Simplest_Caching()
        {
            Console.WriteLine("Test Simplest Caching");

            var sf = DomainMapping.Mapper.BuildSessionFactory();


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
        public void Test_Caching_With_OrderBy()
        {
            Console.WriteLine("Test Caching With Order By");

            var sf = DomainMapping.Mapper.BuildSessionFactory();

            // I don't kno why this problem says the second query with just different OrderBy from the first query receives cached results of first query
            // http://stackoverflow.com/questions/10725241/nhibernate-retrieves-cached-query-results-even-though-the-order-by-clause-differ


            // it looks like a Criteria bug only, there's no problem in Linq

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var list = session.Query<Person>().OrderBy(x => x.FirstName).Cacheable().ToList();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {

                var list = session.Query<Person>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Cacheable().ToList();
            }
        }



    }


}
