using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace Domain
{
    public class Thing
    {
        public int ThingId { get; set; }
        public int YearInvented { get; set; }
    }

    public class ThingTranslation
    {
        ThingTranslationCompositePK _pk = new ThingTranslationCompositePK();
        protected internal ThingTranslationCompositePK ThingTranslationCompositePK 
        {
            get { return _pk;  }
            set { _pk = value; }
        }


        Thing _thing;
        public Thing Thing 
        {
            get { return _thing;  }
            set
            {
                _thing = value;
                _pk.ThingId = _thing.ThingId;
            }
        }


        LanguageCulture _languageCulture;
        public LanguageCulture LanguageCulture 
        {
            get { return _languageCulture; }
            set 
            {
                _languageCulture = value;
                _pk.LanguageCultureCode = _languageCulture.LanguageCultureCode;
            }
        }


        

        public string ThingName { get; set; }
        public string ThingDescription { get; set; }
    }


    public class ThingTranslationCompositePK
    {
        public int ThingId { get; set; }
        public string LanguageCultureCode { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var t = obj as ThingTranslationCompositePK;
            if (t == null)
                return false;

            if (t.ThingId == this.ThingId && t.LanguageCultureCode == this.LanguageCultureCode)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (this.ThingId + "|" + this.LanguageCultureCode).GetHashCode();
        }
    }


}
