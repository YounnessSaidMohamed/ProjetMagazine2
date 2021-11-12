using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devoir1ClassesMetier
{
    public class Magazine
    {
        private int numMagazine;
        private string nomMagazine;
        private Specialite laSpecialite;
        private List<Article> lesArticles;

        public Magazine(int unNum, string unNom, Specialite uneSpecialite)
        {
            NumMagazine = unNum;
            NomMagazine = unNom;
            LaSpecialite = uneSpecialite;
            LesArticles = new List<Article>();
        }

        public int NumMagazine { get => numMagazine; set => numMagazine = value; }
        public string NomMagazine { get => nomMagazine; set => nomMagazine = value; }
        public Specialite LaSpecialite { get => laSpecialite; set => laSpecialite = value; }
        public List<Article> LesArticles { get => lesArticles; set => lesArticles = value; }

        
    }
}
