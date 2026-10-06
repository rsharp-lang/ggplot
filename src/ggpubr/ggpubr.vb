#Region "Microsoft.VisualBasic::1240305cb49576420838dfd7dd80ff79, src\ggpubr\ggpubr.vb"

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

    '   Total Lines: 75
    '    Code Lines: 54 (72.00%)
    ' Comment Lines: 15 (20.00%)
    '    - Xml Docs: 86.67%
    ' 
    '   Blank Lines: 6 (8.00%)
    '     File Size: 3.65 KB


    ' Module Rscript
    ' 
    '     Function: geom_text_repel, stat_ellipse
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot
Imports ggplot.colors
Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Math.Statistics
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("ggpubr")>
Module Rscript

    Const NULL As Object = Nothing

    ''' <summary>
    ''' ### Compute normal data ellipses
    ''' 
    ''' 
    ''' </summary>
    ''' <param name="data">
    ''' The data to be displayed in this layer. There are three options:
    ''' If NULL, the Default, the data Is inherited from the plot data As specified In the Call To ggplot().
    ''' A data.frame, Or other Object, will override the plot data. All objects will be fortified To produce a data frame. See fortify() For which variables will be created.
    ''' A Function will be called With a Single argument, the plot data. The Return value must be a data.frame, And will be used As the layer data. A Function can be created from a formula (e.g. ~ head(.x, 10)).
    ''' </param>
    ''' <param name="color"></param>
    ''' <param name="level">The level at which to draw an ellipse, or, if type="euclid", the radius of the circle to be drawn.</param>
    ''' <param name="alpha"></param>
    ''' <returns></returns>
    <ExportAPI("stat_ellipse")>
    Public Function stat_ellipse(<RRawVectorArgument>
                                 Optional data As Object = Nothing,
                                 Optional color As Object = Nothing,
                                 Optional level As Double = 0.95,
                                 Optional alpha As Double = 0.6) As ggplotLayer

        Return New ggplotConfidenceEllipse With {
            .level = ChiSquareTest.TranslateLevel(level),
            .showLegend = False,
            .alpha = alpha
        }
    End Function

    <ExportAPI("geom_text_repel")>
    Public Function geom_text_repel(Optional mapping As ggplotReader = NULL,
                                    Optional data As Object = NULL,
                                    Optional stat$ = "identity",
                                    Optional position$ = "identity",
                                    Optional parse As Boolean = False,
                                    Optional nudge_x! = 0,
                                    Optional nudge_y! = 0,
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
                                    Optional env As Environment = Nothing) As ggplotTextRepelLabel

        Return New ggplotTextRepelLabel With {
            .reader = mapping,
            .showLegend = show_legend,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env),
            .which = which,
            .check_overlap = True,
            .fontSize = size
        }
    End Function

    ''' <summary>
    ''' ggboxplot(): a box plot of every group, optionally annotated with the
    ''' significance stars of the group comparisons.
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="width">the width of a single box</param>
    ''' <param name="alpha"></param>
    ''' <param name="add_jitter">overlay a jitter plot on the box plot</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggboxplot")>
    Public Function ggboxplot(Optional mapping As ggplotReader = Nothing,
                             Optional data As Object = Nothing,
                             Optional color As Object = Nothing,
                             Optional width As Double = 0.5,
                             Optional alpha As Double = 0.95,
                             Optional add_jitter As Boolean = False,
                             Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotBoxplot With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        }
    End Function

    ''' <summary>
    ''' ggviolin(): a violin plot of every group
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="width">the width of a single violin</param>
    ''' <param name="alpha"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggviolin")>
    Public Function ggviolin(Optional mapping As ggplotReader = Nothing,
                            Optional data As Object = Nothing,
                            Optional color As Object = Nothing,
                            Optional width As Double = 0.9,
                            Optional alpha As Double = 0.95,
                            Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotViolin With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        }
    End Function

    ''' <summary>
    ''' ggstrip(): a jittered scatter plot, which shows every single
    ''' observation of a group
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="width">the jitter width</param>
    ''' <param name="alpha"></param>
    ''' <param name="size">the size of the point</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggstrip")>
    Public Function ggstrip(Optional mapping As ggplotReader = Nothing,
                           Optional data As Object = Nothing,
                           Optional color As Object = Nothing,
                           Optional width As Double = 0.4,
                           Optional alpha As Double = 0.85,
                           Optional size As Double = 1.5,
                           Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotJitter With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .radius = size * 5,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        }
    End Function

    ''' <summary>
    ''' ggbeeswarm(): a beeswarm plot, which arranges the observations of a
    ''' group symmetrically around the group center so that the density of the
    ''' group becomes readable
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="direction">up or down</param>
    ''' <param name="size">the size of a single dot</param>
    ''' <param name="alpha"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggbeeswarm")>
    Public Function ggbeeswarm(Optional mapping As ggplotReader = Nothing,
                              Optional data As Object = Nothing,
                              Optional color As Object = Nothing,
                              Optional direction As String = "up",
                              Optional size As Double = 6,
                              Optional alpha As Double = 1,
                              Optional env As Environment = Nothing) As ggplotBeeswarm

        Return New ggplotBeeswarm With {
            .reader = mapping,
            .direction = direction,
            .swarmSize = size,
            .alpha = alpha,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        }
    End Function

    ''' <summary>
    ''' ggerrorbar(): the mean of every group together with its error bar
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="errorType">mean_se, mean_sd, mean_ci or sqrt_n</param>
    ''' <param name="width">the width of the error bar caps</param>
    ''' <param name="addPoint">draw the mean point</param>
    ''' <param name="alpha"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggerrorbar")>
    Public Function ggerrorbar(Optional mapping As ggplotReader = Nothing,
                              Optional data As Object = Nothing,
                              Optional color As Object = Nothing,
                              Optional errorType As String = "mean_se",
                              Optional width As Double = 0.2,
                              Optional addPoint As Boolean = True,
                              Optional alpha As Double = 1,
                              Optional env As Environment = Nothing) As ggplotErrorBarLayer

        Return New ggplotErrorBarLayer With {
            .reader = mapping,
            .errorType = errorType,
            .errorWidth = width,
            .addPoint = addPoint,
            .alpha = alpha,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env)
        }
    End Function

    ''' <summary>
    ''' ggpaired(): connects the two measurements of every subject by a line
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="group">the column which identifies a subject</param>
    ''' <param name="alpha">the transparency of the connecting line</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggpaired")>
    Public Function ggpaired(Optional mapping As ggplotReader = Nothing,
                            Optional data As Object = Nothing,
                            Optional group As String = Nothing,
                            Optional alpha As Double = 0.4,
                            Optional env As Environment = Nothing) As ggplotPaired

        Return New ggplotPaired With {
            .reader = mapping,
            .group = group,
            .lineAlpha = alpha
        }
    End Function

    ''' <summary>
    ''' ggdotplot(): a dot plot which stacks the observations of a group along
    ''' the value axis
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <param name="data"></param>
    ''' <param name="color"></param>
    ''' <param name="binwidth"></param>
    ''' <param name="stackdir">center, up or down</param>
    ''' <param name="dotsize"></param>
    ''' <param name="alpha"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggdotplot")>
    Public Function ggdotplot(Optional mapping As ggplotReader = Nothing,
                             Optional data As Object = Nothing,
                             Optional color As Object = Nothing,
                             Optional binwidth As Double = 1,
                             Optional stackdir As String = "center",
                             Optional dotsize As Double = 3,
                             Optional alpha As Double = 1,
                             Optional env As Environment = Nothing) As ggplotDotplot

        Return New ggplotDotplot With {
            .reader = mapping,
            .colorMap = ggplotColorMap.CreateColorMap(color, alpha, env),
            .binwidth = binwidth,
            .stackdir = stackdir,
            .dotsize = dotsize,
            .alpha = alpha
        }
    End Function

    ''' <summary>
    ''' ggsignif(): annotate the plot with the significance stars of the group
    ''' comparisons
    ''' </summary>
    ''' <param name="comparisons">the group pairs to be compared</param>
    ''' <param name="method">the group comparision method</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("ggsignif")>
    Public Function ggsignif(<RListObjectArgument> Optional comparisons As list = Nothing,
                            Optional method As String = "t.test",
                            Optional env As Environment = Nothing) As ggplotSignifLayer

        Return New ggplotSignifLayer With {
            .comparision = comparisons,
            .method = method
        }
    End Function

    ''' <summary>
    ''' extract a numeric matrix from the columns of a dataframe
    ''' </summary>
    Private Function asMatrix(data As Object, columns As String()) As Double(,)
        Dim table As dataframe = TryCast(data, dataframe)

        If table Is Nothing Then Return Nothing

        Dim names As String() = If(columns Is Nothing OrElse columns.Length = 0, table.colnames, columns)
        Dim matrix As Double(,) = New Double(names.Length - 1, 0) {}
        Dim vectors As New List(Of Double())

        For Each name As String In names
            If Not table.hasName(name) Then Return Nothing

            Call vectors.Add(CLRVector.asNumeric(table.getColumnVector(name)))
        Next

        Dim n As Integer = vectors.Min(Function(v) v.Length)
        Dim result As Double(,) = New Double(vectors.Count - 1, n - 1) {}

        For i As Integer = 0 To vectors.Count - 1
            For j As Integer = 0 To n - 1
                Call result.SetValue(vectors(i)(j), i, j)
            Next
        Next

        Return result
    End Function

    ''' <summary>
    ''' ggcorr(): the correlation matrix of a dataframe, drawn as a coloured
    ''' square matrix with the correlation coefficients
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="columns">the columns to be correlated, defaults to all the numeric columns</param>
    ''' <param name="method">pearson or spearman</param>
    ''' <param name="label">print the correlation coefficient of every cell</param>
    ''' <param name="upper">only draw the upper triangle</param>
    ''' <returns></returns>
    <ExportAPI("ggcorr")>
    Public Function ggcorr(data As Object,
                          Optional columns As String() = Nothing,
                          Optional method As String = "pearson",
                          Optional label As Boolean = True,
                          Optional upper As Boolean = False) As ggplotCorrMatrix

        Return New ggplotCorrMatrix With {
            .data = asMatrix(data, columns),
            .method = method,
            .showLabel = label,
            .upper = upper
        }
    End Function

    ''' <summary>
    ''' ggpairs(): the scatter plot matrix of a dataframe
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="columns">the columns to be plotted, defaults to all the numeric columns</param>
    ''' <param name="upper">only draw the upper triangle</param>
    ''' <returns></returns>
    <ExportAPI("ggpairs")>
    Public Function ggpairs(data As Object,
                           Optional columns As String() = Nothing,
                           Optional upper As Boolean = False) As ggplotPairs

        Dim table As dataframe = TryCast(data, dataframe)
        Dim names As String() = If(columns Is Nothing OrElse columns.Length = 0, table.colnames, columns)

        Return New ggplotPairs With {
            .data = asMatrix(data, columns),
            .labels = names,
            .upper = upper
        }
    End Function

    ''' <summary>
    ''' ggforest(): a forest plot of the effect sizes of a meta analysis
    ''' </summary>
    ''' <param name="effect">the point estimate of every study</param>
    ''' <param name="lower">the lower bound of the confidence interval</param>
    ''' <param name="upper">the upper bound of the confidence interval</param>
    ''' <param name="label">the label of every study</param>
    ''' <param name="logScale">use a logarithmic scale on the horizontal axis</param>
    ''' <returns></returns>
    <ExportAPI("ggforest")>
    Public Function ggforest(effect As Double(),
                            lower As Double(),
                            upper As Double(),
                            Optional label As String() = Nothing,
                            Optional logScale As Boolean = False) As ggplotForest

        Return New ggplotForest With {
            .effect = effect,
            .lower = lower,
            .upper = upper,
            .label = label,
            .logScale = logScale,
            .showLegend = False
        }
    End Function

    ''' <summary>
    ''' funnel(): the funnel plot of the successive inclusion
    ''' </summary>
    ''' <param name="proportion">the number of the included subjects of every stage</param>
    ''' <param name="label">the label of every stage</param>
    ''' <param name="confLevel">the confidence level of the band</param>
    ''' <returns></returns>
    <ExportAPI("funnel")>
    Public Function funnel(proportion As Double(),
                          Optional label As String() = Nothing,
                          Optional confLevel As Double = 0.95) As ggplotFunnel

        Return New ggplotFunnel With {
            .proportion = proportion,
            .label = label,
            .confLevel = confLevel,
            .showLegend = False
        }
    End Function

    ''' <summary>
    ''' ggroc(): the ROC curve of a binary classifier, annotated with the area
    ''' under the curve
    ''' </summary>
    ''' <param name="score">the prediction score of every observation</param>
    ''' <param name="label">the positive(true) class of every observation</param>
    ''' <param name="auc">annotate the area under the curve</param>
    ''' <param name="color"></param>
    ''' <returns></returns>
    <ExportAPI("ggroc")>
    Public Function ggroc(score As Double(),
                         label As Boolean(),
                         Optional auc As Boolean = True,
                         Optional color As String = "steelblue") As ggplotROC

        Return New ggplotROC With {
            .score = score,
            .label = label,
            .auc = auc,
            .color = color,
            .showLegend = False
        }
    End Function
End Module