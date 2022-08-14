using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchGameCore
{
    public class GameScene : ICollection<GameSprite>
    {
        private readonly Game game;
        private readonly List<GameSprite> sprites = new List<GameSprite>();

        public Image Background { get; set; }
        public List<GameSprite> Sprites => sprites;

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public GameScene(Game game)
        {
            this.game = game;
        }

        public void Add(GameSprite item) => sprites.Add(item);
        public void Clear() => sprites.Clear();
        public bool Contains(GameSprite item) => sprites.Contains(item);
        public void CopyTo(GameSprite[] array, int arrayIndex) => sprites.CopyTo(array, arrayIndex);
        public bool Remove(GameSprite item) => sprites.Remove(item);
        public IEnumerator<GameSprite> GetEnumerator() => sprites.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
