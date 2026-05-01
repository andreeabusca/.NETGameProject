using Silk.NET.Maths;

namespace TheAdventure;

public class GameCamera
{
    private int _x;
    private int _y;
    private Rectangle<int> _bounds;
    public readonly int Width;
    public readonly int Height;

    public GameCamera(int w, int h)
    {
        Width = w;
        Height = h;
    }

    public void SetWorldBounds(Rectangle<int> bounds)
    {
        _bounds = bounds;
        _x = bounds.Size.X / 2;
        _y = bounds.Size.Y / 2;
    }

    public void LookAt(int x, int y)
    {
        _x = Math.Clamp(x,Width / 2, _bounds.Size.X - (Width / 2));
        _y = Math.Clamp(y,Height / 2, _bounds.Size.Y) - (Height / 2);
    }

    public Rectangle<int> ToScreen(Rectangle<int> r)
    {
        return r.GetTranslated( new Vector2D<int>(Width / 2 - _x, Height / 2 - _y));
    }

    public Vector2D<int> ToWorld(int x, int y)
    {
         return new Vector2D<int>(x - (Width / 2 - _x), y - (Height / 2 - _y));
    }
}