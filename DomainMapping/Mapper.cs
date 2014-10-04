using DomainMapping.Mappings;
using System.Collections.Generic;



using NHibernate.Cfg;


namespace DomainMapping
{
    public static class Mapper
    {


        static NHibernate.ISessionFactory _sessionFactory = Mapper.BuildSessionFactory();


        // call this on production
        public static NHibernate.ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }


        // Call this on unit testing, so we can test caching on each test method independently
        public static NHibernate.ISessionFactory BuildSessionFactory(bool useUnitTest = false)
        {
            var mapper = new NHibernate.Mapping.ByCode.ModelMapper();


            mapper.AddMappings(new[]
                {
                    typeof(PersonMapping),
                    typeof(OrderMapping),

                    typeof(ThingMapping),
                    typeof(ThingTranslationMapping),

                    typeof(LanguageCultureMapping)
                });


            var cfg = new NHibernate.Cfg.Configuration();

            // .DatabaseIntegration! Y U EXTENSION METHOD?
            cfg.DataBaseIntegration(c =>
            {
                // SQL Server
                c.Driver<NHibernate.Driver.SqlClientDriver>();
                c.Dialect<NHibernate.Dialect.MsSql2008Dialect>();
                c.ConnectionString = "Server=.;Database=LocalizationWithFallbacksAndCaching;Trusted_Connection=True";

                // // PostgreSQL
                // c.Driver<NHibernate.Driver.NpgsqlDriver>();
                // c.Dialect<NHibernate.Dialect.PostgreSQLDialect>();
                // c.ConnectionString = "Server=.; Database=test_the_database; User=postgres; password=opensesame";				

                c.LogSqlInConsole = true;
                c.LogFormattedSql = true;
            });

            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            cfg.AddMapping(domainMapping);


            var filterDef = new NHibernate.Engine.FilterDefinition("lf", /*default condition*/ null,
                                                                   new Dictionary<string, NHibernate.Type.IType>
                                                                       {
                                                                           { "LanguageCultureCode", NHibernate.NHibernateUtil.String}
                                                                       }, useManyToOne: false);

            cfg.AddFilterDefinition(filterDef);


            cfg.Cache(x =>
            {
                // SysCache is not stable on unit testing
                if (!useUnitTest)
                {
                    x.Provider<NHibernate.Caches.SysCache.SysCacheProvider>();

                    // I don't know why SysCacheProvider is not stable on simultaneous unit testing, 
                    // might be SysCacheProvider is just giving one session factory, so simultaneous test see each other caches
                    // This solution doesn't work: http://stackoverflow.com/questions/700043/mstest-executing-all-my-tests-simultaneously-breaks-tests-what-to-do					
                }
                else
                {
                    // This is more stable in unit testing
                    x.Provider<NHibernate.Cache.HashtableCacheProvider>();
                }





                // http://stackoverflow.com/questions/2365234/how-does-query-caching-improves-performance-in-nhibernate

                // Need to be explicitly turned on so the .Cacheable directive on Linq will work:                    
                x.UseQueryCache = true;
            });


            if (useUnitTest)
                cfg.SetInterceptor(new NHSQLInterceptor());

            var sf = cfg.BuildSessionFactory();




            //using (var file = new System.IO.FileStream(@"c:\x\ddl.txt",
            //       System.IO.FileMode.Create,
            //       System.IO.FileAccess.ReadWrite))
            //using (var sw = new System.IO.StreamWriter(file))
            //{
            //    new SchemaUpdate(cfg)
            //        .Execute(sw.Write, false);
            //}

            return sf;
        }

        class NHSQLInterceptor : NHibernate.EmptyInterceptor
        {
            // http://stackoverflow.com/questions/2134565/how-to-configure-fluent-nhibernate-to-output-queries-to-trace-or-debug-instead-o
            public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
            {

                Mapper.NHibernateSQL = sql.ToString();
                return sql;
            }

        }

        public static string NHibernateSQL { get; set; }


    } // Mapper



}
