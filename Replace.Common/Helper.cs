using Replace.Common.Certification;
using System;
using System.Data;
using System.Reflection;

namespace Replace.Common
{
    public static class Helper
    {
        public static void SetupConsole(int width, int bufferHeight)
        {
            var asm = Assembly.GetCallingAssembly();
            var title = asm.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
            var version = asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            var configuration = asm.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
            var informationalVersion = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            //var product = asm.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            var copyright = asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

            Console.WindowWidth = width;
            Console.BufferHeight = bufferHeight;
            Console.Title = string.Format("{0} {1} ({2}) {3} {4}",
                title,
                version,
                System.IO.File.GetLastWriteTime(asm.Location),
                string.IsNullOrEmpty(configuration) ? string.Empty : string.Format("[{0}]", configuration),
                string.IsNullOrEmpty(informationalVersion) ? string.Empty : string.Format("<{0}>", informationalVersion));

            //Console.WriteLine(copyright);
        }

        public static bool Load<T>(T obj, IDataRecord record) where T : ICertificationRow
        {
            if (record.FieldCount == 0)
                return false;

            foreach (var prop in typeof(T).GetProperties())
            {
                //Skip runtime values.
                if (prop.SetMethod.IsPrivate == false)
                    continue;

                //Skip NULLs
                if (record[prop.Name] is DBNull)
                    continue;

                prop.SetValue(obj, record[prop.Name]);
            }
            return true;
        }
    }
}