# Day 12: Garden Groups

Why not search for the Chief Historian near the gardener and his massive farm? There's plenty of food, so The Historians grab something to eat while they search.

You're about to settle near a complex arrangement of garden plots when some Elves ask if you can lend a hand. They'd like to set up fences around each region of garden plots, but they can't figure out how much fence they need to order or how much it will cost. They hand you a map (your puzzle input) of the garden plots.

## Rules

Each garden plot grows only a single type of plant and is indicated by a single letter on your map. When multiple garden plots are growing the same type of plant and are touching (horizontally or vertically), they form a region. For example:

    AAAA
    BBCD
    BBCC
    EEEC

This 4x4 arrangement includes garden plots growing five different types of plants (labeled `A`, `B`, `C`, `D`, and `E`), each grouped into their own region.

In order to accurately calculate the cost of the fence around a single region, you need to know that region's `area` and `perimeter`.

### Area

The `area` of a region is simply the number of garden plots the region contains. The above map's type `A`, `B`, and `C` plants are each in a region of `area 4`. The type `E` plants are in a region of `area 3`; the type `D` plants are in a region of `area 1`.

### Perimeter

Each garden plot is a square and so has four sides.  
The `perimeter` of a region is the number of sides of garden plots in the region that *do not touch another garden plot in the same region*. The type `A` and `C` plants are each in a region with `perimeter 10`. The type `B` and `E` plants are each in a region with `perimeter 8`. The lone `D` plot forms its own region with `perimeter 4`.

Visually indicating the sides of plots in each region that contribute to the perimeter using `-` and `|`, the above map's regions' perimeters are measured as follows:

    +-+-+-+-+
    |A A A A|
    +-+-+-+-+     +-+
                  |D|
    +-+-+   +-+   +-+
    |B B|   |C|
    +   +   + +-+
    |B B|   |C C|
    +-+-+   +-+ +
              |C|
    +-+-+-+   +-+
    |E E E|
    +-+-+-+

Plants of the same type can appear in multiple separate regions, and regions can even appear within other regions. For example:

    OOOOO
    OXOXO
    OOOOO
    OXOXO
    OOOOO

The above map contains five regions, one containing all of the `O` garden plots, and the other four each containing a single `X` plot.

The four `X` regions each have `area 1` and `perimeter 4`. The region containing 21 type `O` plants is more complicated; in addition to its outer edge contributing a `perimeter of 20`, its boundary with each `X` region contributes an additional `4` to its perimeter, for a `total perimeter of 36`.

| Fence Price |
|-------------|
| The price of fence required for a region is found by multiplying that region's area by its perimeter |
| The total price of fencing all regions on a map is found by adding together the price of fence for every region on the map |

## Examples

### First Example

| Region | Calculation | Price |
|:------:|-------------|:-----:|
| A | $4 \times 10$ | $40$ |
| B | $4 \times 8$ | $32$ |
| C | $4 \times 10$ | $40$ |
| D | $1 \times 4$ | $4$ |
| E | $3 \times 8$ | $24$ |
| **Total** | **Sum all** | $140$ |

### Second Example

| Region | Calculation | Price |
|:------:|-------------|:-----:|
| O | $21 \times 36$ | $756$ |
| X | $1 \times 4$ (for 4 regions)| $16$ |
| **Total** | **Sum all** | $772$ |

### Larger Example

    RRRRIICCFF
    RRRRIICCCF
    VVRRRCCFFF
    VVRCCCJFFF
    VVVVCJJCFE
    VVIVCCJJEE
    VVIIICJJEE
    MIIIIIJJEE
    MIIISIJEEE
    MMMISSJEEE

It contains:

| Region | Calculation | Price |
|:------:|-------------|:-----:|
| R | $12 \times 18$ | $216$ |
| I | $4 \times 8$ | $32$ |
| C | $14 \times 28$ | $392$ |
| F | $10 \times 18$ | $180$ |
| V | $13 \times 20$ | $260$ |
| J | $11 \times 20$ | $220$ |
| C | $1 \times 4$ | $4$ |
| E | $13 \times 18$ | $234$ |
| I | $14 \times 22$ | $308$ |
| M | $5 \times 12$ | $60$ |
| S | $3 \times 8$ | $24$ |
| **Total** | **Sum all** | $1930$ |

## Challenge 1

What is the total price of fencing all regions on your map?

<hr><br>

# Part Two

Fortunately, the Elves are trying to order so much fence that they qualify for a bulk discount!

## Rules

| Bulk discount |
|---------------|
| Don't use the perimeter to calculate the price. |
| Instead use the number of sides each region has. |
| *Each straight section of fence counts as a side, regardless of how long it is.* |

## Examples

    AAAA
    BBCD
    BBCC
    EEEC

- `A`, `B`, `D` and `E` have 4 sides
- `C` has 8 sides

This is the pricing using the bulk discount:

| Region | Pricing |
|:------:|:-------:|
| A | $16$ |
| B | $16$ |
| C | $32$ |
| D | $4$ |
| E | $12$ |
| **Total** | $80$ |

The second example above (full of type X and O plants) would have a total price of `436`.

Here's a map that includes an `E-shaped` region full of type `E` plants:

    EEEEE
    EXXXX
    EEEEE
    EXXXX
    EEEEE

The E-shaped region has an area of `17` and `12` sides for a price of `204`. Including the two regions full of type `X` plants, this map has a total price of `236`.

### New Example Map

This map has a total price of `368`:

    AAAAAA
    AAABBA
    AAABBA
    ABBAAA
    ABBAAA
    AAAAAA

It includes:
- Two regions full of type `B` plants 
    - Each with 4 sides
- A single region full of type `A` plants
    - 4 sides on the outside 
    - 8 more sides on the inside
    - For a total of 12 sides

| Note |
|------|
| Be mindful that fences don't connect diagonally. You will need to keep track of what is considered within the region and outside of the region when counting the sides. |

Be especially careful when counting the fence around regions like the one full of type `A` plants; in particular, each section of fence has an in-side and an out-side, so the fence does not connect across the middle of the region (where the two B regions touch diagonally). (The Elves would have used the Möbius Fencing Company instead, but their contract terms were too one-sided.)

### Large Example Revisited

The larger example from before now has the following updated prices:

| Region | Calculation | Price |
|:------:|-------------|:-----:|
| R | $12 * 10$ | $120$ |
| I | $4 * 4$ | $16$ |
| C | $14 * 22$ | $308$ |
| F | $10 * 12$ | $120$ |
| V | $13 * 10$ | $130$ |
| J | $11 * 12$ | $132$ |
| C | $1 * 4$ | $4$ |
| E | $13 * 8$ | $104$ |
| I | $14 * 16$ | $224$ |
| M | $5 * 6$ | $30$ |
| S | $3 * 6$ | $18$ |
| **Total** | **Sum All** | $1206$ |

Adding these together produces its new total price of `1206`.

## Challenge 2

What is the new total price of fencing all regions on your map?