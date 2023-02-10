using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg.Utils
{
    internal class SafeStringTokenizer
    {
        private readonly string   _source;
        private readonly string[] _results;
        private int _index = 0;

        public SafeStringTokenizer(string source) : this(source, StringSplitOptions.RemoveEmptyEntries, ',')
        {

        }

        public SafeStringTokenizer(string source, params char[] separators) : this(source, StringSplitOptions.RemoveEmptyEntries, separators)
        {
            
        }

        public SafeStringTokenizer(string source, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, params char[] separators)
        {
            _source = source;

            if (string.IsNullOrWhiteSpace(source))
            {
                _results = Array.Empty<string>();
                return;
            }

            _results = source.Split(separators, options);
        }


        /// <summary>
        /// First item. If it does not exist, a exception will be thrown.
        /// </summary>
        public string Item1 => _results[0];
        /// <summary>
        /// Second item. If it does not exist, a exception will be thrown.
        /// </summary>
        public string Item2 => _results[1];
        /// <summary>
        /// Third item. If it does not exist, a exception will be thrown.
        /// </summary>
        public string Item3 => _results[2];
        /// <summary>
        /// Forth item. If it does not exist, a exception will be thrown.
        /// </summary>
        public string Item4 => _results[3];


        public bool HasNext()
        {
            return _results.Length > _index;
        }

        public int GetCount()
        {
            return _results.Length;
        }

        public int GetAvailableCount()
        {
            return _results.Length - _index;
        }

        public bool TryReadString(out string? text)
        {
            if (_results.Length <= _index)
            {
                text = null;
                return false;
            }

            text = _results[_index++];
            return true;
        }

        public bool TryReadInt(out int? integer)
        {
            if (_results.Length <= _index)
            {
                integer = null;
                return false;
            }

            if (int.TryParse(_results[_index], out var value))
            {
                integer = value;
                _index++;
                return true;
            }

            integer = null;
            return false;
        }

        public bool TryReadDouble(out double? @double)
        {
            if (_results.Length <= _index)
            {
                @double = null;
                return false;
            }

            if (double.TryParse(_results[_index], out var value))
            {
                @double = value;
                _index++;
                return true;
            }

            @double = null;
            return false;
        }

        public bool TryReadDate(out DateTime? date)
        {
            if (_results.Length <= _index)
            {
                date = null;
                return false;
            }

            if (DateTime.TryParse(_results[_index], out var value))
            {
                date = value;
                _index++;
                return true;
            }

            date = null;
            return false;
        }

        public bool TryReadColor(out Color? color)
        {
            if (_results.Length <= _index)
            {
                color = null;
                return false;
            }

            if (Color.TryParse(_results[_index], out var value))
            {
                color = value;
                _index++;
                return true;
            }

            color = null;
            return false;
        }

        public bool TryReadBrush(out ILightBrush? brush)
        {
            if (_results.Length <= _index)
            {
                brush = null;
                return false;
            }

            try
            {
                brush = new LightSolidColorBrush(Color.Parse(_results[_index]));
                _index++;
                return true;
            }
            catch
            {
                brush = null;
                return false;
            }
        }

        public IEnumerable<string> GetTokens()
        {
            return _results;
        }
    }
}
