namespace Replace.Common
{
    public static class Caller
    {
        public static string GetMemberName([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            return memberName;
        }

        public static string GetFilePath([System.Runtime.CompilerServices.CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        public static int GetLineNumber([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
    }
}