// //-----------------------------------------------------------------------------
// // <copyright file="InheritanceStrategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Dispose_Pattern_with_Inheritance
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class InheritanceStrategy : FileStrategy
    {
        public override IEnumerable<DebugAllocationData> Run()
        {
            for (int i = 0; i < 10; i++)
            {
                long beforeAlloc = Environment.WorkingSet;
                
                BitmapImage image = new BitmapImage((Bitmap)Image.FromFile(FilePath));

                long afterAlloc = Environment.WorkingSet;

                image.Dispose();

                long afterDispose = Environment.WorkingSet;

                yield return new DebugAllocationData(beforeAlloc, afterAlloc, afterDispose);
            }
        }
    }
}