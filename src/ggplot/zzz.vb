﻿#Region "Microsoft.VisualBasic::e9ec5bca011965d715bd5d8f36bc20d0, src\ggplot\zzz.vb"

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

    '   Total Lines: 52
    '    Code Lines: 41 (78.85%)
    ' Comment Lines: 2 (3.85%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (17.31%)
    '     File Size: 1.67 KB


    ' Class zzz
    ' 
    '     Function: plotGGplot
    ' 
    '     Sub: onLoad
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports R_graphics.Common.Runtime
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

<Assembly: RPackageModule>
' <Assembly: SuppressMessage("", "CA1416")>

Public Class zzz

    Public Shared Sub onLoad()
        Call RInternal.generic.add("plot", GetType(ggplot), AddressOf plotGGplot)

#If NET48 Then

#Else
        Call SkiaDriver.Register()
#End If
    End Sub

    Public Shared Function plotGGplot(ggplot As ggplot, args As list, env As Environment) As Object
        For Each arg In args.slots
            Call ggplot.args.setByName(arg.Key, arg.Value, env)
        Next

        If curDev.isEmpty Then
            Return ggplot
        Else
            ' draw on current graphics context
            Dim dev As graphicsDevice = curDev
            Dim size As Size = dev.g.Size
            Dim css As CSSEnvirnment = dev.g.LoadEnvironment
            Dim margin As PaddingLayout = PaddingLayout.EvaluateFromCSS(css, Padding.TryParse(ggplot.ggplotTheme.padding))
            Dim layout As New Rectangle With {
                .X = margin.Left,
                .Y = margin.Top,
                .Width = size.Width - margin.Horizontal,
                .Height = size.Height - margin.Vertical
            }

            ggplot.clearCanvas = False
            ggplot.Plot(dev.g, layout)
        End If

        Return Nothing
    End Function
End Class
