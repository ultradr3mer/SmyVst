using Jacobi.Vst.Plugin.Framework;
using Smy.Vst.Data;
using Smy.Vst.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smy.Vst.Parameter
{
    internal class GenParameterContainer : ParameterContainer
    {
        public GenParameterContainer(VstParameterCategory paramCategory) : base(paramCategory)
        {
            foreach (var gen in GeneratorList.List)
            {
                CreateSwitch(name: gen.ParameterName,
                            label: gen.ParameterLabel,
                            shortLabel: gen.DisplayName,
                            defaultValue: gen.Factor == 1.0f);
            }
        }
    }
}