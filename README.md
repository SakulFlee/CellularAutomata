# Cellular Automata (/Automaton) - Dungeon Generation
[![.NET](https://github.com/SakulFlee/CellularAutomata/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SakulFlee/CellularAutomata/actions/workflows/dotnet.yml)

This repository is an implementation of [Cellular Automaton](https://en.wikipedia.org/wiki/Cellular_automaton) and added on-top some retaining, room placement and area/room labelling functions.

The standard run follows these following steps in order:

1. Initialize the grid randomly with a bias (default: 60% floor tiles)
2. Perform N steps of Automata (default N: 5)
3. Place N rooms (= squares) of random location and sizes  
   a. Assign room numbers
4. Assign area numbers
5. Find possible doorways locations

> A room is a square which is fully composed out of floor tiles, but the edges which will always be wall tiles.

The result will be looking something like this:

```text
████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
██████████████████████████████████████████████████A3A3A3A3A3██A3████████████████████████████████████████████████████████████████
██████████A2██████████████████████████████████████A3A3A3A3A3A3A3A3A3████████████████████████████████████████████████████████████
██████A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3A3A3A3A3A3A3████████████████████████████████R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3A3A3A3A3A3██████████████████████████████████R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3A3██▒▒▒▒▒▒██████████████████████████████████R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3██████████A1A7A1▒▒R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3████████A1A7A1▒▒R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████A1A7A1▒▒R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████████████R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████████████R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2▒▒R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████████████R2R5R2R5R2R5R2R5R2R5R2████
████A2A2A2A2▒▒R1R4R1R4R1██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3████████████████R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2▒▒R1R4R1R4R1▒▒R1R7R1R7R1R7R1R7▒▒R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████████████R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2▒▒R1R4R1R4R1▒▒R1R7R1R7R1R7R1R7▒▒R4R1▒▒A3A3A3A3████R2R4R2R4R2R4R2██████A3A3A3██████████████R2R5R2R5R2R5R2R5R2R5R2████
████████A2A2██████████████R1R7R1R7R1R7R1R7██▒▒▒▒██A3A3A3██████R2R4R2R4R2R4R2████████A3A3A3A3██████████R2R5R2R5R2R5R2R5R2R5R2████
██████A2A2A2██████████████R1R7R1R7R1R7R1R7▒▒A3A3A3A3A3A3A3████R2R4R2R4R2R4R2████████A3A3A3A3A3A3██████R2R5R2R5R2R5R2R5R2R5R2████
██████████████████████████R1R7R1R7R1R7R1R7▒▒A3A3A3A3A3A3A3██████▒▒▒▒▒▒▒▒▒▒▒▒████████████A3A3A3A3A3████R2R5R2R5R2R5R2R5R2R5R2████
██████████████████████████R1R7R1R7R1R7R1R7▒▒A3A3A3A3A3A3A3A3A3██A3A3A3A3A3A3A3████████A3A3A3A3██▒▒████R2R5R2R5R2R5R2R5R2R5R2████
████████████████A3████████R1R7R1R7R1R7R1R7██████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3████████A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
██████████A3A3A3A3A3██████R1R7R1R7R1R7R1R7██████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3██████A3A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
██████████A3A3A3A3A3A3████R1R7R1R7R1R7R1R7██████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
████████A3A3A3A3A3A3A3████R1R7R1R7R1R7R1R7████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
██████A3A3A3A3A3A3A3A3████R1R7R1R7R1R7R1R7▒▒A3A3A3A3A3A3A3A3██████A3A3A3A3A3A3A3A3A3A3A3A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
██████A3A3A3A3A3A3A3A3████R1R7R1R7R1R7R1R7▒▒A3A3A3A3A3A3A3A3██████████A3A3A3A3A3A3A3A3A3A3A3A3▒▒R7R7▒▒R2R5R2R5R2R5R2R5R2R5R2████
████A3A3A3A3A3A3A3A3A3A3████████▒▒▒▒▒▒▒▒▒▒██A3A3A3A3A3A3A3A3██████████A3A3A3A3A3A3A3A3A3A3A3A3▒▒R7R7██▒▒▒▒▒▒▒▒▒▒▒▒██████▒▒▒▒████
██████A3A3A3A3A3A3A3A3A3A3██████A3A3A3A3A3A3A3██A3██A3A3A3A3A3████████████A3A3A3A3██████A3A3A3▒▒R7R7R7R7R7R7R7R7R7██████R2R1R2██
██████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3██████A3██▒▒▒▒▒▒████████████▒▒▒▒▒▒▒▒██████A3A3A3▒▒R7R7R7R7R7R7R7R7R7██████R2R1R2██
██████████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3████████████R2R2R2R2R2R2R2R2R2R2R2R2R2████████A3A3██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██████R2R1R2██
████████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3██████████R2R2R2R2R2R2R2R2R2R2R2R2R2████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3▒▒R2R1R2██
██████████A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3██▒▒▒▒██████████▒▒▒▒▒▒██R2R2R2R2R2R2R2R2R2████A3A3A3A3A3A3A3A3A3████████A3A3A3██▒▒▒▒▒▒██
████████A3A3A3A3A3██A3A3A3A3A3A3A3A3██A3▒▒R2R0R2R0R2R0R2R0R2R0▒▒R2R2R2R2R2R2R2R2R2▒▒A3A3A3A3A3A3A3A3A3A3████████████A3A3A3A3A3██
████████A3A3A3A3A3██A3A3A3A3A3A3A3A3██████R2R0R2R0R2R0R2R0R2R0▒▒R2R2R2R2R2R2R2R2R2▒▒A3A3A3A3A3A3A3A3████████████████A3A3A3A3A3██
██████A3A3A3A3A3A3A3A3A3A3A3A3A3██████████R2R0R2R0R2R0R2R0R2R0▒▒R2R2R2R2R2R2R2R2R2▒▒A3A3A3A3A3A3A3A3██████████████████A3A3A3A3██
████████A3A3A3A3A3A3A3A3A3████████████████R2R0R2R0R2R0R2R0R2R0▒▒R2R2R2R2R2R2R2R2R2██████████████████████████████████A3A3A3A3A3██
████████A3A3A3A3A3A3A3████████████████████R2R0R2R0██▒▒▒▒▒▒▒▒▒▒██▒▒▒▒▒▒▒▒██▒▒▒▒▒▒▒▒██████████████████████████████A3██A3A3A3A3A3██
██████████A3A3A3A3A3A3A3██████████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3██████████████A3A3A3A3A3A3A3██████
████████A3A3A3A3A3A3A3A3██████████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3██████A3A3A3A3A3A3A3A3A3██████████
██████████A3A3A3A3A3A3A3A3A3██████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3████A3A3A3A3A3A3A3A3A3A3██████████
████████A3A3A3A3A3A3A3A3A3A3██████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3██████A3A3A3A3A3A3A3A3████████████
████████A3A3██████A3A3A3A3A3██████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒A3A3A3A3██A3A3A3A3██████████
██████A3A3A3██████A3A3A3██▒▒██████████████R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒A3A3A3A3A3A3A3A3A3A3████████
██████A3A3A3██████A3A3A3▒▒R4R4R4R4R4R4R4▒▒R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒A3A3A3A3A3A3A3A3A3A3A3██████
██████A3A3A3A3A3A3A3A3████R4R4R4R4R4R4R4▒▒R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒A3A3A3A3A3A3A3██A3A3A3A3████
████A3A3A3A3A3A3A3A3A3████R4R4R4R4R4R4R4▒▒R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒A3A3A3A3A3A3████████A3A3████
██████A3A3A3A3A3A3████████R4██▒▒▒▒▒▒▒▒▒▒██R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8██▒▒▒▒▒▒▒▒▒▒██████████A3A3A3██
████████A3A3A3████████████R4▒▒R1R1R1R1R1▒▒R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9██████████████A3██
██████████████████████████████R1R1R1R1R1▒▒R2R0R2R0▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9██████████████A3██
██████████████████████████████R1R1R1R1R1██▒▒▒▒▒▒▒▒██R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9▒▒A2██████████████
██████████████████████████████R1R1R1R1R1R1R1R1R1R1▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9▒▒A2A2████████████
██████████████████████████████R1R1R1R1R1R1R1R1R1R1▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9▒▒A2A2████████████
██████████████████████████████R1R1R1R1R1R1R1R1R1R1▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9▒▒A2A2A2██████████
██████████████████████████████R1R1R1R1R1R1R1R1R1R1▒▒R6R2R6R2R6R2R6R2R6R2▒▒R2R3R2R3R2R3R2R3R2R3▒▒R8▒▒R9R9R9R9R9▒▒A2A2████████████
██████████████████████████████R1R1R1R1R1R1R1R1R1R1▒▒R6R2R6R2R6R2R6R2R6R2██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██R8▒▒R9R9R9R9R9██████████████████
██████████████████████████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██R6R2R6R2R6R2R6R2R6R2▒▒R1R8R1R8R1R8R1R8R1R8R1R8▒▒R9R9R9R9R9██████████████████
██████████████████████R2R2R2R2R2R2R2R2R2R2R2R2R2R2██▒▒▒▒▒▒▒▒▒▒▒▒██▒▒▒▒▒▒██R1R8R1R8R1R8R1R8R1R8R1R8██████R9R9R9██████████████████
██R5R5R5R5R5R5R5R5R5▒▒R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2▒▒R1R8R1R8R1R8R1R8R1R8R1R8R1R8R1R8██████R9R9R9██████████████████
██R5R5R5R5R5R5R5R5R5▒▒R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2▒▒R1R8R1R8R1R8R1R8R1R8R1R8R1R8R1R8██████R9R9R9██████████████████
██R5R5R5R5R5R5R5R5R5▒▒R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2▒▒R1R8R1R8R1R8R1R8R1R8R1R8R1R8R1R8██████R9R9R9██████████████████
██████████████████████R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2R2▒▒R1R8R1R8R1R8R1R8R1R8R1R8R1R8R1R8██████R9R9R9▒▒A9██A9██████████
██████████████████████████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒▒▒▒▒▒▒██▒▒▒▒██████████████████████████████████▒▒▒▒▒▒██A9A1A9██████████
██████████████████████████████R2R1R2R1R2R1R2R1R2R1R2R1▒▒A3A1A3A1A3A1A3██████████████████████████████A9A1A9A1A9A1A9A1A9██████████
██████████████████████████████R2R1R2R1R2R1R2R1R2R1R2R1▒▒A3A1A3A1A3A1A3A1██████████████████████████A1A9A1A9A1A9A1A9A1A9██████████
████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
```

> **Legend**  
> ██: Floor  
> ▒▒: Possible doorway  
> RX: Room Id  
> AX: Area Id
>
> If the pattern is repeating like "RXRX" (or "AXAX"), this means the room/area Id is "X".  
> If the pattern is alternating like "RXRY" (or "AXAY"), this means the room/area Id is "XY".  
> For example: A1A2 == Area #12, A1A1 == Area #1 (or #11), R1R0 == Room #10, etc.

Additionally, there is some example/debug information:

```text
Highest area number: 24
Area #1: 27 cells in size
Area #2: 48 cells in size
Area #3: 751 cells in size
Area #4: 174 cells in size
Area #5: 98 cells in size
Area #6: 96 cells in size
Area #7: 23 cells in size
Area #8: 65 cells in size
Area #9: 24 cells in size
Area #10: 92 cells in size
Area #11: 190 cells in size
Area #12: 71 cells in size
Area #13: 15 cells in size
Area #14: 77 cells in size
Area #15: 102 cells in size
Area #16: 170 cells in size
Area #17: 9 cells in size
Area #18: 32 cells in size
Area #19: 24 cells in size
Area #20: 60 cells in size
Area #21: 242 cells in size
Area #22: 10 cells in size
Area #23: 12 cells in size
Area #24: 0 cells in size
Largest area is #3 with 751 cells!
Possible Door cells: 279
```

## License

This repository is licensed under [MIT](https://opensource.org/license/MIT/).
