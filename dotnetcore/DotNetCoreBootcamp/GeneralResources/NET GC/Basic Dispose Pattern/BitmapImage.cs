// //-----------------------------------------------------------------------------
// // <copyright file="BitmapImage.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Basic_Dispose_Pattern
{
    using System;
    using System.Diagnostics;
    using System.Drawing;

    public class BitmapImage : IDisposable
    {
        private bool disposed;
        private readonly Bitmap image;

        public BitmapImage(Bitmap image)
        {
            this.image = image;
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

            if (disposing)
            {
                image.Dispose();
            }

            disposed = true;
        }
    }
}