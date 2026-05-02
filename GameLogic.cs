using Silk.NET.Maths;
using TheAdventure.Models;

namespace TheAdventure;

public class GameLogic
{
    private readonly GameRenderer _renderer;
    private Dictionary<int, GameObject> _objects = new();
    private PlayerObject _player = null!;
    private EnemyObject _enemy = null!;
    private bool _gameOver = false;
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
        _groundY = screen.Height - 10;
        _player = new PlayerObject(_renderer, _groundY);
        _enemy = new EnemyObject(_renderer, _groundY);
    }

    public void UpdatePlayer( bool l, bool r, bool u, double dt, bool a)
    {
        _player.Update(l,r,u,dt,a);
    }

    public void RenderFrame(double dt)
    {

        if (_gameOver)
        {
            return;
        }
        var screen = _renderer.GetWindowSize();
        var dst = new Rectangle<int>(0,0,screen.Width,screen.Height);
        _renderer.Clear();
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

        _enemy.Update(dt);
        if(Collides(_player.Dest, _enemy.Dest))
        {
            if (_player.IsAttacking && !_player._didDamage)
            {
                _enemy.HP--;
                _player._didDamage = true;
                Console.WriteLine($"Enemy HP: {_enemy.HP}");
                if(_enemy.HP <= 0)
                {
                    _enemy.Die();
                    Console.WriteLine("YOU WIN");
                    _gameOver = true;
                }
            }

            if (_enemy.IsAttacking && !_enemy._didDamage)
            {
                _player.HP--;
                _enemy._didDamage = true;
                Console.WriteLine($"Player HP: {_player.HP}");
                if(_player.HP <= 0)
                {
                    _player.Die();
                    Console.WriteLine("GAME OVER. YOU LOSE");
                    _gameOver = true;
                }
            }
        }


        _enemy.Render(_renderer);
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