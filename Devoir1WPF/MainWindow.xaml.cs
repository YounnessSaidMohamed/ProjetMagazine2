using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Devoir1ClassesMetier;
using GstBddMag;

namespace Devoir1WPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //List<Pigiste> mesPigistes;
        //List<Magazine> mesMagazines;

        public MainWindow()
        {
            InitializeComponent();
        }

        GstBddMag.GstBddMag gst;

        private void lstMagazines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(lstMagazines.SelectedItem!=null)
            {
                var gst = new GstBddMag.GstBddMag();
                List<TotalPigiste> totpigiste = new List<TotalPigiste>();
                List<string> lesPigistes = new List<string>();
                lstArticles.ItemsSource = gst.getAllArticlesByMagazine((lstMagazines.SelectedItem as Magazine).NumMagazine);
                txtMontantMagazine.Text = gst.getMontantMagazine((lstMagazines.SelectedItem as Magazine).NumMagazine).ToString();
                foreach (Article art in gst.getAllArticlesByMagazine((lstMagazines.SelectedItem as Magazine).NumMagazine))
                {
                        
                        lesPigistes.Add(art.LePigiste.NomPigiste);
                }
                IEnumerable<string> distinctPigistes = lesPigistes.Distinct();
                foreach (string p in distinctPigistes)
                {   
                    
                        totpigiste.AddRange(gst.getTotalPigiste((lstMagazines.SelectedItem as Magazine).NumMagazine, p));
                    
                }
                lstTotalPigiste.ItemsSource = totpigiste;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var gst = new GstBddMag.GstBddMag();

            gst.getLesPigistes();
            gst.getLesSpecialites();
         

            lstMagazines.ItemsSource = gst.getAllMagazines() ;
            cboPigistes.ItemsSource = gst.getLesPigistes();

        }

        private void lstArticles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // A vous de jouer
            if (lstArticles.SelectedItem!=null)
            {
                txtNomPigiste.Text = (lstArticles.SelectedItem as Article).LePigiste.NomPigiste;
                txtMontantArticle.Text = (((lstArticles.SelectedItem as Article).LePigiste.PrixFeuillet) * (lstArticles.SelectedItem as Article).NbFeuillets).ToString();
            }
        }

        private void btnAjouterArticle_Click(object sender, RoutedEventArgs e)
        {
            // A vous de jouer
            var gst = new GstBddMag.GstBddMag();
            if ((lstMagazines.SelectedItem) == null)
            {
                MessageBox.Show("Veuillez choisir un magazine","Erreur de saisie",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else if(txtTitreArticle.Text == "")
            {
                MessageBox.Show("Saisir un titre", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if ((cboPigistes.SelectedItem) == null)
            {
                MessageBox.Show("Veuillez choisir un pigiste", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if ((gst.VerifierSpecialite((cboPigistes.SelectedItem as Pigiste).NomPigiste,((lstMagazines.SelectedItem as Magazine).LaSpecialite.NomSpe)) == ""))
            {
                MessageBox.Show("Le pigiste choisi ne possède pas la spécialité du magazine", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string titre = txtTitreArticle.Text;
                int nbFeuillet = Convert.ToInt32(sldNbFeuillets.Value);
                int numPig = (cboPigistes.SelectedItem as Pigiste).NumPigiste;
                string nomPig = (cboPigistes.SelectedItem as Pigiste).NomPigiste;
                int idMag = (lstMagazines.SelectedItem as Magazine).NumMagazine;
                gst.AjouterArticle(titre, nbFeuillet, numPig, nomPig, idMag);
                lstArticles.ItemsSource = gst.getAllArticlesByMagazine((lstMagazines.SelectedItem as Magazine).NumMagazine);
                txtMontantMagazine.Text = gst.getMontantMagazine((lstMagazines.SelectedItem as Magazine).NumMagazine).ToString();
            }
        }
    }
}
