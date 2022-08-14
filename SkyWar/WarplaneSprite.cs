using ScratchGameCore;
using SkyWar.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SkyWar
{
    internal class WarplaneSprite : GameSprite
    {
        static Bitmap me1 = Resources.me1;
        static Bitmap me2 = Resources.me2;

        public WarplaneSprite(Game game) : base(game)
        {
            SpriteChangeLoop();
            SuperLoop();
        }

        private async void SpriteChangeLoop()
        {
            Bitmap[] mes = new Bitmap[] { me1, me2 };
            while (true)
            {
                foreach (var me in mes)
                {
                    await Task.Delay(100);
                    Sprite = me;
                }
            }
        }

        public async void SuperLoop()
        {
            float degreeAngle = 0;
            while (true)
            {
                await Task.Delay(50);
                degreeAngle += 20;
                degreeAngle %= 360;
                while (Game.IsMouse())
                {
                    var _missile = new BulletSprite(Game)
                    {
                        Position = Position,
                    };

                    float radian = degreeAngle * MathF.PI / 180;
                    float speed = MathF.Sqrt(_missile.Speed.Width * _missile.Speed.Width + _missile.Speed.Height * _missile.Speed.Height);
                    SizeF newSpeed = new SizeF(MathF.Cos(radian) * speed, MathF.Sin(radian) * speed);
                    _missile.Speed = newSpeed;

                    Game.Sprites.Add(_missile);

                    await Task.Delay(50);
                    degreeAngle += 20;
                    degreeAngle %= 360;
                }
            }
        }

        Image missile = Image.FromFile(@"C:\Users\slime\Pictures\english.png");

        public override async void InvokeMouse()
        {
            base.InvokeMouse();
        }

        public override async void InvokeKeyboard(Keys key)
        {
            base.InvokeKeyboard(key);

            Game.Sprites.Add(new BulletSprite(Game)
            {
                Position = Position,
            });
        }

        bool pause = false;

        public override void Update()
        {
            if (pause)
                return;

            var offset = new SizeF(Game.MousePosition) - new SizeF(Position);
            SizeF sizeF = offset / 2;
            float offsetX = sizeF.Width;
            Rotation = MathF.Sign(offsetX) * MathF.Log(MathF.Abs(offsetX) + 1) * 3;
            Position += sizeF;

            base.Update();
        }
    }
}
