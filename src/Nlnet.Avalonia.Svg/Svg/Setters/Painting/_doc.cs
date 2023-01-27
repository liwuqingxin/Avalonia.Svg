///
/// About painting properties
/// https://www.w3.org/TR/SVG2/painting.html#Definitions
/// 
/// Graphical elements that define a shape – ‘path’ elements, basic shapes, and text content
/// elements – are rendered by being filled, which is painting the interior of the object, and
/// stroked, which is painting along the outline of the object. Filling and stroking are both
/// painting operations. SVG 2 supports a number of different paints that the fill and stroke
/// of a graphical element can be painted with:
///     - a single color,
///     - a gradient (linear or radial)
///     - a pattern(vector or raster, possibly tiled),
///     - other images as specified using CSS  Image Value syntax[css - images - 3].
///
/// The fill and stroke properties, defined below, are used to specify the paint used to render
/// the interior of and the stroke around shapes and text. A paint specification describes a
/// way of putting color values on to the canvas and is composed of one or more paint layers.
/// Four types of paints within these paint layers are supported: solid colors, gradients, and
/// patterns.
///
/// A <paint> value is defined as follows:
/// <paint> = none | <color> | <url> [none | <color>] ? | context - fill | context - stroke