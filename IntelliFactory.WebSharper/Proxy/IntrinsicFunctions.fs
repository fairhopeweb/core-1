// $begin{copyright}
// 
// This file is part of WebSharper
// 
// Copyright (c) 2008-2011 IntelliFactory
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
    "Microsoft.FSharp.Core.LanguagePrimitives+IntrinsicFunctions, \
     FSharp.Core, Version=2.0.0.0, Culture=neutral, \
     PublicKeyToken=b03f5f7f11d50a3a">]
module private IntelliFactory.WebSharper.IntrinsicFunctionProxy

[<Inline "$array[$index]">]
let GetArray (array: 'T[]) (index: int) = X<'T>

[<Inline "void ($array[$index] = $value)">]
let SetArray (array: 'T[]) (index: int, value: 'T) = X<unit>

[<Inline "$s.charCodeAt($ix)">]
let GetString (s: string) (ix: int) = X<char>

[<Inline "$value">]
let inline UnboxGeneric<'T> (value: obj) = X<'T>
