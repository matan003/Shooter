#define My_Debug

using Shooter.Properties;
using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooter
{
    public partial class Moleshooter : Form
    {
        const int FrameNum = 8;
        const int ExplosionNum = 3;

        bool explosion = false;

        int _gameFrame = 0;
        int _explosionTime = 0;

        int _hits = 0;
        int _misses = 0;
        int _totalShots = 0;
        double _averageHits = 0;

#if My_Debug
        int _cursX = 0; // Musens X värde
        int _cursY = 0; // Musens Y värde
#endif
        CMole _mole;
        CExplosion _explosion;
        CSign _sign;

        Random rnd = new Random();

        public Moleshooter()
        {
            InitializeComponent();

            Bitmap b = new Bitmap(Resources.crosshair);
            this.Cursor = CustomCursor.CreateCursor(b, b.Height / 2, b.Width / 2);

            _sign = new CSign() { Left = 380, Top = 30 };
            //_score = new CScoreboard() { Left = 10, Top = 30 };
            _explosion = new CExplosion();
            _mole = new CMole() { Left = 10, Top = 200 };
        }

        private void UpdateMole()
        {
            _mole.Update(
                    rnd.Next(Resources.ball.Width, this.Width - Resources.ball.Width),
                    rnd.Next(this.Height / 2, this.Height - Resources.ball.Height * 2)
                );
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timerGameLoop_Tick(object sender, EventArgs e) //Gameloopen
        {
            if(_gameFrame >= FrameNum)
            {
                UpdateMole();
                _gameFrame = 0;
            }
                
            _gameFrame++;
            this.Refresh();

            if (explosion)
            {
                if(_explosionTime >= ExplosionNum)
                {
                    explosion = false;
                    _explosionTime = 0;
                    UpdateMole();
                }
                _explosionTime++;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (explosion)
            {
                _explosion.DrawImage(dc);
            }
            else
            {
                _mole.DrawImage(dc);
            }

            _sign.DrawImage(dc);
            //_score.DrawImage(dc);
            
#if My_Debug
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            //TextRenderer.DrawText(dc, "X=" + _cursX.ToString() + ":" + "Y=" + _cursY.ToString(), _font,
              //  new Rectangle(0, 0, 200, 20), SystemColors.ControlText, flags);
#endif
            TextRenderer.DrawText(e.Graphics, "Shots: " + _totalShots.ToString(), _font, new Rectangle(8, 32, 120, 20), SystemColors.ControlText);
            TextRenderer.DrawText(e.Graphics, "Hits: " + _hits.ToString(), _font, new Rectangle(15, 52, 120, 20), SystemColors.ControlText);
            TextRenderer.DrawText(e.Graphics, "Misses: " + _misses.ToString(), _font, new Rectangle(5, 72, 120, 20), SystemColors.ControlText);
            TextRenderer.DrawText(e.Graphics, "Average: " + Math.Floor(_averageHits).ToString() + '%', _font, new Rectangle(7, 92, 120, 20), SystemColors.ControlText);

            base.OnPaint(e);

        }

        private void Moleshooter_MouseMove(object sender, MouseEventArgs e)
        {
            _cursX = e.X; // 0 582
            _cursY = e.Y; // 255 400

            this.Refresh(); // Uppdatera fönstret
        }

        private void Moleshooter_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.X > 448 && e.X < 502 && e.Y > 60 && e.Y < 76) // Start
            {
                timerGameLoop.Start();
                SkjutPistol();
                _hits = 0;
                _misses = 0;
                _averageHits = 0;
                _totalShots = 0;
            }
            else if (e.X > 400 && e.X < 480 && e.Y > 71 && e.Y < 90) // Stop
            {
                timerGameLoop.Stop();
                SkjutPistol();
            }
            else if (e.X > 400 && e.X < 480 && e.Y > 91 && e.Y < 110) // Reset
            {
                _hits = 0;
                _misses = 0;
                _averageHits = 0;
                _totalShots = 0;
                SkjutPistol();
            }
            else if (e.X > 400 && e.X < 480 && e.Y > 111 && e.Y < 130) // Quit
            {
                timerGameLoop.Stop();
                SkjutPistol();
                Application.Exit();
            }
            else
            {
                if (_mole.Hit(e.X, e.Y) && timerGameLoop.Enabled == true)
                {
                    explosion = true;
                    _explosion.Left = _mole.Left - Resources.explosion.Width / 3;
                    _explosion.Top = _mole.Top - Resources.explosion.Height / 3;

                    _hits++;
                    _averageHits = (double)_hits / (double)_totalShots * 100.0;

                    HitSound();
                }
                else if(timerGameLoop.Enabled == true)
                {
                    _misses++;
                    _totalShots = _hits + _misses;
                    _averageHits = (double)_hits / (double)_totalShots * 100.0;
                    SkjutPistol();
                }

            }

            
        }

        private void SkjutPistol()
        {
            SoundPlayer pistol = new SoundPlayer(Resources.Pistol);

            pistol.Play();
        }

        private void HitSound()
        {
            SoundPlayer hit = new SoundPlayer(Resources.Pop);

            hit.Play();
        }
    }
}
