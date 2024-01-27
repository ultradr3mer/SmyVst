﻿using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.UI;
using Smy.Vst.ViewModels;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace VstNetAudioPlugin.UI
{
  public partial class PluginEditorView : UserControl
  {
    private IList<VstParameterManager> parameters;
    private PluginViewModel viewModel;

    public PluginEditorView()
    {
      ElementHost host = new ElementHost();
      host.Dock = DockStyle.Fill;

      this.viewModel = new PluginViewModel();
      var view = new InnerPluginView() { DataContext = viewModel };
      host.Child = view;

      using (var g = this.CreateGraphics())
      {
        this.Height = (int)(400 * g.DpiX / 96);
        this.Width = (int)(900 * g.DpiY / 96);
      }

      this.Controls.Add(host);
    }

    internal bool InitializeParameters(PluginParameters parameters)
    {
      this.viewModel.InitializeParameters(parameters);

      return true;
    }
    internal void ProcessIdle()
    {
      // TODO: short idle processing here
    }
  }
}