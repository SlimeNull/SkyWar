using ScratchGameCore.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchGameCore
{
    public class GameSprite
    {
        public GameSprite(Game game)
        {
            this.game = game;
        }

        private readonly Game game;

        public Game Game => game;
        public PointF Position { get; set; }
        public float Scale { get; set; } = 1;
        public float Rotation { get; set; }
        public bool Visible { get; set; } = true;
        public Image? Sprite { get; set; }

        private float scaledSpriteRatio = 0;
        private float rotatedSpriteAngle = 0;
        private Bitmap? processedSprite;

        private Action? mouseActions;
        private Dictionary<Keys, Action> keyboardActions = new Dictionary<Keys, Action>();


        public virtual void InvokeMouse()
        {
            mouseActions?.Invoke();
        }

        public virtual void InvokeKeyboard(Keys key)
        {
            if (keyboardActions.TryGetValue(key, out var action))
            {
                action.Invoke();
            }
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public GameSprite OnMouse(Action action)
        {
            if (mouseActions == null)
            {
                mouseActions = action;
            }
            else
            {
                mouseActions += action;
            }

            return this;
        }

        public GameSprite OnKeyboard(Keys key, Action action)
        {

            return this;
        }

        public virtual void Render()
        {
            if (Sprite == null || !Visible)
                return;

            PointF gdilocation = GameUtils.GamePoint2GdiPoint(Position);
            if (Rotation != 0 || Scale != 1)
            {
                if (rotatedSpriteAngle != Rotation || scaledSpriteRatio != Scale || processedSprite == null)
                {
                    processedSprite?.Dispose();

                    rotatedSpriteAngle = Rotation;
                    scaledSpriteRatio = Scale;

                    Size rotatedSpriteSize = ImgUtils.Rotate(Sprite.Size, Rotation);
                    processedSprite = new Bitmap((int)(rotatedSpriteSize.Width * Scale), (int)(rotatedSpriteSize.Height * Scale), PixelFormat.Format32bppArgb);
                    GdiUtils.DrawImage(processedSprite, Sprite, Scale, Rotation);
                }

                SizeF size = (SizeF)processedSprite.Size;
                PointF originLocation = new PointF(gdilocation.X - size.Width / 2, gdilocation.Y - size.Height / 2);
                game.Graphics.DrawImageUnscaled(processedSprite, (int)originLocation.X, (int)originLocation.Y);
            }
            else
            {
                SizeF size = (SizeF)Sprite.Size;
                PointF originLocation = new PointF(gdilocation.X - size.Width / 2, gdilocation.Y - size.Height / 2);
                game.Graphics.DrawImageUnscaled(Sprite, (int)originLocation.X, (int)originLocation.Y);
            }
        }
    }
}
