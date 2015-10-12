' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Threading
Imports Microsoft.CodeAnalysis.Editor.VisualBasic.Outlining
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis.Editor.Implementation.Outlining

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.UnitTests.Outlining
    Public Class ExternalMethodDeclarationOutlinerTests
        Inherits AbstractOutlinerTests(Of DeclareStatementSyntax)

        Friend Overrides Function GetRegions(externalMethodDeclaration As DeclareStatementSyntax) As IEnumerable(Of OutliningSpan)
            Dim outliner As New ExternalMethodDeclarationOutliner
            Return outliner.GetOutliningSpans(externalMethodDeclaration, CancellationToken.None).WhereNotNull()
        End Function

        <WpfFact, Trait(Traits.Feature, Traits.Features.Outlining)>
        Public Sub TestExternalMethodDeclarationWithComments()
            Dim syntaxTree = ParseLines("Class C",
                                  "  'Hello",
                                  "  'World",
                                  "  Declare Ansi Sub ExternSub Lib ""ExternDll"" ()",
                                  "End Class")

            Dim typeBlock = syntaxTree.DigToFirstTypeBlock()
            Dim externalMethodDecl = typeBlock.DigToFirstNodeOfType(Of DeclareStatementSyntax)()
            Assert.NotNull(externalMethodDecl)

            Dim actualRegion = GetRegion(externalMethodDecl)
            Dim expectedRegion = New OutliningSpan(
                                     TextSpan:=TextSpan.FromBounds(11, 27),
                                     bannerText:="' Hello ...",
                                     hintSpan:=TextSpan.FromBounds(11, 27),
                                     autoCollapse:=True)

            AssertRegion(expectedRegion, actualRegion)
        End Sub
    End Class
End Namespace
