const uint SizeX = 70;
const uint SizeY = 70;

var gridGenerator = new GridGenerator((SizeX, SizeY));
gridGenerator.Automate(printFinalResultToConsole: true);

Console.WriteLine($"Highest area number: {gridGenerator.HighestArea}");
uint largestAreaNumber = 0;
uint largestAreaCount = 0;
for (uint i = 1; i <= gridGenerator.HighestArea; i++)
{
    var cellCount = (uint)gridGenerator.FindCellOfArea(i).Count();
    Console.WriteLine($"Area #{i}: {cellCount} cells in size");

    if (cellCount > largestAreaCount)
    {
        largestAreaCount = cellCount;
        largestAreaNumber = i;
    }
}
Console.WriteLine($"Largest area is #{largestAreaNumber} with {largestAreaCount} cells!");

Console.WriteLine($"Possible Door cells: {gridGenerator.FindCell((x, y, cell) => cell.CanBeDoor).Count()}");
