using System;

namespace ZTn.BNet.D3.DataProviders
{
#if !PORTABLE
#endif
    public class FileNotInCacheException : Exception
    {
        public FileNotInCacheException()
            : base("Object was not found in cache")
        {
        }
    }
}