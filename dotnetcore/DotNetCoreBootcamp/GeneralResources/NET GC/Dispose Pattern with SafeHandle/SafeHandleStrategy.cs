namespace NET_GC.Dispose_Pattern_with_SafeHandle
{
    using System.Collections.Generic;

    public class SafeHandleStrategy : FileStrategy
    {
        public override IEnumerable<DebugAllocationData> Run()
        {
            throw new System.NotImplementedException();
        }
    }
}