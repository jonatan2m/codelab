namespace NET_GC
{
    using System;
    using System.Drawing;

    public class UnmanagedBitmap : IDisposable
    {
        private bool disposed;
        private readonly GdiBitmapSafeHandle handle;
        private readonly Bitmap image;

        public UnmanagedBitmap(string file)
        {
            image = (Bitmap) Image.FromFile(file);
            handle = new GdiBitmapSafeHandle(image.GetHbitmap());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                image.Dispose();

                if (!handle.IsInvalid)
                {
                    handle.Dispose();
                }
            }

            disposed = true;
        }
    }
}