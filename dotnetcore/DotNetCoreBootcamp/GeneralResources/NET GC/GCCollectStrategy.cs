// //-----------------------------------------------------------------------------
// // <copyright file="GCCollectStrategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class GCCollectStrategy : Strategy
    {
        public override IEnumerable<DebugAllocationData> Run()
        {
            long beforeGC = Environment.WorkingSet;

            yield return new DebugAllocationData(beforeGC, beforeGC, beforeGC);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            long afterGC = Environment.WorkingSet;

            yield return new DebugAllocationData(beforeGC, beforeGC, afterGC);
        }
    }
}