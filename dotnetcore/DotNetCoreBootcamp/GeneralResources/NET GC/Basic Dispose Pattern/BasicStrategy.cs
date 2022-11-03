// //-----------------------------------------------------------------------------
// // <copyright file="BasicStrategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Basic_Dispose_Pattern
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Demonstrates the basic IDisposable pattern, in its simplest form.
    /// </summary>
    /// <remarks>
    ///   This sample demonstrates two distinct behaviors of the garbage collector for a class that
    ///   implements IDisposable.
    /// 
    ///   Deterministic Mode
    ///     In this mode, BitmapImage.Dispose will be called directly, which will call Bitmap.Dispose,
    ///     freeing any unmanaged memory held by Bitmap immediately. In this case, BitmapImage.Finalize, 
    ///     and Bitmap.Finalize will both be supressed, and the finalizers will not run. Note that BitmapImage
    ///     in this case does not implement a finalizer, but base.Finalize would be suppressed (Object.Finalize)
    ///     instead.
    /// 
    ///   Indeterministic Mode
    ///     In this mode, the strategy will not call BitmapImage.Dispose, and will let the GC determine
    ///     when to collect the object. This will typically be when the GC determines that the reference
    ///     count to the object has reached 0 and enough managed memory has been allocated that the GC 
    ///     has determined it needs to free additional memory.
    /// 
    ///     If the object implements a finalizer, the GC will place the
    ///     object in the finalization queue, and when the finalizer thread runs, the Dispose method will
    ///     be called (Dispose(false)), indicating it was called by the GC, and not the developer, and the
    ///     unmanaged memory will be freed at that time. 
    /// 
    ///     This is indeterministic because there is no knowing exactly when the GC will run, even if the 
    ///     reference count has reached 0, the memory still may not be reclaimed. Especially in the case of
    ///     unmanaged memory, the GC will typically not run again until there is enough *managed* memory 
    ///     allocated to cause the GC to try to free memory, and you will often run out of unmanaged memory
    ///     before this can occur, resulting in an OutOfMemoryException. This is why it is so important to 
    ///     call Dispose explicitly to ensure unmanaged memory is dealt with in a known, and guaranteed way.
    /// 
    ///     This should be considered a memory leak, but you can observe that by calling GC.Collect, that
    ///     the memory can in fact be freed. This differs from a leak where the reference count for the
    ///     object is greater than 0, and regardless of when the GC runs, the memory will not be freed. This
    ///     should be resolved by ensuring Dispose is called from code, and GC.Collect should never be used
    ///     directly.
    /// </remarks>
    public class BasicStrategy : FileStrategy
    {
        private readonly BasicStrategyMode mode;

        public BasicStrategy(BasicStrategyMode mode)
        {
            this.mode = mode;
        }

        public override IEnumerable<DebugAllocationData> Run()
        {
            for (int i = 0; i < 10; i++)
            {
                long beforeAlloc = Environment.WorkingSet;
                
                BitmapImage image = new BitmapImage((Bitmap)Image.FromFile(FilePath));

                long afterAlloc = Environment.WorkingSet;

                if (mode == BasicStrategyMode.Deterministic)
                {
                    image.Dispose();
                }

                long afterDispose = Environment.WorkingSet;

                yield return new DebugAllocationData(beforeAlloc, afterAlloc, afterDispose);
            }
        }
    }
}