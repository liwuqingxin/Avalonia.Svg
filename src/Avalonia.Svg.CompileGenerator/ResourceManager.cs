using System.Collections;
using System.Resources;

namespace Avalonia.Svg.CompileGenerator
{
    internal static class ResourceManager
    {
        public static bool TryGetTextFile(string uri, out string content)
        {
            var assembly = typeof(ResourceManager).Assembly;
            foreach (var resName in assembly.GetManifestResourceNames())
            {
                var stream = assembly.GetManifestResourceStream(resName);
                if (stream == null)
                {
                    continue;
                }

                var rr = new ResourceReader(stream);
                var enumerator = rr.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is DictionaryEntry entry && entry.Key is string name)
                    {
                        // entry.Key是资源名 ("resources/icon-lib/a1/shaixuan.png")
                        // entry.Value是资源内容流
                        if (name.ToLower().StartsWith(uri))
                        {
                            content = entry.Value.ToString();
                            return true;
                        }
                    }
                }
            }

            content = null;
            return false;
        }
    }
}