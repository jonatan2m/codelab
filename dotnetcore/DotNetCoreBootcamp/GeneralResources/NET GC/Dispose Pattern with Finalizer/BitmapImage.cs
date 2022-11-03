// //-----------------------------------------------------------------------------
// // <copyright file="BitmapImage.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Dispose_Pattern_with_Finalizer
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    public class BitmapImage : IDisposable
    {
        private bool disposed;
        private readonly IntPtr image;
        private readonly int status;

        public BitmapImage(string filename)
        {
            status = GdipLoadImageFromFile(filename, out image);
            status = GdipImageForceValidation(new HandleRef(null, image));
        }

        ~BitmapImage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Debug.WriteLine($"{nameof(BitmapImage)}.{nameof(Dispose)}({disposing}) by {(disposing ? "user code" : "garbage collection.")}");

            if (disposed)
            {
                return;
            }

            IntGdipDisposeImage(new HandleRef(null, image));
            
            disposed = true;
        }

        [DllImport("gdiplus.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int GdipImageForceValidation(HandleRef image);

        [DllImport("gdiplus.dll", CharSet=CharSet.Unicode)]
        public static extern int GdipLoadImageFromFile(string filename, out IntPtr image);

        [DllImport("gdiplus.dll", SetLastError=true, ExactSpelling=true, EntryPoint="GdipDisposeImage", CharSet=System.Runtime.InteropServices.CharSet.Unicode)] // 3 = Unicode
        [ResourceExposure(ResourceScope.None)]
        private static extern int IntGdipDisposeImage(HandleRef image);
    }
}