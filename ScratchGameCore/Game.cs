using ScratchGameCore.Utils;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ScratchGameCore
{
    /// <summary>
    /// 游戏对象
    /// 游戏中坐标系与屏幕坐标系不同. 中间为 0, y 向上为正.
    /// </summary>
    public class Game
    {
        public Game(IGameHost host, int width, int height)
        {
            this.host = host;
            this.width = width;
            this.height = height;
            this.Size = new Size(Width, Height);
            GameBounds = new Rectangle(new Point(-width / 2, -height / 2), Size);

            Sprites = new GameSpriteCollection(this);

            bufferedGraphics = BufferedGraphicsManager.Current.Allocate(host.GameGraphics, new Rectangle(0, 0, width, height));
            bufferedGraphics.Graphics.Transform = new Matrix(1, 0, 0, 1, (float)width / 2, (float)height / 2);
        }

        private readonly IGameHost host;
        private readonly int width;
        private readonly int height;

        private BufferedGraphics bufferedGraphics;

        #region BASIC
        public IGameHost Host => host;
        public int Width => width;
        public int Height => height;
        public Size Size { get; }
        public Rectangle GameBounds { get; }
        public Graphics Graphics => bufferedGraphics.Graphics;
        #endregion

        #region Appearence
        public Color BackgroundColor { get; set; } = Color.White;
        public Image? Background { get; set; }
        #endregion

        public GameSpriteCollection Sprites { get; }

        #region Data helper
        private DateTime? lastFrameTime = null;
        public float DeltaTime { get; private set; }
        #endregion

        #region Performence optimization
        private int backgroundOriginSize;
        private Bitmap? processedBackground;
        #endregion

        public bool IsMouse() => host.IsMouse();

        public bool IsKeyboard(Keys key) => host.IsKeyboard(key);

        public PointF MousePosition => GameUtils.OriginPoint2GamePoint(Size, host.MouseOriginPoint);

        public bool IsCollided(GameSprite a, GameSprite b)
        {
            return GameUtils.SpriteCollided(a, b);
        }

        public void InvokeMouse(PointF point)
        {
            foreach (GameSprite sprite in Sprites)
            {
                if (GameUtils.MouseInSprite(sprite, point))
                    sprite.InvokeMouse();
            }
        }

        public void InvokeKeyboard(Keys key)
        {
            foreach (GameSprite sprite in Sprites)
                sprite.InvokeKeyboard(key);
        }

        public void Render()
        {
            bufferedGraphics.Graphics.Clear(BackgroundColor);

            if (Background != null)
            {

                RectangleF bgarea = ImgUtils.UniformToFill(new RectangleF(GameUtils.OriginPoint2GdiPoint(Size, Point.Empty), Size), Background.Size);
                //bufferedGraphics.Graphics.DrawImage(Background, bgarea);
            }

            foreach (GameSprite sprite in Sprites)
            {
                sprite.Render();
            }

            bufferedGraphics.Render();
        }

        /// <summary>
        /// 主循环中每次循环都会执行的操作
        /// </summary>
        public void MainLoopAction()
        {
            if (lastFrameTime == null)
                lastFrameTime = DateTime.Now;

            DeltaTime = (float)(DateTime.Now - lastFrameTime.Value).TotalSeconds;

            foreach (GameSprite sprite in Sprites)
                sprite.Update();

            lastFrameTime = DateTime.Now;
        }

        /// <summary>
        /// 执行主循环
        /// </summary>
        public void MainLoop()
        {
            while (true)
            {
                MainLoopAction();
            }
        }

        public class GameSpriteCollection : ICollection<GameSprite>
        {
            private Game game;
            private readonly List<GameSprite> list = new List<GameSprite>();
            private readonly List<GameSprite> buffer = new List<GameSprite>();

            public int Count => list.Count;
            public bool IsReadOnly => false;

            public GameSpriteCollection(Game game)
            {
                this.game = game;
            }

            public void Add(GameSprite item)
            {
                list.Add(item);
                item.Start();
            }

            public void Clear()
            {
                list.Clear();
            }
            public bool Remove(GameSprite item)
            {
                return list.Remove(item);
            }

            public bool Contains(GameSprite item) => list.Contains(item);
            public void CopyTo(GameSprite[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

            public IEnumerator<GameSprite> GetEnumerator()
            {
                buffer.Clear();
                buffer.AddRange(list);
                return buffer.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
