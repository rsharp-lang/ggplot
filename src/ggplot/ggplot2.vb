#Region "Microsoft.VisualBasic::f013147e9f47b39ef191ae3ef7c3b77d, src\ggplot2.vb"

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

' Module ggplot2
' 
'     Function: add_layer, aes, configPlot, element_text, geom_boxplot
'               geom_histogram, geom_hline, geom_line, geom_path, geom_point
'               geom_text, geom_vline, ggplot, ggtitle, labs
'               scale_colour_manual, scale_x_continuous, scale_y_continuous, scale_y_reverse, theme
'               xlab, ylab
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
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' Create Elegant Data Visualisations Using the Grammar of Graphics
''' </summary>
<Package("ggplot2")>
Public Module ggplot2

    ''' <summary>
    ''' ### Create a new ggplot
    ''' 
    ''' ggplot() initializes a ggplot object. It can be used to declare 
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
    ''' <returns></returns>
    ''' <remarks>
    ''' ggplot() is used to construct the initial plot object, and is 
    ''' almost always followed by + to add component to the plot. There 
    ''' are three common ways to invoke ggplot():
    ''' 
    ''' + ggplot(df, aes(x, y, other aesthetics))
    ''' + ggplot(df)
    ''' + ggplot()
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
            .padding = InteropArgumentHelper.getPadding(args.getByName("padding"), g.DefaultUltraLargePadding),
            .drawLegend = True,
            .legendLabelCSS = "font-style: normal; font-size: 13; font-family: " & FontFace.MicrosoftYaHei & ";",
            .colorSet = RColorPalette.getColorSet(colorSet, [default]:="paper")
        }
        Dim args2 = environment.GetAcceptorArguments
        Dim ggplotDriver As ggplot = ggplot.CreateRender(data, theme)
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
            .environment = environment
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
    ''' <param name="color"></param>
    ''' <param name="class">
    ''' mapping data of the element class group.
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
                        Optional label As Object = Nothing,
                        Optional color As Object = Nothing,
                        Optional title As String = Nothing,
                        Optional shape As Object = Nothing,
                        Optional [class] As Object = Nothing,
                        <RListObjectArgument>
                        Optional args As list = Nothing,
                        Optional env As Environment = Nothing) As ggplotReader

        Return New ggplotReader With {
            .x = x,
            .y = y,
            .z = z,
            .color = color,
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
    ''' <returns></returns>
    <ExportAPI("geom_point")>
    Public Function geom_point(Optional mapping As ggplotReader = NULL,
                               <RRawVectorArgument>
                               Optional color As Object = Nothing,
                               Optional shape As LegendStyles? = Nothing,
                               Optional stroke As Object = Nothing,
                               Optional size As Single = 2,
                               Optional show_legend As Boolean = True,
                               Optional alpha As Double = 1,
                               Optional env As Environment = Nothing) As ggplotLayer

        Dim colorMap As ggplotColorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color, Nothing), alpha, env)
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
            .colorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color), alpha, env),
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
    ''' Histograms (geom_histogram()) display the counts with bars; 
    ''' </summary>
    ''' <param name="bins">
    ''' Number of bins. Overridden by binwidth. Defaults to 30.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("geom_histogram")>
    Public Function geom_histogram(bins As Integer,
                                   Optional color As Object = Nothing,
                                   Optional alpha As Double = 1,
                                   Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotHistogram With {
            .bins = bins,
            .colorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color), alpha, env)
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
    ''' <returns></returns>
    ''' <remarks>
    ''' An alternative parameterisation is geom_segment(), where each line corresponds 
    ''' to a single case which provides the start and end coordinates.
    ''' </remarks>
    <ExportAPI("geom_line")>
    Public Function geom_line(Optional mapping As ggplotReader = NULL,
                              <RRawVectorArgument>
                              Optional color As Object = Nothing,
                              Optional width As Single = 5,
                              Optional show_legend As Boolean = True,
                              Optional alpha As Double = 1,
                              Optional env As Environment = Nothing) As ggplotLayer

        Dim rawColor As String = RColorPalette.getColor(color, Nothing)
        Dim colorMap As ggplotColorMap = ggplotColorMap.CreateColorMap(rawColor, alpha, env)

        Return New ggplotLine With {
            .showLegend = show_legend,
            .colorMap = colorMap,
            .line_width = width,
            .reader = mapping
        }
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="yintercept"></param>
    ''' <param name="color"></param>
    ''' <param name="line_width!"></param>
    ''' <param name="linetype"></param>
    ''' <returns></returns>
    <ExportAPI("geom_hline")>
    Public Function geom_hline(yintercept As Double,
                               Optional color As Object = "black",
                               Optional line_width! = 2,
                               Optional linetype As DashStyle = DashStyle.Solid) As ggplotLayer

        Dim a As New PointF(Single.NegativeInfinity, yintercept)
        Dim b As New PointF(Single.PositiveInfinity, yintercept)
        Dim style As New Pen(RColorPalette.getColor(color).TranslateColor, line_width) With {
            .DashStyle = linetype
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
        Dim style As New Pen(RColorPalette.getColor(color).TranslateColor, line_width) With {
            .DashStyle = linetype
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
    Public Function geom_boxplot(Optional width As Double = 1) As ggplotLayer
        Return New ggplotBoxplot With {.groupWidth = width}
    End Function

    <ExportAPI("geom_barplot")>
    Public Function geom_barplot(Optional width As Double = 1) As ggplotLayer
        Return New ggplotBarplot With {.groupWidth = width}
    End Function

    <ExportAPI("geom_violin")>
    Public Function geom_violin(Optional width As Double = 0.9) As ggplotLayer
        Return New ggplotViolin With {.groupWidth = width}
    End Function

    <ExportAPI("geom_jitter")>
    Public Function geom_jitter(Optional mapping As ggplotReader = NULL,
                                Optional data As Object = NULL,
                                Optional stat As Object = "identity",
                                Optional width As Double = 0.5,
                                Optional radius As Double = 10,
                                Optional alpha As Double = 0.85) As ggplotLayer

        Return New ggplotJitter With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .radius = radius
        }
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
                Return Internal.debug.stop("the given ggplot layer object can not be nothing!", ggplot.environment)
            Else
                Call ggplot.environment.AddMessage("the given ggplot layer object is nothing...", MSG_TYPES.WRN)
                Return ggplot
            End If
        End If

        If Not ggplot.base.reader.isPlain2D Then
            If TypeOf layer Is ggplotScatter Then
                layer = New ggplotScatter3d(layer)
            End If
        End If

        If TypeOf layer Is ggplotHistogram Then
            Call ggplotHistogram.configHistogram(ggplot, layer)
        End If

        ggplot.layers.Add(layer)
        layer.zindex = ggplot.layers.Count

        Return ggplot
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

    <ExportAPI("stat_pvalue_manual")>
    Public Function stat_pvalue_manual() As ggplotLayer
        Return New ggplotStatPvalue
    End Function

    <ExportAPI("stat_compare_means")>
    Public Function stat_compare_means() As ggplotLayer
        Return New ggplotStatPvalue
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
    Public Function ylab(label As String) As ggplotOption
        Return New ggplotAxisLabel With {
            .x = Nothing,
            .y = label,
            .title = Nothing
        }
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
    ''' <returns></returns>
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
    Public Function theme(Optional text As textElement = Nothing,
                          Optional axis_text As textElement = Nothing,
                          Optional axis_title As textElement = Nothing,
                          Optional axis_line As String = Stroke.AxisStroke,
                          Optional axis_text_x As textElement = Nothing,
                          Optional legend_background As String = "white",
                          Optional legend_text As textElement = Nothing,
                          Optional legend_split As Integer = 6,
                          Optional plot_background As String = "white",
                          Optional plot_title As textElement = Nothing,
                          Optional panel_background As String = "white",
                          Optional panel_grid As String = Stroke.AxisGridStroke) As ggplotOption

        Return New ggplotTheme With {
            .axis_text = axis_text,
            .text = text,
            .legend_background = legend_background,
            .plot_background = plot_background,
            .panel_background = panel_background,
            .panel_grid = panel_grid,
            .axis_line = axis_line,
            .legend_text = legend_text,
            .plot_title = plot_title,
            .axis_title = axis_title,
            .legend_split = legend_split,
            .axis_text_x = axis_text_x
        }
    End Function

    <ExportAPI("ggtitle")>
    Public Function ggtitle(title As String) As ggplotOption
        Return New ggplotTitle(title)
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
    Public Function scale_colour_manual(<RRawVectorArgument>
                                        values As Object,
                                        Optional alpha As Double = 1,
                                        Optional env As Environment = Nothing) As ggplotOption

        Return New ggplotColorProfile With {
            .profile = ggplotColorMap.CreateColorMap(values, alpha, env)
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
End Module

