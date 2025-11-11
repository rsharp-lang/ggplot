#Region "Microsoft.VisualBasic::704ac6d7a79319101ea2695bb4c9a624, src\ggplot\Internal\ggplotReader.vb"

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

    '   Total Lines: 254
    '    Code Lines: 171 (67.32%)
    ' Comment Lines: 55 (21.65%)
    '    - Xml Docs: 92.73%
    ' 
    '   Blank Lines: 28 (11.02%)
    '     File Size: 9.24 KB


    ' Class ggplotReader
    ' 
    '     Properties: [class], args, color, isPlain2D, label
    '                 shape, size, title, x, y
    '                 z
    ' 
    '     Function: dataframeSource, (+2 Overloads) FactorLegends, getColorSource, getLegendLabel, getMapColor
    '               (+2 Overloads) getMapData, getSizeSource, ToString, unifySource
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' [aes] / [aes_string] ggplot data mapper
''' </summary>
Public Class ggplotReader

    ''' <summary>
    ''' the x axis mapping
    ''' </summary>
    ''' <returns></returns>
    Public Property x As String
    ''' <summary>
    ''' the y axis mapping
    ''' </summary>
    ''' <returns></returns>
    Public Property y As String
    ''' <summary>
    ''' the z axis mapping
    ''' </summary>
    ''' <returns></returns>
    Public Property z As String

    Public Property xend As String
    Public Property yend As String

    Public Property color As Object
    Public Property shape As Object
    ''' <summary>
    ''' the fill class group
    ''' </summary>
    ''' <returns></returns>
    Public Property [class] As Object
    Public Property group As Object
    Public Property size As Object
    ''' <summary>
    ''' the legend title label text
    ''' </summary>
    ''' <returns></returns>
    Public Property title As String

    ''' <summary>
    ''' the scatter text label mapping
    ''' </summary>
    ''' <returns></returns>
    Public Property label As Object
    Public Property args As list

    ''' <summary>
    ''' test for the 2d render mode based on test of the <see cref="z"/> is empty or not.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property isPlain2D As Boolean
        Get
            Return z.StringEmpty
        End Get
    End Property

    Public Function getLegendLabel() As String
        If title.StringEmpty Then
            Return Me.ToString
        Else
            Return title
        End If
    End Function

    Public Overrides Function ToString() As String
        If isPlain2D Then
            If y.StringEmpty Then
                Return x
            Else
                Return $"{x} ~ {y}"
            End If
        Else
            Return $"[{x}, {y}, {z}]"
        End If
    End Function

    ''' <summary>
    ''' get vector data from the given data source
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="data">the data source of the plot data, usually be a dataframe object</param>
    ''' <param name="map">the data source mapping of the plot data, usually be the field name of the dataframe object</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overridable Function getMapData(Of T)(data As Object, map As Object, env As Environment) As T()
        Dim sourceMaps As Array = REnv.asVector(Of Object)(map)

        If sourceMaps.Length = 1 Then
            Return REnv.asVector(Of T)(unifySource(data, any.ToString(sourceMaps.GetValue(Scan0)), env))
        Else
            Return REnv.asVector(Of T)(sourceMaps)
        End If
    End Function

    ''' <summary>
    ''' create new ggplot data source
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Public Overridable Function getMapData(data As Object, env As Environment) As ggplotData
        Return New ggplotData With {
            .x = axisMap.FromArray(unifySource(data, x, env), x),
            .y = axisMap.FromArray(unifySource(data, y, env), y),
            .z = axisMap.FromArray(If(isPlain2D, Nothing, unifySource(data, z, env)), z),
            .fill = axisMap.FromArray(unifySource(data, If([class], color), env), [class]),
            .xend = axisMap.FromArray(unifySource(data, xend, env), xend),
            .yend = axisMap.FromArray(unifySource(data, yend, env), yend)
        }
    End Function

    Private Shared Function unifySource(data As Object, source As String, env As Environment) As Array
        If source Is Nothing Then
            Return Nothing
        ElseIf TypeOf data Is dataframe Then
            Return dataframeSource(DirectCast(data, dataframe), source, env)
        ElseIf TypeOf data Is list Then
            Throw New NotImplementedException
        Else
            Throw New NotImplementedException(data.GetType.FullName)
        End If
    End Function

    Private Shared Function dataframeSource(table As dataframe, source As String, env As Environment) As Array
        Dim is_eval As Boolean = source.StartsWith("~") AndAlso Not table.hasName(source)
        Dim expression As Expression
        Dim vec As Array

        If is_eval Then
            expression = Expression _
                .ParseLines(Rscript.AutoHandleScript(source.Trim("~"c))) _
                .First
            env = New Environment(env, New StackFrame With {
                .File = "n/a",
                .Line = "n/a",
                .Method = New Method With {
                    .Method = NameOf(unifySource),
                    .[Module] = "n/a",
                    .[Namespace] = "ggplot"
                }
            }, isInherits:=False)

            For Each v As String In table.colnames
                Call env.AssignSymbol(v, table(v))
            Next

            vec = REnv.asVector(Of Object)(expression.Evaluate(env))
        Else
            If table.hasName(source) Then
                vec = table.getColumnVector(source)
            Else
                Throw New InvalidDataException($"the required column(colname: {source}) source data mapping in dataframe is missing!")
            End If
        End If

        Return REnv.TryCastGenericArray(vec, env)
    End Function

    Public Function getColorSource(ggplot As ggplot) As Array
        Return unifySource(ggplot.data, color, ggplot.environment)
    End Function

    Public Function getSizeSource(ggplot As ggplot) As Array
        Return unifySource(ggplot.data, size, ggplot.environment)
    End Function

    Public Function getMapColor(data As Object,
                                shape As LegendStyles,
                                theme As Theme,
                                env As Environment) As (htmlColors As String(), legends As IggplotLegendElement)

        Dim v As String() = CLRVector.asCharacter(unifySource(data, color, env))
        Dim uniqV As Index(Of String) = v.Distinct.Indexing

        If uniqV.Objects.All(Function(vi) Val(vi) > 0 AndAlso vi.IsInteger) Then
            Return (v, Nothing)
        ElseIf uniqV.Objects _
            .All(Function(vi)
                     Dim isColor As Boolean = False
                     Call vi.TranslateColor(success:=isColor, throwEx:=False)
                     Return isColor
                 End Function) Then

            Return (v, Nothing)
        Else
            Dim colors = Designer _
                .GetColors("Set1:c8", uniqV.Count) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray
            Dim html As String() = v _
                .Select(Function(vi) colors(uniqV.IndexOf(vi))) _
                .ToArray

            Return (html, FactorLegends(uniqV, colors, shape, theme.legendLabelCSS))
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="factors"></param>
    ''' <param name="colors"></param>
    ''' <param name="shape"></param>
    ''' <param name="labelCss">label text css style of the legend label</param>
    ''' <returns></returns>
    Public Shared Function FactorLegends(factors As IEnumerable(Of String),
                                         colors As Func(Of Object, String),
                                         shape As LegendStyles,
                                         labelCss As String) As legendGroupElement

        ' create from a unique factor list
        Dim legendItems As LegendObject() = factors _
            .Select(Function(factor)
                        Return New LegendObject With {
                            .color = colors(factor),
                            .style = shape,
                            .title = factor,
                            .fontstyle = labelCss
                        }
                    End Function) _
            .ToArray
        Dim legends As New legendGroupElement With {
            .legends = legendItems
        }

        If legendItems.IsNullOrEmpty Then
            legends = Nothing
        End If

        Return legends
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="factors"></param>
    ''' <param name="colors"></param>
    ''' <param name="shape"></param>
    ''' <param name="labelCss">label text css style of the legend label</param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function FactorLegends(factors As Index(Of String),
                                         colors As String(),
                                         shape As LegendStyles,
                                         labelCss As String) As legendGroupElement

        Return FactorLegends(factors.Objects, Function(xi) colors(factors.IndexOf(xi)), shape, labelCss)
    End Function
End Class
