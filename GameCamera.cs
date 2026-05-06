using Silk.NET.Maths;

namespace TheAdventure;

public class GameCamera
{
    // current focal point coordonates of the camera in the world
    private int _x;
    private int _y;

    // total game area
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
        // stores game area limits and centers the camera initially
        _bounds = bounds;

        _x = bounds.Size.X / 2;
        _y = bounds.Size.Y / 2;
    }

    public void LookAt(int x, int y)
    {
        // updates camear position withing bounds and centers the camera on player object
        _x = Math.Clamp(x,Width / 2, _bounds.Size.X - (Width / 2));
        _y = Math.Clamp(y,Height / 2, _bounds.Size.Y) - (Height / 2);
    }

    // Centers objects according to camera current focal point
    public Rectangle<int> ToScreen(Rectangle<int> r)
    {
        return r.GetTranslated( new Vector2D<int>(Width / 2 - _x, Height / 2 - _y));
    }

    public Vector2D<int> ToWorld(int x, int y)
    {
         return new Vector2D<int>(x - (Width / 2 - _x), y - (Height / 2 - _y));
    }
}