using Silk.NET.Maths;
using TheAdventure.Models;

namespace TheAdventure;

public class GameLogic
{
    private readonly GameRenderer _renderer;
    private HPRenderer _hp = null!;
    private int _bgTexture;
    private Rectangle<int> _bgRect;
    private int _groundY;
    private PlayerObject _player = null!;
    private EnemyObject _enemy = null!;
    private bool _gameOver = false;

    public GameLogic(GameRenderer renderer)
    {
        _renderer = renderer;
    }

    public void Initialize()
    {
        // Loads background
        _bgTexture = _renderer.LoadTexture(@"Assets\2304x1296.png", out var bg);
        _bgRect = new Rectangle<int>(0,0,bg.Width,bg.Height);
        _renderer.SetWorld(new Rectangle<int>(0,0,bg.Width,bg.Height));
        // Loads hp texture 
        _hp = new HPRenderer(_renderer);
        var screen = _renderer.GetWindowSize();
        // Computes ground Y coordonate in relation with background size
        _groundY = screen.Height - 10;
        //Creates player and enemy objects
        _player = new PlayerObject(_renderer, _groundY);
        _enemy = new EnemyObject(_renderer, _groundY);
    }

    public void UpdatePlayer( bool l, bool r, bool u, double dt, bool a)
    {
        _player.Update(l,r,u,dt,a);
    }

    public void RenderFrame(double dt)
    {

        // Cheks if game is finished
        if (_gameOver)
        {
            return;
        }

        var screen = _renderer.GetWindowSize();

        var dst = new Rectangle<int>(0,0,screen.Width,screen.Height);

        // Clears screen and draws background
        _renderer.Clear();
        _renderer.RenderTextureUI(_bgTexture,_bgRect,dst);

        // Updates enemy state
        _enemy.Update(dt);

        // Checks for player and enemy collision
        if(Collides(_player.Dest, _enemy.Dest))
        {
            // Checks if player is attacking
            if (_player.IsAttacking && !_player._didDamage)
            {
                // Updates enemy HP
                _enemy.HP--;
                _player._didDamage = true;
                Console.WriteLine($"Enemy HP: {_enemy.HP}");

                // If enemy gets hit 3 times, enemy dies
                if(_enemy.HP <= 0)
                {
                    _enemy.Die();
                    Console.WriteLine("YOU WIN");
                    _gameOver = true;
                }
            }

            // Checks if enemy is attacking 
            if (_enemy.IsAttacking && !_enemy._didDamage)
            {
                // Updates player HP
                _player.HP--;
                _enemy._didDamage = true;
                Console.WriteLine($"Player HP: {_player.HP}");
                
                // If player gets hit 7 times, player dies
                if(_player.HP <= 0)
                {
                    _player.Die();
                    Console.WriteLine("GAME OVER. YOU LOSE");
                    _gameOver = true;
                }
            }
        }

        // Renders enemy and player
        _enemy.Render(_renderer);
        _player.Render(_renderer);

        // Renders enemy and player hp icons
        _hp.RenderLives(_player, _enemy);
        _renderer.Present();
    }

    // Checks if player and enemy sprites meet at some point
    private bool Collides(Rectangle<int> a, Rectangle<int> b)
    {
        return a.Origin.X < b.Origin.X + b.Size.X &&
               a.Origin.X + a.Size.X > b.Origin.X &&
               a.Origin.Y < b.Origin.Y + b.Size.Y &&
               a.Origin.Y + a.Size.Y > b.Origin.Y;
    }
}