﻿// //-----------------------------------------------------------------------------
// // <copyright file="BitmapImage.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC.Dispose_Pattern_with_Inheritance
{
    using System;
    using System.Diagnostics;
    using System.Drawing;

    public class BitmapImage : ImageBase
    {
        private bool _disposed;

        public BitmapImage(Bitmap image) : base(image)
        {
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{nameof(BitmapImage)}.{nameof(Dispose)}({disposing}) by {(disposing ? "user code" : "garbage collection.")}");
            
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                image.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}