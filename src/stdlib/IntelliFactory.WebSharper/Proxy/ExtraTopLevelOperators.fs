// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2013 IntelliFactory
//
// GNU Affero General Public License Usage
// WebSharper is free software: you can redistribute it and/or modify it under
// the terms of the GNU Affero General Public License, version 3, as published
// by the Free Software Foundation.
//
// WebSharper is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License
// for more details at <http://www.gnu.org/licenses/>.
//
// If you are unsure which license is appropriate for your use, please contact
// IntelliFactory at http://intellifactory.com/contact.
//
// $end{copyright}

[<IntelliFactory.WebSharper.Core.Attributes.Proxy
    "Microsoft.FSharp.Core.ExtraTopLevelOperators, \
     FSharp.Core, Culture=neutral, \
     PublicKeyToken=b03f5f7f11d50a3a">]
module private IntelliFactory.WebSharper.ExtraTopLevelOperatorsProxy

module M = IntelliFactory.WebSharper.Macro

[<Inline "null">]
let DefaultAsyncBuilder : Control.AsyncBuilder =
    As (AsyncBuilderProxy())

[<JavaScript>]
[<Name "array2D">]
let CreateArray2D (rows : seq<#seq<'T>>) =
    let arr = rows |> Seq.map (Array.ofSeq) |> Array.ofSeq |> As<'T[,]>
    arr?dims <- 2
    arr

[<Inline "+$0">]
let ToDouble<'T> (x: 'T) : double = X

[<Inline; JavaScript>]
let PrintFormatToString fp = Printf.sprintf fp 

[<Inline; JavaScript>]
let PrintFormatToStringThen k fp = Printf.ksprintf k fp 

[<Inline; JavaScript>]
let PrintFormatLine fp = Printf.printfn fp 

[<Inline; JavaScript>]
let PrintFormatToStringThenFail fp = Printf.failwithf fp 
