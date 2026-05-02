using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Silk.NET.SDL;

namespace TheAdventure;

public class GameWindow
{
    private readonly Sdl _sdl;
    private readonly IntPtr _window;

    public GameWindow(Sdl sdl)
    {
        _sdl = sdl;

         unsafe
        {
            _window = (IntPtr)sdl.CreateWindow(
                "CrowFightsFox", Sdl.WindowposUndefined, Sdl.WindowposUndefined, 711, 400,
                (uint)WindowFlags.Resizable);

        }

        if (_window == IntPtr.Zero)
        {
            var ex = sdl.GetErrorAsException();
            if (ex != null)
            {
                throw ex;
            }

            throw new Exception("Failed to create window.");
        }
    }

    public unsafe IntPtr CreateRenderer(){
        var renderer= (IntPtr)_sdl.CreateRenderer((Window*)_window, -1, 0);
        return renderer;
    }

    public unsafe (int Width, int Height) Size
    {
        get
        {
            int w = 0, h = 0;
            _sdl.GetWindowSize((Window*)_window, ref w, ref h);
            return (w,h);
        }
    }
}