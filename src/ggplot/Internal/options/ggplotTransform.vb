Namespace options

    ''' <summary>
    ''' A monotonic transformation which is applied to the data of an axis
    ''' before the axis scale is created.
    '''
    ''' ggplot2 implements the log/sqrt/date scales in this way: the plot data
    ''' and the axis ticks share the same transformation, so that the reader
    ''' sees a logarithmically spaced axis.
    '''
    ''' The transformation is declared by the ``scale_x_*``/``scale_y_*``
    ''' functions and is written into the scale_trans_x/scale_trans_y plot
    ''' arguments, which are applied by the 2D scale builder.
    ''' </summary>
    Public MustInherit Class ggplotTransform : Inherits ggplotOption

        Public Property name As String = "identity"
        ''' <summary>
        ''' the target axis: x or y
        ''' </summary>
        Public Property axis As String = "x"

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not ggplot.args Is Nothing Then
                ggplot.args.slots("scale_trans_" & axis) = Me
            End If

            Return ggplot
        End Function

        ''' <summary>
        ''' transform a single value
        ''' </summary>
        Public Overridable Function Apply(value As Double) As Double
            Return value
        End Function

        ''' <summary>
        ''' transform an array of values
        ''' </summary>
        Public Function Apply(values As Double()) As Double()
            If values Is Nothing Then Return values

            Dim buffer As Double() = New Double(values.Length - 1) {}

            For i As Integer = 0 To values.Length - 1
                buffer(i) = Apply(values(i))
            Next

            Return buffer
        End Function

        Public Overrides Function ToString() As String
            Return name
        End Function
    End Class

    ''' <summary>
    ''' the identity transformation
    ''' </summary>
    Public Class ggplotTransformIdentity : Inherits ggplotTransform
        Public Sub New()
            name = "identity"
        End Sub
    End Class

    ''' <summary>
    ''' the log transformation, log(value, base)
    '''
    ''' The non positive values can not be represented on a log axis, they are
    ''' transformed into NaN and then dropped, so that the axis range stays
    ''' finite.
    ''' </summary>
    Public Class ggplotTransformLog : Inherits ggplotTransform

        Public Property [base] As Double = 10

        Public Sub New()
            name = "log"
        End Sub

        Public Overrides Function Apply(value As Double) As Double
            If value <= 0 Then
                Return Double.NaN
            End If

            Return Math.Log(value) / Math.Log([base])
        End Function
    End Class

    ''' <summary>
    ''' the square root transformation
    ''' </summary>
    Public Class ggplotTransformSqrt : Inherits ggplotTransform

        Public Sub New()
            name = "sqrt"
        End Sub

        Public Overrides Function Apply(value As Double) As Double
            If value < 0 Then
                Return Double.NaN
            End If

            Return Math.Sqrt(value)
        End Function
    End Class

    ''' <summary>
    ''' the reverse transformation, upper + lower - value
    ''' </summary>
    Public Class ggplotTransformReverse : Inherits ggplotTransform

        Public Property upper As Double = 0
        Public Property lower As Double = 0
        Public Property enabled As Boolean = True

        Public Sub New()
            name = "reverse"
        End Sub

        Public Overrides Function Apply(value As Double) As Double
            If Not enabled Then Return value

            Return upper + lower - value
        End Function
    End Class

    ''' <summary>
    ''' the date transformation, which maps a date to the elapsed days since
    ''' the unix epoch, so that a date can be placed on a continuous axis.
    ''' </summary>
    Public Class ggplotTransformDate : Inherits ggplotTransform

        Public Property date_labels As String = "%Y-%m-%d"
        Public Property date_breaks As String = ""

        Public Sub New()
            name = "date"
        End Sub

        Public Overrides Function Apply(value As Double) As Double
            Return value
        End Function

        ''' <summary>
        ''' convert an elapsed day number back to the date string
        ''' </summary>
        Public Function Format(value As Double) As String
            Try
                Return DateTimeOffset _
                    .FromUnixTimeMilliseconds(CLng(value * 86400000)) _
                    .ToString(date_labels)
            Catch
                Return value.ToString
            End Try
        End Function

        ''' <summary>
        ''' convert a date string to the elapsed days since the unix epoch
        ''' </summary>
        Public Shared Function Parse(value As String) As Double
            Dim parsed As DateTimeOffset

            If DateTimeOffset.TryParse(value, parsed) Then
                Return parsed.ToUnixTimeMilliseconds / 86400000
            End If

            Return Double.NaN
        End Function
    End Class
End Namespace
