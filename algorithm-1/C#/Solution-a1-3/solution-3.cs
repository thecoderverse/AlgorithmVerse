// See https://aka.ms/new-console-template for more information


[Flags]
enum Movements
{
    None,
    Top,
    Right,
    Bottom,
    Left,
}
class GameState
{
    int[][] _values;
    int[] _addressOfVoid = new int[2];
    Movements _lastMovement;
    Movements _movements;
    int rowNumber = 2;
    int colNumber = 3;
    public GameState(int[][] initialState)
    {
        if (!ValidateInput(initialState))
        {
            Console.WriteLine("Invalid game state as a starting point!");
            return;
        }
        _values = initialState;
        _getAddressOfVoid();
    }
    public void Start()
    {
        SetAvailableMovements();
    }
    private int _next()
    {

    }
    private bool _isSolved()
    {
        for(int i = 0; i < rowNumber; i++)
        {
            for(int j = 0; j < colNumber - 1; j++)
            {
                bool isEqual = _values[i][j] == i *
                if ()
            }
        }
    }
    private bool ValidateInput(int[][] initialState)
    {
        return true;
    }
    private void SetAvailableMovements()
    {
        int row = _addressOfVoid[0];
        int col = _addressOfVoid[1];
        _movements = Movements.None;
        if(row - 1 > 0)
        {
            _movements = _movements | Movements.Top;
        }
        if (row + 1 < _values.Length)
        {
            _movements = _movements | Movements.Bottom;
        }
        if (col - 1 > 0)
        {
            _movements = _movements | Movements.Left;
        }
        if (col + 1 < _values[0].Length)
        {
            _movements = _movements | Movements.Right;
        }
        _movements = _movements ^ _lastMovement; // do not repeat last movement
    }
    private void _getAddressOfVoid()
    {
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < colNumber; j++)
            {
                if (_values[i][j] == 0)
                {
                    _addressOfVoid[0] = i;
                    _addressOfVoid[1] = j;
                }
            }
        }
    }
}