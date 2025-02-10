#Region "Microsoft.VisualBasic::23f44be7380534c22f8d2a2b8f43d8bb, src\ggplot\ggplot2.vb"

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

    '   Total Lines: 1575
    '    Code Lines: 676 (42.92%)
    ' Comment Lines: 798 (50.67%)
    '    - Xml Docs: 90.98%
    ' 
    '   Blank Lines: 101 (6.41%)
    '     File Size: 70.77 KB


    ' Module ggplot2
    ' 
    '     Function: add_layer, aes, annotation_raster, configPlot, coord_flip
    '               element_blank, element_line, element_rect, element_text, geom_bar
    '               geom_barplot, geom_boxplot, geom_col, geom_convexHull, geom_histogram
    '               geom_hline, geom_jitter, geom_line, geom_path, geom_pie
    '               geom_point, geom_raster, geom_scatterheatmap, geom_scatterpie, geom_signif
    '               geom_text, geom_tile, geom_violin, geom_vline, ggplot
    '               ggtitle, labs, scale_color_brewer, scale_colour_manual, scale_fill_distiller
    '               scale_fill_manual, scale_x_continuous, scale_y_continuous, scale_y_reverse, stat_compare_means
    '               stat_pvalue_manual, theme, waiver, xlab, ylab
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports ggplot.options
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

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
#End If

''' <summary>
''' Create Elegant Data Visualisations Using the Grammar of Graphics
''' </summary>
<Package("ggplot2")>
Module ggplot2

    ''' <summary>
    ''' ### Create a new ggplot
    ''' 
    ''' ``ggplot()`` initializes a ggplot object. It can be used to declare 
    ''' the input data frame for a graphic and to specify the set of 
    ''' plot aesthetics intended to be common throughout all subsequent 
    ''' layers unless specifically overridden.
    ''' </summary>
    ''' <param name="data">
    ''' Default dataset to use for plot. If not already a data.frame, 
    ''' will be converted to one by fortify(). If not specified, must be 
    ''' supplied in each layer added to the plot.
    ''' </param>
    ''' <param name="mapping">
    ''' Default list of aesthetic mappings to use for plot. If not specified, 
    ''' must be supplied in each layer added to the plot.
    ''' </param>
    ''' <param name="args">
    ''' Other arguments passed on to methods. Not currently used.
    ''' </param>
    ''' <param name="environment"></param>
    ''' <returns>
    ''' a ggplot base layer object which can be rendering to graphics by 
    ''' invoke the ``plot`` function.
    ''' </returns>
    ''' <remarks>
    ''' ``ggplot()`` is used to construct the initial plot object, and is 
    ''' almost always followed by + to add component to the plot. There 
    ''' are three common ways to invoke ``ggplot()``:
    ''' 
    ''' + ``ggplot(df, aes(x, y, other aesthetics))``
    ''' + ``ggplot(df)``
    ''' + ``ggplot()``
    ''' 
    ''' 
    ''' The first method Is recommended If all layers use the same data 
    ''' And the same Set Of aesthetics, although this method can also be 
    ''' used To add a layer Using data from another data frame. See the 
    ''' first example below. The second method specifies the Default 
    ''' data frame To use For the plot, but no aesthetics are defined up 
    ''' front. This Is useful When one data frame Is used predominantly 
    ''' As layers are added, but the aesthetics may vary from one layer 
    ''' To another. The third method initializes a skeleton ggplot Object
    ''' which Is fleshed out As layers are added. This method Is useful 
    ''' When multiple data frames are used To produce different layers, 
    ''' As Is often the Case In complex graphics.
    ''' </remarks>
    <ExportAPI("ggplot")>
    Public Function ggplot(<RRawVectorArgument>
                           Optional data As Object = Nothing,
                           <RDefaultExpression()>
                           Optional mapping As Object = "~aes()",
                           <RRawVectorArgument>
                           Optional colorSet As Object = "paper",
                           <RListObjectArgument>
                           Optional args As list = Nothing,
                           Optional environment As Environment = Nothing) As ggplot

        Dim driver As Drivers = environment.getDriver
        Dim theme As New Theme With {
            .axisLabelCSS = "font-style: strong; font-size: 12; font-family: " & FontFace.MicrosoftYaHei & ";",
            .axisTickCSS = "font-style: normal; font-size: 10; font-family: " & FontFace.MicrosoftYaHei & ";",
            .padding = InteropArgumentHelper.getPadding(args.getByName("padding"), "padding: 10% 20% 10% 10%;"),
            .drawLegend = True,
            .legendLabelCSS = "font-style: normal; font-size: 13; font-family: " & FontFace.MicrosoftYaHei & ";",
            .colorSet = RColorPalette.getColorSet(colorSet, [default]:="paper"),
            .xAxisLayout = args.getXAxisLayout(),
            .yAxisLayout = args.getYAxisLayout()
        }
        Dim args2 = environment.GetAcceptorArguments
        Dim ggplotDriver As ggplot = ggplot.CreateRender(data, environment, theme)
        Dim base As ggplotBase = ggplotDriver.CreateReader(mapping)

        ' union arguments
        For Each arg In args2
            If Not args.hasName(arg.Key) Then
                Call args.add(arg.Key, arg.Value)
            End If
        Next

        With ggplotDriver
            .driver = driver
            .data = data
            .layers = New List(Of ggplotLayer)
            .base = base
            .args = args
            .xlabel = base.reader.x
            .ylabel = base.reader.y
            .zlabel = base.reader.z
        End With

        Return ggplotDriver
    End Function

    ''' <summary>
    ''' ### Construct aesthetic mappings
    ''' 
    ''' Aesthetic mappings describe how variables in the data are mapped 
    ''' to visual properties (aesthetics) of geoms. Aesthetic mappings 
    ''' can be set in ggplot() and in individual layers.
    ''' </summary>
    ''' <param name="x">
    ''' List of name-value pairs in the form aesthetic = variable describing 
    ''' which variables in the layer data should be mapped to which aesthetics 
    ''' used by the paired geom/stat. The expression variable is evaluated 
    ''' within the layer data, so there is no need to refer to the original 
    ''' dataset (i.e., use ggplot(df, aes(variable)) instead of 
    ''' ``ggplot(df, aes(df$variable)))``. The names for x and y aesthetics 
    ''' are typically omitted because they are so common; all other aesthetics
    ''' must be named.
    ''' </param>
    ''' <param name="y"></param>
    ''' <param name="color">The color mapping name</param>
    ''' <param name="colour">the alias name of the color parameter</param>
    ''' <param name="class">
    ''' mapping data of the element class group.
    ''' </param>
    ''' <param name="label">
    ''' data source for the scatter annotation label text
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns>
    ''' A list with class uneval. Components of the list are either quosures 
    ''' or constants.
    ''' </returns>
    ''' <remarks>
    ''' This function also standardises aesthetic names by converting color to 
    ''' colour (also in substrings, e.g., point_color to point_colour) and 
    ''' translating old style R names to ggplot names (e.g., pch to shape and 
    ''' cex to size).
    ''' </remarks>
    <ExportAPI("aes")>
    Public Function aes(Optional x As Object = Nothing,
                        Optional y As Object = Nothing,
                        Optional z As Object = Nothing,
                        <RRawVectorArgument>
                        Optional label As Object = Nothing,
                        Optional color As Object = Nothing,
                        Optional colour As Object = Nothing,
                        Optional alpha As Object = Nothing,
                        <RLazyExpression>
                        Optional fill As Object = Nothing,
                        Optional title As String = Nothing,
                        Optional shape As Object = Nothing,
                        Optional [class] As Object = Nothing,
                        <RListObjectArgument>
                        Optional args As list = Nothing,
                        Optional env As Environment = Nothing) As ggplotReader

        If color Is Nothing Then
            color = colour
        End If
        If Not fill Is Nothing Then
            If TypeOf fill Is Expression Then
                Dim eval = DirectCast(fill, Expression).Evaluate(env)

                If Program.isException(eval) Then
                    If TypeOf fill Is SymbolReference Then
                        fill = DirectCast(fill, SymbolReference).symbol
                    Else
                        Return eval
                    End If
                Else
                    fill = eval
                End If
            Else
                ' do nothing
            End If
        End If

        Return New ggplotReader With {
            .x = x,
            .y = y,
            .z = z,
            .color = If(color, fill),
            .label = label,
            .args = args,
            .title = title,
            .shape = shape,
            .[class] = [class]
        }
    End Function

    ''' <summary>
    ''' ### Scatter Points
    ''' 
    ''' The point geom is used to create scatterplots. The scatterplot is most 
    ''' useful for displaying the relationship between two continuous variables. 
    ''' It can be used to compare one continuous and one categorical variable, 
    ''' or two categorical variables, but a variation like geom_jitter(), 
    ''' geom_count(), or geom_bin2d() is usually more appropriate. A bubblechart 
    ''' is a scatterplot with a third variable mapped to the size of points.
    ''' </summary>
    ''' <param name="color"></param>
    ''' <param name="shape"></param>
    ''' <param name="size"></param>
    ''' <param name="show_legend">	
    ''' logical. Should this layer be included in the legends? NA, the default, 
    ''' includes if any aesthetics are mapped. FALSE never includes, And TRUE 
    ''' always includes. It can also be a named logical vector to finely select 
    ''' the aesthetics to display.
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns>a scatter plot layer</returns>
    <ExportAPI("geom_point")>
    <RApiReturn(GetType(ggplotScatter))>
    Public Function geom_point(Optional mapping As ggplotReader = NULL,
                               <RRawVectorArgument>
                               Optional color As Object = Nothing,
                               Optional shape As LegendStyles? = Nothing,
                               Optional stroke As Object = Nothing,
                               Optional size As Single = 2,
                               Optional show_legend As Boolean = True,
                               Optional alpha As Double = 1,
                               Optional env As Environment = Nothing) As ggplotLayer

        Dim colorMap As ggplotColorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        Dim strokeCss As String = InteropArgumentHelper.getStrokePenCSS(stroke, [default]:=Nothing)

        If mapping IsNot Nothing AndAlso Not mapping.isPlain2D Then
            ' 3D
            Return New ggplotScatter3d With {
                .colorMap = colorMap,
                .reader = mapping,
                .shape = shape,
                .size = size,
                .showLegend = show_legend,
                .stroke = stroke
            }
        Else
            ' 2D
            Return New ggplotScatter With {
                .colorMap = colorMap,
                .shape = shape,
                .size = size,
                .showLegend = show_legend,
                .reader = mapping,
                .stroke = stroke
            }
        End If
    End Function

    ''' <summary>
    ''' ### Text
    ''' 
    ''' Text geoms are useful for labeling plots. They can be used by themselves 
    ''' as scatterplots or in combination with other geoms, for example, for 
    ''' labeling points or for annotating the height of bars. geom_text() adds 
    ''' only text to the plot. geom_label() draws a rectangle behind the text, 
    ''' making it easier to read.
    ''' </summary>
    ''' <param name="mapping">
    ''' Set of aesthetic mappings created by aes() or aes_(). If specified and 
    ''' inherit.aes = TRUE (the default), it is combined with the default mapping 
    ''' at the top level of the plot. You must supply mapping if there is no plot 
    ''' mapping.
    ''' </param>
    ''' <param name="data">
    ''' The data to be displayed in this layer. There are three options:
    ''' 
    ''' If NULL, the Default, the data Is inherited from the plot data As 
    ''' specified In the Call To ggplot().
    ''' 
    ''' A data.frame, Or other Object, will override the plot data. All objects 
    ''' will be fortified To produce a data frame. See fortify() For which 
    ''' variables will be created.
    ''' 
    ''' A Function will be called With a Single argument, the plot data. The Return 
    ''' value must be a data.frame, And will be used As the layer data. A Function 
    ''' can be created from a formula (e.g. ~ head(.x, 10)).
    ''' </param>
    ''' <param name="stat">
    ''' The statistical transformation to use on the data for this layer, as a 
    ''' string.
    ''' </param>
    ''' <param name="position">
    ''' Position adjustment, either as a string, or the result of a call to a 
    ''' position adjustment function. Cannot be jointy specified with nudge_x or 
    ''' nudge_y.
    ''' </param>
    ''' <param name="parse">
    ''' If TRUE, the labels will be parsed into expressions and displayed as 
    ''' described in ?plotmath.
    ''' </param>
    ''' <param name="nudge_x">
    ''' Horizontal and vertical adjustment to nudge labels by. Useful for 
    ''' offsetting text from points, particularly on discrete scales. Cannot be 
    ''' jointly specified with position.
    ''' </param>
    ''' <param name="nudge_y">
    ''' Horizontal and vertical adjustment to nudge labels by. Useful for 
    ''' offsetting text from points, particularly on discrete scales. Cannot be 
    ''' jointly specified with position.
    ''' </param>
    ''' <param name="check_overlap">
    ''' If TRUE, text that overlaps previous text in the same layer will not be 
    ''' plotted. check_overlap happens at draw time and in the order of the data. 
    ''' Therefore data should be arranged by the label column before calling 
    ''' geom_text(). Note that this argument is not supported by geom_label().
    ''' </param>
    ''' <param name="na_rm">
    ''' If False, the Default, missing values are removed With a warning. 
    ''' If True, missing values are silently removed.
    ''' </param>
    ''' <param name="show_legend">
    ''' logical. Should this layer be included in the legends? NA, the default, 
    ''' includes if any aesthetics are mapped. FALSE never includes, and TRUE 
    ''' always includes. It can also be a named logical vector to finely select
    ''' the aesthetics to display.
    ''' </param>
    ''' <param name="inherit_aes">
    ''' If False, Overrides the Default aesthetics, rather than combining With 
    ''' them. This Is most useful For helper functions that define both data And 
    ''' aesthetics And shouldn't inherit behaviour from the default plot 
    ''' specification, e.g. borders().
    ''' </param>
    ''' <param name="color"></param>
    ''' <param name="args">
    ''' Other arguments passed On To layer(). These are often aesthetics, used To 
    ''' Set an aesthetic To a fixed value, Like colour = "red" Or size = 3. They
    ''' may also be parameters To the paired geom/stat.
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_text")>
    Public Function geom_text(Optional mapping As ggplotReader = NULL,
                              Optional data As Object = NULL,
                              Optional stat$ = "identity",
                              Optional position$ = "identity",
                              Optional parse As Boolean = False,
                              Optional nudge_x! = 0,
                              Optional nudge_y! = 0,
                              Optional check_overlap As Boolean = False,
                              Optional na_rm As Boolean = False,
                              Optional show_legend As Boolean = False,
                              Optional inherit_aes As Boolean = True,
                              <RRawVectorArgument>
                              Optional color As Object = "steelblue",
                              Optional which As Expression = Nothing,
                              Optional alpha As Double = 1,
                              Optional size As Single? = Nothing,
                              <RListObjectArgument>
                              Optional args As list = Nothing,
                              Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotTextLabel With {
            .reader = mapping,
            .showLegend = show_legend,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env),
            .which = which,
            .check_overlap = check_overlap,
            .fontSize = size
        }
    End Function

    ''' <summary>
    ''' ## Histograms and frequency polygons
    ''' 
    ''' Visualise the distribution of a single continuous variable by dividing 
    ''' the x axis into bins and counting the number of observations in each bin. 
    ''' Histograms (``geom_histogram()``) display the counts with bars; 
    ''' </summary>
    ''' <param name="bins">
    ''' Number of bins. Overridden by binwidth. Defaults to 30.
    ''' </param>
    ''' <returns>
    ''' A histogram plot layer
    ''' </returns>
    <ExportAPI("geom_histogram")>
    <RApiReturn(GetType(ggplotHistogram))>
    Public Function geom_histogram(bins As Integer,
                                   Optional color As Object = Nothing,
                                   Optional alpha As Double = 1,
                                   <RRawVectorArgument>
                                   Optional range As Object = Nothing,
                                   Optional env As Environment = Nothing) As ggplotLayer

        Dim minMax As DoubleRange = Nothing

        If Not range Is Nothing Then
            minMax = CLRVector.asNumeric(range)
        End If

        Return New ggplotHistogram With {
            .bins = bins,
            .colorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color), alpha, env),
            .range = minMax
        }
    End Function

    ''' <summary>
    ''' ### Connect observations
    ''' 
    ''' geom_path() connects the observations in the order in which they appear in 
    ''' the data. geom_line() connects them in order of the variable on the x axis. 
    ''' geom_step() creates a stairstep plot, highlighting exactly when changes 
    ''' occur. The group aesthetic determines which cases are connected together.
    ''' </summary>
    ''' <param name="bspline">
    ''' options for the b-spline smooth of the line plot
    ''' </param>
    ''' <returns>A line chart plot layer</returns>
    ''' <remarks>
    ''' An alternative parameterisation is geom_segment(), where each line corresponds 
    ''' to a single case which provides the start and end coordinates.
    ''' </remarks>
    <ExportAPI("geom_line")>
    <RApiReturn(GetType(ggplotLine))>
    Public Function geom_line(Optional mapping As ggplotReader = NULL,
                              <RRawVectorArgument>
                              Optional color As Object = Nothing,
                              Optional width As Single = 5,
                              Optional show_legend As Boolean = True,
                              Optional alpha As Double = 1,
                              Optional bspline As Boolean = False,
                              Optional env As Environment = Nothing) As ggplotLayer

        Dim rawColor As String = RColorPalette.getColor(color, Nothing)
        Dim colorMap As ggplotColorMap = ggplotColorMap.CreateColorMap(rawColor, alpha, env)

        Return New ggplotLine With {
            .showLegend = show_legend,
            .colorMap = colorMap,
            .line_width = width,
            .reader = mapping,
            .bspline = bspline
        }
    End Function

    ''' <summary>
    ''' ### Reference line defined by Y intercept. Useful for annotating plots.
    ''' 
    ''' Using the described geometry, you can insert a simple geometric 
    ''' object into your data visualization – a line defined by a position 
    ''' on the Y axis. 
    ''' </summary>
    ''' <param name="yintercept"></param>
    ''' <param name="color"></param>
    ''' <param name="line_width!"></param>
    ''' <param name="linetype"></param>
    ''' <returns></returns>
    <ExportAPI("geom_hline")>
    Public Function geom_hline(yintercept As Double,
                               Optional color As Object = "black",
                               Optional line_width! = 3,
                               Optional linetype As DashStyle = DashStyle.Solid) As ggplotLayer

        Dim a As New PointF(Single.NegativeInfinity, yintercept)
        Dim b As New PointF(Single.PositiveInfinity, yintercept)
        Dim lineColor As Color = RColorPalette.getColor(color).TranslateColor
        Dim style As New Stroke(lineColor, line_width) With {
            .dash = linetype
        }

        Return New ggplotABLine With {
            .abline = New Line(a, b, style)
        }
    End Function

    ''' <summary>
    ''' ## Reference lines: horizontal, vertical, and diagonal
    ''' 
    ''' These geoms add reference lines (sometimes called rules) to a plot, 
    ''' either horizontal, vertical, or diagonal (specified by slope and
    ''' intercept). These are useful for annotating plots.
    ''' </summary>
    ''' <param name="xintercept">
    ''' Parameters that control the position of the line. If these are set, 
    ''' data, mapping and show.legend are overridden.
    ''' </param>
    ''' <param name="color"></param>
    ''' <param name="line_width!"></param>
    ''' <param name="linetype"></param>
    ''' <returns></returns>
    <ExportAPI("geom_vline")>
    Public Function geom_vline(xintercept As Double,
                               Optional color As Object = "black",
                               Optional line_width! = 2,
                               Optional linetype As DashStyle = DashStyle.Solid) As ggplotLayer

        Dim a As New PointF(xintercept, Single.NegativeInfinity)
        Dim b As New PointF(xintercept, Single.PositiveInfinity)
        Dim line_color = RColorPalette.getColor(color).TranslateColor
        Dim style As New Stroke(line_color, line_width) With {
            .dash = linetype
        }

        Return New ggplotABLine With {
            .abline = New Line(a, b, style)
        }
    End Function

    ''' <summary>
    ''' ## Connect observations
    ''' 
    ''' geom_path() connects the observations in the order in which they 
    ''' appear in the data. geom_line() connects them in order of the 
    ''' variable on the x axis. geom_step() creates a stairstep plot, highlighting 
    ''' exactly when changes occur. The group aesthetic determines which 
    ''' cases are connected together.
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("geom_path")>
    Public Function geom_path() As ggplotLayer
        Throw New NotImplementedException
    End Function

    <ExportAPI("geom_convexHull")>
    Public Function geom_convexHull(Optional mapping As ggplotReader = NULL,
                                    Optional alpha As Double = 1) As ggplotLayer

        Return New ggplotConvexhull With {
            .reader = mapping,
            .alpha = alpha
        }
    End Function

    ''' <summary>
    ''' ## A box and whiskers plot (in the style of Tukey)
    ''' 
    ''' The boxplot compactly displays the distribution of a continuous variable. 
    ''' It visualises five summary statistics (the median, two hinges and two 
    ''' whiskers), and all "outlying" points individually.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' ## Orientation
    ''' This geom treats Each axis differently And, thus, can thus have two 
    ''' orientations. Often the orientation Is easy To deduce from a combination 
    ''' Of the given mappings And the types Of positional scales In use. Thus, 
    ''' ggplot2 will by Default Try To guess which orientation the layer should 
    ''' have. Under rare circumstances, the orientation Is ambiguous And guessing 
    ''' may fail. In that Case the orientation can be specified directly Using the 
    ''' orientation parameter, which can be either "x" Or "y". The value gives 
    ''' the axis that the geom should run along, "x" being the Default orientation 
    ''' you would expect For the geom.
    '''
    ''' ## Summary statistics
    ''' The lower And upper hinges correspond To the first And third quartiles 
    ''' (the 25th And 75th percentiles). This differs slightly from the method 
    ''' used by the boxplot() Function, And may be apparent With small samples. 
    ''' See boxplot.stats() For For more information On how hinge positions are 
    ''' calculated For boxplot().
    '''
    ''' The upper whisker extends from the hinge To the largest value no further 
    ''' than 1.5 * IQR from the hinge (where IQR Is the inter-quartile range, Or 
    ''' distance between the first And third quartiles). The lower whisker extends 
    ''' from the hinge To the smallest value at most 1.5 * IQR Of the hinge. Data 
    ''' beyond the End Of the whiskers are called "outlying" points And are plotted 
    ''' individually.
    '''
    ''' In a notched box plot, the notches extend 1.58 * IQR / sqrt(n). This gives a 
    ''' roughly 95% confidence interval for comparing medians. See McGill et al. 
    ''' (1978) for more details.
    '''
    ''' ## Aesthetics
    ''' geom_boxplot() understands the following aesthetics (required aesthetics are 
    ''' in bold):
    '''
    ''' + x Or y
    ''' + lower Or xlower
    ''' + upper Or xupper
    ''' + middle Or xmiddle
    ''' + ymin Or xmin
    ''' + ymax Or xmax
    ''' + alpha
    ''' + colour
    ''' + fill
    ''' + group
    ''' + linetype
    ''' + shape
    ''' + size
    ''' + weight
    '''
    ''' Learn more about setting these aesthetics In vignette("ggplot2-specs").
    '''
    ''' ## Computed variables
    ''' stat_boxplot() provides the following variables, some of which depend on the orientation:
    '''
    ''' + width: width of boxplot
    ''' + ymin Or xmin: lower whisker = smallest observation greater than Or equal To lower hinge - 1.5 * IQR
    ''' + lower Or xlower: lower hinge, 25% quantile
    ''' + notchlower: lower edge Of notch = median - 1.58 * IQR / sqrt(n)
    ''' + middle Or xmiddle: median, 50% quantile
    ''' + notchupper: upper edge Of notch = median + 1.58 * IQR / sqrt(n)
    ''' + upper Or xupper: upper hinge, 75% quantile
    ''' + ymax Or xmax: upper whisker = largest observation less than Or equal To upper hinge + 1.5 * IQR
    '''
    ''' ## References
    ''' 
    ''' > McGill, R., Tukey, J. W. And Larsen, W. A. (1978) Variations of box plots. 
    '''   The American Statistician 32, 12-16.
    ''' </remarks>
    <ExportAPI("geom_boxplot")>
    Public Function geom_boxplot(Optional color As list = Nothing,
                                 Optional width As Double = 1,
                                 Optional alpha As Double = 0.95,
                                 Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotBoxplot With {
            .groupWidth = width,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env),
            .alpha = alpha
        }
    End Function

    ''' <summary>
    ''' ### Bar charts
    ''' 
    ''' There are two types of bar charts: geom_bar() and geom_col(). geom_bar() 
    ''' makes the height of the bar proportional to the number of cases in each
    ''' group (or if the weight aesthetic is supplied, the sum of the weights).
    ''' If you want the heights of the bars to represent values in the data, use 
    ''' geom_col() instead. geom_bar() uses stat_count() by default: it counts 
    ''' the number of cases at each x position. geom_col() uses stat_identity():
    ''' it leaves the data as is.
    ''' </summary>
    ''' <param name="mapping">Set of aesthetic mappings created by aes(). If specified
    ''' and inherit.aes = TRUE (the default), it is combined with the default mapping 
    ''' at the top level of the plot. You must supply mapping if there is no plot 
    ''' mapping.</param>
    ''' <param name="data">The data to be displayed in this layer. There are three options:
    ''' If NULL, the Default, the data Is inherited from the plot data As specified 
    ''' In the Call To ggplot().
    ''' A data.frame, Or other Object, will override the plot data. All objects will be
    ''' fortified To produce a data frame. See fortify() For which variables will be
    ''' created.
    ''' A Function will be called With a Single argument, the plot data. The Return 
    ''' value must be a data.frame, And will be used As the layer data. A Function 
    ''' can be created from a formula (e.g. ~ head(.x, 10)).</param>
    ''' <param name="position">Position adjustment, either as a string naming the 
    ''' adjustment (e.g. "jitter" to use position_jitter), or the result of a call 
    ''' to a position adjustment function. Use the latter if you need to change the
    ''' settings of the adjustment.</param>
    ''' <param name="just">Adjustment for column placement. Set to 0.5 by default, 
    ''' meaning that columns will be centered about axis breaks. Set to 0 or 1 to place 
    ''' columns to the left/right of axis breaks. Note that this argument may have 
    ''' unintended behaviour when used with alternative positions, e.g. position_dodge().</param>
    ''' <param name="width">Bar width. By default, set to 90% of the resolution() of
    ''' the data.</param>
    ''' <param name="na_rm">If FALSE, the default, missing values are removed with a
    ''' warning. If TRUE, missing values are silently removed.</param>
    ''' <param name="show_legend">logical. Should this layer be included in the legends? 
    ''' NA, the default, includes if any aesthetics are mapped. FALSE never includes, 
    ''' and TRUE always includes. It can also be a named logical vector to finely select 
    ''' the aesthetics to display.</param>
    ''' <param name="inherit_aes">If FALSE, overrides the default aesthetics, rather 
    ''' than combining with them. This is most useful for helper functions that define 
    ''' both data and aesthetics and shouldn't inherit behaviour from the default plot 
    ''' specification, e.g. borders().</param>
    ''' <param name="args">Other arguments passed on to layer(). These are often aesthetics,
    ''' used to set an aesthetic to a fixed value, like colour = "red" or size = 3. They 
    ''' may also be parameters to the paired geom/stat.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_col")>
    Public Function geom_col(Optional mapping As Object = NULL,
                             Optional data As Object = NULL,
                             Optional position As Object = "stack",
                             Optional just As Double = 0.5,
                             Optional width As Double = NULL,
                             Optional na_rm As Boolean = False,
                             Optional show_legend As Boolean? = NULL,
                             Optional inherit_aes As Boolean = True,
                             Optional args As list = Nothing,
                             Optional env As Environment = Nothing)

    End Function

    ''' <summary>
    ''' ### Bar charts
    ''' 
    ''' There are two types of bar charts: geom_bar() and geom_col(). geom_bar() 
    ''' makes the height of the bar proportional to the number of cases in each
    ''' group (or if the weight aesthetic is supplied, the sum of the weights).
    ''' If you want the heights of the bars to represent values in the data, use 
    ''' geom_col() instead. geom_bar() uses stat_count() by default: it counts 
    ''' the number of cases at each x position. geom_col() uses stat_identity():
    ''' it leaves the data as is.
    ''' </summary>
    ''' <param name="stat"></param>
    ''' <param name="position"></param>
    ''' <param name="color"></param>
    ''' <param name="width"></param>
    ''' <param name="size"></param>
    ''' <param name="show_legend"></param>
    ''' <param name="env"></param>
    ''' <returns>A bar plot layer</returns>
    <ExportAPI("geom_bar")>
    <RApiReturn(GetType(geom_bar))>
    Public Function geom_bar(Optional stat As Object = "identity",
                             Optional position As Object = "stack",
                             Optional color As Object = "black",
                             Optional width As Double = 0.7,
                             Optional size As Double = 0.25,
                             Optional show_legend As Boolean = True,
                             Optional env As Environment = Nothing) As geom_bar

        Return New geom_bar With {
            .position = position,
            .stat = stat,
            .groupWidth = width,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env)，
            .showLegend = show_legend
        }
    End Function

    ''' <summary>
    ''' ### Line segments and curves
    ''' 
    ''' geom_segment() draws a straight line between points (x, y) and (xend, yend). Both geoms 
    ''' draw a single segment/curve per case. See geom_path() if you need to connect points 
    ''' across multiple cases.
    ''' </summary>
    ''' <param name="mapping">Set of aesthetic mappings created by aes(). If specified and ``inherit.aes = TRUE`` (the default),
    ''' it is combined with the default mapping at the top level of the plot. You must supply mapping if there is no 
    ''' plot mapping.</param>
    ''' <param name="data">The data to be displayed in this layer. There are three options:
    ''' 
    ''' If NULL, the default, the data is inherited from the plot data as specified in the call to ggplot().
    ''' 
    ''' A data.frame, or other object, will override the plot data. All objects will be fortified to 
    ''' produce a data frame. See fortify() for which variables will be created.
    ''' 
    ''' A function will be called with a single argument, the plot data. The return value must be a data.frame, 
    ''' and will be used as the layer data. A function can be created from a formula (e.g. ~ head(.x, 10)).
    ''' </param>
    ''' <param name="stat">The statistical transformation to use on the data for this layer. When using a geom_*() 
    ''' function to construct a layer, the stat argument can be used the override the default coupling between geoms
    ''' and stats. The stat argument accepts the following:
    ''' 
    ''' A Stat ggproto subclass, for example StatCount.
    ''' 
    ''' A string naming the stat. To give the stat as a string, strip the function name of the stat_ prefix. 
    ''' For example, to use stat_count(), give the stat as "count".
    ''' 
    ''' For more information and other ways to specify the stat, see the layer stat documentation.</param>
    ''' <param name="position">A position adjustment to use on the data for this layer. This can be used in 
    ''' various ways, including to prevent overplotting and improving the display. The position argument 
    ''' accepts the following:
    ''' 
    ''' The result of calling a position function, such as position_jitter(). This method allows for passing 
    ''' extra arguments to the position.
    ''' 
    ''' A string naming the position adjustment. To give the position as a string, strip the function name of 
    ''' the position_ prefix. For example, to use position_jitter(), give the position as "jitter".
    ''' 
    ''' For more information and other ways to specify the position, see the layer position documentation.</param>
    ''' <param name="arrow">specification for arrow heads, as created by grid::arrow().</param>
    ''' <param name="arrow_fill">fill colour to use for the arrow head (if closed). NULL means use colour aesthetic.</param>
    ''' <param name="lineend">Line end style (round, butt, square).</param>
    ''' <param name="linejoin">Line join style (round, mitre, bevel).</param>
    ''' <param name="na_rm">If FALSE, the default, missing values are removed with a warning. If TRUE, missing values are silently removed.</param>
    ''' <param name="show_legend">logical. Should this layer be included in the legends? NA, the default, 
    ''' includes if any aesthetics are mapped. FALSE never includes, and TRUE always includes. It can also 
    ''' be a named logical vector to finely select the aesthetics to display.</param>
    ''' <param name="inherit_aes">If FALSE, overrides the default aesthetics, rather than combining with them. 
    ''' This is most useful for helper functions that define both data and aesthetics and shouldn't inherit 
    ''' behaviour from the default plot specification, e.g. borders().</param>
    ''' <param name="args">Other arguments passed on to layer()'s params argument. These arguments broadly 
    ''' fall into one of 4 categories below. Notably, further arguments to the position argument, or aesthetics 
    ''' that are required can not be passed through .... Unknown arguments that are not part of the 4 
    ''' categories below are ignored.
    ''' 
    ''' Static aesthetics that are not mapped to a scale, but are at a fixed value and apply to the layer as 
    ''' a whole. For example, colour = "red" or linewidth = 3. The geom's documentation has an Aesthetics 
    ''' section that lists the available options. The 'required' aesthetics cannot be passed on to the params.
    ''' Please note that while passing unmapped aesthetics as vectors is technically possible, the order and 
    ''' required length is not guaranteed to be parallel to the input data.
    ''' 
    ''' When constructing a layer using a stat_*() function, the ... argument can be used to pass on parameters 
    ''' to the geom part of the layer. An example of this is stat_density(geom = "area", outline.type = "both").
    ''' The geom's documentation lists which parameters it can accept.
    ''' 
    ''' Inversely, when constructing a layer using a geom_*() function, the ... argument can be used to pass on 
    ''' parameters to the stat part of the layer. An example of this is geom_area(stat = "density", adjust = 0.5).
    ''' The stat's documentation lists which parameters it can accept.
    ''' 
    ''' The key_glyph argument of layer() may also be passed on through .... This can be one of the functions 
    ''' described as key glyphs, to change the display of the layer in the legend.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' geom_segment() understands the following aesthetics (required aesthetics are in bold):
    ''' 
    ''' + **x**
    ''' + **y**
    ''' + **xend or yend**
    ''' + alpha
    ''' + colour
    ''' + group
    ''' + linetype
    ''' + linewidth
    ''' 
    ''' Learn more about setting these aesthetics in vignette("ggplot2-specs").
    ''' </remarks>
    <ExportAPI("geom_segment")>
    Public Function geom_segment(Optional mapping As ggplotReader = NULL,
                                 Optional data As Object = NULL,
                                 Optional stat As String = "identity",
                                 Optional position As String = "identity",
                                 Optional arrow As Triangle = NULL,
                                 Optional arrow_fill As Object = NULL,
                                 Optional lineend As String = "butt",
                                 Optional linejoin As String = "round",
                                 Optional na_rm As Boolean = False,
                                 Optional show_legend As Boolean = False,
                                 Optional inherit_aes As Boolean = True,
                                 <RListObjectArgument>
                                 Optional args As list = Nothing,
                                 Optional env As Environment = Nothing) As ggplotSegments
        Dim maxL As Double = 10

        If mapping IsNot Nothing Then
            Dim xend As Object = mapping.args!xend
            Dim yend As Object = mapping.args!yend

            mapping.x = CLRVector.asScalarCharacter(xend)
            mapping.y = CLRVector.asScalarCharacter(yend)
        End If
        If Not arrow Is Nothing Then
            maxL = arrow.Size.Height
        End If

        Return New ggplotSegments With {
            .reader = mapping,
            .data = Nothing,
            .minCell = maxL,
            .showLegend = show_legend
        }
    End Function

    <ExportAPI("geom_barplot")>
    <RApiReturn(GetType(ggplotBarplot))>
    Public Function geom_barplot(Optional color As list = Nothing,
                                 Optional width As Double = 1,
                                 Optional alpha As Double = 0.95,
                                 Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotBarplot With {
            .groupWidth = width,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env),
            .alpha = alpha
        }
    End Function

    <ExportAPI("geom_violin")>
    <RApiReturn(GetType(ggplotViolin))>
    Public Function geom_violin(Optional color As list = Nothing,
                                Optional width As Double = 0.9,
                                Optional alpha As Double = 0.95,
                                Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotViolin With {
            .groupWidth = width,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env),
            .alpha = alpha
        }
    End Function

    <ExportAPI("geom_jitter")>
    <RApiReturn(GetType(ggplotJitter))>
    Public Function geom_jitter(Optional mapping As ggplotReader = NULL,
                                Optional data As Object = NULL,
                                Optional stat As Object = "identity",
                                Optional width As Double = 0.5,
                                Optional radius As Double = 10,
                                Optional alpha As Double = 0.85,
                                Optional color As list = Nothing,
                                Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotJitter With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .radius = radius,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env)
        }
    End Function

    <ExportAPI("geom_scatterpie")>
    <RApiReturn(GetType(ggplotScatterpie))>
    Public Function geom_scatterpie(data As String()) As ggplotLayer
        Return New ggplotScatterpie With {
            .pie = data
        }
    End Function

    <ExportAPI("geom_scatterheatmap")>
    <RApiReturn(GetType(ggplotScatterheatmap))>
    Public Function geom_scatterheatmap(data As String,
                                        Optional colors As String = Nothing,
                                        Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotScatterheatmap With {
            .layer = data,
            .colorMap = ggplotColorMap.CreateColorMap(colors, 1, env)
        }
    End Function

    <ExportAPI("geom_pie")>
    <RApiReturn(GetType(ggplotPie))>
    Public Function geom_pie() As ggplotLayer
        Return New ggplotPie
    End Function

    <ExportAPI("geom_raster")>
    Public Function geom_raster(bitmap As Object,
                                Optional layout As Object = Nothing,
                                Optional env As Environment = Nothing) As Object

    End Function

    <ExportAPI("geom_tile")>
    <RApiReturn(GetType(ggplotTileLayer))>
    Public Function geom_tile(Optional mapping As ggplotReader = NULL) As Object
        Return New ggplotTileLayer With {.reader = mapping}
    End Function

    ''' <summary>
    ''' annotation_raster: Annotation: high-performance rectangular tiling
    ''' 
    ''' This is a special version of geom_raster() optimised for static 
    ''' annotations that are the same in every panel. These annotations 
    ''' will not affect scales (i.e. the x and y axes will not grow to cover 
    ''' the range of the raster, and the raster must already have its own 
    ''' colours). This is useful for adding bitmap images.
    ''' </summary>
    ''' <param name="raster">
    ''' raster object to display, may be an array or a nativeRaster
    ''' </param>
    ''' <returns></returns>
    ''' 
    <ExportAPI("annotation_raster")>
    Public Function annotation_raster(<RRawVectorArgument> raster As Object) As Object
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' add a ggplot plot layer
    ''' </summary>
    ''' <param name="ggplot"></param>
    ''' <param name="layer"></param>
    ''' <returns></returns>
    <ROperator("+")>
    <RApiReturn(GetType(ggplot))>
    Public Function add_layer(ggplot As ggplot, layer As ggplotLayer) As Object
        If layer Is Nothing Then
            If ggplot.environment.globalEnvironment.options.strict Then
                Return RInternal.debug.stop("the given ggplot layer object can not be nothing!", ggplot.environment)
            Else
                Call ggplot.environment.AddMessage("the given ggplot layer object is nothing...", MSG_TYPES.WRN)
                Return ggplot
            End If
        Else
            Return ggplot + layer
        End If
    End Function

    <ROperator("+")>
    Public Function configPlot(ggplot As ggplot, opts As ggplotOption) As ggplot
        Return opts.Config(ggplot)
    End Function

    ''' <summary>
    ''' ## Modify axis, legend, and plot labels
    ''' 
    ''' Good labels are critical for making your plots accessible to 
    ''' a wider audience. Always ensure the axis and legend labels 
    ''' display the full variable name. Use the plot title and subtitle 
    ''' to explain the main findings. It's common to use the caption 
    ''' to provide information about the data source. tag can be used 
    ''' for adding identification tags to differentiate between multiple 
    ''' plots.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="title">The text for the title.</param>
    ''' <param name="subtitle">
    ''' The text For the subtitle For the plot which will be displayed 
    ''' below the title.
    ''' </param>
    ''' <param name="caption">
    ''' The text for the caption which will be displayed in the 
    ''' bottom-right of the plot by default.
    ''' </param>
    ''' <param name="tag">
    ''' The text for the tag label which will be displayed at the top-left 
    ''' of the plot by default.
    ''' </param>
    ''' <param name="alt">
    ''' Text used for the generation of alt-text for the plot. See 
    ''' get_alt_text for examples.
    ''' </param>
    ''' <param name="alt_insight"></param>
    ''' <param name="args">
    ''' A list of new name-value pairs. The name should be an aesthetic.
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' You can also set axis and legend labels in the individual scales 
    ''' (using the first argument, the name). If you're changing other 
    ''' scale options, this is recommended.
    ''' 
    ''' If a plot already has a title, subtitle, caption, etc., And you want 
    ''' To remove it, you can Do so by setting the respective argument To 
    ''' NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    ''' will remove the subtitle from the plot.
    ''' </remarks>
    <ExportAPI("labs")>
    Public Function labs(Optional x As String = Nothing,
                         Optional y As String = Nothing,
                         Optional title As String = Nothing,
                         Optional subtitle As String = Nothing,
                         Optional caption As String = Nothing,
                         Optional tag As Object = Nothing,
                         Optional alt As Object = Nothing,
                         Optional alt_insight As Object = Nothing,
                         <RListObjectArgument>
                         Optional args As list = Nothing,
                         Optional env As Environment = Nothing) As ggplotOption

        Return New ggplotAxisLabel With {
            .x = x,
            .y = y,
            .title = title
        }
    End Function

    ''' <summary>
    ''' set stats p-value for the plot
    ''' </summary>
    ''' <param name="comparisons">
    ''' 
    ''' a dataframe object that should contains the data fiels at least:
    ''' 
    ''' + group1: the label name of the group 1
    ''' + group2: the label name of the group 2
    ''' + pvalue: a numeric vector of the t-test pvalue between group 1 and group 2.
    ''' 
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("stat_pvalue_manual")>
    <RApiReturn(GetType(ggplotStatsLayer))>
    Public Function stat_pvalue_manual(comparisons As dataframe) As ggplotLayer
        Return New ggplotStatsLayer With {
            .stats = compare_means.fromManualData(comparisons)
        }
    End Function

    ''' <summary>
    ''' default create anova test for compares all groups
    ''' </summary>
    ''' <param name="method"></param>
    ''' <param name="ref_group"></param>
    ''' <param name="hide_ns">hide not sig result?</param>
    ''' <returns></returns>
    <ExportAPI("stat_compare_means")>
    <RApiReturn(GetType(ggplotStatPvalue))>
    Public Function stat_compare_means(Optional method As String = "anova",
                                       Optional ref_group As String = ".all.",
                                       Optional hide_ns As Boolean = True) As ggplotLayer

        Return New ggplotStatPvalue With {
            .method = method,
            .hide_ns = hide_ns,
            .ref_group = ref_group
        }
    End Function

    ''' <summary>
    ''' ## Create significance layer
    ''' 
    ''' </summary>
    ''' <param name="comparisons">
    ''' the comparision groups tuple list, each tuple value should be 
    ''' two group label for extract the corresponding sample vector 
    ''' from the ggplot input raw dataframe for run the ``t.test``.
    ''' </param>
    ''' <param name="test">
    ''' the stats test method between two groups: t.test or wilcox.test
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("geom_signif")>
    <RApiReturn(GetType(ggplotSignifLayer))>
    Public Function geom_signif(comparisons As list, Optional test As String = "t.test") As ggplotLayer
        Return New ggplotSignifLayer With {
            .comparision = comparisons,
            .method = test
        }
    End Function

    ''' <summary>
    ''' ## Modify axis, legend, and plot labels
    ''' 
    ''' Good labels are critical for making your plots accessible to a 
    ''' wider audience. Always ensure the axis and legend labels display 
    ''' the full variable name. Use the plot title and subtitle to 
    ''' explain the main findings. It's common to use the caption to 
    ''' provide information about the data source. tag can be used for 
    ''' adding identification tags to differentiate between multiple 
    ''' plots.
    ''' </summary>
    ''' <param name="label">
    ''' The title of the respective axis (for xlab() or ylab()) or 
    ''' of the plot (for ggtitle()).
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' You can also set axis and legend labels in the individual scales 
    ''' (using the first argument, the name). If you're changing other 
    ''' scale options, this is recommended.
    ''' 
    ''' If a plot already has a title, subtitle, caption, etc., And you want 
    ''' To remove it, you can Do so by setting the respective argument To 
    ''' NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    ''' will remove the subtitle from the plot.
    ''' </remarks>
    <ExportAPI("xlab")>
    <RApiReturn(GetType(ggplotAxisLabel))>
    Public Function xlab(label As String) As ggplotOption
        Return New ggplotAxisLabel With {
            .x = label,
            .y = Nothing,
            .title = Nothing
        }
    End Function

    ''' <summary>
    ''' ## Modify axis, legend, and plot labels
    ''' 
    ''' Good labels are critical for making your plots accessible to a 
    ''' wider audience. Always ensure the axis and legend labels display 
    ''' the full variable name. Use the plot title and subtitle to 
    ''' explain the main findings. It's common to use the caption to 
    ''' provide information about the data source. tag can be used for 
    ''' adding identification tags to differentiate between multiple 
    ''' plots.
    ''' </summary>
    ''' <param name="label">
    ''' The title of the respective axis (for xlab() or ylab()) or 
    ''' of the plot (for ggtitle()).
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' You can also set axis and legend labels in the individual scales 
    ''' (using the first argument, the name). If you're changing other 
    ''' scale options, this is recommended.
    ''' 
    ''' If a plot already has a title, subtitle, caption, etc., And you want 
    ''' To remove it, you can Do so by setting the respective argument To 
    ''' NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    ''' will remove the subtitle from the plot.
    ''' </remarks>
    <ExportAPI("ylab")>
    <RApiReturn(GetType(ggplotAxisLabel))>
    Public Function ylab(label As String) As ggplotOption
        Return New ggplotAxisLabel With {
            .x = Nothing,
            .y = label,
            .title = Nothing
        }
    End Function

    ''' <summary>
    ''' Swapping X- and Y-Axes
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("coord_flip")>
    Public Function coord_flip() As coord_flip
        Return New coord_flip
    End Function

    ''' <summary>
    ''' ## Modify components of a theme
    ''' 
    ''' Themes are a powerful way to customize the non-data components of 
    ''' your plots: i.e. titles, labels, fonts, background, gridlines, and 
    ''' legends. Themes can be used to give plots a consistent customized 
    ''' look. Modify a single plot's theme using theme(); see theme_update() 
    ''' if you want modify the active theme, to affect all subsequent plots. 
    ''' Use the themes available in complete themes if you would like to use 
    ''' a complete theme such as theme_bw(), theme_minimal(), and more. 
    ''' 
    ''' Theme elements are documented together according to inheritance, read
    ''' more about theme inheritance below.
    ''' </summary>
    ''' <param name="axis_text"></param>
    ''' <param name="text">all text elements (element_text())</param>
    ''' <param name="plot_background">background of the entire plot (element_rect(); inherits from rect)</param>
    ''' <param name="legend_background">background of legend (element_rect(); inherits from rect)</param>
    ''' <param name="panel_background">background of plotting area, drawn underneath plot (element_rect(); inherits from rect)</param>
    ''' <param name="legend_text">legend item labels (element_text(); inherits from text)</param>
    ''' <param name="axis_line">lines along axes (element_line()). Specify lines 
    ''' along all axes (axis.line), lines for each plane (using axis.line.x or 
    ''' axis.line.y), or individually for each axis (using axis.line.x.bottom, 
    ''' axis.line.x.top, axis.line.y.left, axis.line.y.right). ``axis.line.*.*`` 
    ''' inherits from axis.line.* which inherits from axis.line, which in turn 
    ''' inherits from line</param>
    ''' <returns>
    ''' A style mapper for create the internal <see cref="Theme"/> object
    ''' </returns>
    ''' <remarks>
    ''' Theme elements inherit properties from other theme elements hierarchically. 
    ''' For example, axis.title.x.bottom inherits from axis.title.x which inherits 
    ''' from axis.title, which in turn inherits from text. All text elements inherit
    ''' directly or indirectly from text; all lines inherit from line, and all 
    ''' rectangular objects inherit from rect. This means that you can modify the 
    ''' appearance of multiple elements by setting a single high-level component.
    ''' 
    ''' Learn more about setting these aesthetics In vignette("ggplot2-specs").
    ''' </remarks>
    <ExportAPI("theme")>
    <RApiReturn(GetType(ggplotTheme))>
    Public Function theme_f(Optional text As textElement = Nothing,
                            Optional axis_text As textElement = Nothing,
                            Optional axis_title As textElement = Nothing,
                            Optional axis_line As Object = Stroke.AxisStroke,
                            Optional axis_text_x As textElement = Nothing,
                            Optional legend_background As String = "white",
                            Optional legend_text As textElement = Nothing,
                            Optional legend_tick As textElement = Nothing,
                            Optional legend_title As textElement = Nothing,
                            Optional legend_split As Integer = 6,
                            Optional plot_background As String = Nothing,
                            Optional plot_title As textElement = Nothing,
                            Optional panel_background As String = Nothing,
                            Optional panel_grid As Object = Stroke.AxisGridStroke,
                            Optional panel_grid_major As Object = Stroke.AxisGridStroke,
                            Optional panel_border As rectElement = Nothing) As ggplotOption
        ' 20220829
        ' 大部分的参数值都应该设置为空值
        ' 否则会在配置theme对象的时候出现错误覆盖的问题
        Return New ggplotTheme With {
            .axis_text = axis_text,
            .text = text,
            .legend_background = legend_background,
            .plot_background = plot_background,
            .panel_background = panel_background,
            .panel_grid = options.element_blank.GetCssStroke(If(panel_grid, panel_grid_major)),
            .axis_line = options.element_blank.GetCssStroke(axis_line),
            .legend_text = legend_text,
            .plot_title = plot_title,
            .axis_title = axis_title,
            .legend_split = legend_split,
            .axis_text_x = axis_text_x,
            .panel_border = panel_border,
            .legend_tick = legend_tick,
            .legend_title = legend_title
        }
    End Function

    ''' <summary>
    ''' means nothing
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("element_blank")>
    Public Function element_blank() As element_blank
        Return New element_blank()
    End Function

    ''' <summary>
    ''' ### A waiver object.
    ''' 
    ''' A waiver is a "flag" object, similar to NULL, that indicates the calling 
    ''' function should just use the default value. It is used in certain functions 
    ''' to distinguish between displaying nothing (NULL) and displaying a default 
    ''' value calculated elsewhere (waiver())
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("waiver")>
    Public Function waiver() As element_blank
        Return New element_blank With {.waiver = True}
    End Function

    ''' <summary>
    ''' Theme element: line.
    ''' </summary>
    ''' <param name="colour">line colour</param>
    ''' <param name="size">line size</param>
    ''' <param name="linetype">line type</param>
    ''' <param name="lineend">line end</param>
    ''' <param name="color">an alias for ``colour``</param>
    ''' <returns></returns>
    <ExportAPI("element_line")>
    Public Function element_line(Optional colour As Object = NULL,
                                 Optional size As Object = NULL,
                                 Optional linetype As Object = NULL,
                                 Optional lineend As Object = NULL,
                                 Optional color As Object = NULL) As lineElement

        Dim col As String = RColorPalette.getColor(If(colour, color), [default]:="black")
        Dim w As Double = CLRVector.asNumeric(size).DefaultFirst(-1)
        Dim dash As DashStyle = DashStyle.Solid

        If linetype IsNot Nothing Then
            If TypeOf linetype Is String OrElse TypeOf linetype Is Char Then
                dash = Stroke.GetDashStyle(CStr(linetype))
            ElseIf TypeOf linetype Is Integer OrElse TypeOf linetype Is Long Then
                dash = CType(CInt(linetype), DashStyle)
            End If
        End If

        Return New lineElement With {
            .color = col,
            .width = w,
            .linetype = dash
        }
    End Function

    ''' <summary>
    ''' ### Modify axis, legend, and plot labels
    ''' 
    ''' Good labels are critical for making your plots accessible 
    ''' to a wider audience. Always ensure the axis and legend 
    ''' labels display the full variable name. Use the plot title 
    ''' and subtitle to explain the main findings. It's common to 
    ''' use the caption to provide information about the data 
    ''' source. tag can be used for adding identification tags to 
    ''' differentiate between multiple plots.
    ''' </summary>
    ''' <param name="title">The text for the title.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' You can also set axis and legend labels in the individual 
    ''' scales (using the first argument, the name). If you're
    ''' changing other scale options, this is recommended.
    ''' 
    ''' If a plot already has a title, subtitle, caption, etc., And
    ''' you want To remove it, you can Do so by setting the respective 
    ''' argument To NULL. For example, If plot p has a subtitle, Then
    ''' p + labs(subtitle = NULL) will remove the subtitle from the 
    ''' plot.
    ''' </remarks>
    <ExportAPI("ggtitle")>
    Public Function ggtitle(title As String, Optional text_wrap As Boolean = False) As ggplotOption
        Return New ggplotTitle(title) With {.text_wrap = text_wrap}
    End Function

    ''' <summary>
    ''' ### Create your own discrete scale
    ''' 
    ''' These functions allow you to specify your own set of 
    ''' mappings from levels in the data to aesthetic values.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' ### Color Blindness
    ''' 
    ''' Many color palettes derived from RGB combinations (Like 
    ''' the "rainbow" color palette) are Not suitable To support 
    ''' all viewers, especially those With color vision 
    ''' deficiencies. 
    ''' 
    ''' Using viridis type, which Is perceptually uniform In both 
    ''' colour And black-And-white display Is an easy Option To 
    ''' ensure good perceptive properties Of your visulizations. 
    ''' The colorspace package offers functionalities.
    ''' 
    ''' to generate color palettes with good perceptive properties,
    ''' 
    ''' to analyse a given color palette, Like emulating color 
    ''' blindness,
    ''' 
    ''' And to modify a given color palette for better perceptivity.
    ''' 
    ''' For more information on color vision deficiencies And 
    ''' suitable color choices see the paper on the colorspace 
    ''' package And references therein.
    ''' </remarks>
    <ExportAPI("scale_colour_manual")>
    <RApiReturn(GetType(ggplotColorProfile))>
    Public Function scale_colour_manual(<RRawVectorArgument>
                                        values As Object,
                                        Optional alpha As Double = 1,
                                        Optional env As Environment = Nothing) As ggplotOption

        Return New ggplotColorProfile With {
            .profile = ggplotColorMap.CreateColorMap(values, alpha, env)
        }
    End Function

    ''' <summary>
    ''' Sequential, diverging and qualitative colour scales from ColorBrewer
    ''' </summary>
    ''' <param name="name">
    ''' The name of the scale. Used as the axis or legend title. If waiver(), the default,
    ''' the name of the scale is taken from the first mapping used for that aesthetic. 
    ''' If NULL, the legend title will be omitted.
    ''' </param>
    ''' <param name="direction">
    ''' Sets the order Of colours In the scale. If 1, the Default, colours are 
    ''' As output by RColorBrewer:brewer.pal(). If -1, the order of colours 
    ''' Is reversed.
    ''' </param>
    ''' <param name="alpha">
    ''' color alpha channel value between [0,1]
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' The brewer scales provide sequential, diverging and qualitative colour
    ''' schemes from ColorBrewer. These are particularly well suited to display 
    ''' discrete values on a map. See https://colorbrewer2.org for more 
    ''' information.
    ''' 
    ''' The brewer scales were carefully designed and tested on discrete data. 
    ''' They were not designed to be extended to continuous data, but results 
    ''' often look good. Your mileage may vary.
    ''' 
    ''' The following palettes are available for use with these scales:
    ''' 
    ''' #### Diverging
    ''' BrBG, PiYG, PRGn, PuOr, RdBu, RdGy, RdYlBu, RdYlGn, Spectral
    ''' 
    ''' #### Qualitative
    ''' Accent, Dark2, Paired, Pastel1, Pastel2, Set1, Set2, Set3
    ''' 
    ''' #### Sequential
    ''' Blues, BuGn, BuPu, GnBu, Greens, Greys, Oranges, OrRd, PuBu, PuBuGn, PuRd, Purples, RdPu, Reds, YlGn, YlGnBu, YlOrBr, YlOrRd
    ''' 
    ''' Modify the palette through the palette argument.
    ''' </remarks>
    <ExportAPI("scale_color_brewer")>
    <RApiReturn(GetType(ggplotOption))>
    Public Function scale_color_brewer(<RRawVectorArgument(TypeCodes.string)>
                                       Optional name As Object = "BrBG|PiYG|PRGn|PuOr|RdBu|RdGy|RdYlBu|RdYlGn|Spectral|Accent|Dark2|Paired|Pastel1|Pastel2|Set1|Set2|Set3|Blues|BuGn|BuPu|GnBu|Greens|Greys|Oranges|OrRd|PuBu|PuBuGn|PuRd|Purples|RdPu|Reds|YlGn|YlGnBu|YlOrBr|YlOrRd",
                                       Optional direction As Integer = 1,
                                       Optional alpha As Double = 1,
                                       Optional env As Environment = Nothing) As Object

        Dim pal_name As String = CLRVector.asCharacter(name).DefaultFirst

        If pal_name.StringEmpty Then
            Return RInternal.debug.stop("the required color brewer color palette name should not be empty!", env)
        End If

        Dim colors As ggplotColorMap = ggplotColorProfile.MapColorBrewer(pal_name, alpha)

        If colors Is Nothing Then
            Return RInternal.debug.stop({
                $"the given color palette name({pal_name}) is not a color brewer palette name!",
                $"given palette: {pal_name}"
            }, env)
        End If

        Return New ggplotColorProfile With {
            .profile = colors
        }
    End Function

    <ExportAPI("scale_fill_manual")>
    <RApiReturn(GetType(ggplotColorProfile))>
    Public Function scale_fill_manual(<RRawVectorArgument>
                                      values As Object,
                                      Optional alpha As Double = 1,
                                      Optional env As Environment = Nothing) As ggplotOption

        Return New ggplotColorProfile With {
            .profile = ggplotColorMap.CreateColorMap(values, alpha, env)
        }
    End Function

    <ExportAPI("scale_fill_distiller")>
    <RApiReturn(GetType(ggplotColorProfile))>
    Public Function scale_fill_distiller(Optional palette As String = "YlGnBu",
                                         Optional direction As Integer = 1,
                                         Optional alpha As Double = 1,
                                         Optional env As Environment = Nothing) As Object

        Return New ggplotColorProfile With {
            .profile = ggplotColorMap.CreateColorMap(palette, alpha, env)
        }
    End Function

    ''' <summary>
    ''' Position scales for continuous data (x &amp; y)
    ''' </summary>
    ''' <param name="labels">
    ''' One of:
    ''' 
    ''' + ``NULL`` for no labels
    ''' + waiver() for the default labels computed by the transformation object
    ''' + A character vector giving labels (must be same length As breaks)
    ''' + A Function that() takes the breaks As input And returns labels As output. Also accepts rlang lambda Function notation.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("scale_x_continuous")>
    <RApiReturn(GetType(ggplotTicks))>
    Public Function scale_x_continuous(Optional labels As String = Nothing,
                                       Optional limits As Double() = Nothing,
                                       Optional env As Environment = Nothing) As ggplotOption

        If limits Is Nothing Then
            limits = {}
        End If
        If Not limits.IsNullOrEmpty AndAlso limits.Length = 1 Then

        End If

        Return New ggplotTicks With {
            .axis = "x",
            .format = ggplotTicks.ParseFormat(labels),
            .min = limits.FirstOrDefault,
            .max = limits.LastOrDefault
        }
    End Function

    ''' <summary>
    ''' ### Position scales for continuous data (x &amp; y)
    ''' </summary>
    ''' <param name="labels">
    ''' One of:
    ''' 
    ''' + ``NULL`` for no labels
    ''' + waiver() for the default labels computed by the transformation object
    ''' + A character vector giving labels (must be same length As breaks)
    ''' + A Function that() takes the breaks As input And returns labels As output. Also accepts rlang lambda Function notation.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("scale_y_continuous")>
    Public Function scale_y_continuous(Optional labels As String = Nothing,
                                       Optional limits As Double() = Nothing,
                                       Optional env As Environment = Nothing) As ggplotOption

        If limits Is Nothing Then
            limits = {}
        End If
        If Not limits.IsNullOrEmpty AndAlso limits.Length = 1 Then

        End If

        Return New ggplotTicks With {
            .axis = "y",
            .format = ggplotTicks.ParseFormat(labels),
            .min = limits.FirstOrDefault,
            .max = limits.LastOrDefault
        }
    End Function

    ''' <summary>
    ''' ### Position scales for continuous data (x &amp; y)
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("scale_y_reverse")>
    Public Function scale_y_reverse() As ggplotOption
        Return New ggplotTicks With {
            .axis = "y",
            .reverse = True,
            .format = Nothing
        }
    End Function

    Const NULL As Object = Nothing

    ReadOnly defaultTextColor As [Default](Of String) = "black"

    ''' <summary>
    ''' ### Theme elements
    ''' 
    ''' text.
    ''' </summary>
    ''' <param name="family">Font family</param>
    ''' <param name="face">Font face ("plain", "italic", "bold", "bold.italic")</param>
    ''' <param name="size">Line/border size in mm; text size in pts.</param>
    ''' <param name="hjust">Horizontal justification (in [0, 1])</param>
    ''' <param name="vjust">Vertical justification (in [0, 1])</param>
    ''' <param name="angle">Angle (in [0, 360])</param>
    ''' <param name="lineheight">Line height</param>
    ''' <param name="color">Line/border colour. Color is an alias for colour.</param>
    ''' <param name="margin">
    ''' Margins around the text. See margin() for more details. When creating a theme, 
    ''' the margins should be placed on the side of the text facing towards the center 
    ''' of the plot.
    ''' </param>
    ''' <param name="debug">
    ''' If TRUE, aids visual debugging by drawing a solid rectangle behind the complete 
    ''' text area, and a point where each label is anchored.
    ''' </param>
    ''' <param name="inherit_blank">
    ''' Should this element inherit the existence of an element_blank among its parents? 
    ''' If TRUE the existence of a blank element among its parents will cause this 
    ''' element to be blank as well. If FALSE any blank parent element will be ignored 
    ''' when calculating final element state.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("element_text")>
    Public Function element_text(Optional family$ = NULL,
                                 Optional face$ = NULL,
                                 Optional size! = NULL,
                                 Optional hjust! = NULL,
                                 Optional vjust! = NULL,
                                 Optional angle! = NULL,
                                 Optional lineheight! = NULL,
                                 Optional color$ = NULL,
                                 Optional margin! = NULL,
                                 Optional debug As Boolean = False,
                                 Optional inherit_blank As Boolean = False) As textElement

        Dim css As New CSSFont With {
            .family = If(family, FontFace.MicrosoftYaHei),
            .color = color Or defaultTextColor,
            .size = If(size = 0, 24, size),
            .weight = lineheight
        }

        Return New textElement With {
            .style = css,
            .color = color Or defaultTextColor,
            .angle = angle
        }
    End Function

    ''' <summary>
    ''' ## Theme elements
    ''' 
    ''' In conjunction with the theme system, the ``element_`` functions
    ''' specify the display of how non-data components of the plot are 
    ''' drawn.
    ''' 
    ''' borders and backgrounds.
    ''' </summary>
    ''' <param name="fill">Fill colour.</param>
    ''' <param name="color">Line/border colour. Color is an alias for colour.</param>
    ''' <param name="colour">Line/border colour. Color is an alias for colour.</param>
    ''' <param name="size">Line/border size in mm; text size in pts.</param>
    ''' <param name="linetype">
    ''' Line type. An integer (0:8), a name (blank, solid, dashed, dotted, 
    ''' dotdash, longdash, twodash), or a string with an even number (up 
    ''' to eight) of hexadecimal digits which give the lengths in consecutive 
    ''' positions in the string.
    ''' </param>
    ''' <param name="inherit_blank">
    ''' Should this element inherit the existence of an element_blank 
    ''' among its parents? If TRUE the existence of a blank element
    ''' among its parents will cause this element to be blank as well. 
    ''' If FALSE any blank parent element will be ignored when calculating
    ''' final element state.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("element_rect")>
    Public Function element_rect(Optional fill As Object = NULL,
                                 Optional colour As Object = NULL,
                                 Optional size As Single = 1,
                                 Optional linetype As DashStyle = DashStyle.Solid,
                                 Optional color As Object = NULL,
                                 Optional inherit_blank As Boolean = False,
                                 Optional env As Environment = Nothing) As rectElement

        Dim css As New Stroke

        css.fill = RColorPalette.getColor(If(colour, color), [default]:="black")
        css.dash = linetype
        css.width = size

        Return New rectElement With {
            .fill = RColorPalette.getColor(fill, [default]:=Nothing),
            .border = css
        }
    End Function
End Module
