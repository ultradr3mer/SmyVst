using Jacobi.Vst.Plugin.Framework;
using System;
using System.ComponentModel;

namespace Smy.Vst.Util
{
  internal class SmyParameterManager : INotifyPropertyChanged
  {
    internal bool IsInteger = false;
    internal bool IsSwitch = false;
    private VstParameterManager innerManager;

    public class ParameterChangedEventArgs : EventArgs
    {
      public ParameterChangedEventArgs(float newValue)
      {
        NewValue = newValue;
      }

      public float NewValue { get; set; }
    }

    public event EventHandler<ParameterChangedEventArgs> ParameterChanged;

    public SmyParameterManager(VstParameterInfo paramInfo, Action<float>? updateAction = null, float min = 0, float max = 1)
    {
      this.innerManager = paramInfo
                        .Normalize()
                        .ToManager();

      if(updateAction != null)
        this.ParameterChanged += (o,a) => updateAction(a.NewValue);

      Min = min;
      Max = max;

      this.innerManager.PropertyChanged += InnerManager_PropertyChanged;
      this.ParameterChanged?.Invoke(this, new ParameterChangedEventArgs(paramInfo.DefaultValue));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public float CurrentValue
    {
      get => this.innerManager.CurrentValue;
      set
      {
        if (value == CurrentValue)
          return;

        if (this.innerManager.ActiveParameter != null)
        {
          this.innerManager.ActiveParameter.Value = (float)value;
        }
      }
    }

    public float Max { get; }
    public float Min { get; }
    public ModPara ModPara { get; internal set; }
    public VstParameterInfo ParameterInfo { get => this.innerManager.ParameterInfo; }
    public int ModParaIndex { get; internal set; }

    private void InnerManager_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(VstParameterManager.CurrentValue))
      {
        ParameterChanged?.Invoke(this, new ParameterChangedEventArgs(this.innerManager.CurrentValue));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentValue)));
      }
    }

    public override string ToString()
    {
      return $"{this.ParameterInfo.Label}: {this.CurrentValue}";
    }
  }
}