Imports System.Drawing
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace options

    ''' <summary>
    ''' The alpha(transparency) scale of the plot.
    '''
    ''' + scale_alpha(range): map the alpha aesthetic linearly into the given range
    ''' + scale_alpha_manual(values): map the alpha aesthetic onto the given values
    '''
    ''' The resolved alpha of each observation is applied on top of the color
    ''' alpha channel of the plot layer.
    ''' </summary>
    Public MustInherit Class ggplotAlphaScale : Inherits ggplotOption

        Public Property name As String = "alpha"

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not ggplot.args Is Nothing Then
                ggplot.args.slots("scale_alpha") = Me
            End If

            Return ggplot
        End Function

        ''' <summary>
        ''' resolve the alpha value of each observation
        ''' </summary>
        Public Overridable Function Resolve(values As Double()) As Double()
            Return values
        End Function

        Protected Shared Function linear(values As Double(), lower As Double, upper As Double) As Double()
            If values.IsNullOrEmpty Then Return values

            Dim valid As Double() = values.Where(Function(v) Not Double.IsNaN(v)).ToArray

            If valid.IsNullOrEmpty Then Return values

            Dim min As Double = valid.Min()
            Dim max As Double = valid.Max()

            If max <= min Then
                Dim mid As Double = (lower + upper) / 2

                Return values.Select(Function(v) mid).ToArray
            End If

            Return values _
                .Select(Function(v) mapLinear(v, min, max, lower, upper)) _
                .ToArray
        End Function

        Private Shared Function mapLinear(v As Double, min As Double, max As Double, lower As Double, upper As Double) As Double
            If Double.IsNaN(v) Then
                Return (lower + upper) / 2
            End If

            Return lower + (upper - lower) * (v - min) / (max - min)
        End Function
    End Class

    ''' <summary>
    ''' scale_alpha(range): maps the alpha aesthetic linearly into the range
    ''' </summary>
    Public Class ggplotAlphaRange : Inherits ggplotAlphaScale

        Public Property lower As Double = 0.1
        Public Property upper As Double = 1

        Public Sub New()
            name = "alpha"
        End Sub

        Public Overrides Function Resolve(values As Double()) As Double()
            Return linear(values, lower, upper)
        End Function
    End Class

    ''' <summary>
    ''' scale_alpha_manual(values): maps the alpha aesthetic onto the given
    ''' values
    ''' </summary>
    Public Class ggplotAlphaManual : Inherits ggplotAlphaScale

        Public Property values As Double()

        Public Sub New()
            name = "alpha"
        End Sub

        Public Overrides Function Resolve(values As Double()) As Double()
            If Me.values.IsNullOrEmpty OrElse values.IsNullOrEmpty Then
                Return values
            End If

            Dim index As New Dictionary(Of Double, Double)

            For Each v As Double In Me.values
                If Not index.ContainsKey(v) Then
                    Call index.Add(v, v)
                End If
            Next

            Dim fallback As Double = Me.values(Me.values.Length \ 2)

            Return values _
                .Select(Function(v) lookup(index, v, fallback)) _
                .ToArray
        End Function

        Private Shared Function lookup(index As Dictionary(Of Double, Double), v As Double, fallback As Double) As Double
            If index.ContainsKey(v) Then
                Return index(v)
            End If

            Return fallback
        End Function
    End Class
End Namespace
