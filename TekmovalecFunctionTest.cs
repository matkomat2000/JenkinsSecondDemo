using Microsoft.VisualStudio.TestTools.UnitTesting;
using PRO_Vaja_1_Preizkusanje_enot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TekmovalecTest
{
    [TestClass]
    public class TekmovalecFunctionTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException), "This is not possible, please check your input.")]     //  To je za throw izjeme (line 95)
        public void ValidPoomseAdding()
        {
            //  Pravilno dodaja prvo tekmo
            Tekmovalec pomzer = new Tekmovalec(0, "Hari", "Mate Hari", "BiH", true, false, false);    

            double beginingPoints = pomzer.Tocke_Tekmovalec;

            pomzer.AddPoomsePoints(1, 13, 2);

            Assert.AreNotEqual(pomzer.Tocke_Tekmovalec, beginingPoints);

            //  Pravilno doda vec tock
            double currentPoints = pomzer.Tocke_Tekmovalec;
            pomzer.AddPoomsePoints(3, 5, 1);

            Assert.AreNotEqual(currentPoints, pomzer.Tocke_Tekmovalec);

            //  Da deluje blokiranje neregistriranega v kategoriji pravilno
            Tekmovalec koren = new Tekmovalec(1, "Matevz", "Koren", "Slovenia", false, true, false);

            koren.AddPoomsePoints(2, 13, 2);

            Assert.AreEqual(koren.Tocke_Tekmovalec, 0);

            //  Ce je vnos napacen (je tekmovalec slabse uvrscen kot je mogoce)
            double final = pomzer.Tocke_Tekmovalec;
            pomzer.AddPoomsePoints(7, 6, 1);
            Assert.AreEqual(final, pomzer.Tocke_Tekmovalec);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException), "This is not possible, please check your input.")]     //  To je za throw izjeme (line 114)
        public void ValidBorbeSystem()
        {
            Tekmovalec koren = new Tekmovalec(1, "Matevz", "Koren", "Slovenia", false, true, false);
            //  1 tournament
            double current = koren.Tocke_Tekmovalec;
            koren.AddBorbePoints(3, 4, 32);
            Assert.AreNotEqual(current, koren.Tocke_Tekmovalec);

            //  Multi
            current = koren.Tocke_Tekmovalec;
            koren.AddBorbePoints(1, 2, 64);
            koren.AddBorbePoints(0, 8, 55);
            koren.AddBorbePoints(2, 1, 2);                          //  namerna napaka (tu vrže ERROR!) ---> ne more bit več borb kot pa je nasprotnikov
            Assert.AreNotEqual(current, koren.Tocke_Tekmovalec);

            //  Da deluje blokiranje neregistriranega v kategoriji pravilno
            Tekmovalec pomzer = new Tekmovalec(1, "Kr", "en", "Afganistan", true, false, true);
            pomzer.AddBorbePoints(1, 1, 4);

            Assert.AreEqual(pomzer.Tocke_Tekmovalec, 0);

            //  Če je št. tekmovalcev na tekmovanju manjše od 12, se toče prepolovijo
            Tekmovalec ziheras = new Tekmovalec(1, "Ziherca", "Benzevuti", "Vatikan", false, true, false);
            ziheras.AddBorbePoints(1, 1, 4);

            double taTekma = 1 * 1;             //  1 zmaga * rang turnirja 1
            taTekma /= 2;                       //  Ker je < 12 tekmovalcev
            Assert.AreEqual(taTekma, ziheras.Tocke_Tekmovalec);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException), "This country does not exist!")]
        public void CanChangeCountry()
        {
            //  Preverimo, če se ddržava ob spremembi spremeni
            string firstCountry = "Slovenia";
            List<Tekmovalec> vsi = new List<Tekmovalec>();

            Tekmovalec koren = new Tekmovalec(0, "Matevz", "Koren", firstCountry, false, true, false);
            vsi.Add(koren);
            string newCountry = "Iceland";

            koren.changeCountry(newCountry);

            string currentCountry = koren.Country_Tekmovalec;

            Assert.AreEqual(newCountry, currentCountry);

            //  Preverimo če to deluje v foreach zanki z vektorji in poiskusimo 'Assert.AreEqual' s for zanko!
            vsi.Add(new Tekmovalec(1, "Glupi", "Mimek", "Estonia", true, true, false));
            vsi.Add(new Tekmovalec(2, "Tupi", "Pipek", "Monaco", false, false, true));

            foreach (var el in vsi)
            {
                el.changeCountry(newCountry);
            }
            for (int i = 0; i < vsi.Count - 1; i++)
            {
                Assert.AreEqual(vsi[i].Country_Tekmovalec, vsi[i + 1].Country_Tekmovalec);
            }

            //  Preverimo, če država sploh obstaja
            koren.changeCountry("Tata mata");
            Assert.AreEqual("Iceland", koren.Country_Tekmovalec);
        }

        [TestMethod]
        public void AddingMultipleDiffPoints()
        {
            Tekmovalec koren = new Tekmovalec(0, "Matevz", "Koren", "San Marino", true, true, true);
            koren.AddBorbePoints(4, 4, 16);
            koren.AddBorbePoints(5, 2, 62);
            koren.AddBorbePoints(2, 1, 11);
            Assert.AreEqual(koren.BorbeTockeVektor.Sum(), koren.Tocke_Tekmovalec);
            koren.AddPoomsePoints(4, 8, 2);
            double smthInTheMiddle = koren.BorbeTockeVektor.Sum() + koren.PoomseTockeVektor.Sum();
            Assert.AreEqual(smthInTheMiddle, koren.Tocke_Tekmovalec);
            koren.AddKickPoints(3);
            double first = koren.Tocke_Tekmovalec;
            double allForKoren = koren.sumAllPoints();
            Assert.AreEqual(first, allForKoren);
        }

        [TestMethod]
        public void TestingPrint()
        {
            List<Tekmovalec> vsi = new List<Tekmovalec>();
            Tekmovalec koren = new Tekmovalec(0, "Matevz", "Koren", "San Marino", true, true, true);
            koren.AddBorbePoints(4, 4, 16);
            koren.AddBorbePoints(5, 2, 62);
            vsi.Add(koren);
            string korenString = koren.Print();
            Tekmovalec kicker = new Tekmovalec(1, "Veso", "Lola Ribar", "Kosovo", false, false, true);
            kicker.AddKickPoints(3);
            kicker.AddKickPoints(5);
            string kickerString = kicker.Print();
            vsi.Add(kicker);
            Tekmovalec pomzer = new Tekmovalec(2, "Hari", "Mata Hari", "BiH", true, false, false);
            pomzer.AddPoomsePoints(1, 4, 1);
            vsi.Add(pomzer);
            string pomzerString = pomzer.Print();
            string nestoOkupno = "";
            foreach(var el in vsi)
            {
                nestoOkupno += el.Print();
            }
            Assert.AreEqual(korenString + kickerString + pomzerString, nestoOkupno);
            vsi = vsi.OrderBy(p => p.Tocke_Tekmovalec).ToList();
            string nestoOkupnoPt2 = "";
            foreach (var el in vsi)
            {
                nestoOkupnoPt2 += el.Print();
            }
            Assert.AreNotEqual(nestoOkupnoPt2, nestoOkupno);
            vsi = vsi.OrderBy(p => p.ID_Tekmovalec).ToList();
            string nestoOkupnoPt3 = "";
            foreach (var el in vsi)
            {
                nestoOkupnoPt3 += el.Print();
            }
            Assert.AreEqual(nestoOkupnoPt3, nestoOkupno);
        }
        
        [TestMethod]
        public void TestingNosilec()
        {
            List<Tekmovalec> vsi = new List<Tekmovalec>();
            Tekmovalec koren = new Tekmovalec(0, "Matevz", "Koren", "San Marino", true, true, true);
            koren.AddBorbePoints(4, 4, 16);
            koren.AddBorbePoints(5, 2, 62);
            vsi.Add(koren);
            Tekmovalec kicker = new Tekmovalec(1, "Veso", "Lola Ribar", "Kosovo", false, false, true);
            kicker.AddKickPoints(3);
            kicker.AddKickPoints(5);
            vsi.Add(kicker);
            Tekmovalec pomzer = new Tekmovalec(2, "Hari", "Mata Hari", "BiH", true, false, false);
            pomzer.AddPoomsePoints(1, 4, 1);
            vsi.Add(pomzer);
            foreach (var el in vsi)
            {
                Assert.AreEqual(0, el.Raking_Tekmovalec);
            }
            int korenRanking = koren.Raking_Tekmovalec;
            int borbeNosilec = koren.SetNosilec(vsi);
            Assert.AreNotEqual(koren.Raking_Tekmovalec, korenRanking);
            int pomzerRaking = pomzer.Raking_Tekmovalec;
            int poomseNosilec = pomzer.SetNosilec(vsi);
            Assert.AreNotEqual(pomzer.Raking_Tekmovalec, pomzerRaking);
            int kickerRaking = kicker.Raking_Tekmovalec;
            int kickerNosilec = kicker.SetNosilec(vsi);
            Assert.AreNotEqual(kicker.Raking_Tekmovalec, kickerRaking);
            foreach(var el in vsi)
            {
                Assert.AreNotEqual(0, el.Raking_Tekmovalec);
            }
            Assert.AreNotEqual(borbeNosilec, poomseNosilec);            //  Samo 'koren' je nosilec, saj ima najvec tock
            Assert.AreEqual(kickerNosilec, poomseNosilec);              //  Noben ni nosilec
        }
        /*
        [TestMethod]
        public void AddingMultipleDiffPoints()
        {
            List<Tekmovalec> vsi = new List<Tekmovalec>();
        }*/
    }
}
