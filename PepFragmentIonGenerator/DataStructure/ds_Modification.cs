using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PepFragmentIonGenerator.DataStructure
{
    public interface Modification {
        double getMass();
        void accept(PeptideSequenceVisitor visitor);
    }


    public class ds_Modification : Modification
    {
        public string ModName { get; set;}
        public double MassDif { get; set; }
        public int SiteInSequence { get; set; }
        List<ds_NeutralLoss> _Losses = new List<ds_NeutralLoss>();

        public bool AddNeutralLoss(ds_NeutralLoss neutralloss)
        {
            if (!_Losses.Contains(neutralloss))
            {
                neutralloss.Index = _Losses.Count;
                _Losses.Add(neutralloss);
                return true;
            }
            return false;
        }
        public int NumberOfNeutralLoss
        {
            get
            {
                return _Losses.Count;
            }
        }
        public double getMass()
        {
            return MassDif;
        }

        public ds_NeutralLoss GetNeutralLossByIndex(int idx)
        {
            if (idx >= 0 && idx < _Losses.Count)
                return _Losses[idx];
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public void accept(PeptideSequenceVisitor visitor)
        {
            visitor.visit(this);
        }
      
    }


    // a modification of numeral form ex. [198] 

    public class ds_NumModidication : Modification {


        public double MassDif { get; set; }
        public int SiteInSequence { get; set; }
        public int SiteInString { get; set; }
        public string ModPeptide { get; set; }


        List<ds_NeutralLoss> _Losses = new List<ds_NeutralLoss>();

        public bool AddNeutralLoss(ds_NeutralLoss neutralloss)
        {
            if (!_Losses.Contains(neutralloss))
            {
                neutralloss.Index = _Losses.Count;
                _Losses.Add(neutralloss);
                return true;
            }
            return false;
        }
        public int NumberOfNeutralLoss
        {
            get
            {
                return _Losses.Count;
            }
        }
        /// <summary>
        /// Returning the calculated Mass of modification 
        /// </summary>
        /// <returns></returns>
        public double getMass()
        {
            CalculateMass cal = new CalculateMass();
            return (MassDif - cal.EvaluatePeptideMassWithoutWater(ModPeptide));
        }

        public ds_NeutralLoss GetNeutralLossByIndex(int idx)
        {
            if (idx >= 0 && idx < _Losses.Count)
                return _Losses[idx];
            return null;
        }

        public void accept(PeptideSequenceVisitor visitor)
        {
            visitor.visit(this);
        }

    }

}
