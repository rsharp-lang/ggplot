#Region "Microsoft.VisualBasic::203007048bb4e08fd566ad477b2f275d, G:/GCModeller/src/runtime/ggplot/src/ggplot//Internal/ggplotPipeline.vb"

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

    '   Total Lines: 59
    '    Code Lines: 45
    ' Comment Lines: 6
    '   Blank Lines: 8
    '     File Size: 1.85 KB


    ' Class ggplotPipeline
    ' 
    '     Properties: defaultTitle, theme
    ' 
    '     Function: GetRawData, TranslateX
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

<Assembly: InternalsVisibleTo("MSImaging")>
<Assembly: InternalsVisibleTo("ggraph")>
<Assembly: InternalsVisibleTo("ggpubr")>

''' <summary>
''' a wrapper of a collection of the ggplot rendering objects
''' </summary>
Public Class ggplotPipeline

    Friend g As IGraphics
    Friend canvas As GraphicsRegion
    Friend x As Array
    Friend y As Double()
    ''' <summary>
    ''' Do [x,y] translation
    ''' </summary>
    Friend scale As DataScaler
    Friend ggplot As ggplot
    Friend layout As Dictionary(Of String, PointF)

    Public ReadOnly Property theme As Theme
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return ggplot.ggplotTheme
        End Get
    End Property

    Public ReadOnly Property defaultTitle As String
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return ggplot.base.reader.ToString
        End Get
    End Property

    Public Function TranslateX() As Double()
        If scale.X.type = d3js.scale.scalers.linear Then
            Return (From xi As Object
                    In x.AsQueryable
                    Select scale.TranslateX(CDbl(xi))).ToArray
        Else
            Return (From xi As String
                    In DirectCast(x, String())
                    Select scale.TranslateX(xi)).ToArray
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetRawData(Of T)() As T
        Return DirectCast(ggplot.data, T)
    End Function

End Class
