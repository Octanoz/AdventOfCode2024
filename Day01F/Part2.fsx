// let input = System.IO.File.ReadAllLines @"Day01F\example1.txt" |> List.ofSeq
let input = System.IO.File.ReadAllLines @"Day01F\input.txt" |> List.ofSeq

let parse (lines : string list) =
    lines
    |> List.map (fun line -> line.Split "   " |> Array.map int |> Array.toList)
    |> List.transpose
    
let [first; second] = parse input

let numberCounts = second |> List.countBy id |> Map.ofList

let counts = [ for element in first -> (element, numberCounts |> Map.tryFind element |> Option.defaultValue 0) ]

let result = counts |> List.map (fun (x,y) -> x*y) |> List.sum 
