#Region "Microsoft.VisualBasic::d3a6dd9f147977c2dcc48c56882799e3, src\ggraph\zzz.vb"

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

    '   Total Lines: 14
    '    Code Lines: 11 (78.57%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 3 (21.43%)
    '     File Size: 405 B


    ' Class zzz
    ' 
    '     Sub: onLoad
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Diagnostics.CodeAnalysis
Imports ggplot.ggraph
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>
<Assembly: SuppressMessage("", "CA1416")>

Public Class zzz

    Public Shared Sub onLoad()
        Call ggplot.Register(GetType(NetworkGraph), Function(theme) New graphRender(theme))
    End Sub
End Class
