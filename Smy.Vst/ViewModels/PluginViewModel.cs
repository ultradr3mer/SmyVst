using Jacobi.Vst.Core;
using Smy.Vst.Smy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using VstNetAudioPlugin;

namespace Smy.Vst.ViewModels
{
  internal class PluginViewModel : INotifyPropertyChanged
  {
    public BindingList<EnvelopeViewModel> EnvelopeVms { get; set; } = new BindingList<EnvelopeViewModel>();
    public BindingList<FilterViewModel> FilterVms { get; set; } = new BindingList<FilterViewModel>();
    public BindingList<DailViewModel> GeneralVms { get; set; } = new BindingList<DailViewModel>();
    public BindingList<KnobViewModel> GeneratorVms { get; set; } = new BindingList<KnobViewModel>();
    public List<Point>? Plot { get; set; }
    public BindingList<EnvelopeLinkViewModel> UnasignedEnvelopeLinkVms { get; set; } = new BindingList<EnvelopeLinkViewModel>();
    public PluginParameters parameters { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

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

        if (linkEnvNr == -1)
        {
          UnasignedEnvelopeLinkVms.Add(vm);
          continue;
        }

        int targetId = item.Parameter.TargetId;
        var paraMgr = parameters.SmyParameters.ModParaManager[targetId];
        var envVm = EnvelopeVms[linkEnvNr];
        envVm.Link(vm, targetId, paraMgr.ParameterInfo.Label, paraMgr.ParameterInfo.ShortLabel);
      }

      this.parameters = parameters;

      this.DrawGraph();

      foreach (var item in parameters.SmyParameters.AllManager)
      {
        item.ParameterChanged += Item_ParameterChanged;
      }
    }

    private void Item_ParameterChanged(object? sender, Util.SmyParameterManager.ParameterChangedEventArgs e)
    {
      this.DrawGraph();
    }

    private void DrawGraph()
    {
      int sampleRate = 44180;

      var engine = new NativeEngineHost(parameters);
      engine.ProcessNoteOnEvent(64);
      var ary = new float[44180 / 30];
      unsafe
      {
        fixed (float* floatPtr = ary)
        {
          var buffAry = new[] { new VstAudioBuffer(floatPtr, ary.Length, true) };
          engine.Generate(sampleRate, buffAry);
        }
      }

      int i = 0;
      var max = Math.Max(ary.Max(), -ary.Min());
      var normalized = ary.Select(v => v / max / 2.0 + 0.5);
      var data = normalized.Select(v => new Point((float)i++ / ary.Length, v)).ToList();
      this.Plot = data;
    }
  }
}