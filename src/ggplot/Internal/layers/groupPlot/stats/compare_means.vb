﻿#Region "Microsoft.VisualBasic::37eb43626c6f18bf8520f5809ea72329, src\ggplot\Internal\layers\groupPlot\stats\compare_means.vb"

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

    '   Total Lines: 76
    '    Code Lines: 51 (67.11%)
    ' Comment Lines: 15 (19.74%)
    '    - Xml Docs: 86.67%
    ' 
    '   Blank Lines: 10 (13.16%)
    '     File Size: 2.72 KB


    '     Class compare_means
    ' 
    '         Properties: group1, group2, padj, psignif, pvalue
    '                     y
    ' 
    '         Function: fromManualData, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    ''' <summary>
    ''' group stats comparision result
    ''' </summary>
    Public Class compare_means

        Public Property y As String
        Public Property group1 As String
        Public Property group2 As String
        Public Property pvalue As Double
        Public Property padj As Double

        Public Const ns As String = "n.sig"

        Public ReadOnly Property psignif As String
            Get
                If pvalue.ToString = "0" Then
                    Return $"*****(<1e-100)"
                End If

                If pvalue <= 0.00001 Then
                    Return $"*****({pvalue.ToString("G3")})"
                ElseIf pvalue <= 0.0001 Then
                    Return $"****({pvalue.ToString("G3")})"
                ElseIf pvalue <= 0.001 Then
                    Return $"***({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.01 Then
                    Return $"**({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.05 Then
                    Return $"*({pvalue.ToString("F3")})"
                ElseIf pvalue <= 0.1 Then
                    Return "."
                Else
                    Return "n.sig"
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return psignif
        End Function

        ''' <summary>
        ''' load stats pvalue result from the group compares
        ''' </summary>
        ''' <param name="data">
        ''' a dataframe object that should contains the data fiels at least:
        ''' 
        ''' + group1: the label name of the group 1
        ''' + group2: the label name of the group 2
        ''' + pvalue: a numeric vector of the t-test pvalue between group 1 and group 2.
        ''' 
        ''' </param>
        ''' <returns></returns>
        Public Shared Function fromManualData(data As dataframe) As compare_means()
            Dim group1 As String() = data.getColumnVector("group1")
            Dim group2 As String() = data.getColumnVector("group2")
            Dim pvalue As Double() = data.getColumnVector("pvalue")

            Return pvalue _
                .Select(Function(p, i)
                            Return New compare_means With {
                                .group1 = group1(i),
                                .group2 = group2(i),
                                .pvalue = p,
                                .padj = p
                            }
                        End Function) _
                .ToArray
        End Function

    End Class
End Namespace
