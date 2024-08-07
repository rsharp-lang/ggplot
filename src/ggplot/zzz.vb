﻿#Region "Microsoft.VisualBasic::466e7560a33dd46834c75a289693078c, src\ggplot\zzz.vb"

    ' Author:
    ' 
    '       xieguigang (I@xieguigang.me)
    ' 
    ' Copyright (c) 2021 R# language
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 45
    '    Code Lines: 36 (80.00%)
    ' Comment Lines: 1 (2.22%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (17.78%)
    '     File Size: 1.47 KB


    ' Class zzz
    ' 
    '     Function: plotGGplot
    ' 
    '     Sub: onLoad
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Diagnostics.CodeAnalysis
Imports System.Drawing
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal
Imports SMRUCC.Rsharp.Runtime.Internal.Invokes
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>

<Assembly: SuppressMessage("", "CA1416")>

Public Class zzz

    Public Shared Sub onLoad()
        Call Internal.generic.add("plot", GetType(ggplot), AddressOf plotGGplot)
    End Sub

    Public Shared Function plotGGplot(ggplot As ggplot, args As list, env As Environment) As Object
        For Each arg In args.slots
            Call ggplot.args.setByName(arg.Key, arg.Value, env)
        Next

        If Invokes.graphics.curDev.isEmpty Then
            Return ggplot
        Else
            ' draw on current graphics context
            Dim dev As graphicsDevice = curDev
            Dim padding As Padding = Padding.TryParse(ggplot.ggplotTheme.padding)
            Dim size As Size = dev.g.Size
            Dim layout As New Rectangle With {
                .X = padding.Left,
                .Y = padding.Top,
                .Width = size.Width - padding.Horizontal,
                .Height = size.Height - padding.Vertical
            }

            ggplot.clearCanvas = False
            ggplot.Plot(dev.g, layout)
        End If

        Return Nothing
    End Function
End Class
