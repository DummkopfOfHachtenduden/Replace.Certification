using System;
using System.Runtime.InteropServices;

namespace Replace.Common.PaddedString
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PaddedString32 : IPaddedString
    {
        private const int SIZE = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE)]
        private string _value;

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.Length > SIZE)
                    throw new ArgumentOutOfRangeException(nameof(value), $"Exceeds maximum padding of {SIZE}");

                _value = value;
            }
        }

        public int Padding => SIZE;

        public PaddedString32(string value)
        {
            if (value.Length > SIZE)
                throw new ArgumentOutOfRangeException(nameof(value), $"Exceeds maximum padding of {SIZE}");

            _value = value;
        }
    }
}