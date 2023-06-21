// See https://aka.ms/new-console-template for more information


using System.Text;


Test1();
Test2();
Test3();

static void Test1()
{
    Console.WriteLine("==============");
    Console.WriteLine("Test2");
    Console.WriteLine("_____");
    int reqStepCount = 1;
    Game game_2 = new("[[1,2,3],[4,0,5]]");
    GameState resolvedState = game_2.GetSolution();
    Console.WriteLine(resolvedState.ToString());
    Console.WriteLine(resolvedState.Status.ToString());
    Console.WriteLine($"resolved in {resolvedState.StepCount} steps");
    Console.WriteLine($"Test result {(resolvedState.StepCount == reqStepCount ? "passed" : "failed")}.");
}

static void Test2()
{
    Console.WriteLine("==============");
    Console.WriteLine("Test2");
    Console.WriteLine("_____");
    int reqStepCount = 5;
    Game game_2 = new("[[4,1,2],[5,0,3]]");
    GameState resolvedState = game_2.GetSolution();
    Console.WriteLine(resolvedState.ToString());
    Console.WriteLine(resolvedState.Status.ToString());
    Console.WriteLine($"resolved in {resolvedState.StepCount} steps");
    Console.WriteLine($"Test result {(resolvedState.StepCount == reqStepCount ? "passed" : "failed")}.");
}

static void Test3()
{
    Console.WriteLine("==============");
    Console.WriteLine("Test2");
    Console.WriteLine("_____");
    int reqStepCount = -1;
    Game game_2 = new("[[1,2,3],[5,4,0]]");
    GameState resolvedState = game_2.GetSolution();
    Console.WriteLine(resolvedState.ToString());
    Console.WriteLine(resolvedState.Status.ToString());
    Console.WriteLine($"resolved in {resolvedState.StepCount} steps");
    Console.WriteLine($"Test result {(resolvedState.StepCount == reqStepCount ? "passed" : "failed")}.");
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

    int _stepCount;
    public int StepCount
    {
        get
        {
            if(Status == StateStatus.FAILED)
            {
                return -1;
            }
            return _stepCount;
        }
        private set
        {
            _stepCount = value;
        }
    }
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
    public GameState(int[][] initialState, Movements lastMovementFrom, List<Movements> movesHistory, int t)
    {
        Status = StateStatus.IN_PROGRESS;
        StepCount = t;
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
        StepCount++;
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("[");
        bool reapet = false;
        foreach (int[] row in Values)
        {
            if (reapet)
                sb.Append(",");
            sb.Append("[");
            foreach (int val in row)
            {
                sb.Append(val);
            }
            sb.Append("]");
        }
        sb.Append("]");
        return sb.ToString();
    }

    public void SetAvailableMovements()
    {
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
    GameState _initState;
    public Dictionary<string, int> stateHist { get; set; }

    /// <summary>
    /// init state as "n,n,n/n,n,n" or "[[n,n,n],[n,n,n]]"
    /// </summary>
    /// <param name="initState"></param>
    public Game(string initState)
    {
        Console.WriteLine("New Game:" + initState);
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
            Console.WriteLine("<!> Invalid string format for initial state <!>");
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
            Console.WriteLine("Invalid game state as a starting point!");
            return;
        }
        _initState = new GameState(initialState, Movements.NONE, new List<Movements>(), 0);
        stateHist = new();
    }
    public GameState GetSolution()
    {
        return GetSolution(_initState);
    }
    private GameState GetSolution(GameState state)
    {
        string stateToken = state.ToString();
        if (!stateHist.TryAdd(stateToken, state.StepCount))
        {
            // This state has been passed before
            // if this rout reaches this state by longer path, then this path is failed.
            //  -> The other one is more closer to the solution then this branch, no need to continue on this rout
            if(state.StepCount >= stateHist[stateToken])
            {
                state.Status = StateStatus.FAILED;
                return state;
            }
            stateHist[stateToken] = state.StepCount;
        }
        if (_isSolved(state))
        {
            state.Status = StateStatus.SUCCESS;
            return state;
        }
        state.SetAvailableMovements();
        List<GameState> nextStates = new();
        CheckMovement(state, Movements.TOP, nextStates);
        CheckMovement(state, Movements.BOTTOM, nextStates);
        CheckMovement(state, Movements.RIGHT, nextStates);
        CheckMovement(state, Movements.LEFT, nextStates);        

        GameState resolvedState = state;
        int count = int.MaxValue;
        bool successFound = false;
        foreach (GameState newState in nextStates)
        {
            GameState newResolvedState = GetSolution(newState);
            if (successFound == false && newResolvedState.Status == StateStatus.SUCCESS)
            {
                successFound = true;
                resolvedState = newResolvedState;
                count = resolvedState.StepCount;
            }
            else if (successFound && newResolvedState.Status == StateStatus.SUCCESS)
            {
                if (newResolvedState.StepCount < count)
                {
                    resolvedState = newResolvedState;
                    count = resolvedState.StepCount;
                }
            }
            else if(successFound == false)
            {
                if (newResolvedState.StepCount < count)
                {
                    resolvedState = newResolvedState;
                    count = resolvedState.StepCount;
                }
            }
        }
        return resolvedState;
    }
    private void CheckMovement(GameState state, Movements movement, List<GameState> nexStates)
    {
        // check availablity
        bool isMovementAvailable = state.NextMovements.HasFlag(movement);
        if (!isMovementAvailable) return;
        // we return early if this movement is not available
        GameState _state;
        _state = new GameState(state.Values, state.LastMovementFrom, state.moveHist, state.StepCount);
        nexStates.Add(_state);

        _state.Move(movement);
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
    private bool ValidateInput(int[][] initialState)
    {
        return true;
    }
}