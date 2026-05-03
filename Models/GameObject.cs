namespace TheAdventure.Models;

// Base class for game objects
// Gives every object a unique id
public class GameObject
{
    public int Id { get; private set; }

    private static int _nextId = -1;

    public GameObject()
    {
        Id = System.Threading.Interlocked.Increment(ref _nextId);
    }
}