namespace ScratchGameCore
{
    public interface IGameHost
    {
        public Graphics GameGraphics { get; }
        public Rectangle GameBounds { get; }

        public bool IsMouse();
        public bool IsKeyboard(Keys key);
        public Point MouseOriginPoint { get; }
    }
}
