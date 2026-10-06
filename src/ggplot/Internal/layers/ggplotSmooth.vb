Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Bootstrapping
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotSmooth : Inherits ggplotLayer

        Public Property method As String = "lm"
        Public Property formula As String
        Public Property se As Boolean = True
        Public Property level As Double = 0.95
        Public Property span As Double = 0.75
        Public Property degree As Integer = 1
        Public Property npoints As Integer = 128
        Public Property lineend As String = "butt"
        Public Property line_width As Single = 1
        Public Property nmax As Integer = 600

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim plt As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim reader As ggplotReader = If(useCustomData, Me.reader, plt.base.reader)
            Dim source As Object = If(useCustomData, dataset, plt.data)
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim y As Double() = stream.y

            If x.Length = 0 OrElse y.Length <> x.Length Then Return Nothing

            Dim groups As String() = resolveGroups(reader, source, x.Length, plt.environment)

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="black", alpha:=alpha, env:=plt.environment)
            End If

            Dim colors As Func(Of Object, String) = colorMap.ColorHandler(plt, groups.Distinct.ToArray)
            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            For Each group As NamedCollection(Of Integer) In groupIndexes(groups)
                Dim gx As Double() = group.value.Select(Function(i) x(i)).ToArray
                Dim gy As Double() = group.value.Select(Function(i) y(i)).ToArray

                If gx.Length < 2 Then Continue For

                Dim grid As Double() = linspace(gx.Min(), gx.Max(), npoints)
                Dim fit As Double()
                Dim lower As Double()
                Dim upper As Double()
                Dim fitMethod As String = If(method Is Nothing, "lm", method.Trim.ToLower)

                Select Case fitMethod
                    Case "loess"
                        fit = grid.Select(Function(ix) fitLoess(gx, gy, ix, span, degree)).ToArray
                    Case "glm"
                        fit = grid.Select(Function(ix) logisticFit(gx, gy, ix)).ToArray
                    Case Else
                        Dim result As FitResult

                        If fitMethod = "poly" Then
                            result = LeastSquares.PolyFit(gx, gy, Math.Max(degree, 2))
                        Else
                            result = LeastSquares.LinearFit(gx, gy)
                        End If

                        fit = grid.Select(Function(ix) result(ix)).ToArray

                        If se Then
                            Dim band As Double() = leverageBand(gx, result, grid, level)
                            lower = band.Take(npoints).ToArray
                            upper = band.Skip(npoints).ToArray
                        End If
                End Select

                If fit.IsNullOrEmpty Then Continue For

                If se AndAlso lower Is Nothing Then
                    Dim band As Double() = residualBand(gx, gy, fit, level)
                    lower = band.Take(npoints).ToArray
                    upper = band.Skip(npoints).ToArray
                End If

                If se AndAlso Not lower Is Nothing AndAlso Not upper Is Nothing Then
                    Dim band As PointF() = grid _
                        .Select(Function(ix, i) stream.scale.Translate(New PointF(ix, upper(i)))) _
                        .Concat(grid.Select(Function(ix, i) stream.scale.Translate(New PointF(ix, lower(i)))).Reverse()) _
                        .ToArray

                    Using fill As New SolidBrush(colors(group.name).TranslateColor.Alpha(CSng(80 * alpha)))
                        Call g.FillPolygon(fill, band)
                    End Using
                End If

                Dim curve As PointF() = grid _
                    .Select(Function(ix, i) stream.scale.Translate(New PointF(ix, fit(i)))) _
                    .ToArray

                For i As Integer = 1 To curve.Length - 1
                    Call g.DrawLine(pen, curve(i - 1), curve(i))
                Next
            Next

            Return Nothing
        End Function

        Private Shared Function leverageBand(x As Double(), fit As FitResult, grid As Double(), level As Double) As Double()
            Dim n As Double = x.Length
            Dim xbar As Double = x.Average
            Dim sxx As Double = 0

            For Each v As Double In x
                sxx += (v - xbar) * (v - xbar)
            Next

            Dim t As Double = tQuantile(level, n - 2)
            Dim sigma As Double = fit.RMSE
            Dim lower As Double() = New Double(grid.Length - 1) {}
            Dim upper As Double() = New Double(grid.Length - 1) {}

            For i As Integer = 0 To grid.Length - 1
                Dim leverage As Double = 1 / n

                If sxx > 0 Then
                    leverage += (grid(i) - xbar) * (grid(i) - xbar) / sxx
                End If

                Dim half As Double = t * sigma * Math.Sqrt(leverage)
                Dim yhat As Double = fit(grid(i))

                lower(i) = yhat - half
                upper(i) = yhat + half
            Next

            Return lower.JoinIterates(upper).ToArray
        End Function

        Private Shared Function residualBand(x As Double(), y As Double(), fit As Double(), level As Double) As Double()
            Dim n As Double = x.Length
            Dim sigma As Double = 0

            For i As Integer = 0 To x.Length - 1
                sigma += (y(i) - fit(i)) * (y(i) - fit(i))
            Next

            sigma = Math.Sqrt(sigma / Math.Max(n - 1, 1))
            Dim t As Double = tQuantile(level, n - 2)
            Dim lower As Double() = New Double(fit.Length - 1) {}
            Dim upper As Double() = New Double(fit.Length - 1) {}

            For i As Integer = 0 To fit.Length - 1
                lower(i) = fit(i) - t * sigma
                upper(i) = fit(i) + t * sigma
            Next

            Return lower.JoinIterates(upper).ToArray
        End Function

        Private Shared Function tQuantile(level As Double, df As Double) As Double
            If df <= 0 Then Return 1.96

            Select Case df
                Case Is < 2 : Return 12.706
                Case 2 : Return 4.303
                Case 3 : Return 3.182
                Case 4 : Return 2.776
                Case 5 : Return 2.571
                Case 6 : Return 2.447
                Case 7 : Return 2.365
                Case 8 : Return 2.306
                Case 9 : Return 2.262
                Case 10 : Return 2.228
                Case Is < 15 : Return 2.131
                Case Is < 20 : Return 2.093
                Case Is < 25 : Return 2.064
                Case Is < 30 : Return 2.045
                Case Is < 40 : Return 2.023
                Case Is < 60 : Return 2.000
                Case Is < 120 : Return 1.980
                Case Else : Return 1.960
            End Select
        End Function

        ''' <summary>
        ''' the local polynomial(loess) fit at a single evaluation point
        ''' </summary>
        Private Shared Function fitLoess(x As Double(), y As Double(), x0 As Double, bandwidth As Double, degree As Integer) As Double
            Dim n As Integer = x.Length
            Dim k As Integer = Math.Max(Math.Min(CInt(Math.Ceiling(bandwidth * n)), n), 2)
            Dim distances As Double() = x.Select(Function(v) Math.Abs(v - x0)).ToArray
            Dim h As Double = distances.OrderBy(Function(d) d)(k - 1)

            If h <= 0 Then h = 1E-12

            Dim weighted As New List(Of Tuple(Of Double, Double, Double))

            For i As Integer = 0 To n - 1
                Dim u As Double = Math.Abs(x(i) - x0) / h

                If u >= 1 Then Continue For

                Dim w As Double = (1 - u * u * u) ^ 3

                If w > 0 Then
                    Call weighted.Add(Tuple.Create(x(i), y(i), w))
                End If
            Next

            If weighted.Count < 2 Then Return Double.NaN

            If weighted.Count = 2 OrElse degree <= 1 Then
                Return linearFit(weighted, x0)
            End If

            Return quadraticFit(weighted, x0)
        End Function

        Private Shared Function linearFit(data As List(Of Tuple(Of Double, Double, Double)), x0 As Double) As Double
            Dim sw As Double = 0
            Dim sx As Double = 0
            Dim sy As Double = 0
            Dim sxx As Double = 0
            Dim sxy As Double = 0

            For Each t As Tuple(Of Double, Double, Double) In data
                Dim w As Double = t.Item3
                sw += w
                sx += w * t.Item1
                sy += w * t.Item2
                sxx += w * t.Item1 * t.Item1
                sxy += w * t.Item1 * t.Item2
            Next

            Dim denominator As Double = sw * sxx - sx * sx

            If denominator = 0 Then Return sy / sw

            Dim slope As Double = (sw * sxy - sx * sy) / denominator

            Return (sy - slope * sx) / sw + slope * x0
        End Function

        Private Shared Function quadraticFit(data As List(Of Tuple(Of Double, Double, Double)), x0 As Double) As Double
            Dim s0 As Double = 0, s1 As Double = 0, s2 As Double = 0, s3 As Double = 0, s4 As Double = 0
            Dim t0 As Double = 0, t1 As Double = 0, t2 As Double = 0

            For Each item As Tuple(Of Double, Double, Double) In data
                Dim xv As Double = item.Item1
                Dim yv As Double = item.Item2
                Dim w As Double = item.Item3

                s0 += w
                s1 += w * xv
                s2 += w * xv * xv
                s3 += w * xv * xv * xv
                s4 += w * xv * xv * xv * xv
                t0 += w * yv
                t1 += w * xv * yv
                t2 += w * xv * xv * yv
            Next

            Dim m As Double(,) = New Double(2, 2) {}
            Dim v As Double() = New Double() {t0, t1, t2}

            m(0, 0) = s0 : m(0, 1) = s1 : m(0, 2) = s2
            m(1, 0) = s1 : m(1, 1) = s2 : m(1, 2) = s3
            m(2, 0) = s2 : m(2, 1) = s3 : m(2, 2) = s4

            Dim solution As Double() = solve(m, v)

            If solution Is Nothing Then Return Double.NaN

            Return solution(0) + solution(1) * x0 + solution(2) * x0 * x0
        End Function

        ''' <summary>
        ''' solve a small dense linear system by the gaussian elimination
        ''' </summary>
        Private Shared Function solve(m As Double(,), v As Double()) As Double()
            Dim n As Integer = v.Length
            Dim a As Double(,) = DirectCast(m.Clone(), Double(,))
            Dim b As Double() = DirectCast(v.Clone(), Double())

            For k As Integer = 0 To n - 1
                Dim pivot As Integer = k

                For i As Integer = k + 1 To n - 1
                    If Math.Abs(a(i, k)) > Math.Abs(a(pivot, k)) Then
                        pivot = i
                    End If
                Next

                If Math.Abs(a(pivot, k)) < 1E-14 Then Return Nothing

                If pivot <> k Then
                    For j As Integer = k To n - 1
                        Dim tmp As Double = a(k, j)
                        a(k, j) = a(pivot, j)
                        a(pivot, j) = tmp
                    Next

                    Dim tb As Double = b(k)
                    b(k) = b(pivot)
                    b(pivot) = tb
                End If

                For i As Integer = k + 1 To n - 1
                    Dim factor As Double = a(i, k) / a(k, k)

                    For j As Integer = k To n - 1
                        a(i, j) -= factor * a(k, j)
                    Next

                    b(i) -= factor * b(k)
                Next
            Next

            Dim result As Double() = New Double(n - 1) {}

            For i As Integer = n - 1 To 0 Step -1
                Dim sum As Double = b(i)

                For j As Integer = i + 1 To n - 1
                    sum -= a(i, j) * result(j)
                Next

                result(i) = sum / a(i, i)
            Next

            Return result
        End Function

        ''' <summary>
        ''' the binomial logistic regression, fitted by the iteratively
        ''' reweighted least squares
        ''' </summary>
        Private Shared Function logisticFit(x As Double(), y As Double(), x0 As Double) As Double
            Dim n As Integer = x.Length
            Dim xbar As Double = x.Average
            Dim scale As Double = 0

            For Each v As Double In x
                scale = Math.Max(scale, Math.Abs(v - xbar))
            Next

            If scale = 0 Then scale = 1

            Dim beta As Double() = New Double() {0, 0}

            For iter As Integer = 1 To 50
                Dim gradient As Double() = New Double() {0, 0}
                Dim hessian As Double(,) = New Double(1, 1) {}

                For i As Integer = 0 To n - 1
                    Dim xi As Double = (x(i) - xbar) / scale
                    Dim p As Double = sigmoid(beta(0) + beta(1) * xi)
                    Dim r As Double = If(y(i) > 0.5, 1.0, 0.0)
                    Dim w As Double = Math.Max(p * (1 - p), 1E-6)

                    gradient(0) += r - p
                    gradient(1) += (r - p) * xi
                    hessian(0, 0) += w
                    hessian(0, 1) += w * xi
                    hessian(1, 0) += w * xi
                    hessian(1, 1) += w * xi * xi
                Next

                Dim delta As Double() = solve(hessian, gradient)

                If delta Is Nothing Then Exit For

                beta(0) += delta(0)
                beta(1) += delta(1)

                If Math.Abs(delta(0)) < 1E-10 AndAlso Math.Abs(delta(1)) < 1E-10 Then
                    Exit For
                End If
            Next

            Return sigmoid(beta(0) + beta(1) * ((x0 - xbar) / scale))
        End Function

        Private Shared Function sigmoid(v As Double) As Double
            If v > 30 Then Return 1
            If v < -30 Then Return 0

            Return 1 / (1 + Math.Exp(-v))
        End Function

        Private Shared Function linspace(lower As Double, upper As Double, n As Integer) As Double()
            If n < 2 Then Return New Double() {lower}

            Dim buffer As Double() = New Double(n - 1) {}
            Dim delta As Double = (upper - lower) / (n - 1)

            For i As Integer = 0 To n - 1
                buffer(i) = lower + delta * i
            Next

            Return buffer
        End Function

        Private Shared Iterator Function groupIndexes(groups As String()) As IEnumerable(Of NamedCollection(Of Integer))
            Dim data As New Dictionary(Of String, List(Of Integer))

            For i As Integer = 0 To groups.Length - 1
                If Not data.ContainsKey(groups(i)) Then
                    Call data.Add(groups(i), New List(Of Integer))
                End If

                Call data(groups(i)).Add(i)
            Next

            For Each group In data
                Yield New NamedCollection(Of Integer) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        Private Shared Function resolveGroups(reader As ggplotReader,
                                              source As Object,
                                              nsize As Integer,
                                              env As SMRUCC.Rsharp.Runtime.Environment) As String()
            Dim mapping As String = If(reader.color,
                                       DirectCast(reader.[class], String),
                                       DirectCast(reader.group, String))

            If mapping Is Nothing Then
                Return Enumerable.Repeat(".", nsize).ToArray
            End If

            Return reader.getMapData(Of String)(source, mapping, env)
        End Function
    End Class
End Namespace