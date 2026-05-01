using Silk.NET.Maths;
using TheAdventure.Models;

namespace TheAdventure;

public class GameLogic
{
    private readonly GameRenderer _renderer;
    private Dictionary<int, GameObject> _objects = new();
    private PlayerObject _player = null!;
    private KnightObject _knight = null!;
    private int _bgTexture;
    private Rectangle<int> _bgRect;
    private int _groundY;

    public GameLogic(GameRenderer renderer)
    {
        _renderer = renderer;
    }

    public void Initialize()
    {
        _bgTexture = _renderer.LoadTexture(@"Assets\2304x1296.png", out var bg);
        _bgRect = new Rectangle<int>(0,0,bg.Width,bg.Height);
        _renderer.SetWorld(new Rectangle<int>(0,0,bg.Width,bg.Height));
        var screen = _renderer.GetWindowSize();
        _groundY = screen.Height - 100;
        _player = new PlayerObject(_renderer, _groundY);
        _knight = new KnightObject(_renderer, _groundY);
    }

    public void UpdatePlayer( bool l, bool r, double dt, bool a)
    {
        _player.Update(l,r,dt,a);
    }

    public void RenderFrame(double dt)
    {
        var screen = _renderer.GetWindowSize();
        var dst = new Rectangle<int>(0,0,screen.Width,screen.Height);
        _renderer.Clear();
         //_renderer.SetCamera(_player.X,_player.Y);
        _renderer.RenderTextureUI(_bgTexture,_bgRect,dst);

        List<int> remove = new();

        foreach(var obj in _objects.Values.OfType<RenderableGameObject>())
        {
            if (!obj.Update(dt))
            {
                remove.Add(obj.Id);
            }
            else
            {
                obj.Render(_renderer);
            }
        }
        foreach (var id in remove)
        {
            _objects.Remove(id);
        }

        _knight.Update(dt);
        if(Collides(_player.Dest, _knight.Dest))
        {
            if (_player.IsAttacking)
            {
                _knight.HP--;
            }

            if (_knight.IsAttacking)
            {
                _player.HP--;
            }
        }

        if(_player.HP <= 0)
        {
            Console.WriteLine("GAME OVER");
        }

        if(_knight.HP <= 0)
        {
            Console.WriteLine("YOU WIN");
        }

        _knight.Render(_renderer);
        _player.Render(_renderer);
        _renderer.Present();
    }

    private bool Collides(Rectangle<int> a, Rectangle<int> b)
    {
        return a.Origin.X < b.Origin.X + b.Size.X &&
               a.Origin.X + a.Size.X > b.Origin.X &&
               a.Origin.Y < b.Origin.Y + b.Size.Y &&
               a.Origin.Y + a.Size.Y > b.Origin.Y;
    }
}