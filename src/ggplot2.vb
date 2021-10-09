Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.MetaData
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
                           Optional mapping As Object = "~aes()",
                           <RListObjectArgument>
                           Optional args As list = Nothing,
                           Optional environment As Environment = Nothing)

        Dim base As New ggplotBase With {.reader = mapping}
        Dim theme As New Theme With {
            .axisLabelCSS = "font-style: strong; font-size: 12; font-family: " & FontFace.MicrosoftYaHei & ";",
            .axisTickCSS = "font-style: normal; font-size: 10; font-family: " & FontFace.MicrosoftYaHei & ";",
            .padding = InteropArgumentHelper.getPadding(args.getByName("padding"), g.DefaultUltraLargePadding),
            .drawLegend = True,
            .legendLabelCSS = "font-style: normal; font-size: 13; font-family: " & FontFace.MicrosoftYaHei & ";"
        }
        Dim args2 = environment.GetAcceptorArguments

        ' union arguments
        For Each arg In args2
            If Not args.hasName(arg.Key) Then
                Call args.add(arg.Key, arg.Value)
            End If
        Next

        Return New ggplot(theme) With {
            .data = data,
            .layers = New Queue(Of ggplotLayer),
            .base = base,
            .args = args,
            .environment = environment,
            .xlabel = base.reader.x,
            .ylabel = base.reader.y
        }
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
                        Optional color As Object = Nothing,
                        Optional env As Environment = Nothing) As Object

        Return New ggplotReader With {
            .x = x,
            .y = y,
            .color = color
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
                               Optional color As Object = "steelblue",
                               Optional shape As LegendStyles = LegendStyles.Circle,
                               Optional size As Single = 2,
                               Optional show_legend As Boolean = True,
                               Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotScatter With {
            .colorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color), env),
            .shape = shape,
            .size = size,
            .showLegend = show_legend,
            .reader = mapping
        }
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
                              <RListObjectArgument>
                              Optional args As list = Nothing,
                              Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotTextLabel With {
            .reader = mapping,
            .showLegend = show_legend,
            .colorMap = ggplotColorMap.CreateColorMap(RColorPalette.getColor(color), env)
        }
    End Function

    <ExportAPI("geom_histogram")>
    Public Function geom_histogram() As ggplotLayer
        Return New ggplotHistogram
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
    Public Function geom_line() As ggplotLayer
        Return New ggplotLine
    End Function

    <ExportAPI("geom_path")>
    Public Function geom_path() As ggplotLayer

    End Function

    <ExportAPI("geom_boxplot")>
    Public Function geom_boxplot() As ggplotLayer
        Return New ggplotBoxplot
    End Function

    <ROperator("+")>
    Public Function addLayer(ggplot As ggplot, layer As ggplotLayer) As ggplot
        ggplot.layers.Enqueue(layer)
        Return ggplot
    End Function

    <ROperator("+")>
    Public Function configPlot(ggplot As ggplot, opts As ggplotOption) As ggplot
        Return opts.Config(ggplot)
    End Function

    <ExportAPI("labs")>
    Public Function labs(Optional x As String = Nothing,
                         Optional y As String = Nothing,
                         Optional title As String = Nothing) As ggplotOption

        Return New ggplotAxisLabel With {
            .x = x,
            .y = y,
            .title = title
        }
    End Function

    <ExportAPI("theme")>
    Public Function theme() As ggplotOption
        Return New ggplotTheme
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
    Public Function scale_colour_manual(<RRawVectorArgument> values As Object, Optional env As Environment = Nothing) As ggplotOption
        Return New ggplotColorProfile With {.profile = ggplotColorMap.CreateColorMap(values, env)}
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

    Const NULL As Object = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="family$"></param>
    ''' <param name="face$"></param>
    ''' <param name="colour$"></param>
    ''' <param name="size!"></param>
    ''' <param name="hjust!"></param>
    ''' <param name="vjust!"></param>
    ''' <param name="angle!"></param>
    ''' <param name="lineheight!"></param>
    ''' <param name="color$"></param>
    ''' <param name="margin!"></param>
    ''' <param name="debug"></param>
    ''' <param name="inherit_blank"></param>
    ''' <returns></returns>
    <ExportAPI("element_text")>
    Public Function element_text(Optional family$ = NULL,
                                 Optional face$ = NULL,
                                 Optional colour$ = NULL,
                                 Optional size!? = NULL,
                                 Optional hjust!? = NULL,
                                 Optional vjust!? = NULL,
                                 Optional angle!? = NULL,
                                 Optional lineheight!? = NULL,
                                 Optional color$ = NULL,
                                 Optional margin!? = NULL,
                                 Optional debug As Boolean = False,
                                 Optional inherit_blank As Boolean = False) As textElement

        Dim css As New CSSFont With {
            .family = family,
            .color = color,
            .size = size,
            .weight = lineheight
        }

        Return New textElement With {
            .style = css
        }
    End Function
End Module
