﻿using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.unknown))]
public class SvgUnknown : SvgRenderable, ISvgContainer, ISvgRenderable
{
    
}