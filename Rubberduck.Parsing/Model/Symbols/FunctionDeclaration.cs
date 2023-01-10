﻿using Rubberduck.InternalApi.Model;
using Rubberduck.Parsing.Annotations;
using Rubberduck.Parsing.Model.ComReflection;
using Rubberduck.VBEditor;
using System.Collections.Generic;
using System.Linq;
using static Rubberduck.Parsing.Grammar.VBAParser;

namespace Rubberduck.Parsing.Model.Symbols
{
    public sealed class FunctionDeclaration : ModuleBodyElementDeclaration
    {
        public FunctionDeclaration(
            QualifiedMemberName name,
            Declaration parent,
            Declaration parentScope,
            string asTypeName,
            AsTypeClauseContext asTypeContext,
            string typeHint,
            Accessibility accessibility,
            DocumentOffset offset,
            bool isArray,
            bool isUserDefined,
            IEnumerable<IParseTreeAnnotation> annotations,
            Attributes attributes)
            : base(
                name,
                parent,
                parentScope,
                asTypeName,
                asTypeContext,
                typeHint,
                accessibility,
                DeclarationType.Function,
                offset,
                isArray,               
                isUserDefined,
                annotations,
                attributes)
        { }

        public FunctionDeclaration(ComMember member, Declaration parent, QualifiedModuleName module, Attributes attributes) 
            : this(
                module.QualifyMemberName(member.Name),
                parent,
                parent,
                member.AsTypeName.TypeName,
                null,
                null,
                Accessibility.Global,
                DocumentOffset.Invalid,
                member.AsTypeName.IsArray,
                false,
                null,
                attributes)
        {
            AddParameters(member.Parameters.Select(decl => new ParameterDeclaration(decl, this, module)));
        }

        /// <inheritdoc/>
        protected override bool Implements(IInterfaceExposable member)
        {
            if (ReferenceEquals(member, this))
            {
                return false;
            }

            return member.DeclarationType == DeclarationType.Function
                && member.IsInterfaceMember
                && IdentifierName.Equals(member.ImplementingIdentifierName)
                   && ((ClassModuleDeclaration)member.ParentDeclaration).Subtypes.Any(implementation => ReferenceEquals(implementation, ParentDeclaration));
        }
    }
}
