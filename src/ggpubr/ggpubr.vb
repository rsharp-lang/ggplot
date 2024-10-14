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
End Module
