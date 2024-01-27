using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Smy.Vst.Parameter
{
  internal class ParameterContainer : List<SmyParameterManager>
  {
    private VstParameterCategory paramCategory;

    public ParameterContainer(VstParameterCategory paramCategory)
    {
      this.paramCategory = paramCategory;
    }

    protected SmyParameterManager CreateFloatMod(string name, string label, string shortLabel, ModPara modPara, float defaultValue = 0, float min = 0, float max = 1)
    {
      var paramInfo = new VstParameterInfo
      {
        Category = paramCategory,
        CanBeAutomated = true,
        Name = name,
        Label = label,
        ShortLabel = shortLabel,
        LargeStepFloat = 0.1f,
        SmallStepFloat = 0.01f,
        StepFloat = 0.05f,
        DefaultValue = defaultValue,
      };

      modPara.SetMin(min);
      modPara.SetMax(max);

      Action<float>? updateAction = v => modPara.SetBase(v);
      var SmyManager = new SmyParameterManager(paramInfo, updateAction, min, max);
      SmyManager.ModPara = modPara;
      Add(SmyManager);
      return SmyManager;
    }

    protected SmyParameterManager CreateFloat(string name, string label, string shortLabel, float defaultValue = 0, Action<float> updateAction = null, float min = 0, float max = 1)
    {
      var paramInfo = new VstParameterInfo
      {
        Category = paramCategory,
        CanBeAutomated = true,
        Name = name,
        Label = label,
        ShortLabel = shortLabel,
        LargeStepFloat = 0.1f,
        SmallStepFloat = 0.01f,
        StepFloat = 0.05f,
        DefaultValue = defaultValue,
      };

      var SmyManager = new SmyParameterManager(paramInfo, updateAction, min, max);
      Add(SmyManager);
      return SmyManager;
    }

    protected SmyParameterManager CreateInteger(string name, string label, string shortLabel, int max, int defaultValue = 0, int min = 0, Action<int> updateAction = null)
    {
      var paramInfo = new VstParameterInfo
      {
        Category = paramCategory,
        CanBeAutomated = true,
        CanRamp = true,
        Name = name,
        Label = label,
        ShortLabel = shortLabel,
        MinInteger = min,
        MaxInteger = max,
        StepInteger = 1,
        LargeStepInteger = 3,
        DefaultValue = defaultValue,
      };

      Action<float>? intUpdateAction = updateAction != null ? f => updateAction((int)f) : null;
      var SmyManager = new SmyParameterManager(paramInfo, intUpdateAction, min, max);
      SmyManager.IsInteger = true;
      Add(SmyManager);
      return SmyManager;
    }

    protected SmyParameterManager CreateSwitch(string name, string label, string shortLabel, bool defaultValue = false, Action<bool> updateAction = null)
    {
      var paramInfo = new VstParameterInfo
      {
        Category = paramCategory,
        CanBeAutomated = true,
        Name = name,
        Label = label,
        ShortLabel = shortLabel,
        IsSwitch = true,
        DefaultValue = defaultValue ? 1 : 0
      };

      Action<float>? boolUpdateAction = updateAction != null ? f => updateAction(f == 1) : null;
      var SmyManager = new SmyParameterManager(paramInfo, boolUpdateAction);
      SmyManager.IsSwitch = true;
      Add(SmyManager);
      return SmyManager;
    }
  }
}