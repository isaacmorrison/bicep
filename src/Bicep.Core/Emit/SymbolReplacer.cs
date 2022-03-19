// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using System.Collections.Generic;

namespace Bicep.Core.Emit
{
    public class SymbolReplacer : SyntaxRewriteVisitor
    {
        private readonly SemanticModel semanticModel;
        IReadOnlyDictionary<Symbol, SyntaxBase> replacements;

        public SymbolReplacer(SemanticModel semanticModel, IReadOnlyDictionary<Symbol, SyntaxBase> replacements)
        {
            this.semanticModel = semanticModel;
            this.replacements = replacements;
        }

        protected override SyntaxBase ReplaceVariableAccessSyntax(VariableAccessSyntax syntax)
        {
            if (this.semanticModel.GetSymbolInfo(syntax) is not { } symbol || !this.replacements.TryGetValue(symbol, out var replacementSyntax))
            {
                // unbound variable access or not a symbol that we need to replace
                // leave syntax as-is
                return base.ReplaceVariableAccessSyntax(syntax);
            }

            // inject the replacement syntax
            return replacementSyntax;
        }
    }
}
