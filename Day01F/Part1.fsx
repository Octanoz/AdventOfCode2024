// let input = System.IO.File.ReadAllLines @"Day01F\example1.txt" |> List.ofSeq
let input = System.IO.File.ReadAllLines @"Day01F\input.txt" |> List.ofSeq

let parse (lines : string list) =
    lines
    |> List.map (fun line -> line.Split "   " |> Array.map int |> Array.toList)
    |> List.transpose
    
let [first; second] = parse input

let sortedFirst = first |> List.sort
let sortedSecond = second |> List.sort

let difference =
    List.zip sortedFirst sortedSecond
    |> List.map (fun (x,y) -> abs (x - y))
    |> List.sum