using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PepFragmentIonGenerator.DataStructure
{
    public enum IonFamilyType
    {
        Peptide = 0,
        B = 1,
        Y = 2,
        A = 3,
        X = 4,
        C = 5,
        Z = 6,
        I = 7,      // ?? check
        NeutralLoss = 8,
        PrecursorIons = 9,

    }
}
