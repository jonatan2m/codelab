namespace NET_GC
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    public class GdiBitmapSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public GdiBitmapSafeHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }
        
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        protected override bool ReleaseHandle()
        {
            return DeleteObject(handle);
        }
    }
}