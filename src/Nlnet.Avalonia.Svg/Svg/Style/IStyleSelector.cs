using System;

namespace Nlnet.Avalonia.Svg
{
    public interface IStyleSelector
    {
        public bool Match(ISvgTag tag);
    }

    public class TagSelector : IStyleSelector
    {
        private readonly string _tagName;

        public TagSelector(string tagName)
        {
            _tagName = tagName;

        }

        public bool Match(ISvgTag tag)
        {
            return tag.GetTagName() == _tagName;
        }
    }

    public class ClassSelector : IStyleSelector
    {
        private readonly string _className;

        public ClassSelector(string className)
        {
            _className = className[1..];
        }

        public bool Match(ISvgTag tag)
        {
            return tag.Class == _className;
        }
    }

    public class IdSelector : IStyleSelector
    {
        private readonly string _id;

        public IdSelector(string id)
        {
            _id = id[1..];
        }

        public bool Match(ISvgTag tag)
        {
            return tag.Id == _id;
        }
    }
}
