#Region "Microsoft.VisualBasic::4934fddfecbbd8cf30006d0a2c5073c3, src\ggplot\Internal\ggplotData.vb"

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
    '    Code Lines: 34 (65.38%)
    ' Comment Lines: 7 (13.46%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 11 (21.15%)
    '     File Size: 1.24 KB


    ' Class ggplotData
    ' 
    '     Properties: fill, nsize, x, xscale, y
    '                 z
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Components

''' <summary>
''' the plot data source in ggplot
''' </summary>
Public Class ggplotData : Inherits ggplotAdapter

    Public Property x As axisMap
    Public Property y As axisMap
    Public Property z As axisMap

    ''' <summary>
    ''' fill class groups 
    ''' </summary>
    ''' <returns></returns>
    Public Property fill As axisMap

    Public ReadOnly Property nsize As Integer
        Get
            Return x.size
        End Get
    End Property

    Public ReadOnly Property xscale As scalers
        Get
            Dim type As MapperTypes = x.mapper

            If type = MapperTypes.Discrete Then
                Return scalers.ordinal
            Else
                Return scalers.linear
            End If
        End Get
    End Property

    Sub New()
    End Sub

    Sub New(x As axisMap, y As axisMap, Optional z As axisMap = Nothing)
        Me.x = x
        Me.y = y
        Me.z = z
    End Sub

    Public Shared Widening Operator CType(ex As Message) As ggplotData
        Return New ggplotData With {.[error] = ex}
    End Operator

End Class
