using System.Text;

const uint SizeX = 70;
const uint SizeY = 70;

const double FLOOR_PERCENTAGE = 0.60;
const uint MIN_NEIGHBOUR_FLOOR_TILES_FOR_FLOOR = 4;
const uint AUTOMATA_STEPS = 5;

const char FLOOR = ' ';
const char WALL = '█';
const char PATH = '░';

char[,] initialize(uint sizeX, uint sizeY)
{
    var grid = new char[sizeX, sizeY];

    for (var x = 0; x < sizeX; x++)
        for (var y = 0; y < sizeY; y++)
        {
            grid[x, y] = Random.Shared.NextDouble() <= FLOOR_PERCENTAGE ? FLOOR : WALL;
        }

    return grid;
}

void print(char[,] grid)
{
    var output = "";
    for (var x = 0; x < grid.GetUpperBound(0); x++)
    {
        for (var y = 0; y < grid.GetUpperBound(1); y++)
        {
            var c = grid[x, y];
            output += $"{c}{c}";
        }

        output += "\n";
    }
    Console.WriteLine(output);
}

char? GetCell(char[,] inputGrid, uint x, uint y)
{
    if (x < 0 || y < 0 || x >= inputGrid.GetUpperBound(0) || y >= inputGrid.GetUpperBound(1)) return null;
    else return inputGrid[x, y];
}

uint countNeighbourWalls(char[,] inputGrid, uint x, uint y)
{
    // Cardinals
    var valueN = GetCell(inputGrid, x + 1, y);
    var valueS = GetCell(inputGrid, x - 1, y);
    var valueE = GetCell(inputGrid, x, y + 1);
    var valueW = GetCell(inputGrid, x, y - 1);

    // Inter-Cardinals
    var valueNE = GetCell(inputGrid, x + 1, y + 1);
    var valueSE = GetCell(inputGrid, x - 1, y + 1);
    var valueNW = GetCell(inputGrid, x + 1, y - 1);
    var valueSW = GetCell(inputGrid, x - 1, y - 1);

    return (uint)(0
        + (valueN == null ? 1 : valueN == WALL ? 1 : 0)
        + (valueS == null ? 1 : valueS == WALL ? 1 : 0)
        + (valueE == null ? 1 : valueE == WALL ? 1 : 0)
        + (valueW == null ? 1 : valueW == WALL ? 1 : 0)
        + (valueNE == null ? 1 : valueNE == WALL ? 1 : 0)
        + (valueSE == null ? 1 : valueSE == WALL ? 1 : 0)
        + (valueNW == null ? 1 : valueNW == WALL ? 1 : 0)
        + (valueSW == null ? 1 : valueSW == WALL ? 1 : 0)
    );
}

char[,] automata(char[,] inputGrid)
{
    var outputGrid = new char[inputGrid.GetUpperBound(0) + 1, inputGrid.GetUpperBound(1) + 1];

    for (uint x = 0; x < SizeX; x++)
    {
        for (uint y = 0; y < SizeY; y++)
        {
            var neighbourWallCount = countNeighbourWalls(inputGrid, x, y);
            outputGrid[x, y] = neighbourWallCount < MIN_NEIGHBOUR_FLOOR_TILES_FOR_FLOOR ? FLOOR : WALL;
        }
    }

    return outputGrid;
}

char[,] PlaceRoom(char[,] grid, uint startX, uint startY, uint sizeX, uint sizeY)
{
    for (var x = startX; x < startX + sizeX; x++)
        for (var y = startY; y < startY + sizeY; y++)
        {
            if (x == startX || y == startY || x == startX + sizeX - 1 || y == startY + sizeY - 1) grid[x, y] = WALL;
            else grid[x, y] = FLOOR;

        }

    return grid;
}

char[,] AreaNeighbours(char[,] grid, uint x, uint y, uint area)
{
    var tileSelf = GetCell(grid, x, y);
    if (tileSelf != null && tileSelf == FLOOR)
    {
        grid[x, y] = $"{area}".ToCharArray()[0];
    }

    var tileN = GetCell(grid, x + 1, y);
    if (tileN != null && tileN == FLOOR)
    {
        grid[x + 1, y] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x + 1, y, area);
    }

    var selfS = GetCell(grid, x - 1, y);
    if (selfS != null && selfS == FLOOR)
    {
        grid[x - 1, y] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x - 1, y, area);
    }

    var selfE = GetCell(grid, x, y - 1);
    if (selfE != null && selfE == FLOOR)
    {
        grid[x, y - 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x, y - 1, area);
    }

    var selfW = GetCell(grid, x, y + 1);
    if (selfW != null && selfW == FLOOR)
    {
        grid[x, y + 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x, y + 1, area);
    }

    var tileNE = GetCell(grid, x + 1, y - 1);
    if (tileNE != null && tileNE == FLOOR)
    {
        grid[x + 1, y - 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x + 1, y - 1, area);
    }

    var tileNW = GetCell(grid, x + 1, y + 1);
    if (tileNW != null && tileNW == FLOOR)
    {
        grid[x + 1, y + 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x + 1, y + 1, area);
    }

    var selfSE = GetCell(grid, x - 1, y - 1);
    if (selfSE != null && selfSE == FLOOR)
    {
        grid[x - 1, y - 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x - 1, y - 1, area);
    }

    var selfSW = GetCell(grid, x - 1, y + 1);
    if (selfSW != null && selfSW == FLOOR)
    {
        grid[x - 1, y + 1] = $"{area}".ToCharArray()[0];
        grid = AreaNeighbours(grid, x - 1, y + 1, area);
    }

    return grid;
}

char[,] MakeAreaMap(char[,] inputGrid)
{
    var outputGrid = new char[inputGrid.GetUpperBound(0), inputGrid.GetUpperBound(1)];
    uint area = 1;

    for (var x = 0; x < outputGrid.GetUpperBound(0); x++)
        for (var y = 0; y < outputGrid.GetUpperBound(0); y++)
            outputGrid[x, y] = inputGrid[x, y];

    // Start with -1 as we increment at the beginning of the loop
    var currentX = -1;
    var currentY = 0;
    do
    {
        // Increment X & Y
        if (currentX++ >= outputGrid.GetUpperBound(0))
        {
            currentX = 0;
            // Break the loop if we are out of bounce
            if (currentY++ >= outputGrid.GetUpperBound(1))
                break;

        }

        // Get the tile at this location, skip if not floor
        var tile = GetCell(outputGrid, (uint)currentX, (uint)currentY);
        if (tile != FLOOR) continue;

        // Tile must be a floor AND in an area we haven't been in yet
        outputGrid = AreaNeighbours(outputGrid, (uint)currentX, (uint)currentY, area);
        print(outputGrid);

        // Increment area counter
        area++;
    } while (true);

    return outputGrid;
}


/// ---

Console.OutputEncoding = Encoding.UTF8;

var grid = initialize(SizeX, SizeY);
print(grid);

grid = PlaceRoom(grid, 5, 5, 20, 20);
grid = PlaceRoom(grid, 40, 40, 15, 15);
grid = PlaceRoom(grid, 30, 10, 15, 15);
grid = PlaceRoom(grid, 60, 60, 10, 10);
print(grid);

for (var i = 0; i < AUTOMATA_STEPS; i++)
{
    Console.WriteLine($"Automata ({i + 1}/{AUTOMATA_STEPS})");
    grid = automata(grid);
    print(grid);
}

Console.Write($"\n\n\n---\n\n\n");

var areaMapGrid = MakeAreaMap(grid);
print(areaMapGrid);
