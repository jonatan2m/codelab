// //-----------------------------------------------------------------------------
// // <copyright file="FinalizerStrategy.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Dispose_Pattern_with_Finalizer
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    public class FinalizerStrategy : FileStrategy
    {
        private bool isInitialized;
        private IntPtr initToken;

        public override IEnumerable<DebugAllocationData> Run()
        {
            if (!isInitialized)
            {
                var input = StartupInput.GetDefault();
                GdiplusStartup(out initToken, ref input, out var output);
                AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                {
                    GdiplusShutdown(new HandleRef(null, initToken));
                };
                isInitialized = true;
            }
            
            for (int i = 0; i < 10; i++)
            {
                long beforeAlloc = Environment.WorkingSet;
                
                BitmapImage image = new BitmapImage(FilePath);

                long afterAlloc = Environment.WorkingSet;
                
                long afterDispose = Environment.WorkingSet;

                yield return new DebugAllocationData(beforeAlloc, afterAlloc, afterDispose);
            }
        }

        

        [DllImport("gdiplus.dll", SetLastError=true, ExactSpelling=true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern int GdiplusStartup(out IntPtr token, ref StartupInput input, out StartupOutput output);

        [DllImport("gdiplus.dll", SetLastError=true, ExactSpelling=true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]

        private static extern void GdiplusShutdown(HandleRef token);

        private struct StartupInput
        {
            public int GdiplusVersion;
            public IntPtr DebugEventCallback;
            public bool SuppressBackgroundThread;
            public bool SuppressExternalCodecs;

            public static StartupInput GetDefault()
            {
                return new StartupInput
                       {
                           GdiplusVersion = 1,
                           SuppressBackgroundThread = false,
                           SuppressExternalCodecs = false
                       };
            }
        }

        private struct StartupOutput
        {
            public IntPtr hook;
            public IntPtr unhook;
        }
    }
}