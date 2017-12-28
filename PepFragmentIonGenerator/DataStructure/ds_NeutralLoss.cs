using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PepFragmentIonGenerator.DataStructure
{
    public class ds_NeutralLoss
    {

        //private string _formula;
        private double _massDifference = 0.0d;
        private int _idx = -1;
        private string _label = "";

        public int Index
        {
            get { return _idx; }
            set { _idx = value; }
        }

        public double MassDifference
        {
            get { return _massDifference; }
            set { _massDifference = value; }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        //public string ChemicalFormula
        //{
        //    get { return _formula; }
        //    set { _formula = value; }
        //}
    }
}
