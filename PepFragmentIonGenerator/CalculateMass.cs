using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;

namespace PepFragmentIonGenerator
{
    public class CalculateMass
    {
        private Dictionary<string, double> aminoAcidTableLi = new Dictionary<string, double>();
        public CalculateMass()
        {
            Assembly asb = Assembly.GetExecutingAssembly();
            StreamReader sr = new StreamReader(asb.GetManifestResourceStream(asb.GetName().Name + "." + "AminoAcid.txt"));
            sr.ReadLine();
            while (sr.Peek() != -1)
            {
                string[] oneline = sr.ReadLine().Split('\t');
                if (oneline.Length <= 1)
                    continue;
                double mz = double.Parse(oneline[3]);
                string aa = oneline[2];
                aminoAcidTableLi.Add(oneline[2], mz);
            }
        }

        public bool QualifiedPeptideSequence(string sequence)
        {
            string Useq = sequence.ToUpper().Replace(" ", "");

            for (int i = 0; i < Useq.Length; i++)
            {
                string tmpStr = Useq[i].ToString();
                if (tmpStr.Contains(" "))
                    continue;
                if (!aminoAcidTableLi.ContainsKey(tmpStr))
                {
                    return false;
                }
            }
            return true;
        }

        public double EvaluatePeptideMass(string sequence)
        {
            //double mass = -1.0d;
            double mass = 0.0d;

            for (int i = 0; i < sequence.Length; i++)
            {
                mass += (double)aminoAcidTableLi[sequence[i].ToString()];
            }
            mass += 18.010565d;
            if (mass == 0)
                return -1d;
            return mass;

        }

        public double EvaluatePeptideMassWithoutWater(string sequence)
        {
            double mass = EvaluatePeptideMass(sequence);
            if (mass > 0)
                return mass - 18.010565d;
            return mass;
        }

        public static Dictionary<string, int> GetPeptideCombination(string peptide)
        {
            Dictionary<string, int> aminoAcidDi = new Dictionary<string, int>();
            string alphabet = "";
            for (int i = 0; i < peptide.Length; i++)
            {
                alphabet = peptide[i].ToString();
                if (aminoAcidDi.ContainsKey(alphabet))
                    aminoAcidDi[alphabet] = (int)aminoAcidDi[alphabet] + 1;
                else
                    aminoAcidDi.Add(alphabet, 1);
            }
            return aminoAcidDi;
        }

        public Dictionary<string,double> AminoAcidTable
        {
            get { return aminoAcidTableLi; }
        }
    }
}
