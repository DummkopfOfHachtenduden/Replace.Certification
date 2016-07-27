using Replace.Common.PaddedString;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Replace.Common.Security
{
    [DebuggerDisplay("Opcode = _opcode.ToString(\"X4\")")]
    public class Packet : IDisposable
    {
        private ushort _opcode;
        private PacketWriter _writer;
        private PacketReader _reader;
        private bool _encrypted;
        private bool _massive;
        private bool _locked;
        private byte[] _readerBytes;
        private object _lock;

        public byte Type => Convert.ToByte((_opcode >> 12) & byte.MaxValue);
        public ushort Opcode => _opcode;
        public string HexCode => _opcode.ToString("X4");
        public bool Encrypted => _encrypted;
        public bool Massive => _massive;

        public Packet(Packet rhs)
        {
            lock (rhs._lock)
            {
                _lock = new object();
                _opcode = rhs._opcode;
                _encrypted = rhs._encrypted;
                _massive = rhs._massive;

                _locked = rhs._locked;
                if (!_locked)
                {
                    _writer = new PacketWriter();
                    _reader = null;
                    _readerBytes = null;
                    _writer.Write(rhs._writer.GetBytes());
                }
                else
                {
                    _writer = null;
                    _readerBytes = rhs._readerBytes;
                    _reader = new PacketReader(_readerBytes);
                }
            }
        }

        public Packet(ushort opcode)
        {
            _lock = new object();
            _opcode = opcode;
            _encrypted = false;
            _massive = false;
            _writer = new PacketWriter();
            _reader = null;
            _readerBytes = null;
        }

        public Packet(ushort opcode, bool encrypted)
        {
            _lock = new object();
            _opcode = opcode;
            _encrypted = encrypted;
            _massive = false;
            _writer = new PacketWriter();
            _reader = null;
            _readerBytes = null;
        }

        public Packet(ushort opcode, bool encrypted, bool massive)
        {
            if (encrypted && massive)
                throw new PacketException("[Packet::ctor] Packets cannot both be massive and encrypted!");

            _lock = new object();
            _opcode = opcode;
            _encrypted = encrypted;
            _massive = massive;
            _writer = new PacketWriter();
            _reader = null;
            _readerBytes = null;
        }

        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes)
        {
            if (encrypted && massive)
                throw new PacketException("[Packet::ctor] Packets cannot both be massive and encrypted!");

            _lock = new object();
            _opcode = opcode;
            _encrypted = encrypted;
            _massive = massive;
            _writer = new PacketWriter();
            _writer.Write(bytes);
            _reader = null;
            _readerBytes = null;
        }

        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes, int offset, int length)
        {
            if (encrypted && massive)
            {
                throw new PacketException("[Packet::ctor] Packets cannot both be massive and encrypted!");
            }
            _lock = new object();
            _opcode = opcode;
            _encrypted = encrypted;
            _massive = massive;
            _writer = new PacketWriter();
            _writer.Write(bytes, offset, length);
            _reader = null;
            _readerBytes = null;
        }

        public byte[] GetBytes()
        {
            lock (_lock)
            {
                if (_locked)
                {
                    return _readerBytes;
                }
                return _writer.GetBytes();
            }
        }

        public override string ToString()
        {
            var packet_bytes = this.GetBytes();
            if (packet_bytes.Length > 0)
                return BitConverter.ToString(_readerBytes).Replace("-", "");
            else
                return string.Empty;
        }

        public void Lock()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    _readerBytes = _writer.GetBytes();
                    _reader = new PacketReader(_readerBytes);
                    _writer.Close();
                    _writer = null;
                    _locked = true;
                }
            }
        }

        #region Read

        public long SeekRead(long offset, SeekOrigin orgin)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot SeekRead on an unlocked Packet.");
                }
                return _reader.BaseStream.Seek(offset, orgin);
            }
        }

        public int Position
        {
            get
            {
                lock (_lock)
                {
                    if (!_locked)
                    {
                        throw new PacketException("Cannot get Position from an unlocked Packet.");
                    }
                    return (int)(_reader.BaseStream.Position);
                }
            }
            set
            {
                lock (_lock)
                {
                    if (!_locked)
                    {
                        throw new PacketException("Cannot set Position from an unlocked Packet.");
                    }
                    _reader.BaseStream.Position = value;
                }
            }
        }

        public int Remaining
        {
            get
            {
                lock (_lock)
                {
                    if (!_locked)
                    {
                        throw new PacketException("Cannot read Remaining from an unlocked Packet.");
                    }
                    return (int)(_reader.BaseStream.Length - _reader.BaseStream.Position);
                }
            }
        }

        public int Length
        {
            get
            {
                lock (_lock)
                {
                    if (!_locked)
                    {
                        throw new PacketException("Cannot read Length from an unlocked Packet.");
                    }
                    return (int)(_reader.BaseStream.Length);
                }
            }
        }

        public bool ReadBool()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadBoolean();
            }
        }

        public byte ReadByte()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadByte();
            }
        }

        public sbyte ReadSByte()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadSByte();
            }
        }

        public ushort ReadUShort()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadUInt16();
            }
        }

        public short ReadShort()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadInt16();
            }
        }

        public uint ReadUInt()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadUInt32();
            }
        }

        public int ReadInt()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadInt32();
            }
        }

        public ulong ReadULong()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadUInt64();
            }
        }

        public long ReadLong()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadInt64();
            }
        }

        public float ReadFloat()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadSingle();
            }
        }

        public double ReadDouble()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return _reader.ReadDouble();
            }
        }

        public string ReadAscii()
        {
            return ReadAscii(1252);
        }

        public string ReadAscii(int codepage)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }

                ushort length = _reader.ReadUInt16();
                byte[] bytes = _reader.ReadBytes(length);

                return Encoding.GetEncoding(codepage).GetString(bytes);
            }
        }

        public string ReadUnicode()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }

                UInt16 length = _reader.ReadUInt16();
                byte[] bytes = _reader.ReadBytes(length * 2);

                return Encoding.Unicode.GetString(bytes);
            }
        }

        public DateTime ReadDateTime()
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                return new DateTime(_reader.ReadUInt16(), _reader.ReadUInt16(), _reader.ReadUInt16(), //DD-MM-YYYY
                                    _reader.ReadUInt16(), _reader.ReadUInt16(), _reader.ReadUInt16(), //HH:MM:SS
                                    _reader.ReadInt32()); //FFF
            }
        }

        public TStruct ReadMarshalled<TStruct>() where TStruct : Unmanaged.IMarshalled
        {
            var buffer = this.ReadByteArray(Marshal.SizeOf(typeof(TStruct)));
            return Unmanaged.BufferToStruct<TStruct>(buffer);
        }

        public TPaddedString ReadPaddedString<TPaddedString>() where TPaddedString : Unmanaged.IMarshalled, IPaddedString
        {
            return this.ReadMarshalled<TPaddedString>();
        }

        public bool[] ReadBoolArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                bool[] values = new bool[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadBoolean();
                }
                return values;
            }
        }

        public byte[] ReadByteArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                byte[] values = new byte[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadByte();
                }
                return values;
            }
        }

        public sbyte[] ReadSByteArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                sbyte[] values = new sbyte[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadSByte();
                }
                return values;
            }
        }

        public ushort[] ReadUShortArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                UInt16[] values = new UInt16[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadUInt16();
                }
                return values;
            }
        }

        public short[] ReadShortArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                Int16[] values = new Int16[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadInt16();
                }
                return values;
            }
        }

        public uint[] ReadUIntArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                UInt32[] values = new UInt32[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadUInt32();
                }
                return values;
            }
        }

        public int[] ReadIntArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                Int32[] values = new Int32[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadInt32();
                }
                return values;
            }
        }

        public ulong[] ReadULongArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                UInt64[] values = new UInt64[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadUInt64();
                }
                return values;
            }
        }

        public long[] ReadLongArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                Int64[] values = new Int64[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadInt64();
                }
                return values;
            }
        }

        public float[] ReadFloatArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                Single[] values = new Single[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadSingle();
                }
                return values;
            }
        }

        public double[] ReadDoubleArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                Double[] values = new Double[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = _reader.ReadDouble();
                }
                return values;
            }
        }

        public string[] ReadStringArray(int count)
        {
            return ReadStringArray(1252);
        }

        public string[] ReadStringArray(int codepage, int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                String[] values = new String[count];
                for (int x = 0; x < count; ++x)
                {
                    UInt16 length = _reader.ReadUInt16();
                    byte[] bytes = _reader.ReadBytes(length);
                    values[x] = Encoding.UTF7.GetString(bytes);
                }
                return values;
            }
        }

        public string[] ReadUnicodeArray(int count)
        {
            lock (_lock)
            {
                if (!_locked)
                {
                    throw new PacketException("Cannot Read from an unlocked Packet.");
                }
                String[] values = new String[count];
                for (int x = 0; x < count; ++x)
                {
                    UInt16 length = _reader.ReadUInt16();
                    byte[] bytes = _reader.ReadBytes(length * 2);
                    values[x] = Encoding.Unicode.GetString(bytes);
                }
                return values;
            }
        }

        #endregion Read

        #region Write

        public long SeekWrite(long offset, SeekOrigin orgin)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot SeekWrite on a locked Packet.");
                }
                return _writer.BaseStream.Seek(offset, orgin);
            }
        }

        public void WriteBool(bool value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteByte(byte value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteSByte(sbyte value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteUShort(ushort value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteShort(short value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteUInt(uint value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteInt(int value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteULong(ulong value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteLong(long value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteFloat(float value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteDouble(double value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(value);
            }
        }

        public void WriteAscii(string value)
        {
            WriteAscii(value, 1252);
        }

        public void WriteAscii(string value, int code_page)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }

                byte[] codepage_bytes = Encoding.GetEncoding(code_page).GetBytes(value);
                string utf7_value = Encoding.UTF7.GetString(codepage_bytes);
                byte[] bytes = Encoding.Default.GetBytes(utf7_value);

                _writer.Write((ushort)bytes.Length);
                _writer.Write(bytes);
            }
        }

        public void WriteUnicode(string value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }

                byte[] bytes = Encoding.Unicode.GetBytes(value);

                _writer.Write((ushort)value.ToString().Length);
                _writer.Write(bytes);
            }
        }

        public void WriteDateTime(DateTime date)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((ushort)date.Year);
                _writer.Write((ushort)date.Month);
                _writer.Write((ushort)date.Day);
                _writer.Write((ushort)date.Hour);
                _writer.Write((ushort)date.Minute);
                _writer.Write((ushort)date.Second);
                _writer.Write((uint)date.Millisecond);
            }
        }

        public void WriteBool(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(Convert.ToBoolean(Convert.ToUInt64(value) & 0xFF));
            }
        }

        public void WriteByte(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((byte)(Convert.ToUInt64(value) & 0xFF));
            }
        }

        public void WriteSByte(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((sbyte)(Convert.ToInt64(value) & 0xFF));
            }
        }

        public void WriteUShort(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((ushort)(Convert.ToUInt64(value) & 0xFFFF));
            }
        }

        public void WriteShort(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((ushort)(Convert.ToInt64(value) & 0xFFFF));
            }
        }

        public void WriteUInt(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((uint)(Convert.ToUInt64(value) & 0xFFFFFFFF));
            }
        }

        public void WriteInt(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write((int)(Convert.ToInt64(value) & 0xFFFFFFFF));
            }
        }

        public void WriteULong(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(Convert.ToUInt64(value));
            }
        }

        public void WriteLong(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(Convert.ToInt64(value));
            }
        }

        public void WriteFloat(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(Convert.ToSingle(value));
            }
        }

        public void WriteDouble(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                _writer.Write(Convert.ToDouble(value));
            }
        }

        public void WriteAscii(object value)
        {
            WriteAscii(value, 1252);
        }

        public void WriteAscii(object value, int code_page)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }

                byte[] codepage_bytes = Encoding.GetEncoding(code_page).GetBytes(value.ToString());
                string utf7_value = Encoding.UTF7.GetString(codepage_bytes);
                byte[] bytes = Encoding.Default.GetBytes(utf7_value);

                _writer.Write((ushort)bytes.Length);
                _writer.Write(bytes);
            }
        }

        public void WriteUnicode(object value)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }

                byte[] bytes = Encoding.Unicode.GetBytes(value.ToString());

                _writer.Write((ushort)value.ToString().Length);
                _writer.Write(bytes);
            }
        }

        public void WriteMarshalled<TMarshal>(TMarshal value) where TMarshal : Unmanaged.IMarshalled
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }

                var bytes = Unmanaged.StructToBuffer(value);
                _writer.Write(bytes);
            }
        }

        public void WritePaddedString<TPaddedString>(TPaddedString value) where TPaddedString : Unmanaged.IMarshalled, IPaddedString
        {
            this.WriteMarshalled<TPaddedString>(value);
        }

        #region WriteArray

        public void WriteByteArray(byte[] values)
        {
            if (_locked)
            {
                throw new PacketException("Cannot Write to a locked Packet.");
            }
            _writer.Write(values);
        }

        public void WriteByteArray(byte[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteUShortArray(ushort[] values)
        {
            WriteUShortArray(values, 0, values.Length);
        }

        public void WriteUShortArray(ushort[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteShortArray(short[] values)
        {
            WriteShortArray(values, 0, values.Length);
        }

        public void WriteShortArray(short[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteUIntArray(uint[] values)
        {
            WriteUIntArray(values, 0, values.Length);
        }

        public void WriteUIntArray(uint[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteIntArray(int[] values)
        {
            WriteIntArray(values, 0, values.Length);
        }

        public void WriteIntArray(int[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteULongArray(ulong[] values)
        {
            WriteULongArray(values, 0, values.Length);
        }

        public void WriteULongArray(ulong[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteLongArray(long[] values)
        {
            WriteLongArray(values, 0, values.Length);
        }

        public void WriteLongArray(long[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteFloatArray(float[] values)
        {
            WriteFloatArray(values, 0, values.Length);
        }

        public void WriteFloatArray(float[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteDoubleArray(double[] values)
        {
            WriteDoubleArray(values, 0, values.Length);
        }

        public void WriteDoubleArray(double[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    _writer.Write(values[x]);
                }
            }
        }

        public void WriteAsciiArray(string[] values, int codepage)
        {
            WriteAsciiArray(values, 0, values.Length, codepage);
        }

        public void WriteAsciiArray(string[] values, int index, int count, int codepage)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteAscii(values[x], codepage);
                }
            }
        }

        public void WriteAsciiArray(string[] values)
        {
            WriteAsciiArray(values, 0, values.Length, 1252);
        }

        public void WriteAsciiArray(string[] values, int index, int count)
        {
            WriteAsciiArray(values, index, count, 1252);
        }

        public void WriteUnicodeArray(string[] values)
        {
            WriteUnicodeArray(values, 0, values.Length);
        }

        public void WriteUnicodeArray(string[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUnicode(values[x]);
                }
            }
        }

        public void WriteByteArray(object[] values)
        {
            WriteByteArray(values, 0, values.Length);
        }

        public void WriteByteArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteByte(values[x]);
                }
            }
        }

        public void WriteInt8Array(object[] values)
        {
            WriteInt8Array(values, 0, values.Length);
        }

        public void WriteInt8Array(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteSByte(values[x]);
                }
            }
        }

        public void WriteUShortArray(object[] values)
        {
            WriteUShortArray(values, 0, values.Length);
        }

        public void WriteUShortArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUShort(values[x]);
                }
            }
        }

        public void WriteShortArray(object[] values)
        {
            WriteShortArray(values, 0, values.Length);
        }

        public void WriteShortArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteShort(values[x]);
                }
            }
        }

        public void WriteUIntArray(object[] values)
        {
            WriteUIntArray(values, 0, values.Length);
        }

        public void WriteUIntArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUInt(values[x]);
                }
            }
        }

        public void WriteIntArray(object[] values)
        {
            WriteIntArray(values, 0, values.Length);
        }

        public void WriteIntArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteInt(values[x]);
                }
            }
        }

        public void WriteULongArray(object[] values)
        {
            WriteULongArray(values, 0, values.Length);
        }

        public void WriteULongArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteULong(values[x]);
                }
            }
        }

        public void WriteLongArray(object[] values)
        {
            WriteLongArray(values, 0, values.Length);
        }

        public void WriteLongArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteLong(values[x]);
                }
            }
        }

        public void WriteFloatArray(object[] values)
        {
            WriteFloatArray(values, 0, values.Length);
        }

        public void WriteFloatArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteFloat(values[x]);
                }
            }
        }

        public void WriteDoubleArray(object[] values)
        {
            WriteDoubleArray(values, 0, values.Length);
        }

        public void WriteDoubleArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteDouble(values[x]);
                }
            }
        }

        public void WriteAsciiArray(object[] values, int codepage)
        {
            WriteAsciiArray(values, 0, values.Length, codepage);
        }

        public void WriteAsciiArray(object[] values, int index, int count, int codepage)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteAscii(values[x].ToString(), codepage);
                }
            }
        }

        public void WriteAsciiArray(object[] values)
        {
            WriteAsciiArray(values, 0, values.Length, 1252);
        }

        public void WriteAsciiArray(object[] values, int index, int count)
        {
            WriteAsciiArray(values, index, count, 1252);
        }

        public void WriteUnicodeArray(object[] values)
        {
            WriteUnicodeArray(values, 0, values.Length);
        }

        public void WriteUnicodeArray(object[] values, int index, int count)
        {
            lock (_lock)
            {
                if (_locked)
                {
                    throw new PacketException("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUnicode(values[x].ToString());
                }
            }
        }

        #endregion WriteArray

        #endregion Write

        #region Dispose

        private bool disposed = false;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    if (_writer != null)
                        _writer.Dispose();

                    if (_reader != null)
                        _reader.Dispose();
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                _readerBytes = null;
                _lock = null;

                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~Packet()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion Dispose
    }
}