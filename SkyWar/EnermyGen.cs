using ScratchGameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyWar
{
    internal class EnermyGen : GameSprite
    {
        public EnermyGen(Game game) : base(game)
        {
            EnermyGenLoop();
        }

        private async void EnermyGenLoop()
        {
            while (true)
            {
                await Task.Delay(2000);

            }
        }
    }
}
