using System.Text;

public class GridGenerator
{
    public uint MinimumNeighbourWallsForFloor = 4;

    public Random R;
    public GridCell[,] Grid { get; private set; }
    public int SizeX { get => Grid.GetUpperBound(0); }
    public int SizeY { get => Grid.GetUpperBound(1); }

    public uint HighestArea { get; private set; } = 0;

    public GridGenerator((uint, uint) gridSize, int seed = 12345, double floorPercentage = 0.60, bool printToConsole = false)
    {
        R = new Random(seed);

        Grid = new GridCell[gridSize.Item1, gridSize.Item2];
        for (var x = 0; x < gridSize.Item1; x++)
            for (var y = 0; y < gridSize.Item2; y++)
                Grid[x, y] = R.NextDouble() <= floorPercentage
                    ? new GridCell(GridType.Floor)
                    : new GridCell(GridType.Wall);

        if (printToConsole)
        {
            Console.WriteLine(">>> Randomized Grid:");
            PrintToConsole();
        }
    }

    public void ApplyRooms(List<IGridRoom> rooms, bool findDoorways = false, bool fixAreas = false, bool printToConsole = false, bool printWithArea = true)
    {
        foreach (var room in rooms)
        {
            Grid = room.Apply(Grid, findDoorways);
        }

        if (fixAreas) FixAreas();

        if (printToConsole)
        {
            Console.WriteLine(">>> Applied rooms:");
            PrintToConsole(printWithArea);
        }
    }

    public void FixAreas()
    {
        var floorsWithoutAreaList = FindCell((x, y, cell) => cell.Type == GridType.Floor && cell.Area == 0);
        var floorsWithoutArea = new Queue<(uint, uint)>(floorsWithoutAreaList.Count());
        foreach (var a in floorsWithoutAreaList) floorsWithoutArea.Enqueue(a);

        while (floorsWithoutArea.Count() > 0)
        {
            var cellPosition = floorsWithoutArea.Dequeue();

            var cellN = GetCell((cellPosition.Item1 - 1, cellPosition.Item2));
            if (cellN != null && cellN.Area > 0)
            {
                Grid[cellPosition.Item1, cellPosition.Item2].Area = cellN.Area;
                continue;
            }

            var cellS = GetCell((cellPosition.Item1 + 1, cellPosition.Item2));
            if (cellS != null && cellS.Area > 0)
            {
                Grid[cellPosition.Item1, cellPosition.Item2].Area = cellS.Area;
                continue;
            }

            var cellE = GetCell((cellPosition.Item1, cellPosition.Item2 - 1));
            if (cellE != null && cellE.Area > 0)
            {
                Grid[cellPosition.Item1, cellPosition.Item2].Area = cellE.Area;
                continue;
            }

            var cellW = GetCell((cellPosition.Item1, cellPosition.Item2 + 1));
            if (cellW != null && cellW.Area > 0)
            {
                Grid[cellPosition.Item1, cellPosition.Item2].Area = cellW.Area;
                continue;
            }

            floorsWithoutArea.Enqueue(cellPosition);
        }
    }

    public void PerformAutomataRepetitive(int steps = 5, bool printStepsToConsole = false)
    {
        if (printStepsToConsole)
        {
            Console.WriteLine($">>> Automata Basis:");
            PrintToConsole();
        }

        for (var step = 1; step <= steps; step++)
        {
            PerformAutomata();

            if (printStepsToConsole)
            {
                Console.WriteLine($">>> Automata #{step}/{steps}:");
                PrintToConsole();
            }
        }
    }

    public void PerformAutomata()
    {
        var outputGrid = new GridCell[SizeX, SizeY];

        for (uint x = 0; x < SizeX; x++)
            for (uint y = 0; y < SizeY; y++)
                outputGrid[x, y] = CountNeighboursOfType((x, y), GridType.Wall, countNull: true) < MinimumNeighbourWallsForFloor
                    ? new GridCell(GridType.Floor)
                    : new GridCell(GridType.Wall);

        Grid = outputGrid;
    }

    public void AssignAreas(bool printStepsToConsole = false)
    {
        uint area = 1;

        var currentX = -1;
        var currentY = 0;
        do
        {
            // Increment X & Y
            if (currentX++ >= SizeX)
            {
                currentX = 0;
                // Break the loop if we are out of bounce
                if (currentY++ >= SizeY)
                    break;
            }

            // If the cell at the current location is invalid, not a floor or already has an area assigned, skip it.
            var cell = GetCell(((uint)currentX, (uint)currentY));
            if (cell == null || cell.Type != GridType.Floor || cell.HasAreaData()) continue;

            // Cell must be a floor AND in an area we haven't been in yet
            AssignAreaNeighboursAndSelf(((uint)currentX, (uint)currentY), area);

            if (printStepsToConsole)
            {
                Console.WriteLine($">>> Area #{area}:");
                PrintToConsole(printAreaMap: true);
            }

            // Increment area counter
            area++;
        } while (true);

        // Set counter
        HighestArea = area;
    }

    public void AssignAreaNeighboursAndSelf((uint, uint) position, uint area)
    {
        var cellSelf = GetCell(position);

        // Skip invalid cells
        if (cellSelf == null) return;

        // Skip already assigned areas
        if (cellSelf.HasAreaData()) return;

        // Skip non-floor cells
        if (cellSelf.Type != GridType.Floor) return;

        Grid[position.Item1, position.Item2].Area = area;

        var cellN = GetCell((position.Item1 - 1, position.Item2));
        if (cellN != null && cellN.Type == GridType.Floor)
        {
            // Grid[position.Item1 - 1, position.Item2].Area = area;
            AssignAreaNeighboursAndSelf((position.Item1 - 1, position.Item2), area);
        }

        var cellS = GetCell((position.Item1 + 1, position.Item2));
        if (cellS != null && cellS.Type == GridType.Floor)
        {
            // Grid[position.Item1 + 1, position.Item2].Area = area;
            AssignAreaNeighboursAndSelf((position.Item1 + 1, position.Item2), area);
        }

        var cellE = GetCell((position.Item1, position.Item2 - 1));
        if (cellE != null && cellE.Type == GridType.Floor)
        {
            // Grid[position.Item1, position.Item2 - 1].Area = area;
            AssignAreaNeighboursAndSelf((position.Item1, position.Item2 - 1), area);
        }

        var cellW = GetCell((position.Item1, position.Item2 + 1));
        if (cellW != null && cellW.Type == GridType.Floor)
        {
            // Grid[position.Item1, position.Item2 + 1].Area = area;
            AssignAreaNeighboursAndSelf((position.Item1, position.Item2 + 1), area);
        }
    }

    public List<(uint, uint)> FindCell(Func<uint, uint, GridCell, bool> lambda)
    {
        var output = new List<(uint, uint)>();

        for (uint x = 0; x < SizeX; x++)
            for (uint y = 0; y < SizeY; y++)
                if (lambda.Invoke(x, y, Grid[x, y]))
                    output.Add((x, y));

        return output;
    }

    public List<(uint, uint)> FindCellOfType(GridType type)
    {
        return FindCell((x, y, cell) => cell.Type == type);
    }

    public List<(uint, uint)> FindCellOfArea(uint area)
    {
        return FindCell((x, y, cell) => cell.Area == area);
    }

    public GridCell? GetCell((uint, uint) position) => GetCell(Grid, position, ((uint)SizeX, (uint)SizeY));

    public static GridCell? GetCell(GridCell[,] grid, (uint, uint) position, (uint, uint) size)
    {
        if (position.Item1 < 0 || position.Item2 < 0 || position.Item1 >= size.Item1 || position.Item2 >= size.Item2) return null;
        else return grid[position.Item1, position.Item2];
    }

    public uint CountNeighboursOfType((uint, uint) position, GridType type, bool countNull = true)
    {
        // Cardinals
        var valueN = GetCell((position.Item1 - 1, position.Item2));
        var valueS = GetCell((position.Item1 + 1, position.Item2));
        var valueE = GetCell((position.Item1, position.Item2 - 1));
        var valueW = GetCell((position.Item1, position.Item2 + 1));

        // Inter-Cardinals
        var valueNE = GetCell((position.Item1 - 1, position.Item2 - 1));
        var valueSE = GetCell((position.Item1 + 1, position.Item2 - 1));
        var valueNW = GetCell((position.Item1 - 1, position.Item2 + 1));
        var valueSW = GetCell((position.Item1 + 1, position.Item2 + 1));

        return (uint)(0
            + (valueN == null
                ? countNull ? 1 : 0
                : valueN.Type == type ? 1 : 0)
            + (valueS == null
                ? countNull ? 1 : 0
                : valueS.Type == type ? 1 : 0)
            + (valueE == null
                ? countNull ? 1 : 0
                : valueE.Type == type ? 1 : 0)
            + (valueW == null
                ? countNull ? 1 : 0
                : valueW.Type == type ? 1 : 0)
            + (valueNE == null
                ? countNull ? 1 : 0
                : valueNE.Type == type ? 1 : 0)
            + (valueSE == null
                ? countNull ? 1 : 0
                : valueSE.Type == type ? 1 : 0)
            + (valueNW == null
                ? countNull ? 1 : 0
                : valueNW.Type == type ? 1 : 0)
            + (valueSW == null
                ? countNull ? 1 : 0
                : valueSW.Type == type ? 1 : 0)
        );
    }

    public void PrintToConsole(bool printAreaMap = false)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var output = "";
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                if (!printAreaMap)
                {
                    output += Grid[x, y].MakeConsoleString(2);
                }
                else
                {
                    var cell = Grid[x, y];
                    if (cell.Type != GridType.Floor)
                    {
                        output += cell.MakeConsoleString(2);
                    }
                    else
                    {
                        var area = cell.Area;
                        if (area == 0) output += "AA"; // AREA isn't properly tracked!!!
                        else if (area < 10) output += $"{area}{area}";
                        else output += $"{area}";
                    }
                }
            }

            output += "\n";
        }
        Console.WriteLine(output);
    }
}
