namespace LacunaSpace.API.Extensions
{
    public static class ByteArrayExtensions
    {
        public static long ToLittleEndianBytesToLong(this byte[] bytes)
        {
            if (bytes.Length != 8)
                throw new ArgumentException("O array de bytes deve ter comprimento 8.", nameof(bytes));

            return BitConverter.ToInt64(bytes, 0);
        }

        public static long ToBigEndianBytesToLong(this byte[] bytes)
        {
            if (bytes.Length != 8)
                throw new ArgumentException("O array de bytes deve ter comprimento 8.", nameof(bytes));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
    }

}
