using Microsoft.VisualBasic.Devices;
using ScratchGameCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScratchGameCore
{
    public partial class GamePanel : Control, IGameHost
    {
        public GamePanel()
        {
            InitializeComponent();
        }

        public Game? Game { get; set; }
        public Graphics GameGraphics { get => CreateGraphics(); }
        public Rectangle GameBounds => ClientRectangle;
        public Point MouseOriginPoint => PointToClient(Control.MousePosition);

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        readonly Dictionary<Keys, bool> keystates = new Dictionary<Keys, bool>();
        protected override bool ProcessKeyMessage(ref Message m)
        {
            Keys key = (Keys)m.WParam;
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                    keystates[key] = true;
                    Game?.InvokeKeyboard(key);
                    break;
                case WM_KEYUP:
                    keystates[key] = false;
                    break;
            }

            return true;
        }

        private bool mouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Game?.InvokeMouse(GameUtils.OriginPoint2GamePoint(Game.Size, e.Location));

            mouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;
        }

        public void Render()
        {
            OnPaint(new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Game != null)
            {
                Game.Render();
            }

            base.OnPaint(pe);
        }

        public bool IsMouse()
        {
            return mouseDown;
        }

        public bool IsKeyboard(Keys key)
        {
            return keystates.TryGetValue(key, out bool v) ? v : false;
        }
    }
}
