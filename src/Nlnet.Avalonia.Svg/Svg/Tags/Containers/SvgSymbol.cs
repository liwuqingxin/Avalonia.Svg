﻿using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.symbol))]
public class SvgSymbol : SvgRenderable, ISvgContainer, ISvgRenderable
{
    
}