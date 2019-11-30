using Shooter.Properties;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class CMole : CImageBase
    {
        private Rectangle _moleHotSpot = new Rectangle();
        
        public CMole()
            : base(Resources.ball)
        {
            _moleHotSpot.X = Left + 20;
            _moleHotSpot.Y = Top - 1;
            _moleHotSpot.Width = 30;
            _moleHotSpot.Height = 40;
            
        }

        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            _moleHotSpot.X = Left + 20;
            _moleHotSpot.Y = Top - 1;
        }

        public bool Hit(int X, int Y)
        {
            if (X >= _moleHotSpot.X-19 && X <= _moleHotSpot.X + 25 && Y >= _moleHotSpot.Y && Y <= _moleHotSpot.Y+36)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }
}
