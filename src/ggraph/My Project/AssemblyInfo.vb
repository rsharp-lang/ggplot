Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' 有关程序集的一般信息由以下
' 控制。更改这些特性值可修改
' 与程序集关联的信息。

'查看程序集特性的值
#if netcore5=0 then 

<Assembly: AssemblyTitle("Graph")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("SMRUCC genomics institute")>
<Assembly: AssemblyProduct("")>
<Assembly: AssemblyCopyright("Copyright © SMRUCC genomics institute 2022")>
<Assembly: AssemblyTrademark("")>

<Assembly: ComVisible(False)>

'如果此项目向 COM 公开，则下列 GUID 用于 typelib 的 ID
<Assembly: Guid("1d5ee54c-070b-42bf-91f5-e98aa9d962aa")>

' 程序集的版本信息由下列四个值组成: 
'
'      主版本
'      次版本
'      生成号
'      修订号
'
'可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
'通过使用 "*"，如下所示:

<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>
#end if