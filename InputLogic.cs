using Silk.NET.SDL;

namespace TheAdventure;

public class InputLogic
{
    private readonly Sdl _sdl;
    private readonly GameLogic _logic;

    // Time of last frame
    private DateTime _last = DateTime.Now;

    public InputLogic(Sdl sdl, GameLogic logic)
    {
        _sdl = sdl;
        _logic = logic;
    }

    public unsafe bool Process()
    {
        Event ev = new();

        while(_sdl.PollEvent(ref ev) != 0)
        {
            if(ev.Type == (uint)EventType.Quit)
            {
                return true; // Exits game
            }

        }

        var now = DateTime.Now;
        double dt = (now - _last).TotalMilliseconds;
        _last = now;

        // Checks keyboard input
        var keys = _sdl.GetKeyboardState(null);
        bool left = keys[(int)KeyCode.Left] == 1 ? true : false;
        bool right = keys[(int)KeyCode.Right] == 1 ? true : false;
        bool up = keys[(int)KeyCode.Up] == 1 ? true : false;
        bool attack = keys[(int)KeyCode.Tab] == 1 ? true : false;

        // Updates player so that movement and animation are updated 
        _logic.UpdatePlayer(left,right,up,dt,attack);

        return false; // Continues game

    }
}