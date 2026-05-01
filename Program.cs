using Silk.NET.SDL;
using TheAdventure;

var sdl = new Sdl(new TheAdventure.SdlContext());
sdl.Init(Sdl.InitVideo);

var window = new GameWindow(sdl);
var renderer = new GameRenderer(sdl, window);
var logic = new GameLogic(renderer);
var input = new InputLogic(sdl, logic);

logic.Initialize();

bool quit = false;

while (!quit)
{
    quit = input.Process();
    logic.RenderFrame(16);
    System.Threading.Thread.Sleep(13);
}
sdl.Quit();


