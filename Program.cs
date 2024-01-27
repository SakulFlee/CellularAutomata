﻿var gridGenerator = new GridGenerator((70, 70), printToConsole: true);

var rooms = new List<IGridRoom>() {
    new GridRoomRectangular((10, 10), (10, 10))
};
gridGenerator.ApplyRooms(rooms);
gridGenerator.PerformAutomataRepetitive();
gridGenerator.AssignAreas();
gridGenerator.ApplyRooms(rooms, fixAreas: true, printToConsole: true, findDoorways: true);

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

Console.WriteLine($"Possible Door cells: {gridGenerator.FindCell((x, y, cell) => cell.Type == GridType.PossibleDoor).Count()}");
