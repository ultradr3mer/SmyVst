using Jacobi.Vst.Core;
using Jacobi.Vst.Plugin.Framework.Plugin;
using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Data;
using Smy.Vst.Parameter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using VstNetAudioPlugin;

namespace Smy.Vst.Smy
{
  internal class NativeEngineHost
  {
    private readonly PluginParameters parameters;

    private EngineParameter engineParameter;
    private Dictionary<short,int> keys = new Dictionary<short,int>();

    private AudioEngine nativeEngine;
    private long processedSamples = 0;
    private long runtimeTicks = 0;
    private Stopwatch sw = new Stopwatch();
    private int keyNumber;

    public NativeEngineHost(PluginParameters parameters)
    {
      this.parameters = parameters;

      engineParameter = this.parameters.SmyParameters.GeneralParameter.EngineParameter;
      engineParameter.EnvelopeLinks = this.parameters.SmyParameters.EnvelopeLinkManagerAry.Select(o => o.Parameter).ToList();
      engineParameter.ModParameter = this.parameters.SmyParameters.ModParaManager.Select(o => o.ModPara).ToList();
      nativeEngine = new AudioEngine(engineParameter);
      
      foreach (var container in this.parameters.SmyParameters.FilterManagerAry)
      {
        engineParameter.ActiveFilter.Add(container.FilterParamer);
      }

      engineParameter.ActiveEnvelopes = this.parameters.SmyParameters.EnvelopeManagerAry.Select(m => m.Parameter).ToList();
    }

    public bool IsPlaying => this.keys.Any() || this.nativeEngine.GetHasActiveKeys();

    internal void Generate(float sampleRate, VstAudioBuffer[] outChannels)
    {
      Debug.WriteLine("Value: "+ this.parameters.SmyParameters.FilterManagerAry.First().FltrCtMgr.CurrentValue.ToString());
      engineParameter.SampleRate = sampleRate;
      engineParameter.ActiveGenerators = GeneratorList.List.Where(g => this.parameters.SmyParameters.GenParameterContainer[g.Index].CurrentValue == 1)
                                  .OfType<GeneratorParameter>()
                                  .ToList();
      engineParameter.MinGenFactor = engineParameter.ActiveGenerators.Any() ?
         (float)engineParameter.ActiveGenerators.Min(g => g.Factor)
         : 0.0f;

      int length = outChannels[0].SampleCount;
      unsafe
      {
        int channelCount = outChannels.Length;
        var bufferAry = new float*[channelCount];
        for (int i = 0; i < channelCount; i++)
        {
          bufferAry[i] = ((IDirectBufferAccess32)outChannels[i]).Buffer;
        }

        nativeEngine.Run(keys, length, bufferAry);
      }
    }

    internal void ProcessNoteOffEvent(byte v)
    {
      keys.Remove(v);
    }

    internal void ProcessNoteOnEvent(byte v)
    {
      unchecked
      {
        keys[v] = keyNumber++;
      }
    }
  }
}