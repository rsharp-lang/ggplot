﻿#Region "Microsoft.VisualBasic::8c6cb6f04f884bb976c736d3b0a810ed, src\ggpubr\ggpubr.vb"

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

    '   Total Lines: 37
    '    Code Lines: 19 (51.35%)
    ' Comment Lines: 15 (40.54%)
    '    - Xml Docs: 86.67%
    ' 
    '   Blank Lines: 3 (8.11%)
    '     File Size: 1.73 KB


    ' Module Rscript
    ' 
    '     Function: stat_ellipse
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggpubr")>
Module Rscript

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
            .level = level,
            .showLegend = False,
            .alpha = alpha
        }
    End Function
End Module
