#Region "Microsoft.VisualBasic::58a36755f24674cb480bf324aaad11fa, src\ggplot\Internal\layers\groupPlot\ggplotViolin.vb"

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

    '   Total Lines: 87
    '    Code Lines: 79 (90.80%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (9.20%)
    '     File Size: 3.91 KB


    '     Class ggplotViolin
    ' 
    '         Properties: nbins, showStats, splineDegree, zero_break
    ' 
    '         Function: PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports LineCap = System.Drawing.Drawing2D.LineCap
Imports TextureBrush = System.Drawing.TextureBrush
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports LineCap = Microsoft.VisualBasic.Imaging.LineCap
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace layers

    ''' <summary>
    ''' The violin plot is a statistical graph used to visualize the distribution of numerical data, similar to a box plot but with more information. 
    ''' It was introduced byHintze and Nelson in 1998. Here's how it works:
    ''' 
    ''' ### Key Components:
    ''' 1. **Kernel Density Estimation (KDE)**: The violin plot uses KDE to provide a better understanding of the distribution of data. 
    '''                                         The width of the violin at a given y-value represents the density of data points at that 
    '''                                         value.
    ''' 2. **Box Plot Elements**: Inside the violin, there is often a smaller box plot which provides summary statistics:
    '''    - **Median**: A line inside the box that indicates the median value of the data.
    '''    - **Interquartile Range (IQR)**: The box's width represents the IQR, which is the range between the first quartile (25th percentile) and the third quartile (75th percentile).
    '''    - **Whiskers**: Lines extending from the box that represent the range of the data, typically 1.5 * IQR above the third quartile and below the first quartile.
    '''    - **Outliers**: Individual points outside the whiskers, indicating values that are significantly different from the rest of the dataset.
    '''    
    ''' ### How to Interpret:
    ''' - **Shape**: The shape of the violin plot reflects the underlying distribution of the data. A symmetric shape indicates a normal distribution, while an asymmetric shape indicates a skewed distribution.
    ''' - **Size**: The width of the violin at different points is proportional to the number of data points in that region. Wider sections indicate more data points, while narrower sections indicate fewer data points.
    ''' - **Box Plot Inside**: This gives you a quick view of the median, IQR, and potential outliers, which can be compared across violins if multiple violins are plotted side by side.
    ''' 
    ''' ### Advantages:
    ''' - Provides more detailed information about the distribution of data than a traditional box plot.
    ''' - Can easily compare distributions between groups by plotting multiple violins side by side.
    ''' 
    ''' ### Disadvantages:
    ''' - Can be more complex to understand for those not familiar with statistical distributions.
    ''' - May be less precise in representing the exact distribution of data, especially in the tails, compared to a histogram or density plot.
    ''' 
    ''' ### Use Cases:
    ''' - Violin plots are commonly used in fields like data science, biostatistics, and economics to visualize distributions of datasets, 
    '''   especially when comparing multiple datasets or groups.
    ''' </summary>
    Public Class ggplotViolin : Inherits ggplotGroup

        Public Property splineDegree As Single = 2
        Public Property showStats As Boolean = False
        Public Property nbins As Integer = 25
        Public Property zero_break As Boolean = True

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim semiWidth As Double = binWidth / 2 * groupWidth
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim lineStroke As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke))
            Dim labelFont As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim gridPen As Pen = css.GetPen(Stroke.TryParse(stream.theme.gridStrokeY))
            Dim plotRect As Rectangle = stream.canvas.PlotRegion(css)
            Dim bottom = plotRect.Bottom
            Dim top = plotRect.Top

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(alpha * 255)

                If Not gridPen Is Nothing Then
                    Call g.DrawLine(gridPen, New PointF(x, top), New PointF(x, bottom))
                End If

                Call Violin.PlotViolin(
                    group:=group,
                    x:=x,
                    yscale:=yscale,
                    semiWidth:=semiWidth,
                    splineDegree:=splineDegree,
                    polygonStroke:=lineStroke,
                    showStats:=showStats,
                    labelFont:=labelFont,
                    color:=color,
                    g:=g,
                    canvas:=stream.canvas,
                    theme:=stream.theme,
                    zeroBreak:=zero_break,
                    nbins:=nbins
                )
            Next

            Return Nothing
        End Function
    End Class
End Namespace
