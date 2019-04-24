// //-----------------------------------------------------------------------------
// // <copyright file="DebugAllocationData.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC
{
    public class DebugAllocationData
    {
        public DebugAllocationData(long beforeAllocBytes, long afterAllocBytes, long afterDisposeBytes)
        {
            BeforeAllocBytes = beforeAllocBytes;
            AfterAllocBytes = afterAllocBytes;
            AfterDisposeBytes = afterDisposeBytes;
        }

        public long AfterDisposeBytes { get; }
        public long BeforeAllocBytes { get; }
        public long AfterAllocBytes { get; }
    }
}