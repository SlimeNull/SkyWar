using PolygonIntersection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchGameCore.Utils
{
    internal class GameUtils
    {
        public static PointF GamePoint2OriginPoint(Size gameSize, Point gamePoint)
        {
            Point unflipped = gamePoint + gameSize / 2;
            return new PointF(unflipped.X, -unflipped.Y);
        }

        public static PointF OriginPoint2GamePoint(Size gameSize, Point originPoint)
        {
            Point unflipped = originPoint - gameSize / 2;
            return new PointF(unflipped.X, -unflipped.Y);
        }

        public static PointF GamePoint2GdiPoint(PointF point)
        {
            return new PointF(point.X, -point.Y);
        }

        public static PointF GdiPoint2GamePoint(PointF point)
        {
            return new PointF(point.X, -point.Y);
        }

        public static PointF OriginPoint2GdiPoint(Size gameSize, Point originPoint)
        {
            return originPoint - gameSize / 2;
        }

        public static PointF Image2CenterOffset(Size imgSize)
        {
            return new PointF(-(float)imgSize.Width / 2, -(float)imgSize.Height / 2);
        }

        public static RectangleF GetOriginBounds(Game game, GameSprite sprite)
        {
            SizeF originSize = sprite.Sprite != null ? sprite.Sprite.Size * sprite.Scale : SizeF.Empty;
            PointF originPosition = sprite.Position - originSize / 2 + game.Size / 2;
            return new RectangleF(originPosition, originSize);
        }

        public static PointF[] GetSpriteVertexes(GameSprite sprite)
        {
            SizeF size = (sprite.Sprite?.Size ?? Size.Empty) * sprite.Scale;
            PointF position = sprite.Position - size / 2;
            PointF[] rectangle = ImgUtils.RotateAt(new RectangleF(position, size), position + size / 2, ImgUtils.Degree2Radian(sprite.Rotation));
            return rectangle;
        }

        public static bool MouseInSprite(GameSprite sprite, PointF gamePoint)
        {
            using Region region = new Region();
            using GraphicsPath path = new GraphicsPath();

            PointF[] vertexes = GetSpriteVertexes(sprite);

            path.AddPolygon(vertexes);
            region.Union(path);

            return region.IsVisible(gamePoint);
        }

        public static bool SpriteCollided(GameSprite a, GameSprite b)
        {
            PointF[] vertexes1 = GetSpriteVertexes(a);
            PointF[] vertexes2 = GetSpriteVertexes(b);
            Polygon polygon1 = new Polygon(vertexes1);
            Polygon polygon2 = new Polygon(vertexes2);

            return Polygon.Collision(polygon1, polygon2);
        }
    }
}
