using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PepFragmentIonGenerator.DataStructure
{/// <summary>
///  This is a visitor interface of the PeptideSequenceVisitor
/// </summary>
    public interface Visitor
    {
        void visit(ds_Modification mod);
        void visit(ds_NumModidication mod);
        void visit(List<Modification> modLi);

    }

    /// <summary>
    /// This is a class the decode the List <Modification></Modification>
    /// </summary>
    public class PeptideSequenceVisitor : Visitor
    {
        private Dictionary<int, double> siteModWeight;
        private Dictionary<int, List<ds_NeutralLoss>> varSiteModWeightsDi ;
        private Boolean b_NeutralPeak;
        /// <summary>
        /// Constructor of PeptideSequenceVisitor
        /// </summary>
        /// <param name="b_CreatNeutralLossPeak"></param>
        /// 

        public PeptideSequenceVisitor(Boolean b_CreatNeutralLossPeak)
        {
            this.siteModWeight = new Dictionary<int, double>();
            this.varSiteModWeightsDi = new Dictionary<int, List<ds_NeutralLoss>>();
            this.b_NeutralPeak = b_CreatNeutralLossPeak;
        }

        public void visit(ds_Modification mod)
        {
            try
            {
                siteModWeight.Add(mod.SiteInSequence, mod.MassDif);
                
                if (b_NeutralPeak)
                {
                    varSiteModWeightsDi.Add(mod.SiteInSequence, new List<ds_NeutralLoss>());
                    for (int idx = 0; idx < mod.NumberOfNeutralLoss; idx++)
                    {
                        (varSiteModWeightsDi[mod.SiteInSequence]).Add(mod.GetNeutralLossByIndex(idx));
                    }
                }
            }
            catch
            {
                throw new ArgumentException("Input peptide Sequence dulplicated site");
            }

           
        }



        public void visit(ds_NumModidication mod)
        {
            try
            {
                siteModWeight.Add(mod.SiteInSequence, mod.getMass());

                if (b_NeutralPeak)
                {
                    varSiteModWeightsDi.Add(mod.SiteInSequence, new List<ds_NeutralLoss>());
                    for (int idx = 0; idx < mod.NumberOfNeutralLoss; idx++)
                    {
                        (varSiteModWeightsDi[mod.SiteInSequence]).Add(mod.GetNeutralLossByIndex(idx));
                    }
                }
            }
            catch
            {
                throw new ArgumentException("Input peptide Sequence dulplicated site");
            }
        }

        public void visit(List<Modification> mLi)
        {
            /// iterating the modification to check type
            /// 
            for(int i =0; i < mLi.Count; i++)
            {
                if (mLi[i].GetType() == typeof(ds_Modification))
                {
                    mLi[i].accept(this);
                }
                else if (mLi[i].GetType() == typeof(ds_NumModidication))
                {
                    mLi[i].accept(this);

                }
                else
                {
                    throw new Exception("Wrong datastructure, ");
                }
            }
                
            }

        /// <summary>
        ///  Get the intended dictionary of modification site and mass
        /// </summary>
        /// <returns></returns>

        public Dictionary<int, double> getModSiteList()
        {
            return this.siteModWeight;
        }

        /// <summary>
        /// Return the dictionary list of neutral list and site location
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<ds_NeutralLoss>> getNeutralLossLi()
        {
            return this.varSiteModWeightsDi;
        }
    }
}