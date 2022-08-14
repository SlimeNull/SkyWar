using ScratchGameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyWar
{
    internal class ConorAwa : GameSprite
    {
        public ConorAwa(Game game) : base(game)
        {

        }

        public GameSprite target;

        private bool pause = false;
        public override async void Update()
        {
            if (pause)
                return;

            pause = true;
            if (Game.IsCollided(this, target))
            {
                for (int i = 0; i < 2; i++)
                {
                    Visible ^= true;
                    await Task.Delay(100);
                }
            }
            pause = false;
        }
    }
}
