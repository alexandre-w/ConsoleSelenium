using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.IO;
using ConsoleSelenium.Watcher;
using ConsoleSelenium.Selenium;
using ConsoleSelenium.PDF;
using ConsoleSelenium.Helpers;

namespace ConsoleSelenium
{
    public class Program
    {

        /// <summary>
        /// Dictionnary of parameters 
        /// </summary>
        public static Dictionary<string, string> dicParam { get; set; }

        static void Main(string[] args)
        {

            Console.WriteLine(Const.goText);

            //Lecture fichier avec les paramètres de recherche
            dicParam = new Dictionary<string, string>();
            ReadFileParam();

            if (isDicOk())
            {
                //Lancement du Navigateur
                GoLearnup();

                //Librairie pour la création du PDF
                CustomPdf cPdf = new CustomPdf();
                //Liste des fichiers créés par le navigateur
                cPdf.files = CustomWatcher.files;
                //Création du PDF final 
                cPdf.concatPDF();
            }
            //Fin
            Console.WriteLine(Const.endScriptText);
            Console.ReadLine();

        }

        /// <summary>
        /// Check if the dictionnary is OK
        /// </summary>
        /// <returns>Dictionnary ok or not</returns>
        static private bool isDicOk()
        {
            if (dicParam.ContainsKey(Const.login) && dicParam.ContainsKey(Const.motdePasse) && dicParam.ContainsKey(Const.url) && dicParam.ContainsKey(Const.search))
                return true;

            return false;
        }

        /// <summary>
        /// Read trhe parameters file
        /// </summary>
        public static void ReadFileParam()
        {
            string line;
            //Separator
            char c = ';';
            //Read the file
            using (StreamReader file = new StreamReader(Const.parametersFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] res = line.Split(c);
                    dicParam.Add(res[0], res[1]);
                }
            }

        }

        /// <summary>
        /// Go to the web site
        /// </summary>
        public static void GoLearnup()
        {
            //Initialisation du watcher 
            CustomWatcher CsWatcher = new CustomWatcher();

            CustomDriver nav = new CustomDriver();
            nav.InitDriverChrome();

            nav.Go(dicParam[Const.url]);
            nav.driverWait.Until(ExpectedConditions.ElementExists(By.Id("fldUserName")));
            //Remplissage du user
            IWebElement userNameField = nav.GetOne("#fldUserName");
            userNameField.SendKeys(dicParam[Const.login]);

            //Remplissage du mot de passe
            IWebElement passField = nav.GetOne("#fldPassword");
            passField.SendKeys(dicParam[Const.motdePasse]);

            //Now submit the form. WebDriver will find the form for us from the element
            passField.Submit();

            //On attend que la page se charge
            nav.driverWait.Until(d => d.FindElement(By.ClassName("learnerName")).Text.StartsWith("Bienvenue", StringComparison.OrdinalIgnoreCase));

            //On clique sur le lien de la bibliothèque Numérique
            nav.driver.FindElement(By.PartialLinkText("Bibliothèque")).Click();

            //On attend que la page se charge
            nav.driverWait.Until(ExpectedConditions.ElementExists(By.ClassName("goButton")));

            //Go bouton
            nav.GetOne(".goButton").Click();

            //On attend et on va à la frame 
            nav.driverWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("scormIframe")));

            //On clique sur l'image
            nav.GetOne("#scormLauncher").Click();

            //On attend
            nav.driverWait.Until(d => d.WindowHandles.Count().Equals(3));

            nav.driver.SwitchTo().Window(nav.driver.WindowHandles.Last());

            nav.driverWait.Until(ExpectedConditions.ElementExists(By.Id("SearchEngine")));

            IWebElement searchEng = nav.GetOne("#SearchEngine");
            searchEng.SendKeys(dicParam[Const.search]);
            nav.GetOne("#btn_SearchEngine").Click();

            //On attend le chargement de la page
            nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#Resources")));

            //Clique sur le livre
            nav.GetOne("span.highlight").Click();

            //On attend le chargement de la page du livre
            nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#Presentation")));

            //Nombre de titre du livre
            var titres = nav.GetAll("ul#Root > li");
            int nbrTitre = titres.Count();

            Console.WriteLine("Nombre de titre : " + nbrTitre);

            //Nombre de sous-titre du chapitre en cours
            int nbrSousTitre = nav.GetAll("li.Current li").Count();
            Console.WriteLine("Nombre de sous titre : " + nbrSousTitre);

            //Boucle sur tous les chapitres
            for (int i = 0; i < nbrTitre; i++)
            {
                for (int j = 0; j < nbrSousTitre; j++)
                {
                    try
                    {
                        nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#Resource")));
                        nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#btn_NextFooter")));
                        nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#btn_Next")));
                        nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#Resource H1")));
                        nav.driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#btn_getPDF")));
                        var sousTitre = nav.GetAll("#Resource H1").First();
                        Console.WriteLine("Sous titre : " + sousTitre.Text);
                        Thread.Sleep(2000);
                        nav.GetOne("#btn_getPDF").Click();
                        Thread.Sleep(1000);
                        nav.GetOne("#btn_Next").Click();
                        Thread.Sleep(3000);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error :" + e.Message);
                        Console.WriteLine("Continue ? ");
                        Console.ReadLine();
                    }

                }
                //Nombre de sous-titre du chapitre en cours
                nbrSousTitre = nav.GetAll("li.Current li").Count();
                Console.WriteLine("Nombre de sous titre : " + nbrSousTitre);
            }
        }



    }
}
