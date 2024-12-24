# Day 13: Claw Contraption

Next up: the lobby of a resort on a tropical island. The Historians take a moment to admire the hexagonal floor tiles before spreading out.

Fortunately, it looks like the resort has a new arcade! Maybe you can win some prizes from the claw machines?

## Rules

The claw machines here are a little unusual. Instead of a joystick or directional buttons to control the claw, these machines have two buttons labeled `A` and `B`. Worse, you can't just put in a token and play; it costs `3 tokens` to push the `A` button and `1 token` to push the `B` button.

| Button | Cost |
|:------:|-----:|
| A | 3 tokens |
| B | 1 token |

With a little experimentation, you figure out that each machine's buttons are configured to move the claw a specific amount to the right (along the X axis) and a specific amount forward (along the Y axis) each time that button is pressed.

Each machine contains one prize; to win the prize, the claw must be positioned exactly above the prize on both the X and Y axes.

You wonder: what is the smallest number of tokens you would have to spend to win as many prizes as possible? You assemble a list of every machine's button behavior and prize location (your puzzle input). For example:

## Examples

### Machine 1

| Input | X | Y |
|:------:|:-:|:-:|
| A | +94 | +34 |
| B | +22 | +67 |
| Prize | 8400 | 5400 |

### Machine 2

| Input | X | Y |
|:------:|:-:|:-:|
| A | +26 | +66 |
 B | +67 | +21 |
| Prize | 12748 | 12176 |

### Machine 3

| Input | X | Y |
|:------:|:-:|:-:|
| A | +17 | +86 |
| B | +84 | +37 |
| Prize | 7870 | 6450 |

### Machine 4

| Input | X | Y |
|:------:|:-:|:-:|
| A | +69 | +23 |
| B | +27 | +71 |
| Prize | 18641 | 10279 |

This list describes the button configuration and prize location of four different claw machines.
For now, consider just the first claw machine in the list:

Pushing the machine's `A` button would move the claw
- `94 units` along the `X axis`
- `34 units` along the `Y axis`

Pushing the `B` button would move the claw
- `22 units` along the `X axis`
- `67 units` along the `Y axis`

The prize is located at `X=8400`, `Y=5400`; this is relative to the claw's initial position.  
The claw would need to move exactly `8400 units` along the `X axis` and exactly `5400 units` along the `Y axis` to be perfectly aligned with the prize in this machine.

The cheapest way to win the prize is:
- $80 \times A$
- $40 \times B$

This would line up the claw along the X axis
- $80 \times 94 + 40 \times 22 = 8400$

And along the Y axis
- $80 \times 34 + 40 \times 67 = 5400$

The costs: 
- $80 \times 3$ tokens for the `A` presses
- $40 \times 1$ for the `B` presses
- Total of $280$ tokens.

For the second and fourth claw machines, there is no combination of `A` and `B` presses that will ever win a prize.

For the third claw machine, the cheapest way to win the prize is
- $30 \times A$
- $86 \times B$
- For a total of $200$ tokens.

So, the most prizes you could possibly win is two.  
The minimum tokens you would have to spend to win all (two) prizes is $480$

You estimate that each button would need to be pressed no more than `100 times` to win a prize. How else would someone be expected to play?

## Challenge 1

Figure out how to win as many prizes as possible.
What is the fewest tokens you would have to spend to win all possible prizes?

# Part Two

As you go to win the first prize, you discover that the claw is nowhere near where you expected it would be. Due to a unit conversion error in your measurements, the position of every prize is actually `10000000000000` higher on both the X and Y axis!

## Rules

Add `10000000000000` to the `X` and `Y` position of every prize. After making this change, the example above would now look like this:

    Button A: X+94, Y+34
    Button B: X+22, Y+67
    Prize: X=10000000008400, Y=10000000005400

    Button A: X+26, Y+66
    Button B: X+67, Y+21
    Prize: X=10000000012748, Y=10000000012176

    Button A: X+17, Y+86
    Button B: X+84, Y+37
    Prize: X=10000000007870, Y=10000000006450

    Button A: X+69, Y+23
    Button B: X+27, Y+71
    Prize: X=10000000018641, Y=10000000010279


## Examples

Now, it is only possible to win a prize on the second and fourth claw machines. Unfortunately, it will take many more than 100 presses to do so.

Using the corrected prize coordinates, figure out how to win as many prizes as possible. What is the fewest tokens you would have to spend to win all possible prizes?