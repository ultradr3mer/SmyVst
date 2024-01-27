﻿using Smy.Vst.Util;
using System;
using System.ComponentModel;

namespace Smy.Vst.ViewModels
{
  internal class DailViewModel : INotifyPropertyChanged
  {
    private SmyParameterManager manager;

    public DailViewModel(SmyParameterManager manager)
    {
      this.manager = manager;
      PropertyChanged += DailViewModel_PropertyChanged;
      this.defaultValue = manager.ParameterInfo.DefaultValue;
      this.Value = manager.CurrentValue;
      this.ShortLabel = manager.ParameterInfo.ShortLabel+":";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsPressed { get; set; }

    public double NormalizedValue
    {
      get
      {
        var targetScale = this.manager.Max - this.manager.Min;
        return (Value - this.manager.Min) / targetScale;
      }
      set
      {
        var targetScale = this.manager.Max - this.manager.Min;
        this.Value = value * targetScale + this.manager.Min;
        if (manager.IsInteger || manager.IsSwitch)
        {
          this.Value = (int)this.Value;
        }
      }
    }

    public string ShortLabel { get; }

    private float defaultValue;

    public double Value { get; set; }
    public string? ValueString { get; set; } = "0.00";

    private void DailViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(Value))
      {
        ValueString = Value.ToString("0.00");
        manager.CurrentValue = (float)this.Value;
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NormalizedValue)));
      }
    }

    internal void Reset()
    {
      this.Value = defaultValue;
    }
  }
}