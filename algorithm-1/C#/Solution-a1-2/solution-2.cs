// See https://aka.ms/new-console-template for more information


using System.Data;
using System.Text;


Test("[[1,2,3],[4,0,5]]", 1);
Test("[[4,1,2],[5,0,3]]", 5);
Test("[[1,2,3],[5,4,0]]", -1);

static void Test(string input, int expected)
{
    Console.WriteLine("<Test>");
    Game game = new(input);
    int t = game.GetSolution();
    GameState resultState = game.ResultState;
    Console.WriteLine("    last rootState:" + resultState.ToString());
    Console.WriteLine("    " + resultState.Status.ToString());
    Console.WriteLine($"    resolved in {t} steps");
    Console.WriteLine($"    Test result {(t == expected ? "passed" : "failed")}.");
    Console.WriteLine("</Test>");

}

[Flags]
public enum Movements
{
    NONE = 0,
    TOP = 2,
    RIGHT = 4,
    BOTTOM = 8,
    LEFT = 16,
}

public enum StateStatus
{
    IN_PROGRESS,
    FAILED,
    SUCCESS
}
public class GameState
{
    public static int rowNumber = 2;
    public static int colNumber = 3;
    public int[][] Values { get; set; }
    public int[] AddressOfVoid { get; set; } = new int[2];
    public Movements LastMovementFrom { get; set; }
    public List<Movements> moveHist { get; set; }
    public StateStatus Status { get; set; }
    public Movements NextMovements { get; set; }
    /// <summary>
    /// New GameState by the copy of the initialState. 
    /// </summary>
    /// <remarks>
    /// Constructor copies the initialState nested array into its Values property.
    /// Constructor also copies movesHistory into moveHist property.
    /// </remarks>
    /// <param name="initialState"></param>
    /// <param name="lastMovementFrom"></param>
    public GameState(int[][] initialState, Movements lastMovementFrom, List<Movements> movesHistory)
    {
        Status = StateStatus.IN_PROGRESS;
        Values = new int[initialState.Length][];
        LastMovementFrom = lastMovementFrom;
        moveHist = new();
        // copy initialState
        int rowIndex = 0;
        foreach (int[] row in initialState)
        {
            Values[rowIndex] = new int[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                Values[rowIndex][i] = row[i];
            }
            rowIndex++;
        }
        foreach (Movements move in movesHistory)
        {
            moveHist.Add(move);
        }
        _getAddressOfVoid();
    }
    public void Move(Movements movement)
    {
        int row = AddressOfVoid[0];
        int col = AddressOfVoid[1];
        switch (movement)
        {
            case Movements.LEFT:
                Values[row][col] = Values[row][col - 1];
                Values[row][col - 1] = 0;
                --AddressOfVoid[1];
                LastMovementFrom = Movements.RIGHT;
                break;
            case Movements.TOP:
                Values[row][col] = Values[row - 1][col];
                Values[row - 1][col] = 0;
                --AddressOfVoid[0];
                LastMovementFrom = Movements.BOTTOM;
                break;
            case Movements.BOTTOM:
                Values[row][col] = Values[row + 1][col];
                Values[row + 1][col] = 0;
                ++AddressOfVoid[0];
                LastMovementFrom = Movements.TOP;
                break;

            case Movements.RIGHT:
                Values[row][col] = Values[row][col + 1];
                Values[row][col + 1] = 0;
                ++AddressOfVoid[1];
                LastMovementFrom = Movements.LEFT;
                break;
        }
        moveHist.Add(movement);
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("[");
        bool isFirstRow = true;
        foreach (int[] row in Values)
        {
            if (isFirstRow)
            {
                sb.Append("[");
            }
            else
            {
                sb.Append(",");
                sb.Append("[");
            }
            isFirstRow = true;
            bool isFirstNum = true;
            foreach (int val in row)
            {

                if (isFirstNum)
                {
                    sb.Append(val);
                }
                else
                {
                    sb.Append(",");
                    sb.Append(val);
                }
                isFirstNum = false;
            }
            sb.Append("]");
        }
        sb.Append("]");
        return sb.ToString();
    }

    public void SetAvailableMovements()
    {
        NextMovements = Movements.NONE;
        int row = AddressOfVoid[0];
        int col = AddressOfVoid[1];
        if (row - 1 >= 0)
        {
            NextMovements |= Movements.TOP;
        }
        if (row + 1 < rowNumber)
        {
            NextMovements |= Movements.BOTTOM;
        }
        if (col - 1 >= 0)
        {
            NextMovements |= Movements.LEFT;
        }
        if (col + 1 < colNumber)
        {
            NextMovements |= Movements.RIGHT;
        }
        NextMovements ^= LastMovementFrom; // do not repeat last movement
    }
    private void _getAddressOfVoid()
    {
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < colNumber; j++)
            {
                if (Values[i][j] == 0)
                {
                    AddressOfVoid[0] = i;
                    AddressOfVoid[1] = j;
                }
            }
        }
    }
}
class Game
{
    int rowNumber;
    int colNumber;
    bool inProgress;
    GameState _initState;
    List<GameState> statesOnRoute = new();
    public GameState ResultState { get; private set; }

    public Dictionary<string, int> stateHist { get; set; }

    /// <summary>
    /// init state as "n,n,n/n,n,n" or "[[n,n,n],[n,n,n]]"
    /// </summary>
    /// <param name="initState"></param>
    public Game(string initState)
    {
        int[][] initialState;
        initState = initState.Replace("[[", "");
        initState = initState.Replace("]]", "");
        initState = initState.Replace("],[", "/");
        string[] rows = initState.Split("/");
        initialState = new int[rows.Length][];
        try
        {
            int i = 0;
            foreach (string row in rows)
            {
                string[] cols = row.Split(",");
                initialState[i] = new int[cols.Length];
                int j = 0;
                foreach (string col in cols)
                {
                    initialState[i][j] = int.Parse(col);
                    j++;
                }
                i++;
            }
            SetState(initialState);
        }
        catch (Exception e)
        {
            Console.WriteLine("<!> Invalid string format for initial rootState <!>");
            Console.WriteLine(e.Message);
        }
    }
    public Game(int[][] initialState)
    {
        SetState(initialState);
    }
    public void SetState(int[][] initialState)
    {
        rowNumber = GameState.rowNumber;
        colNumber = GameState.colNumber;
        if (!ValidateInput(initialState))
        {
            Console.WriteLine("Invalid game rootState as a starting point!");
            return;
        }
        _initState = new GameState(initialState, Movements.NONE, new List<Movements>());
        stateHist = new();
    }
    public int GetSolution()
    {
        return GetSolution(_initState);
    }
    private bool _dropAndCheckProgress(GameState state)
    {
        if(statesOnRoute.Count > 1)
        {
            statesOnRoute.Remove(state);
        }
        else
        {
            return true;
        }
        return false;
    }
    private int GetSolution(GameState rootState)
    {
        statesOnRoute.Clear();
        statesOnRoute.Add(rootState);
        inProgress = true;
        MovementMatrtix movementMat = new();
        int t = 0;
        while (inProgress)
        {
            List<GameState> newStates = new List<GameState>();
            for (int i = 0; i < statesOnRoute.Count; i++)
            {
                GameState state = statesOnRoute[i];
                string stateToken = state.ToString();
                if (!stateHist.TryAdd(stateToken, t))
                {
                    state.Status = StateStatus.FAILED;
                    stateHist[stateToken] = t;
                    // remove failed states from list of states on route to solution.
                    bool progressFailed = _dropAndCheckProgress(state);
                    if (progressFailed)
                    {
                        ResultState = state;
                        inProgress = false;
                        return -1;
                    }
                    continue;
                }
                if (_isSolved(state))
                {
                    state.Status = StateStatus.SUCCESS;
                    // we were looking the shortest solution, so return the first solution.
                    inProgress = false;
                    ResultState = state;
                    return t;
                }
                movementMat.Clear();
                state.SetAvailableMovements();
                bool createNewRout = false;
                CheckMovement(state, Movements.TOP, newStates, ref createNewRout, ref movementMat);
                CheckMovement(state, Movements.BOTTOM, newStates, ref createNewRout, ref movementMat);
                CheckMovement(state, Movements.RIGHT, newStates, ref createNewRout, ref movementMat);
                CheckMovement(state, Movements.LEFT, newStates, ref createNewRout, ref movementMat);

                movementMat.MoveAll();
            }
            foreach (GameState state in newStates)
            {
                statesOnRoute.Add(state);
            }
            newStates.Clear();
            t++;
        }
        return t;
    }
    private void CheckMovement(GameState state, Movements movement, List<GameState> nextStates, ref bool createNewState, ref MovementMatrtix matrix)
    {
        // check availablity
        bool isMovementAvailable = state.NextMovements.HasFlag(movement);
        if (!isMovementAvailable) return;
        // we return early if this movement is not available
        GameState _state;
        if (createNewState)
        {
            _state = new GameState(state.Values, state.LastMovementFrom, state.moveHist);
            nextStates.Add(_state);
        }
        else
        {
            _state = state;
        }
        matrix.Add(_state, movement);
        createNewState = true;
    }
    private bool _isSolved(GameState state)
    {
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < colNumber; j++)
            {
                int a = state.Values[i][j];
                int b = (i * colNumber + j + 1) % (rowNumber * colNumber);
                bool isNotEqual = a != b;
                if (isNotEqual)
                {
                    return false;
                }
            }
        }
        return true;
    }
    class MovementMatrtix
    {
        private Dictionary<GameState, Movements> _statesToMove;
        public MovementMatrtix()
        {
            _statesToMove = new();
        }
        public void Add(GameState state, Movements movement)
        {
            _statesToMove.Add(state, movement);
        }
        public void Clear()
        {
            _statesToMove.Clear();
        }
        public void MoveAll()
        {
            foreach(var move in _statesToMove)
            {
                move.Key.Move(move.Value);
            }
        }
    }
        private bool ValidateInput(int[][] initialState)
    {
        return true;
    }
}