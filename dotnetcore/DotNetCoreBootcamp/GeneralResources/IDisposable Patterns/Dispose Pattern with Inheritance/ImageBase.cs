// //-----------------------------------------------------------------------------
// // <copyright file="ImageBase.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Dispose_Pattern_with_Inheritance
{
    using System;
    using System.Diagnostics;
    using System.Drawing;

    public abstract class ImageBase : IDisposable
    {
        private bool disposed;
        protected readonly Image image;

        public ImageBase(Image image)
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
            Debug.WriteLine($"{nameof(ImageBase)}.{nameof(Dispose)}({disposing}) by {(disposing ? "user code" : "garbage collection.")}");
            
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