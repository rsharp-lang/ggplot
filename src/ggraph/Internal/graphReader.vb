#Region "Microsoft.VisualBasic::afe4f28aac89fb414a9be0f5f16fefd5, G:/GCModeller/src/runtime/ggplot/src/ggraph//Internal/graphReader.vb"

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

    '   Total Lines: 12
    '    Code Lines: 9
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 320 B


    '     Class graphReader
    ' 
    '         Function: getMapData
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph

    Public Class graphReader : Inherits ggplotReader

        Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
            Return MyBase.getMapData(data, env)
        End Function
    End Class
End Namespace
