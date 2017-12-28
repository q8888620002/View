using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PepFragmentIonGenerator
{
    public class ds_Modification
    {
        private string _ModName = "";
        private double _MassDif = 0.0d;
        private List<string> _LabelSites = new List<string>();
        private int _Index = -1;
        List<NeutralLoss> _Losses = new List<NeutralLoss>();

        public string ModName
        {
            get { return _ModName; }
            set { _ModName = value; }
        }
        public double MassDif
        {
            get { return _MassDif; }
            set { _MassDif = value; }
        }
        public List<string> LabelSites
        {
            get { return _LabelSites; }
            set { _LabelSites = value; }
        }
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }
        public bool AddNeutralLoss(NeutralLoss neutralloss)
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
        public NeutralLoss GetNeutralLossByIndex(int idx)
        {
            if (idx >= 0 && idx < _Losses.Count)
                return _Losses[idx];
            return null;
        }
    }
}
