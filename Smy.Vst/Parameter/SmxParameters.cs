using Jacobi.Vst.Core;
using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Util;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using VstNetAudioPlugin;

//using static Smy.Vst.Smy.Filter;

namespace Smy.Vst.Parameter
{
  /// <summary>
  /// Encapsulated delay parameters.
  /// </summary>
  internal sealed class SmyParameters
  {

    /// <summary>
    /// Initializes the paramaters for the Delay component.
    /// </summary>
    /// <param name="parameters"></param>
    public SmyParameters(PluginParameters parameters)
    {
      Throw.IfArgumentIsNull(parameters, nameof(parameters));

      InitializeParameters(parameters);
    }

    public GeneralParameterContainer GeneralParameter { get; private set; }
    public EnvelopeParameterContainer[] EnvelopeManagerAry { get; private set; }
    public GenParameterContainer GenParameterContainer { get; private set; }
    public FilterParameterContainer[] FilterManagerAry { get; private set; }
    public EnvelopeLinkParameterContainer[] EnvelopeLinkManagerAry { get; private set; }
    public List<SmyParameterManager> AllManager { get; private set; }
    public List<SmyParameterManager> ModParaManager { get; private set; }

    private void InitializeParameters(PluginParameters parameters)
    {
      AllManager = new List<SmyParameterManager>();

      VstParameterInfoCollection parameterInfos = parameters.ParameterInfos;

      VstParameterCategory paramCategory = parameters.GetParameterCategory("General");
      GeneralParameter = new GeneralParameterContainer(paramCategory);
      AllManager.AddRange(GeneralParameter);

      paramCategory = parameters.GetParameterCategory("Filter");
      FilterManagerAry = Enumerable.Range(0, AudioEngine.MaxFilter)
                                    .Select(i => new FilterParameterContainer(paramCategory, i))
                                    .ToArray();
      AllManager.AddRange(FilterManagerAry.SelectMany(m => m));

      paramCategory = parameters.GetParameterCategory("Oscillators");
      GenParameterContainer = new GenParameterContainer(paramCategory);
      AllManager.AddRange(GenParameterContainer);
      
      paramCategory = parameters.GetParameterCategory("Envelopes");
      EnvelopeManagerAry = Enumerable.Range(0, AudioEngine.MaxEnvelopes)
                                    .Select(i => new EnvelopeParameterContainer(paramCategory, i))
                                    .ToArray();
      AllManager.AddRange(EnvelopeManagerAry.SelectMany(m => m));

      var envelopeParameter  = EnvelopeManagerAry.Select(p => p.Parameter).ToArray();
      ModParaManager = this.AllManager.Where(m => m.ModPara != null).ToList();

      int modParaId = 0;
      foreach (var modPara in ModParaManager)
      {
        modPara.ModParaIndex = modParaId++;
      }

      EnvelopeLinkManagerAry = Enumerable.Range(0, AudioEngine.MaxEnvelopeLinks)
                                    .Select(i => new EnvelopeLinkParameterContainer(paramCategory, i))
                                    .ToArray();
      AllManager.AddRange(EnvelopeLinkManagerAry.SelectMany(m => m));

      parameterInfos.AddRange(AllManager.Select(m => m.ParameterInfo));

    }
  }
}