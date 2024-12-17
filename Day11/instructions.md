# Day 11: Plutonian Pebbles

The ancient civilization on Pluto was known for its ability to manipulate spacetime, and while `The Historians` explore their infinite corridors, you've noticed a strange set of physics-defying stones.

At first glance, they seem like normal stones: they're arranged in a perfectly straight line, and each stone has a number engraved on it.

The strange part is that every time you blink, the stones change.

Sometimes, the number engraved on a stone changes. Other times, a stone might split in two, causing all the other stones to shift over a bit to make room in their perfectly straight line.

## Rules

As you observe them for a while, you find that the stones have a consistent behavior. Every time you blink, the stones each simultaneously change according to the first applicable rule in this list:

- The stone was engraved with the number `0`
    - Replaced by a stone engraved with the number `1`.
- The stone was engraved with a number that has an even number of digits
    - Replaced by two stones
    - Left stone has the left half of the digits engraved on it
    - Right stone has right half of the digits engraved on it
    - Leading zeroes do not persist
        - `1000` would become stones `10` and `0`
- If none of the above apply, the stone is replaced by a new stone
    - Replacement stone will have the original number multiplied by `2024`

No matter how the stones change, their order is preserved, and they stay on their perfectly straight line.

How will the stones evolve if you keep blinking at them? You take a note of the number engraved on each stone in the line (your puzzle input).

## Examples

If you have an arrangement of five stones engraved with the numbers `0` `1` `10` `99` `999` and you blink once, the stones transform as follows:

- The first stone, `0`
    - becomes a stone marked `1`
- The second stone, `1`
    - Multiplied by `2024` to become `2024`
- The third stone, `10`
    - Left split marked `1`
    - Right split marked `0`
- The fourth stone, `99`
    - Left split marked `9`
    - Right split marked `9`
- The fifth stone, `999`
    - Replaced by a stone marked `2021976`.

So, after blinking once, your five stones would become an arrangement of seven stones

    0 1 10 99 999
    1 2024 1 0 9 9 2021976

### Longer example

#### Initial arrangement

    125 17

    253000 1 7

    253 0 2024 14168

    512072 1 20 24 28676032

    512 72 2024 2 0 2 4 2867 6032

    1036288 7 2 20 24 4048 1 4048 8096 28 67 60 32

    2097446912 14168 4048 2 0 2 4 40 48 2024 40 48 80 96 2 8 6 7 6 0 3 2

In this example, after blinking `six times`, you would have `22 stones`. After blinking `25 times`, you would have `55312 stones`!

## Challenge 1

Consider the arrangement of stones in front of you. How many stones will you have after blinking `25 times`?

<hr><br>

# Part Two

The Historians sure are taking a long time. To be fair, the infinite corridors are very large.

## Challenge 2

How many stones would you have after blinking a total of `75 times`?