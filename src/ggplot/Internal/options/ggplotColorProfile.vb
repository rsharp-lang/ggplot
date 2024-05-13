#Region "Microsoft.VisualBasic::e0c8aef299dd90f7aa7babcdf6ce0fe6, src\ggplot\Internal\options\ggplotColorProfile.vb"

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

    '   Total Lines: 72
    '    Code Lines: 53
    ' Comment Lines: 13
    '   Blank Lines: 6
    '     File Size: 4.97 KB


    '     Class ggplotColorProfile
    ' 
    '         Properties: profile
    ' 
    '         Function: Config, MapColorBrewer
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.colors
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace options

    Public Class ggplotColorProfile : Inherits ggplotOption

        Public Property profile As ggplotColorMap

        ''' <summary>
        ''' config the last <see cref="ggplotLayer"/> its color map value.
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.layers.Last.colorMap = profile
            Return ggplot
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="alpha">
        ''' color alpha channel value between [0,1]
        ''' </param>
        ''' <returns></returns>
        Public Shared Function MapColorBrewer(name As String, alpha As Double) As ggplotColorMap
            Select Case name.ToLower
                Case "brbg" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.BrBG11, alpha)
                Case "piyg" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.PiYG11, alpha)
                Case "prgn" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.PRGn11, alpha)
                Case "puor" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.PuOr11, alpha)
                Case "rdbu" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.RdBu11, alpha)
                Case "rdgy" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.RdGy11, alpha)
                Case "rdylbu" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.RdYlBu11, alpha)
                Case "rdylgn" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.RdYlGn11, alpha)
                Case "spectral" : Return ggplotColorMap.stringMap(ColorBrewer.DivergingSchemes.Spectral11, alpha)
                Case "accent" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Accent8, alpha)
                Case "dark2" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Dark2_8, alpha)
                Case "paired" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Paired12, alpha)
                Case "pastel1" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Pastel1_9, alpha)
                Case "pastel2" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Pastel2_8, alpha)
                Case "set1" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Set1_9, alpha)
                Case "set2" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Set2_8, alpha)
                Case "set3" : Return ggplotColorMap.stringMap(ColorBrewer.QualitativeSchemes.Set3_12, alpha)
                Case "blues" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Blues9, alpha)
                Case "bugn" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.BuGn9, alpha)
                Case "bupu" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.BuPu9, alpha)
                Case "gnbu" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.GnBu9, alpha)
                Case "greens" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Greens9, alpha)
                Case "greys" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Greys9, alpha)
                Case "oranges" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Oranges9, alpha)
                Case "orrd" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.OrRd9, alpha)
                Case "pubu" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.PuBu9, alpha)
                Case "pubugn" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.PuBuGn9, alpha)
                Case "purd" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.PuRd9, alpha)
                Case "purples" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Purples9, alpha)
                Case "rdpu" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.RdPu9, alpha)
                Case "reds" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.Reds9, alpha)
                Case "ylgn" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.YlGn9, alpha)
                Case "ylgnbu" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.YlGnBu9, alpha)
                Case "ylorbr" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.YlOrBr9, alpha)
                Case "ylorrd" : Return ggplotColorMap.stringMap(ColorBrewer.SequentialSchemes.YlOrRd8, alpha)

                Case Else
                    Return Nothing
            End Select
        End Function
    End Class
End Namespace
