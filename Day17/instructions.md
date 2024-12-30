# Day 17: Chronospatial Computer

The Historians push the button on their strange device, but this time, you all just feel like you're falling.

"Situation critical", the device announces in a familiar voice. "Bootstrapping process failed. Initializing debugger...."

The small handheld device suddenly unfolds into an entire computer! The Historians look around nervously before one of them tosses it to you.

## Rules

This seems to be a `3-bit computer`: its program is a list of `3-bit numbers` (`0` through `7`), like $0,1,2,3$. The computer also has three registers named A, B, and C, but these registers aren't limited to 3 bits and can instead hold any integer.

The computer knows `eight instructions`, each identified by a `3-bit number` (called the instruction's `opcode`). Each instruction also reads the `3-bit number` after it as an input; this is called its `operand`.

A number called the instruction pointer identifies the position in the program from which the next `opcode` will be read; it starts at `0`, pointing at the first `3-bit number` in the program. Except for jump instructions, the instruction pointer increases by 2 after each instruction is processed (to move past the instruction's `opcode` and its `operand`). If the computer tries to read an `opcode` past the end of the program, it instead halts.

So, the program 0,1,2,3 would run:
- The instruction with `opcode` is 0
    - Pass it the `operand` 1
- Run the instruction with `opcode` 2
    - Pass it the `operand` 3
- Halt

### Operands

There are two types and each instruction specifies the type of its `operand`. 

| Operand Type | Operand Value |
|--------------|---------------|
| Literal Operand | The value of the operand itself <br> i.e. Operand 7 has value 7 |
| Combo Operand 0 - 3 | Literal values 0 - 3 |
| Combo Operand 4 | Value of register A |
| Combo Operand 5 | Value of register B |
| Combo Operand 6 | Value of register C |
| Combo Operand 7 | Reserved <br> Should not appear in valid programs |

### Instructions

The eight instructions are as follows:

| Instruction | Behaviour |
|-------------|-----------|
| adv / opCode 0 <br> Combo Operand | Division <br><br> Numerator: value in register A <br> Denominator: raise 2 to the power of the operand <br><br> Truncated to integer and stored in register A |
| bxl / opCode 1 <br> Literal Operand | Bitwise XOR of register B and the operand <br><br> Result stored in register B |
| bst / opCode 2 <br> Combo Operand | Operand modulo 8 <br> Will leave the lowest 3 bits <br><br> Result stored in register B |
| jnz / opCode 3 <br> Literal Operand | Register A is 0 -> nothing <br><br> Will jump by setting the instruction pointer to the value of its operand <br><br> If this instruction jumps, the instruction pointer is not increased by 2 after this instruction |
| bxc / opCode 4 <br> Operand Ignored | XOR of register B and register C <br><br> Result stored in register B |
| out / opCode 5 <br> Combo Operand | Operand modulo 8 <br><br> Outputs the value |
| bdv / opCode 6 <br> Combo Operand | Same as adv but stored in register B |
| cdv / opCode 7 <br> Combo Operand | Same as adv but stored in register C |

>If there are multiple outputs, they will be comma separated.

## Examples

### adv

- An operand of $2$ would divide A by 4 $(2^2)$
- An operand of $5$ would divide A by $2^B$

| Scenario | Result |
|----------|--------|
| Register C contains $9$ <br> program $2,6$ | Sets register B to $1$ |
| Register A contains $10$ <br> program $5,0,5,1,5,4$ | output $0,1,2$ |
| Register A contains $2024$ <br> program $0,1,5,4,3,0$ | output $4,2,5,6,7,7,7,7,3,1,0$ <br> $0$ in register A |
| Register B contains $29$ <br> program $1,7$ | Sets register B to $26$ |
| Register B contains $2024$ <br> register C contains $43690$ <br> program $4,0$ | Sets register B to $44354$ |

The Historians' strange device has finished initializing its debugger and is displaying some information about the program it is trying to run (your puzzle input).

- Register A: $729$
- Register B: $0$
- Register C: $0$

- Program: $0,1,5,4,3,0$

Your first task is to determine what the program is trying to output. To do this, initialize the registers to the given values, then run the given program, collecting any output produced by out instructions. (Always join the values produced by out instructions with commas.) After the above program halts, its final output will be $4,6,3,5,6,3,5,2,1,0$.

## Challenge 1

Using the information provided by the debugger, initialize the registers to the given values, then run the program. Once it halts, what do you get if you use commas to join the values it output into a single string?

# Part Two

Digging deeper in the device's manual, you discover the problem: this program is supposed to output another copy of the program! Unfortunately, the value in register A seems to have been corrupted. You'll need to find a new value to which you can initialize register A so that the program's output instructions produce an exact copy of the program itself.

For example:

    Register A: 2024
    Register B: 0
    Register C: 0

    Program: 0,3,5,4,3,0
    
This program outputs a copy of itself if register A is instead initialized to 117440. (The original initial value of register A, 2024, is ignored.)

What is the lowest positive initial value for register A that causes the program to output a copy of itself?