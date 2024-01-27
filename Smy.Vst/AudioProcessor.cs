namespace Smy.Vst
{
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Plugin.Framework.Plugin;
    using System;

    /// <summary>
    /// Implements the audio processing of the plugin using the <see cref="Smy.NativeEngineHost"/>.
    /// </summary>
    internal sealed class AudioProcessor : VstPluginAudioProcessor
  {
    private readonly Smy.NativeEngineHost generator;

    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    /// <param name="plugin">Must not be null.</param>
    public AudioProcessor(Smy.NativeEngineHost generator)
        : base(2, 2, 0, noSoundInStop: true)
    {
      this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
    {
      if (generator.IsPlaying)
      {
        generator.Generate(this.SampleRate, outChannels);
      }
      else // audio thru
      {
        base.Process(inChannels, outChannels);
      }
    }
  }
}
