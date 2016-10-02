﻿using System;

namespace ZTn.BNet.D3.DataProviders
{
#if !PORTABLE
#endif
    public sealed class BNetResponseFailedException : Exception
    {
        public BNetResponseFailedException()
            : base("Battle.net failed at sending back a valid response")
        {
        }
    }
}