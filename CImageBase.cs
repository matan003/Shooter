using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class CImageBase : IDisposable
    {
        bool disposed = false;

        Bitmap _bitmap;
        private int X; // Koordinat för bitmappen
        private int Y; // Koordinat för bitmappen

        public int Left
        {
            get
            {
                return this.X;
            }
            set
            {
                this.X = value;
            }
        }

        public CImageBase(Bitmap _resource)
        {
            _bitmap = new Bitmap(_resource);
        }

        public void DrawImage(Graphics gfx)
        {
            gfx.DrawImage(_bitmap, X, Y);
        }

        public int Top
        {
            get
            {
                return this.Y;
            }
            set
            {
                this.Y = value;
            }
        }

        public void Dispose() // En metod som kan anropas utanför klassen och som anropar Dispose metoden i den här klassen
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) // En metod för att ta bort bitmaps
        {
            if (disposed)
                return;
            if (disposing)
            {
                _bitmap.Dispose();
            }

            disposed = true;
        }
    }
}
