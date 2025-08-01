#Region "Microsoft.VisualBasic::f22e7fa8f3ef1ba13dd7e70ae4543022, src\ggplot\Internal\options\ggplotSize.vb"

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

    '   Total Lines: 56
    '    Code Lines: 40 (71.43%)
    ' Comment Lines: 5 (8.93%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 11 (19.64%)
    '     File Size: 1.89 KB


    '     Class ggplotSize
    ' 
    '         Properties: range, unify
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: Config, getSizeValues
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace options

    Public Class ggplotSize : Inherits ggplotOption

        Public Property range As DoubleRange
        Public Property unify As Single

        Sub New(size As Single)
            Me.unify = size
        End Sub

        Sub New(min As Single, max As Single)
            Me.range = New DoubleRange(min, max)
        End Sub

        Public Iterator Function getSizeValues(values As IEnumerable(Of Double)) As IEnumerable(Of Single)
            Dim alldata As Double() = values.ToArray

            If range Is Nothing Then
                For i As Integer = 0 To alldata.Length - 1
                    Yield unify
                Next
            Else
                Dim valueRange As New DoubleRange(alldata)

                For Each xi As Double In alldata
                    Yield valueRange.ScaleMapping(xi, _range)
                Next
            End If
        End Function

        ''' <summary>
        ''' config the size value for the ggplot layer
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Dim last As ggplotLayer = ggplot.layers.LastOrDefault

            If last Is Nothing Then
                Throw New InvalidOperationException("there is no ggplot layer for config the size value")
            End If
            If last.GetType.ImplementInterface(Of IggplotSize) Then
                DirectCast(last, IggplotSize).size = Me
            Else
                Call $"the ggplot layer {last.GetType.Name} could not config size value.".Warning
            End If

            Return ggplot
        End Function
    End Class
End Namespace
