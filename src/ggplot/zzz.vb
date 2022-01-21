#Region "Microsoft.VisualBasic::05c9d34ade80083e8a832c489c286e7f, src\zzz.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

' Class zzz
' 
'     Sub: onLoad
' 
' /********************************************************************************/

#End Region

Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime

<Assembly: RPackageModule>

Public Class zzz

    Public Shared Sub onLoad()
        Call Internal.generic.add("plot", GetType(ggplot), AddressOf plotGGplot)
    End Sub

    Private Shared Function plotGGplot(ggplot As ggplot, args As list, env As Environment) As Object
        For Each arg In args.slots
            Call ggplot.args.setByName(arg.Key, arg.Value, env)
        Next

        Return ggplot
    End Function
End Class

