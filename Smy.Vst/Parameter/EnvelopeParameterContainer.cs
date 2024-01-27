using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Util;

namespace Smy.Vst.Parameter
{
  internal class EnvelopeParameterContainer : ParameterContainer
  {
    public EnvelopeParameterContainer(VstParameterCategory paramCategory, int i) : base(paramCategory)
    {
      Parameter = new EnvelopeParameter();

      AttackMgr = CreateFloat(name: "EnvAtt" + i,
                                    label: "Envelope Attack " + i,
                                    shortLabel: "Env.At." + i,
                                    updateAction: v => Parameter.Attack = v);
      DecayMgr = CreateFloat(name: "EnvDec" + i,
                             label: "Envelope Decay " + i,
                             shortLabel: "Env.Dc." + i,
                             defaultValue: 0.5f,
                             updateAction: v => Parameter.Decay = v);
      SustainMgr = CreateFloat(name: "EnvSus" + i,
                               label: "Envelope Sustain " + i,
                               shortLabel: "Env.Su." + i,
                               defaultValue: 1.0f,
                               updateAction: v => Parameter.Sustain = v);
      ReleaseMgr = CreateFloat(name: "EnvRel" + i,
                               label: "Envelope Release " + i,
                               shortLabel: "Env.Re." + i,
                               updateAction: v => Parameter.Release = v);
    }

    public EnvelopeParameter Parameter { get; }
    public SmyParameterManager DecayMgr { get; }
    public SmyParameterManager AttackMgr { get; }
    public SmyParameterManager ReleaseMgr { get; }
    public SmyParameterManager SustainMgr { get; }
  }
}