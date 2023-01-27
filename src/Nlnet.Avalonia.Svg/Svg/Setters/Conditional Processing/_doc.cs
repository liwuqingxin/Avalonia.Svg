///
/// About conditional processing attributes
/// https://www.w3.org/TR/SVG2/struct.html#ConditionalProcessing
/// 
/// SVG contains a ‘switch’ element along with attributes ‘requiredExtensions’ and
/// ‘systemLanguage’ to provide an ability to specify alternate viewing depending on
/// the capabilities of a given user agent or the user's language.
/// 
/// Attributes ‘requiredExtensions’ and ‘systemLanguage’ act as tests and evaluate to
/// either true or false. The ‘switch’ renders the first of its children for which
/// all of these attributes test true. If the given attribute is not specified, then
/// a true value is assumed.

///
/// NOTE We do not support conditional processing attributes now because we have few
/// developers. Maybe we will do it in future.