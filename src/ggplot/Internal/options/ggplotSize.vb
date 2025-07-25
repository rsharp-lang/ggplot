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