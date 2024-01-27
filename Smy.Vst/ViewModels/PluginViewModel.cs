﻿using System.ComponentModel;
using VstNetAudioPlugin;

namespace Smy.Vst.ViewModels
{
  internal class PluginViewModel
  {
    public BindingList<KnobViewModel> GeneratorVms { get; set; } = new BindingList<KnobViewModel>();
    public BindingList<DailViewModel> GeneralVms { get; set; } = new BindingList<DailViewModel>();
    public BindingList<FilterViewModel> FilterVms { get; set; } = new BindingList<FilterViewModel>();
    public BindingList<EnvelopeLinkViewModel> UnasignedEnvelopeLinkVms { get; set; } = new BindingList<EnvelopeLinkViewModel>();
    public BindingList<EnvelopeViewModel> EnvelopeVms { get; set; } = new BindingList<EnvelopeViewModel>();

    internal void InitializeParameters(PluginParameters parameters)
    {
      foreach (var item in parameters.SmyParameters.GenParameterContainer)
      {
        GeneratorVms.Add(new KnobViewModel(item));
      }

      foreach (var item in parameters.SmyParameters.GeneralParameter)
      {
        GeneralVms.Add(new DailViewModel(item));
      }

      int filterNr = 0;
      foreach (var item in parameters.SmyParameters.FilterManagerAry)
      {
        FilterVms.Add(new FilterViewModel(item, filterNr++));
      }

      int envNr = 0;
      foreach (var item in parameters.SmyParameters.EnvelopeManagerAry)
      {
        EnvelopeVms.Add(new EnvelopeViewModel(item, parameters.SmyParameters.ModParaManager, UnasignedEnvelopeLinkVms, envNr++));
      }

      foreach (var item in parameters.SmyParameters.EnvelopeLinkManagerAry)
      {
        var vm = new EnvelopeLinkViewModel(item);
        int linkEnvNr = item.Parameter.EnvelopeNr;

        if (linkEnvNr == -1) {
          UnasignedEnvelopeLinkVms.Add(vm);
          continue;
        }

        int targetId = item.Parameter.TargetId;
        var paraMgr = parameters.SmyParameters.ModParaManager[targetId];
        var envVm = EnvelopeVms[linkEnvNr];
        envVm.Link(vm, targetId, paraMgr.ParameterInfo.Label, paraMgr.ParameterInfo.ShortLabel);
      }
    }
  }
}