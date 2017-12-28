using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PepFragmentIonGenerator.DataStructure;

namespace PepFragmentIonGenerator
{
    class IonDealer
    {
        private CalculateMass calculator = new CalculateMass();

        private bool WaterOrAmmoniaLoss(string sequence, IonType type)
        {

            /// modification to List< string >
            List<String> alphali = new List<string>();
            for (int i = 0; i < sequence.Length; i++)
            {
                if (!alphali.Contains(sequence[i].ToString()))
                    alphali.Add(sequence[i].ToString());
            }
            string typestring = type.ToString();


            // * = ammonia loss
            // Ammonia is mainly lost form the side chains of R, K, N, or Q
            if (typestring.Contains("star"))
            //if (type == IonType.bstar || type == IonType.bstarcharge2 || type == IonType.istar || type == IonType.istarcharge2 || type == IonType.ystar || type == IonType.ystarcharge2)
            {
                if (alphali.Contains("R") || alphali.Contains("K") || alphali.Contains("N") || alphali.Contains("Q"))
                {
                    return true;
                }
                else
                    return false;
            }
            // 0 = water loss
            // Water is mainly lost form the side chains of S, T, D, or E
            else if (typestring.Contains("0"))
            //else if (type == IonType.b0 || type == IonType.b0charge2 || type == IonType.i0 || type == IonType.i0charge2 || type == IonType.y0 || type == IonType.y0charge2)
            {
                if (alphali.Contains("S") || alphali.Contains("T") || alphali.Contains("D") || alphali.Contains("E"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        public static IonFamilyType DetermineIonFamilyType(IonType type)
        {
            string typeString = type.ToString();

            if (type == IonType.neutralLoss)
                return IonFamilyType.NeutralLoss;
            else if (type == IonType.precursors)
                return IonFamilyType.PrecursorIons;
            else if (typeString.StartsWith("b"))
                return IonFamilyType.B;
            else if (typeString.StartsWith("y"))
                return IonFamilyType.Y;
            else if (typeString.StartsWith("i"))
                return IonFamilyType.I;
            else if (typeString.StartsWith("a"))
                return IonFamilyType.A;
            else if (typeString.StartsWith("x"))
                return IonFamilyType.X;
            else if (typeString.StartsWith("c"))
                return IonFamilyType.C;
            else if (typeString.StartsWith("z"))
                return IonFamilyType.Z;
            else
                return IonFamilyType.Peptide;

        }

        public static string TagLabel(IonType type)
        {
            //
            if (type == IonType.peptide || type == IonType.neutralLoss)
                return type.ToString();

            //
            string label = type.ToString();

            label = label.Replace("star", "*");

            if (label.Contains("charge2"))
                label = label.Replace("charge2", "++");


            return label;

        }
        /// <summary>
        /// Get the mass-to-charge ration  
        /// </summary>
        /// <param name="seqMass"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private double GetMZ(double seqMass, IonType type)
        {
            //double md = 1;
            double proton = 1.007276d;
            double cterm = 17.00274d;
            double nterm = 1.007825d;
            double water = cterm + nterm;
            double ammonia = 17.026549d;
            double y = cterm + nterm + proton;//double y = cterm + nterm + nterm;
            double b = proton;
            double iby = proton;
            double iay = 27;


            #region Type determine
            switch (type)
            {
                case IonType.peptide:
                    {
                        seqMass += cterm + nterm;
                        break;
                    }
                //b-ion
                case IonType.b:
                    {
                        seqMass += b;
                        break;
                    }
                case IonType.bcharge2:
                    {
                        seqMass = (seqMass + b + proton) / 2;
                        break;
                    }
                case IonType.b0:
                    {
                        seqMass += (b - water);
                        break;
                    }
                case IonType.b0charge2:
                    {
                        seqMass = (seqMass + b - water + proton) / 2;
                        break;
                    }
                case IonType.bstar:
                    {
                        seqMass += (b - ammonia);
                        break;
                    }
                case IonType.bstarcharge2:
                    {
                        seqMass = (seqMass + b - ammonia + proton) / 2;
                        break;
                    }
                //y-ion
                case IonType.y:
                    {
                        seqMass += y;
                        break;
                    }
                case IonType.ycharge2:
                    {
                        seqMass = (seqMass + y + proton) / 2;
                        break;
                    }
                case IonType.y0:
                    {
                        seqMass = (seqMass + y - water);
                        break;
                    }
                case IonType.y0charge2:
                    {
                        seqMass = (seqMass + y - water + proton) / 2;
                        break;
                    }
                case IonType.ystar:
                    {
                        seqMass = (seqMass + y - ammonia);
                        break;
                    }
                case IonType.ystarcharge2:
                    {
                        seqMass = (seqMass + y - ammonia + proton) / 2;
                        break;
                    }
                //i-ion 
                case IonType.iby:
                    {
                        seqMass += iby;
                        break;
                    }
                case IonType.ibycharge2:
                    {
                        seqMass = (seqMass + iby + proton) / 2;
                        break;
                    }
                case IonType.iby0:
                    {
                        seqMass = (seqMass + iby - water);
                        break;
                    }
                case IonType.iby0charge2:
                    {
                        seqMass = (seqMass + iby - water + proton) / 2;
                        break;
                    }
                case IonType.ibystar:
                    {
                        seqMass = (seqMass + iby - ammonia);
                        break;
                    }
                case IonType.ibystarcharge2:
                    {
                        seqMass = (seqMass + iby - ammonia + proton) / 2;
                        break;
                    }
                case IonType.iay:
                    {
                        seqMass += iay;
                        break;
                    }
                case IonType.iaycharge2:
                    {
                        seqMass = (seqMass + iay + proton) / 2;
                        break;
                    }
                case IonType.iay0:
                    {
                        seqMass = (seqMass + iay - water);
                        break;
                    }
                case IonType.iay0charge2:
                    {
                        seqMass = (seqMass + iay - water + proton) / 2;
                        break;
                    }
                case IonType.iaystar:
                    {
                        seqMass = (seqMass + iay - ammonia);
                        break;
                    }
                case IonType.iaystarcharge2:
                    {
                        seqMass = (seqMass + iay - ammonia + proton) / 2;
                        break;
                    }

                // c-ion
                case IonType.c:
                    {
                        seqMass = seqMass + 18;
                        break;
                    }
                case IonType.ccharge2:
                    {
                        seqMass = (seqMass + 18 + proton) / 2;
                        break;
                    }
                case IonType.c0:
                    {
                        seqMass = seqMass + 18 - water;
                        break;
                    }
                case IonType.c0charge2:
                    {
                        seqMass = (seqMass + 18 - water + proton) / 2;
                        break;
                    }
                case IonType.cstar:
                    {
                        seqMass = seqMass + 18 - ammonia;
                        break;
                    }
                case IonType.cstarcharge2:
                    {
                        seqMass = (seqMass + 18 - ammonia + proton) / 2;
                        break;
                    }
                //z ion
                case IonType.z_1:
                    {
                        seqMass = seqMass + 2;
                        break;
                    }
                case IonType.z_1charge2:
                    {
                        seqMass = (seqMass + 2 + proton) / 2;
                        break;
                    }
                case IonType.z_2:
                    {
                        seqMass = seqMass + 2 + 1;
                        break;
                    }
                case IonType.z_2charge2:
                    {
                        seqMass = (seqMass + 2 + 1 + proton) / 2;
                        break;
                    }
                //a ion
                case IonType.a:
                    {
                        seqMass -= 27;
                        break;
                    }
                case IonType.acharge2:
                    {
                        seqMass = (seqMass - 27 + proton) / 2;
                        break;
                    }
                case IonType.a0:
                    {
                        seqMass = seqMass - 27 - water;
                        break;
                    }
                case IonType.a0charge2:
                    {
                        seqMass = (seqMass - 27 - water + proton) / 2;
                        break;
                    }
                case IonType.astar:
                    {
                        seqMass = seqMass - 27 - ammonia;
                        break;
                    }
                case IonType.astarcharge2:
                    {
                        seqMass = (seqMass - 27 - ammonia + proton) / 2;
                        break;
                    }
                //x ion
                case IonType.x:
                    {
                        seqMass = seqMass + 45;
                        break;
                    }
                case IonType.xcharge2:
                    {
                        seqMass = (seqMass + 45 + proton) / 2;
                        break;
                    }
                case IonType.x0:
                    {
                        seqMass = seqMass + 45 - water;
                        break;
                    }
                case IonType.x0charge2:
                    {
                        seqMass = (seqMass + 45 - water + proton) / 2;
                        break;
                    }
                case IonType.xstar:
                    {
                        seqMass = seqMass + 45 - ammonia;
                        break;
                    }
                case IonType.xstarcharge2:
                    {
                        seqMass = (seqMass + 45 - ammonia + proton) / 2;
                        break;
                    }
            }
            #endregion

            return seqMass;
        }

        public static int DetermineIonCharge(IonType type)
        {

            if (type == IonType.neutralLoss || type == IonType.peptide)
                return -1;
            string typestring = type.ToString();
            if (typestring.Contains("2"))
                return 2;
            else
                return 1;
        }

        public List<ds_Ion> GetNonStandardIons(List<ds_Ion> standardIons, Dictionary<int, List<ds_NeutralLoss>> varSiteModWeights)
        {
            List<ds_Ion> nonStandardIonsLi = new List<ds_Ion>();
            ds_Ion nsIon;
            string tag = "";
            bool b_create = false;
            foreach (ds_Ion i in standardIons)
            {
                IonFamilyType ftype = DetermineIonFamilyType(i.IonType);

                foreach (int site in varSiteModWeights.Keys)
                {
                    b_create = false;

                    if (ftype == IonFamilyType.B || ftype == IonFamilyType.A || ftype == IonFamilyType.C)
                    {
                        if (site < i.CleavageSite)
                            b_create = true;
                    }
                    else if (ftype == IonFamilyType.Y || ftype == IonFamilyType.X || ftype == IonFamilyType.Z)
                    {
                        if (site >= i.CleavageSite)
                            b_create = true;
                    }
                    else if (ftype == IonFamilyType.I)
                    {
                        if (site >= i.CleavageSite && site < i.CleavageSite2)
                            b_create = true;
                    }
                    else
                    {
                        Console.WriteLine("Waring!!!!! Ion Type Exception");
                    }

                

                    if (b_create)
                    {
                        //List<NeutralLoss> abc = (List<NeutralLoss>)varSiteModWeights[site];
                        foreach (ds_NeutralLoss Neu in (List<ds_NeutralLoss>)varSiteModWeights[site])
                        {
                            double mod = 0 - Neu.MassDifference;

                            double checkmz = i.MZ + (mod / i.Charge);

                            if (checkmz > 0)
                            {
                                //tag = "";
                                //if (mod > 0)
                                //    tag += "+";
                                //tag += mod.ToString();
                                //tag = "[" + tag + "]";
                                tag = "[" + Neu.Label + "]";


                                nsIon = i.Clone();
                                nsIon.IonType = IonType.neutralLoss;
                                //nsIon.MZ = nsIon.MZ + (mod / nsIon.Charge);
                                nsIon.MZ = checkmz;
                                nsIon.Label = nsIon.Label + tag;
                                //nsIon.Point = new IASL.proteomics.mass.MSDataStructure.MSPoint();
                                nsIon.ParentIon = i;
                                nonStandardIonsLi.Add(nsIon);
                            }
                        }
                    }
                }
            }
            return nonStandardIonsLi;
        }
        public List<ds_Ion> GetIons(IonFamilyType dedicate_type, string PepSeq, Dictionary<int, double> siteModWeight, List<IonType> IonTypes)
        {
            foreach (IonType type in IonTypes)
            {
                if (DetermineIonFamilyType(type) != dedicate_type)
                    throw new System.Exception("Wrong ion types in input FragmentIonTypes:" + dedicate_type.ToString());
            }
            List<ds_Ion> ionsLi = new List<ds_Ion>();
            string subseq = "";
            double mass;
            string label;


            if (dedicate_type == IonFamilyType.B || dedicate_type == IonFamilyType.C || dedicate_type == IonFamilyType.A)
            {
                for (int i = 0; i < PepSeq.Length - 1; i++)
                {
                    subseq = PepSeq.Substring(0, i + 1);

                    mass = calculator.EvaluatePeptideMassWithoutWater(subseq);
                    foreach (int site in siteModWeight.Keys)
                    {
                        if (site <= i)
                        {
                            //mass += (double) siteModWeight[site];
                            mass = mass + siteModWeight[site];
                        }
                    }
                    foreach (IonType type in IonTypes)
                    {
                        if (!WaterOrAmmoniaLoss(subseq, type))
                        {
                            continue;
                        }

                        label = TagLabel(type) + "(" + (i + 1).ToString() + ")";
                        ionsLi.Add(new ds_Ion(label, GetMZ(mass, type), type));
                        ionsLi[ionsLi.Count - 1].CleavageSite = (i + 1);
                        if (DetermineIonCharge(type) > 0)
                            ionsLi[ionsLi.Count - 1].Charge = DetermineIonCharge(type);
                    }
                }
            }
            else if (dedicate_type == IonFamilyType.Y || dedicate_type == IonFamilyType.Z || dedicate_type == IonFamilyType.X)
            {
                for (int i = 1; i < PepSeq.Length; i++)
                {
                    subseq = PepSeq.Substring(i, PepSeq.Length - i);
                    mass = calculator.EvaluatePeptideMassWithoutWater(subseq);
                    foreach (int site in siteModWeight.Keys)
                    {
                        if (site >= i)
                        {
                            //mass += (double)siteModWeight[site];
                            mass = mass + siteModWeight[site];
                        }
                    }
                    foreach (IonType type in IonTypes)
                    {
                        if (!WaterOrAmmoniaLoss(subseq, type))
                        {
                            continue;
                        }
                        label = TagLabel(type) + "(" + (PepSeq.Length - i).ToString() + ")";
                        ionsLi.Add(new ds_Ion(label, GetMZ(mass, type), type));
                        ionsLi[ionsLi.Count - 1].CleavageSite = i;
                        if (DetermineIonCharge(type) > 0)
                            ionsLi[ionsLi.Count - 1].Charge = DetermineIonCharge(type);
                    }
                }
            }
            else if (dedicate_type == IonFamilyType.I)
            {
                for (int i = 1; i < PepSeq.Length - 1; i++)
                {

                    for (int j = 1; j < PepSeq.Length - i; j++)
                    {
                        subseq = PepSeq.Substring(i, j);
                        mass = calculator.EvaluatePeptideMassWithoutWater(subseq);
                        foreach (int site in siteModWeight.Keys)
                        {
                            if (site >= i && site < (i + j))
                            {
                                //mass += (double)siteModWeight[site];
                                mass = mass + siteModWeight[site];
                            }
                        }
                        foreach (IonType type in IonTypes)
                        {
                            if (!WaterOrAmmoniaLoss(subseq, type))
                            {
                                continue;
                            }

                            label = TagLabel(type) + "(" + (i).ToString() + "-" + (i + j).ToString() + ")";

                            ionsLi.Add(new ds_Ion(label, GetMZ(mass, type), type));
                            ionsLi[ionsLi.Count - 1].CleavageSite = i;
                            ionsLi[ionsLi.Count - 1].CleavageSite2 = i + j;

                            if (DetermineIonCharge(type) > 0)
                                ionsLi[ionsLi.Count - 1].Charge = DetermineIonCharge(type);
                        }
                    }


                }
            }
            return ionsLi;
        }
    }
}
