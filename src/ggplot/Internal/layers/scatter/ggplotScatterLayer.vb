Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports stdNum = System.Math

Namespace layers

    Public MustInherit Class ggplotScatterLayer : Inherits ggplotLayer

        ''' <summary>
        ''' the min cell width/height
        ''' </summary>
        ''' <returns></returns>
        Public Property minCell As Integer = 16

        Protected Function getMeanCell(data As Double(), top_n As Integer) As Double
            Return NumberGroups.diff(data.OrderBy(Function(xi) xi).ToArray) _
                .OrderByDescending(Function(a) a) _
                .Take(top_n) _
                .Average
        End Function

        Protected Function getCellsize(x As Double(), y As Double()) As SizeF
            ' evaluate cells grid
            Dim topN As Integer = stdNum.Min(x.Length / 5, 100)
            Dim cellWidth = getMeanCell(x, topN)
            Dim cellHeight = getMeanCell(y, topN)

            Return New SizeF(cellWidth, cellHeight)
        End Function
    End Class
End Namespace