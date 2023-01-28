namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Define the svg property which provides <see cref="Value"/> and ability of <see cref="CanInherit"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISvgProperty<T> where T : class, ISvgProperty<T>
    {
        public bool CanInherit { get; }

        public object? Value { get; }

        public object? DefaultValue { get; }
    }
}
