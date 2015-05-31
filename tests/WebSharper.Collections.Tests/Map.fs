// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

module WebSharper.Collections.Tests.Map

open System
open WebSharper
open WebSharper.Testing

[<JavaScript>]
let Tests =

    Section "Map" {

        Test "Map.add" {
            isFalse (Map.isEmpty (Map.add 1 2 Map.empty))
        }

        Test "Map.remove" {
            let map2 = Map.ofSeq [(1,2); (3,4)]
            let map1 = Map.remove 1 map2 // remove existing element 
            equal map1.Count 1
            let map1 = Map.remove 100 map1 // remove non-existing element
            equal map1.Count 1
            let map0 = Map.remove 3 map1 // remove existing element
            equal map0.Count 0
        }

        Test "Map.empty" {
            isTrue (Map.isEmpty Map.empty)
        }

        Test "Map.exists" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13)]
            isFalse (Map.exists (fun x y -> x = 7 && y = 6) m)
            isFalse (Map.exists (fun _ _ -> true) Map.empty)
            isTrue (Map.exists (fun x y -> x = 3 && y = 6) m)
        }

        Test "Map/array consistency" {
            let a = [| (1, 5); (2, 9); (3, 6); (4, 13) |]
            equal a (Map.toArray <| Map.ofArray a)
            let g = Random.ArrayOf (Random.Tuple2Of (Random.Int, Random.Const ()))
            forRandom 100 g (fun x -> Do {
                let x1 = x |> Map.ofArray |> Map.toArray
                let x2 = x |> Set.ofArray |> Set.toArray
                equal x1 x2
            })
        }

        Test "Map.filter" {
            let even x = x % 2 = 0
            let trueF (x: int) = true
            let falseF (x: int) = false
            let ltF x = x < 10
            let check (xs: (int * unit) []) (f : int -> bool) =
                let xs1 =
                    Map.ofArray xs |> Map.filter (fun k _ -> f k) |> Map.toArray
                let xs2 =
                    xs
                    |> Array.sort
                    |> Seq.distinct
                    |> Seq.filter (fun (k, _) -> f k)
                    |> Seq.toArray
                Do { equal xs2 xs1 }
            forRandom 100
                (Random.ArrayOf (Random.Tuple2Of (Random.Int, Random.Const ())))
                (fun xs -> Do {
                    run (check xs even)
                    run (check xs trueF)
                    run (check xs falseF)
                    run (check xs ltF)
                })
        }

        Test "Map.find" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 67); (6, 6); (7, 8); (8, 9)]
            equal (Map.find 1 m) 5
            equal (Map.find 8 m) 9
            equal (Map.find 3 m) 6
            raises (Map.empty |> Map.find 4)
        }

        Test "Map.findKey" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 4); (6, 9)]
            equal (Map.findKey (fun k v -> k = 6 && v = 9) m) 6
            equal (Map.findKey (fun x y -> y = 13) m) 4
            raises (Map.findKey (fun x y -> y = 100) m)
        }

        Test "Map.fold" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 5); (6, 9)]
            let f x y z = x || (y % 2 = 0 && z % 2 = 0)
            equal (Map.fold (fun x y z -> x + y + z) 0 m) 68
            isFalse (Map.fold f false m)
            isTrue (Map.fold f true m)
            equal (Map.fold (fun x _ _ -> x) 0 m) 0
        }

        Test "Map.fold/foldBack" {
            forRandom 100 (Random.ListOf (Random.Tuple2Of (Random.Int, Random.Int))) (fun x -> Do {
                let f = fun y z x -> x + y + z
                equal (Map.fold f 0 (Map.ofList x))
                    (Map.foldBack f (Map.ofList x) 0)
            })
        }

        Test "Map.foldBack" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 5); (6, 9)]
            let f y z x = x || (y % 2 = 0 && z % 2 = 0)
            equal (Map.foldBack (fun y z x -> x + y + z) m 0) 68
            isFalse (Map.foldBack f m false)
            isTrue (Map.foldBack f m true)
            equal (Map.foldBack (fun _ _ x -> x) m 0) 0
        }

        Test "Map.forall" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 5); (6, 9)]
            isFalse (Map.forall (fun x y -> x = 6 && y = 9) m)
            isFalse (Map.forall (fun x y -> y = 13) m)
            isFalse (Map.forall (<>) m)
            isTrue (Map.forall (<>) Map.empty)
        }

        Test "Map.isEmpty" {
            let emp = Map.remove 1 <| Map.add 1 2 Map.empty
            isTrue (Map.isEmpty Map.empty)
            isFalse (Map.isEmpty (Map.add 1 2 Map.empty))
            isTrue (Map.isEmpty emp)
        }

        Test "Map.partition" {
            forRandom 100 (Random.ListOf (Random.Tuple2Of (Random.Int, Random.Const ()))) (
                fun xs -> Do {
                    let xs = xs |> Set.ofList |> Set.toList
                    let f (k : int) () = k % 2 = 0
                    let (a, b) =
                        List.partition (fun (k, _) -> k % 2 = 0) xs
                    let (x, y) =
                        Map.partition (fun k _ -> k % 2 = 0) (Map.ofList xs)
                    equal (Map.ofList a) x
                    equal (Map.ofList b) y
            })
        }

        Test "Map.pick" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 4); (6, 9)]
            let finder x y = if x = 5 && y = 4 then Some x else None
            equal (Map.pick finder m) 5
        }

        Test "Map.tryFind" {
            let findList = [None; None; Some 4; None]
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 5); (6, 9)]
            let wasFound = Map.tryFind 2 m
            let notFound = Map.tryFind 8 m
            equal wasFound (Some 9)
            equal notFound None
        }

        Test "Map.tryPick" {
            let m = Map.ofList [(1, 5); (2, 9); (3, 6); (4, 13); (5, 4); (6, 9)]
            let finder x y =
                if x = 5 && y = 4 then
                    Some "foo"
                else
                    None
            equal (Map.tryPick finder m) (Some "foo")
            equal (Map.tryPick finder (Map.remove 5 m)) None
        }

    }