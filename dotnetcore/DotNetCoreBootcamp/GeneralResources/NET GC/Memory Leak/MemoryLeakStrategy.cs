// //-----------------------------------------------------------------------------
// // <copyright file="MemoryLeakStrategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Memory_Leak
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;

    /// <summary>
    /// Demonstrates the basic IDisposable pattern, in its simplest form.
    /// </summary>
    /// <remarks>
    ///   This sample creates bitmap images in a loop and adds them to a static collection. Because the static
    ///   collection will hold a reference to each bitmap, the GC won't collect them unless the developer
    ///   manually calls Dispose. The sample ensures that Dispose is never called or marked for collection,
    ///   and because of the reference held in the static collection, memory will not be freed until the
    ///   process terminates, or until the reference is removed allowing the GC to determine that no other
    ///   references are held and the objects are elidgable for collection.
    /// 
    ///   The reference could be removed by removing the bitmap objects from the collection. Remember that the
    ///   GC is based on reference counting, so only once the ref count is 0 will the objects become elidgable
    ///   for collection.
    /// </remarks>
    public class MemoryLeakStrategy : FileStrategy
    {
        private static List<BitmapImage> images = new List<BitmapImage>();

        public static void ZeroRefCount()
        {
            int count = images.Count;
            images.Clear();

            if (count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write(count);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" references to BitmapImage removed.");
            Console.WriteLine();
        }

        public override IEnumerable<DebugAllocationData> Run()
        {
            for (int i = 0; i < 10; i++)
            {
                long beforeAlloc = Environment.WorkingSet;

                Bitmap bitmap = (Bitmap)Image.FromFile(FilePath);
                BitmapImage image = new BitmapImage(bitmap);

                long afterAlloc = Environment.WorkingSet;

                try
                {
                    images.Add(image);
                }
                finally
                {
                    // bitmap.Dispose();
                    // image.Dispose();
                }
                
                long afterDispose = Environment.WorkingSet;

                yield return new DebugAllocationData(beforeAlloc, afterAlloc, afterDispose);
            }
        }
    }
}