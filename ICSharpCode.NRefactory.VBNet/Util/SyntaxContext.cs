﻿//
// SyntaxContext.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Threading;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace ICSharpCode.NRefactory6.VisualBasic
{
	class SyntaxContext
	{
		readonly  VisualBasicSyntaxContext ctx;
		readonly List<ITypeSymbol> inferredTypes;

		public List<ITypeSymbol> InferredTypes {
			get {
				return inferredTypes;
			}
		}
		
		public SyntaxToken LeftToken {
			get {
				return ctx.LeftToken;
			}
		}
		
		public SyntaxToken TargetToken {
			get {
				return ctx.TargetToken;
			}
		}

		public bool IsIsOrAsTypeContext {
			get {
				return ctx.IsIsOrAsTypeContext;
			}
		}

		public bool IsInstanceContext {
			get {
				return ctx.IsInstanceContext;
			}
		}

		public bool IsNonAttributeExpressionContext {
			get {
				return ctx.IsNonAttributeExpressionContext;
			}
		}

		public bool IsPreProcessorKeywordContext {
			get {
				return ctx.IsPreProcessorKeywordContext;
			}
		}

		public bool IsPreProcessorExpressionContext {
			get {
				return ctx.IsPreProcessorExpressionContext;
			}
		}
		/*
		public TypeDeclarationSyntax ContainingTypeDeclaration {
			get {
				return ctx.ContainingTypeDeclaration;
			}
		}*/

		public bool IsGlobalStatementContext {
			get {
				return ctx.IsGlobalStatementContext;
			}
		}

		public bool IsParameterTypeContext {
			get {
				return ctx.IsParameterTypeContext;
			}
		}
		
		public SyntaxTree SyntaxTree {
			get {
				return ctx.SyntaxTree;
			}
		}

		public bool IsInsideNamingContext (bool isRightAfterIdentifier)
		{
			var parent = ctx.TargetToken.Parent;
			return false;
			/*if (isRightAfterIdentifier) {
				return parent.IsKind(SyntaxKind.IdentifierName) ||
					parent.IsKind(SyntaxKind.PredefinedType);
			}
			if (parent.IsKind(SyntaxKind.NamespaceDeclaration)) {
				return !ctx.TargetToken.IsKind(SyntaxKind.OpenBraceToken);
			}
			return parent.IsKind(SyntaxKind.IdentifierName) && (
			    parent.Parent.IsKind(SyntaxKind.Parameter) ||
			    parent.Parent.IsKind(SyntaxKind.ArrayType) ||
				parent.Parent.IsKind(SyntaxKind.VariableDeclaration) ||
				parent.Parent.IsKind(SyntaxKind.ForEachStatement)
			);*/
		}

		SyntaxContext(VisualBasicSyntaxContext ctx, List<ITypeSymbol> inferredTypes)
		{
			this.inferredTypes = inferredTypes;
			this.ctx = ctx;
		}

		static readonly VisualBasicTypeInferenceService inferenceService = new VisualBasicTypeInferenceService ();

		public static SyntaxContext Create(Workspace workspace, Document document, SemanticModel semanticModel, int position, CancellationToken cancellationToken)
		{
			return new SyntaxContext(
				VisualBasicSyntaxContext.CreateContext(workspace, semanticModel, position, cancellationToken),
				inferenceService.InferTypes(semanticModel, position, cancellationToken).ToList()
			);
		}
	}
}

