using Silk.NET.SDL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Silk.NET.Maths;
using TheAdventure;
using TheAdventure.Models;
using System.Numerics;

public unsafe class GameRenderer
{
    private Sdl _sdl;
    private Renderer* _renderer;
    private GameCamera _camera;
    private  GameWindow _window;
    private Rectangle<int> _worldBounds;
    private Dictionary<int,IntPtr> _textures = new();
    private int _id = 0;

    public GameRenderer(Sdl sdl, GameWindow window)
    {
    _sdl = sdl;
    _renderer = (Renderer*)window.CreateRenderer();
    _window = window;
    var size = window.Size;
    _camera = new GameCamera(size.Width, size.Height);
    }

    public int LoadTexture(string file, out TextureData data)
    {
        using var img = Image.Load<Rgba32>(file);
        data = new TextureData { Width = img.Width, Height = img.Height};
        byte[] raw = new byte[data.Width * data.Height * 4];
        img.CopyPixelDataTo(raw);

        fixed(byte* ptr = raw)
        {
            var surf = _sdl.CreateRGBSurfaceWithFormatFrom(ptr, data.Width, data.Height, 32, data.Width*4, (uint)PixelFormatEnum.Rgba32);
            var tex = _sdl.CreateTextureFromSurface(_renderer, surf);
            _sdl.FreeSurface(surf);
            _textures[_id] = (IntPtr)tex;
            return _id++;
        }
    }

    public void RenderTexture(int id, Rectangle<int> src, Rectangle<int> dst)
    {
        if(_textures.TryGetValue(id, out var tex))
        {
             var screen = _camera.ToScreen(dst);
             _sdl.RenderCopy(_renderer, (Texture*)tex, in src, in screen);
        }
    }

     public void RenderTextureUI(int id, Rectangle<int> src, Rectangle<int> dst)
    {
        if(_textures.TryGetValue(id, out var tex))
        {
             _sdl.RenderCopy(_renderer, (Texture*)tex, in src, in dst);
        }
    }

    public (int Width, int Height) GetWindowSize()
    {
        return _window.Size;
    }

    public Rectangle<int> GetWorld()
    {
        return _worldBounds;
    }

    public void Clear() => _sdl.RenderClear(_renderer);
    public void Present() => _sdl.RenderPresent(_renderer);
     public void SetCamera(int x, int y) => _camera.LookAt(x,y);
     public void SetWorld(Rectangle<int> bounds)
    {
        _camera.SetWorldBounds(bounds);
        _worldBounds = bounds;
    }
     public Vector2D<int> ScreenToWorld(int x, int y) => _camera.ToWorld(x,y);

}

