using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PepFragmentIonGenerator;
using PepFragmentIonGenerator.DataStructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        List<ds_Ion> ionLi = new List<ds_Ion>();
        String annotationFile = "PeptideAnnotation.xml";
        GetFragments gfObj = new GetFragments();

        /// <summary>
        /// This is the first test of GetFragment after rearragning DS and Dictionary

        /// </summary>
        [TestMethod]
       
            public void TestGetFragmentatWithOutMod()
            {

              String protein = "DFFDS.";
                
                string aaSeq = protein.Substring(0, protein.IndexOf('.'));
                this.ionLi = gfObj.GetFragments_AAStr_Mod(aaSeq, false, annotationFile);

                Console.WriteLine("Ion List COunt: "+ ionLi.Count);
                
                
                for(int i=0; i< ionLi.Count; i++)
             {
                 Console.WriteLine(ionLi[i].Label + ionLi[i].IonType + ionLi[i].MZ);
             }
                
            //   Assert.AreEqual(expected, "Account not debited correctly");
        }
        /// <summary>
        ///  This is a test for Text modification such as [ Carba ]
        /// </summary>
        [TestMethod]

        public void TestModnWithText()
        {
            String protein = "A[Carba]DAS[Carba]DSD.";
            string aaSeq = protein.Substring(0, protein.IndexOf('.'));
            this.ionLi = gfObj.GetFragments_AAStr_Mod(aaSeq, false, annotationFile);

            ///   Console.WriteLine("Ion List COunt: " + ionLi.Count);
           
           

            //   Assert.AreEqual(expected, "Account not debited correctly");
        }



        [TestMethod]

        public void RefractoringTextParser()
        {
            String protein = "DFFDS[Carba].";

            double expectedMz = 116.034219;
            int expectedCount = 148;


            string aaSeq = protein.Substring(0, protein.IndexOf('.'));
            this.ionLi = gfObj.GetFragments_AAStr_Mod(aaSeq, false, annotationFile);



            Assert.AreEqual(expectedCount, ionLi.Count);
            Assert.AreEqual(expectedMz, ionLi[0].MZ, 0.000001);
            Console.WriteLine((int)protein[0]);
        }


        [TestMethod]
        /// Check if the function could recoginze modification correctly
        public void CheckModification()
        {
            String peptide = "SDASDADS[Cabra][Biotin][Carbamidometyl(C)][Carbamidomethyl (C)]SDADSD";
            string peptide2 = "DSADDQWDE";
            string numeralPeptide = "SDAS[213]DAD[198]";
            string numeralPeptide2 = "SDSDF[123.123]";
            string falsePeptide = "SDAS[19.12312";


            Assert.AreEqual(true, gfObj.checkMod(numeralPeptide));
            Assert.AreEqual(true, gfObj.checkMod(numeralPeptide2));
            Assert.AreEqual(false, gfObj.checkMod(falsePeptide));

            ///  List<Modification> expected = gfObj.GetModification(numeralPeptide);

            /// Console.Write(expected[0]);

            Assert.AreEqual(false, gfObj.checkMod(peptide2));
            Assert.AreEqual(true, gfObj.checkMod(peptide));

        }


        [TestMethod]
        /// Check if the function could return modification list properly
        public void GetModificationLi()
        {
            String peptide = "[Cabra][Biotin][Carbamidometyl(C)][Carbamidomethyl (C)]";
            List<string> mod_list = new List<string>();


            mod_list.Add("[Cabra]");
            mod_list.Add("[Biotin]");
            mod_list.Add("[Carbamidometyl(C)]");
            mod_list.Add("[Carbamidometyl (C)]");

            ///List<string> expected = gfObj.GetModification(peptide);
            ///    foreach (string s in expected)
         ///       {
          ///          Console.WriteLine(s);
         ///       }

        ///    foreach (string s in mod_list)
         ///   {
             ///   Console.WriteLine(s);
          ///  }


         /// Assert.AreEqual(mod_list.Count, expected.Count);
         ///   Assert.IsTrue(mod_list.SequenceEqual(expected));

        }
        [TestMethod]

        public void TestRegex()
        {
            string NumPeptide = "DFFDA[123.3214142][Carbamidometyl(C)]SS[Carba]FFFF";
            string pattern = @"\[.*\]";

            Assert.AreEqual(Regex.Replace(NumPeptide, pattern, ""), "DFFDAFFFF");

        }

      

        [TestMethod]
        //// This is a test of numeric modification 
        public void InitiateNumMod()
        {
            string peptide = "DFFDA[Carba]";
            string NumPeptide = "DFFDA[128.058514]";

            this.ionLi = gfObj.GetFragments_AAStr_Mod(peptide, false, annotationFile);
            List<ds_Ion> testResult = gfObj.GetFragments_AAStr_Mod(NumPeptide, false, annotationFile);

            // check if the returning result is the same
            Assert.AreEqual(testResult.Count, ionLi.Count);


            for (int i = 0; i< ionLi.Count; i++)
            {
                Assert.AreEqual(testResult[i].MZ, ionLi[i].MZ, 0.000001);

                Assert.AreEqual(testResult[i].Label, ionLi[i].Label);
                Assert.AreEqual(testResult[i].IonType, ionLi[i].IonType);
            }


        }
        [TestMethod]

        //// Test if the GetGrag function can extract the modification correctly 

        public void GetModification()
        {
            string NumPeptide = "DFFDA[123.3214142]DFSDF[Carbamidometyl(C)]SS[Carba]FFFF";
            string pattern = @"\[.*\]";

            this.ionLi = gfObj.GetFragments_AAStr_Mod(NumPeptide, false, annotationFile);

            Assert.AreEqual(Regex.Replace(NumPeptide, pattern, ""), "DFFDAFFFF");

        }


    }











}
