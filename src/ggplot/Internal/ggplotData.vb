#Region "Microsoft.VisualBasic::85fb7631438fcea5295b4c5305f87e34, ggplot\src\ggplot\Internal\ggplotData.vb"

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

    '   Total Lines: 42
    '    Code Lines: 32
    ' Comment Lines: 0
    '   Blank Lines: 10
    '     File Size: 1.03 KB


    ' Class ggplotData
    ' 
    '     Properties: nsize, x, xscale, y, z
    ' 
    ' Class ggplotAdapter
    ' 
    '     Properties: [error]
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Components

Public Class ggplotData : Inherits ggplotAdapter

    Public Property x As Array
    Public Property y As Array
    Public Property z As Array

    Public ReadOnly Property nsize As Integer
        Get
            If x Is Nothing Then
                Return 0
            Else
                Return x.Length
            End If
        End Get
    End Property

    Public ReadOnly Property xscale As scalers
        Get
            Dim type As Type = x.GetType.GetElementType

            If type Is GetType(String) Then
                Return scalers.ordinal
            Else
                Return scalers.linear
            End If
        End Get
    End Property

    Public Shared Widening Operator CType(ex As Message) As ggplotData
        Return New ggplotData With {.[error] = ex}
    End Operator

End Class

Public Class ggplotAdapter

    Public Property [error] As Message

End Class
