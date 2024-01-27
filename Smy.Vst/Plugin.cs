using Jacobi.Vst.Core;
using Jacobi.Vst.Plugin.Framework;
using Jacobi.Vst.Plugin.Framework.Plugin;
using Smy.Vst.Smy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using VstNetAudioPlugin;
using MahApps.Metro.Controls;

namespace Smy.Vst
{
    /// <summary>
    /// The Plugin root class that derives from the framework provided base class that also include the interface manager.
    /// </summary>
    internal sealed class Plugin : VstPluginWithServices
  {
    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public Plugin()
        : base(name: "VST.NET Midi Smy",
               pluginID: 36373435,
               productInfo: new VstProductInfo("VST.NET 2 Code Samples", "Clara © 2023", 20231201),
               category: VstPluginCategory.Synth)
    { }

    protected override void ConfigureServices(IServiceCollection services)
    {
      // This Call fixes XAML error 
      var hack = new MetroWindow();
      
      services.AddSingleton<PluginParameters>()
                .AddSingletonAll<PluginPrograms>()
                .AddSingleton<VstMidiProgram>()
                .AddSingleton<Smy.NativeEngineHost>()
                .AddSingletonAll<AudioProcessor>()
                .AddSingletonAll<MidiProcessor>()
                .AddSingletonAll<PluginEditor>();
    }
  }
}
