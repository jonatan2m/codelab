namespace NET_GC.Dispose_Pattern_with_SafeHandle
{
    using System;
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