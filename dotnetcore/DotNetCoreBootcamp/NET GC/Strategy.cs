// //-----------------------------------------------------------------------------
// // <copyright file="Strategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC
{
    using System.Collections;
    using System.Collections.Generic;
    using Basic_Dispose_Pattern;

    public abstract class Strategy
    {
        public abstract IEnumerable<DebugAllocationData> Run();
    }
}