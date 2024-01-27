﻿using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Util;

namespace Smy.Vst.Parameter
{
  internal class FilterParameterContainer : ParameterContainer
  {
    public FilterParameterContainer(VstParameterCategory paramCategory, int i) : base(paramCategory)
    {
      FilterParamer = new FilterParameter();

      FltrMdMgr = CreateInteger(name: "FltrMd" + i,
                                label: "Filter Mode " + i,
                                shortLabel: "Fil.Md." + i,
                                min: (int)FilterMode.LowpassMix,
                                max: (int)FilterMode.BandpassAdd,
                                defaultValue: (int)FilterMode.None,
                                updateAction: v => FilterParamer.Mode = (FilterMode)v);
      FltrCyclesMgr = CreateInteger(name: "FltrCy" + i,
                                label: "Filter Cycles " + i,
                                shortLabel: "Fil.Cy." + i,
                                min: 1,
                                max: 4,
                                defaultValue: 1,
                                updateAction: v => FilterParamer.Cycles = v);
      FltrCtMgr = CreateFloatMod(name: "FltrCt" + i,
                                 label: "Filter Cutoff " + i,
                                 shortLabel: "Fil.Ct." + i,
                                 defaultValue: 0.5f,
                                 modPara: FilterParamer.Cutoff);
      FltrWAMgr = CreateFloatMod(name: "FltrWA" + i,
                                 label: "Filter Wet Amt" + i,
                                 shortLabel: "Fl.W.A." + i,
                                 defaultValue: 1.0f,
                                 modPara: FilterParamer.WetAmt);
      FltrReMgr = CreateFloatMod(name: "FltrRe" + i,
                                 label: "Filter Resonance " + i,
                                 shortLabel: "Fl.F.b." + i,
                                 modPara: FilterParamer.Resonance);
    }

    public FilterParameter FilterParamer { get; }
    public SmyParameterManager FltrCtMgr { get; }
    public SmyParameterManager FltrMdMgr { get; }
    public SmyParameterManager FltrCyclesMgr { get; }
    public SmyParameterManager FltrReMgr { get; }
    public SmyParameterManager FltrWAMgr { get; }
  }
}