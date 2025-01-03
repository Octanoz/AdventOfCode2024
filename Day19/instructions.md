# Day 19: Linen Layout

Today, The Historians take you up to the hot springs on Gear Island! Very suspiciously, absolutely nothing goes wrong as they begin their careful search of the vast field of helixes.

Could this finally be your chance to visit the onsen next door? Only one way to find out.

After a brief conversation with the reception staff at the onsen front desk, you discover that you don't have the right kind of money to pay the admission fee. However, before you can leave, the staff get your attention. Apparently, they've heard about how you helped at the hot springs, and they're willing to make a deal: if you can simply help them arrange their towels, they'll let you in for free!

## Rules

Every towel at this onsen is marked with a pattern of colored stripes. There are only a few patterns, but for any particular pattern, the staff can get you as many towels with that pattern as you need.

| Stripe | Marking |
|---------------|:-------:|
| White | w |
| Blue | u |
| Black | b |
| Red | r |
| Green | g |

So, a towel with the pattern ggr would have a green stripe, a green stripe, and then a red stripe, in that order. (You can't reverse a pattern by flipping a towel upside-down, as that would cause the onsen logo to face the wrong way.)

The Official Onsen Branding Expert has produced a list of designs - each a long sequence of stripe colors - that they would like to be able to display. You can use any towels you want, but all of the towels' stripes must exactly match the desired design. 

So, to display the design rgrgr, you could use:

- 2 rg towels and 1 r towel
- 1 rgr towel and 1 gr towel
- 1 rgrgr towel

## Examples

To start, collect together all of the available towel patterns and the list of desired designs (your puzzle input). For example:

r, wr, b, g, bwu, rb, gb, br

    brwrr
    bggr
    gbbr
    rrbgbr
    ubwu
    bwurrg
    brgr
    bbrgwb

The first line indicates the available towel patterns; in this example, the onsen has unlimited towels with a single red stripe (r), unlimited towels with a white stripe and then a red stripe (wr), and so on.

After the blank line, the remaining lines each describe a design the onsen would like to be able to display. In this example, the first design (brwrr) indicates that the onsen would like to be able to display a black stripe, a red stripe, a white stripe, and then two red stripes, in that order.

Not all designs will be possible with the available towels. In the above example, the designs are possible or impossible as follows:

| (Im/)Possible | Combination |
|----------|-------------|
| brwrr is possible | a br, then a wr, and an r towel. |
| bggr is possible | a b, two g, and an r towel. |
| gbbr is possible | a gb and then a br towel. |
| rrbgbr is possible | an r, rb, g, and br towel. |
| ubwu is impossible | X |
| bwurrg is possible | a bwu, 2 x r, and g towel. |
| brgr is possible | a br, g, and r towel. |
| bbrgwb is impossible | X |

In this example, 6 of the eight designs are possible with the available towel patterns.

## Challenge 1

To get into the onsen as soon as possible, consult your list of towel patterns and desired designs carefully. How many designs are possible?

<hr><br>

# Part Two

The staff don't really like some of the towel arrangements you came up with. To avoid an endless cycle of towel rearrangement, maybe you should just give them every possible option.

## Examples

Here are all of the different ways the previous example's designs can be made:

| Design | Posibble combinations |
|--------|-----------------------|
| brwrr | b, r, wr, r <br> br, wr, r |
| bggr | b, g, g, and r |
| gbbr | g, b, b, r <br> g, b, br <br> gb, b, r <br> gb, br |
| rrbgbr | r, r, b, g, b, r <br> r, r, b, g, br <br> r, r, b, gb, r <br> r, rb, g, b, r <br> r, rb, g, br <br> r, rb, gb, r |
| bwurrg | bwu, r, r, g |
| brgr | b, r, g, r <br> br, g, r |
| ubwu | X |
| bbrgwb | X |


Adding up all of the ways the towels in this example could be arranged into the desired designs yields $16$ $(2 + 1 + 4 + 6 + 1 + 2)$

## Challenge 2

They'll let you into the onsen as soon as you have the list. What do you get if you add up the number of different ways you could make each design?