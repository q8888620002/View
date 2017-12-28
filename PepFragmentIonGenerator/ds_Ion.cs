using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PepFragmentIonGenerator
{
    public class Ion
    {
        public string Label;
        public double MZ;
        public int Charge;
        public int CleavageSite = 0;
        public int CleavageSite2 = 0;
        public IonType IonType = IonType.peptide;
        public Ion ParentIon = null;

        public Ion(string label, double mz, IonType iontype)
        {
            Label = label;
            MZ = mz;
            //IonType = iontype;
        }

        public Ion Clone()
        {

            Stream stream = new MemoryStream();
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, this);
                stream.Position = 0;
                return (Ion)bf.Deserialize(stream);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //Ion i = new Ion();
            //i.Label = this.Label;
            //i.MZ = this.MZ;
            //i.Charge = this.Charge;
            //i.Sequence = this.Sequence;
            //i.StartIndex = this.StartIndex;
            //i.EndIndex = this.EndIndex;
            //i.Point = this.Point;
            //i.IonType = this.IonType;
            //return i;
        }
    }

    public enum IonType
    {
        peptide = 0,
        b = 1,
        y = 2,
        iby = 3,

        bcharge2 = 4,
        b0 = 5,
        b0charge2 = 6,
        bstar = 7,
        bstarcharge2 = 8,

        ycharge2 = 9,
        y0 = 10,
        y0charge2 = 11,
        ystar = 12,
        ystarcharge2 = 13,

        ibycharge2 = 14,
        iby0 = 15,
        iby0charge2 = 16,
        ibystar = 17,
        ibystarcharge2 = 18,

        iaycharge2 = 19,
        iay0 = 20,
        iay0charge2 = 21,
        iaystar = 22,
        iaystarcharge2 = 23,

        a = 24,
        acharge2 = 25,
        a0 = 26,
        a0charge2 = 27,
        astar = 28,
        astarcharge2 = 29,

        x = 30,
        xcharge2 = 31,
        x0 = 32,
        x0charge2 = 33,
        xstar = 34,
        xstarcharge2 = 35,


        #region c and z ions
        c = 36,
        ccharge2 = 37,
        c0 = 38,
        c0charge2 = 39,
        cstar = 40,
        cstarcharge2 = 41,

        z = 42,
        zcharge2 = 43,
        z0 = 44,
        z0charge2 = 45,
        zstar = 46,
        zstarcharge2 = 47,
        #endregion

        neutralLoss = 48,
        precursors = 49,

        z_1 = 50,
        z_1charge2 = 51,
        z_2 = 52,
        z_2charge2 = 53,
        iay = 54,

    }

    public enum IonFamilyType
    {
        Peptide = 0,
        B = 1,
        Y = 2,
        A = 3,
        X = 4,
        C = 5,
        Z = 6,
        I = 7,
        NeutralLoss = 8,
        PrecursorIons = 9,

    }

    public class NeutralLoss
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
