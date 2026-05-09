using Silk.NET.SDL;
using TheAdventure;
// Entry point of the game
var sdl = new Sdl(new TheAdventure.SdlContext());
sdl.Init(Sdl.InitVideo);

// Initializes everything and runs game loop
var window = new GameWindow(sdl);
var renderer = new GameRenderer(sdl, window);
var logic = new GameLogic(renderer);
var input = new InputLogic(sdl, logic);

logic.Initialize();

bool quit = false;

while (!quit)
{
    quit = input.Process(); // updates input
    logic.RenderFrame(16); // updates and renders
    System.Threading.Thread.Sleep(13); // limits FPS
}
renderer.Dispose(); // destroys all textures before closing game
sdl.Quit();


