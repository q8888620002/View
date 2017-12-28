using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using PepFragmentIonGenerator.DataStructure;

namespace PepFragmentIonGenerator
{
    public class GetFragments
    {
        private CalculateMass calculator = new CalculateMass();
        private IonDealer ionDealer = new IonDealer();
       

        /// <summary>
        ///   It's a peptide fragmentation generatior. It will  return a list of fragmentation.    
        /// </summary>
        /// <param name="pepSeq">eg. AKVMMSTGD[Carba], AKMEI[192.3213124]</param>
        /// <param name="b_CreatNeutralLossPeak">true or falso</param>
        /// <param name="PeptideAnnotationFile"></param>>
        /// <returns></returns>
        public List<ds_Ion> GetFragments_AAStr_Mod(string pepSeq, bool b_CreatNeutralLossPeak, string PeptideAnnotationFile) //regardless modified or non-modified peptide sequence
        {

            // Check for empty string.

            if (!string.IsNullOrEmpty(pepSeq))
            {
                List<ds_Ion> allIonsLi = new List<ds_Ion>();
                Dictionary<int, List<ds_NeutralLoss>> varSiteModWeightsDi = new Dictionary<int, List<ds_NeutralLoss>>();
                Dictionary<int, double> siteModWeightDi = new Dictionary<int, double>();

                // Regex pattern that replace all texts in the brackets of peptide sequence
                string ReplqcePattern = @"\[.*\]";

                // dictionary of modification pairs
                Dictionary<String, ds_Modification> AnnotationChartDi = modGenerator(PeptideAnnotationFile);

                // check if the modification exists
                if (checkMod(pepSeq))
                {
                    // Get modifications of the peptide sequence
                    List<Modification> modificationLi = GetModification(pepSeq, AnnotationChartDi);

                    /// initiate visitor to parse the modification list 
                    PeptideSequenceVisitor SequenceVisitor = new PeptideSequenceVisitor(b_CreatNeutralLossPeak);
                    SequenceVisitor.visit(modificationLi);
                    siteModWeightDi = SequenceVisitor.getModSiteList();

                    //// If the sequence contains neutral, returning the neutral loss list
                    if (b_CreatNeutralLossPeak)
                    {
                        varSiteModWeightsDi = SequenceVisitor.getNeutralLossLi();
                    }
                    // replace all modification into empty value
                    pepSeq = Regex.Replace(pepSeq, ReplqcePattern, "");
                }


                List<IonType> FragmentIonTypes = GetDefaultIonTypes();

                List<IonType> bTypesLi = new List<IonType>();
                List<IonType> yTypesLi = new List<IonType>();

                List<IonType> aTypesLi = new List<IonType>();
                List<IonType> xTypesLi = new List<IonType>();

                List<IonType> cTypesLi = new List<IonType>();
                List<IonType> zTypesLi = new List<IonType>();

                List<IonType> iTypesLi = new List<IonType>();




                foreach (IonType type in FragmentIonTypes)
                {
                    switch (IonDealer.DetermineIonFamilyType(type))
                    {
                        case IonFamilyType.B:
                            bTypesLi.Add(type);
                            break;
                        case IonFamilyType.Y:
                            yTypesLi.Add(type);
                            break;
                        case IonFamilyType.A:
                            aTypesLi.Add(type);
                            break;
                        case IonFamilyType.X:
                            xTypesLi.Add(type);
                            break;
                        case IonFamilyType.C:
                            cTypesLi.Add(type);
                            break;
                        case IonFamilyType.Z:
                            zTypesLi.Add(type);
                            break;
                        case IonFamilyType.I:
                            iTypesLi.Add(type);
                            break;

                        // other ion types
                        default:
                            System.Console.WriteLine("Other ion family type");
                            break;
                    }
                }

                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.B, pepSeq, siteModWeightDi, bTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.Y, pepSeq, siteModWeightDi, yTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.A, pepSeq, siteModWeightDi, aTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.X, pepSeq, siteModWeightDi, xTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.C, pepSeq, siteModWeightDi, cTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.Z, pepSeq, siteModWeightDi, zTypesLi));
                allIonsLi.AddRange(ionDealer.GetIons(IonFamilyType.I, pepSeq, siteModWeightDi, iTypesLi));
                /// Add neutral loss if tit exists. 
                allIonsLi.AddRange(ionDealer.GetNonStandardIons(allIonsLi, varSiteModWeightsDi));

                return allIonsLi;

            }
            else
            {
                /// Should not be here  
             
                throw new ArgumentException(" A propiate peptide sequence string should be provided. ");
            }
        }

        /// <summary>
        /// Get the default list of IonType 
        /// </summary>
        /// <returns></returns>
        private List<IonType> GetDefaultIonTypes()
        {
            List<IonType> DefaultLi = Enum.GetValues(typeof(IonType)).Cast<IonType>().ToList<IonType>();
            return DefaultLi;
        }

      
        /// <summary>
        /// This is a method to check if this peptide sequence contain modification, returning true if there is a modification within it
        /// </summary>
        /// <param name="peptideSeq"></param>
        /// <returns></returns>
        /// 
        public bool checkMod(string peptideSeq)
        {
            if (!string.IsNullOrEmpty(peptideSeq))
            {
                string pattern = @"\[.*\]";
                return new Regex(pattern).Match(peptideSeq).Success;
            }

            return false;
        }
        /// <summary>
        /// return a list of string of midification detected in the peptide sequence
        /// </summary>
        /// <param name="peptideSeq"></param>
        /// <returns></returns>


        public List<Modification> GetModification(string peptideSeq, Dictionary<String, ds_Modification> AnnotationChart) 
        {


            if (!string.IsNullOrEmpty(peptideSeq))
            {
                string pattern = @"(\[((\()*(\w+)?([0-9]+(\.[0-9]+)?)?\s*(\))*)+\])";
                Regex r = new Regex(pattern);
                MatchCollection matches = r.Matches(peptideSeq);

                // Name of mod
                List<string> modificationsNameLi = matches.Cast<Match>().Select(match => match.Value).ToList();

                // location of mod in the peptide string 

                List<int> MatchLociLi = matches.Cast<Match>().Select(match => match.Index).ToList();
                List<int> LengthOfModLi = new List<int>();

                List<Modification> modsLi = new List<Modification>();

                for (int i = 0; i < modificationsNameLi.Count; i++)
                {
                    string modName = modificationsNameLi[i];

                    modName = modName.Substring(1, modName.Length - 2);

                    /// Check if the modification form is character or numbers
                    if (modName != null && modName.Length != 0)
                    {
                        if (IsDigitsOnly(modName))
                        {

                            ds_NumModidication newMod = new ds_NumModidication();
                            newMod.MassDif = Convert.ToDouble(modName);
                            newMod.SiteInSequence = MatchLociLi[i] - 1;
                            newMod.SiteInString = MatchLociLi[i] - 1;
                            newMod.ModPeptide = peptideSeq.Substring(MatchLociLi[i] - 1, 1);


                            if (LengthOfModLi.Any())
                            {
                                foreach (int length in LengthOfModLi)
                                {
                                    newMod.SiteInSequence -= length;
                                }
                            }

                            modsLi.Add(newMod);
                        }
                        else
                        {

                            ds_Modification m = AnnotationChart[modName];
                            ds_Modification newMod = new ds_Modification();

                            newMod.ModName = m.ModName;
                            newMod.MassDif = m.MassDif;

                            newMod.SiteInSequence = MatchLociLi[i] - 1;

                            if (LengthOfModLi.Any())
                            {
                                foreach (int length in LengthOfModLi)
                                {
                                    newMod.SiteInSequence -= length;
                                }
                            }

                            modsLi.Add(newMod);
                        }
                    }
                    else
                    {
                        throw new Exception("This function does not support modification in this form");
                    }

                    /// record the modification length includign brackets
                    LengthOfModLi.Add(modName.Length + 2);
                }

                return modsLi;

            }
            else
            {   //
                throw new Exception("A peptide string must provided");
            }
        }
        /// <summary>
        ///  Check if the modification is in the form of [digits]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsDigitsOnly(string text)
        {
            Double num = 0;

            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return Double.TryParse(text, out num); ;
            
        }

        /// <summary>
        /// 
        /// It's function to read peptide annotation file and generate modification pairs, returning a dictionary form of Modification pairs
        /// 
        /// </summary>
        /// <param name="xmlAnnotation"> The  expert information of peptide annotation </param>
        /// <returns></returns>

        private Dictionary<string, ds_Modification> modGenerator(String xmlAnnotation)
        {
            // Check for empty string.
            if (!string.IsNullOrEmpty(xmlAnnotation))
            {
                Dictionary<String, ds_Modification> modHTLi = new Dictionary<string, ds_Modification>();

                /// Load modification information;

                //Assembly asb = Assembly.GetExecutingAssembly();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\" + xmlAnnotation);
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(asb.GetManifestResourceStream(asb.GetName().Name + "." + "PeptideAnnotation.xml"));
                XmlNodeList rootnode = xmlDoc.GetElementsByTagName("PeptideAnnotation");

                foreach (XmlNode xnode in rootnode[0].ChildNodes)
                {
                    switch (xnode.Name)
                    {
                        case "ModificationSetting":
                            foreach (XmlNode modnode in xnode.ChildNodes)
                            {
                                switch (modnode.Name)
                                {
                                    case "modification":
                                        ds_Modification mod = new ds_Modification();
                                        foreach (XmlNode snode in modnode.ChildNodes)
                                        {
                                            switch (snode.Name)
                                            {
                                                case "name":
                                                    mod.ModName = snode.InnerText;
                                                    break;
                                                case "delta":
                                                    mod.MassDif = double.Parse(snode.InnerText);
                                                    break;
                                                case "neutral_loss":
                                                    ds_NeutralLoss nl = new ds_NeutralLoss();
                                                    nl.MassDifference = double.Parse(snode.InnerText);
                                                    nl.Label = snode.Attributes["name"].InnerText;
                                                    mod.AddNeutralLoss(nl);
                                                    break;
                                            }
                                        }
                                        modHTLi.Add(mod.ModName, mod);
                                        break;
                                }
                            }
                            break;
                    }
                }
                return modHTLi;
            }
            else
            {
                throw new ArgumentNullException("A file name must be provided");
            }


        }

    }
   
}
