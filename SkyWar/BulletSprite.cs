using ScratchGameCore;
using SkyWar.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyWar
{
    internal class BulletSprite : GameSprite
    {
        static Image bullet1 = Resources.bullet1;
        static Image bullet2 = Resources.bullet2;

        public BulletSprite(Game game) : base(game)
        {
            gameBounds = Game.GameBounds;
            Sprite = bullet1;
        }

        public SizeF Speed { get; set; } = new SizeF(0, 600);

        Rectangle gameBounds;
        public override void Update()
        {
            Position += new SizeF(Speed.Width * Game.DeltaTime, Speed.Height * Game.DeltaTime);

            if (Position.X < gameBounds.Left ||
                Position.Y < gameBounds.Top ||
                Position.X > gameBounds.Right ||
                Position.Y > gameBounds.Bottom)
            {
                Game.Sprites.Remove(this);
            }
        }
    }
}
