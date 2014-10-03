using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainMapping
{
    public static class Helper
    {
        public static NHibernate.ISession SetLanguageCultureCode(this NHibernate.ISession session, string languageCultureCode)
        {

            session.EnableFilter("lf").SetParameter("LanguageCultureCode", languageCultureCode);

            return session;
        }
    }

    //public static class Helper
    //{
    //    public static Transactional CreateTransaction(this NHibernate.ISessionFactory sessionFactory, string languageCultureCode)
    //    {
           

    //        var session = sessionFactory.OpenSession();
    //        session.EnableFilter("lf").SetParameter("LanguageCultureCode", languageCultureCode);

    //        var transaction = session.BeginTransaction();

    //        var t = new Transactional(session, transaction);
            

    //        return t;
    //    }
    //}

    //public class Transactional : IDisposable
    //{

    //    void IDisposable.Dispose()
    //    {
    //        this.Transaction.Commit();

    //        this.Transaction.Dispose();
    //        this.Session.Dispose();
    //    }

    //    NHibernate.ISession _session;
    //    public NHibernate.ISession Session 
    //    { 
    //        get { return _session; }
    //        internal set { _session = value; }
    //    }

    //    NHibernate.ITransaction Transaction { get; set; }

    //    internal Transactional(NHibernate.ISession session, NHibernate.ITransaction transaction)
    //    {
            
    //        this.Session = session;
    //        this.Transaction = transaction;
    //    }
    //}
}
