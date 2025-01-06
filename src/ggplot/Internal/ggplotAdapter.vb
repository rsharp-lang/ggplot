#Region "Microsoft.VisualBasic::ed18b8971344c62a489d277a19a4ce7f, src\ggplot\Internal\ggplotAdapter.vb"

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

'   Total Lines: 7
'    Code Lines: 4 (57.14%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 3 (42.86%)
'     File Size: 125 B


' Class ggplotAdapter
' 
'     Properties: [error]
' 
' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports ggplot.layers
Imports SMRUCC.Rsharp.Runtime.Components

Public Class ggplotAdapter

    Public Property [error] As Message

    Public Shared Iterator Function getLayers(ggplot As ggplot) As IEnumerable(Of ggplotLayer)
        Dim source As IEnumerable(Of ggplotLayer) = If(
            ggplot.UnionGgplotLayers Is Nothing,
            ggplot.layers,
            ggplot.UnionGgplotLayers(ggplot.layers)
        )

        For Each layer As ggplotLayer In source
            Call layer.initDataSet(ggplot:=ggplot)
            Yield layer
        Next
    End Function

    Private Shared Function getAxisLayerData(layers As IEnumerable(Of ggplotLayer), getter As Func(Of ggplotLayer, axisMap)) As axisMap
        Dim datas = layers.Select(Function(i) i.data).ToArray

    End Function

    Public Shared Function getXAxis(layers As IEnumerable(Of ggplotLayer), baseData As ggplotData) As axisMap
        If baseData.x Is Nothing Then
            ' use custom data from each layer
            Return getAxisLayerData(layers, Function(i) i.data.x)
        Else
            Return baseData.x
        End If
    End Function

    Public Shared Function getYAxis(layers As IEnumerable(Of ggplotLayer), baseData As ggplotData) As axisMap
        If baseData.y Is Nothing Then
            ' use custom data from each layer
            Return getAxisLayerData(layers, Function(i) i.data.y)
        Else
            Return baseData.y
        End If
    End Function

    Public Shared Function getZAxis(layers As IEnumerable(Of ggplotLayer), baseData As ggplotData) As axisMap
        If baseData.z Is Nothing Then
            ' use custom data from each layer
            Return getAxisLayerData(layers, Function(i) i.data.z)
        Else
            Return baseData.z
        End If
    End Function

End Class
