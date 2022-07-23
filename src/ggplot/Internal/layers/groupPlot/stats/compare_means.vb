Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    Public Class compare_means

        Public Property y As String
        Public Property group1 As String
        Public Property group2 As String
        Public Property pvalue As Double
        Public Property padj As Double

        Public Const ns As String = "n.sig"

        Public ReadOnly Property psignif As String
            Get
                If pvalue.ToString = "0" Then
                    Return $"*****(<1e-100)"
                End If

                If pvalue <= 0.00001 Then
                    Return $"*****({pvalue.ToString("G3")})"
                ElseIf pvalue <= 0.0001 Then
                    Return $"****({pvalue.ToString("G3")})"
                ElseIf pvalue <= 0.001 Then
                    Return $"***({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.01 Then
                    Return $"**({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.05 Then
                    Return $"*({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.1 Then
                    Return "."
                Else
                    Return "n.sig"
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return psignif
        End Function

        Public Shared Function fromManualData(data As dataframe) As compare_means()
            Dim group1 As String() = data.getColumnVector("group1")
            Dim group2 As String() = data.getColumnVector("group2")
            Dim pvalue As Double() = data.getColumnVector("pvalue")

            Return pvalue _
                .Select(Function(p, i)
                            Return New compare_means With {
                                .group1 = group1(i),
                                .group2 = group2(i),
                                .pvalue = p,
                                .padj = p
                            }
                        End Function) _
                .ToArray
        End Function

    End Class
End Namespace