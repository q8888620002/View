using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PepFragmentIonGenerator.DataStructure
{
    public class ds_Ion
    {
        public string Label;
        public double MZ;
        public int Charge;
        public int CleavageSite = 0;
        public int CleavageSite2 = 0;
        public IonType IonType = IonType.peptide;
        public ds_Ion ParentIon = null;

        public ds_Ion(string label, double mz, IonType iontype)
        {
            Label = label;
            MZ = mz;
            //IonType = iontype;
        }

        public ds_Ion Clone()
        {

            Stream stream = new MemoryStream();
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, this);
                stream.Position = 0;
                return (ds_Ion)bf.Deserialize(stream);
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
}
