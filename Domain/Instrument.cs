using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Instrument
    {
        public virtual int InstrumentId { get; set; }

        public virtual int YearInvented { get; set; }
    }


    [Serializable]
    public class InstrumentLocalizationId
    {
        public virtual int InstrumentId { get; set; }
        public virtual string LanguageCultureCode { get; set; }



        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var a = obj as InstrumentLocalizationId;

            if (a == null)
                return false;

            if (a.InstrumentId == this.InstrumentId && a.LanguageCultureCode == this.LanguageCultureCode)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (this.InstrumentId + "|" + this.LanguageCultureCode).GetHashCode();
        }
    }


    public class InstrumentLocalization
    {
        InstrumentLocalizationId _pk = new InstrumentLocalizationId();
        protected internal virtual InstrumentLocalizationId InstrumentLocalizationId
        {
            get { return _pk; }
            set { _pk = value; }
        }

        Instrument _instrument;
        public virtual Instrument Instrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;
                _pk.InstrumentId = _instrument.InstrumentId;
            }
        }

        public virtual string LanguageCultureCode
        {
            get { return _pk.LanguageCultureCode; }
            set { _pk.LanguageCultureCode = value; }
        }

        public virtual string InstrumentName { get; protected internal set; }


        // A guide for the user, so he/she could know the source language of the untranslated string came from
        public virtual string ActualLanguageCultureCode { get; protected internal set; }

    }



}
