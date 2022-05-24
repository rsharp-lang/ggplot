#Region "Microsoft.VisualBasic::ae90560a2d1488442ea59ace210e6b5f, src\Internal\ggplotReader.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

' Class ggplotReader
' 
'     Properties: args, color, isPlain2D, label, title
'                 x, y, z
' 
'     Function: getMapColor, getMapData, ToString, unifySource
' 
' Class ggplotData
' 
'     Properties: [error], x, y
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
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
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' ggplot data mapper
''' </summary>
Public Class ggplotReader

    Public Property x As String
    Public Property y As String
    Public Property z As String
    Public Property color As Object
    Public Property shape As Object
    Public Property [class] As Object
    ''' <summary>
    ''' the legend title label text
    ''' </summary>
    ''' <returns></returns>
    Public Property title As String
    Public Property label As String
    Public Property args As list

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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overridable Function getMapData(Of T)(data As Object, map As Object, env As Environment) As T()
        Dim sourceMaps As Array = REnv.asVector(Of Object)(map)

        If sourceMaps.Length = 1 Then
            Return REnv.asVector(Of T)(unifySource(data, any.ToString(sourceMaps.GetValue(Scan0)), env))
        Else
            Return REnv.asVector(Of T)(sourceMaps)
        End If
    End Function

    Public Overridable Function getMapData(data As Object, env As Environment) As ggplotData
        Return New ggplotData With {
            .x = unifySource(data, x, env),
            .y = unifySource(data, y, env),
            .z = If(isPlain2D, Nothing, unifySource(data, z, env))
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

            Dim vec As Array = REnv.asVector(Of Object)(expression.Evaluate(env))

            Return vec
        Else
            Return table.getColumnVector(source)
        End If
    End Function

    Public Function getMapColor(data As Object,
                                shape As LegendStyles,
                                theme As Theme,
                                env As Environment) As (htmlColors As String(), legends As IggplotLegendElement)

        Dim v As String() = REnv.asVector(Of String)(unifySource(data, color, env))
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
            Dim factors As String() = uniqV.Objects
            Dim legendItems As LegendObject() = factors _
                .Select(Function(factor)
                            Return New LegendObject With {
                                .color = colors(uniqV.IndexOf(factor)),
                                .style = shape,
                                .title = factor,
                                .fontstyle = theme.legendLabelCSS
                            }
                        End Function) _
                .ToArray
            Dim legends As New legendGroupElement With {
                .legends = legendItems
            }

            If legendItems.IsNullOrEmpty Then
                legends = Nothing
            End If

            Return (html, legends)
        End If
    End Function

End Class

