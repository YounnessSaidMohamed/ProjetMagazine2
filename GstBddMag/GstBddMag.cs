using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Devoir1ClassesMetier;

namespace GstBddMag
{
    public class GstBddMag
    {
        private MySqlConnection cnx;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        public GstBddMag()
        {
            string chaine = "Server=localhost;Database=bddpigiste;Uid=root;Pwd=";
            cnx = new MySqlConnection(chaine);
            cnx.Open();
        }

        public List<Specialite> getLesSpecialites()
        {
            List<Specialite> mesSpecialites = new List<Specialite>();
            cmd = new MySqlCommand("SELECT idSpe,nomSpe FROM specialite", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Specialite unNouvelleSpecialite = new Specialite(Convert.ToInt16(dr[0].ToString()), (dr[1].ToString()));
                mesSpecialites.Add(unNouvelleSpecialite);
            }
            dr.Close();
            return mesSpecialites;
        }

        public List<Pigiste> getLesPigistes()
        {
            List<Pigiste> mesPigistes = new List<Pigiste>();
            cmd = new MySqlCommand("SELECT numPigiste,nomPigiste,prixFeuillet,idSpe,nomSpe FROM pigiste INNER JOIN avoir ON pigiste.numPigiste = avoir.numPig INNER JOIN specialite ON avoir.numSpe = specialite.idSpe GROUP BY nomPigiste", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Pigiste unNouveauPigiste = new Pigiste(Convert.ToInt16(dr[0].ToString()), (dr[1].ToString()), Convert.ToDouble(dr[2].ToString()), new Specialite(Convert.ToInt16(dr[3].ToString()), (dr[4].ToString())));
                mesPigistes.Add(unNouveauPigiste);
            }
            dr.Close();
            return mesPigistes;
        }

        public List<Magazine> getAllMagazines()
        {
            List<Magazine> mesMagazines = new List<Magazine>();
            cmd = new MySqlCommand("SELECT numMag,nomMag,numSpe,nomSpe FROM magazine INNER JOIN specialite ON magazine.numSpe = specialite.idSpe", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Magazine unNouveauMagazine = new Magazine(Convert.ToInt16(dr[0].ToString()), (dr[1].ToString()), new Specialite(Convert.ToInt16(dr[2].ToString()), (dr[3].ToString())));
                mesMagazines.Add(unNouveauMagazine);
                
            }
            dr.Close();
            return mesMagazines;
        }

        public List<Article> getAllArticlesByMagazine(int idMag)
        {
            List<Article> mesArticles = new List<Article>();
            cmd = new MySqlCommand("SELECT titre,nbFeuillet,numPigiste,nomPig,prixFeuillet,idSpe,nomSpe FROM article INNER JOIN pigiste ON article.numPig = pigiste.numPigiste INNER JOIN avoir ON pigiste.numPigiste = avoir.numPig INNER JOIN specialite ON avoir.numSpe = specialite.idSpe WHERE idMag = "+idMag+" GROUP by titre" , cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Article unNouvelArticle = new Article((dr[0].ToString()), Convert.ToInt16(dr[1].ToString()), new Pigiste(Convert.ToInt16(dr[2].ToString()), (dr[3].ToString()), Convert.ToDouble(dr[4].ToString()), new Specialite(Convert.ToInt16(dr[5].ToString()), (dr[6].ToString()))));
                mesArticles.Add(unNouvelArticle);

            }
            dr.Close();
            return mesArticles;
        }

        public double getMontantMagazine(int idMag)
        {
            double montant = 0;
            cmd = new MySqlCommand("SELECT sum((nbFeuillet* prixFeuillet)) FROM article INNER JOIN pigiste ON article.numPig = pigiste.numPigiste WHERE idMag =" + idMag, cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            montant = Convert.ToDouble(dr[0].ToString());
            dr.Close();
            return montant;
        }

        public void AjouterArticle(string titre,int nbFeuillet,int numPig,string nomPig,int idMag )
        {
            cmd = new MySqlCommand("INSERT INTO article(titre, nbFeuillet, numPig, nomPig, idMag) VALUES ('"+titre+"','"+nbFeuillet+"','"+numPig+"','"+nomPig+"','"+idMag+"')" , cnx);
            cmd.ExecuteNonQuery();
            dr.Close();
        }

        public string VerifierSpecialite(string nomPig,string nomSpe)
        {
            string uneSpecialite = "";
            cmd = new MySqlCommand("SELECT nomSpe FROM pigiste INNER JOIN avoir ON pigiste.numPigiste = avoir.numPig INNER JOIN specialite ON avoir.numSpe = specialite.idSpe WHERE nomSpe = '"+nomSpe+"' AND nomPigiste = '"+nomPig+"'", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                uneSpecialite = (dr[0].ToString());

            }
            dr.Close();
            return uneSpecialite.ToString();
        }

        public List<TotalPigiste> getTotalPigiste(int idMag,string nomPig)
        {
            List<TotalPigiste> lesPigistes = new List<TotalPigiste>();
            cmd = new MySqlCommand("SELECT nomPigiste,SUM(nbFeuillet*prixFeuillet) FROM pigiste INNER JOIN article ON pigiste.numPigiste = article.numPig WHERE idMag = '"+idMag+"' and nomPigiste = '"+nomPig+"'" , cnx);
            dr = cmd.ExecuteReader();
            //dr.Read();
            while (dr.Read())
            {
                TotalPigiste unNouveauPigiste = new TotalPigiste((dr[0].ToString()), Convert.ToDouble(dr[1].ToString()));
                lesPigistes.Add(unNouveauPigiste);

            }
            dr.Close();
            return lesPigistes;
        }
    }
}
