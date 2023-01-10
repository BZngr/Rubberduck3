﻿using Rubberduck.InternalApi.Model;
using Rubberduck.DataServer.Abstract;

namespace Rubberduck.DataServer.Entities
{
    internal class Member : DbEntity
    {
        public int DeclarationId { get; set; }
        public int? ImplementsDeclarationId { get; set; }
        public int Accessibility { get; set; }
        public int IsAutoAssigned { get; set; }
        public int IsWithEvents { get; set; }
        public int IsDimStmt { get; set; }
        public string ValueExpression { get; set; }
    }

    internal class MemberInfo : Member
    {
        public DeclarationType DeclarationType { get; set; }
        public string IdentifierName { get; set; }
        public string DocString { get; set; }
        public bool IsUserDefined { get; set; }

        public string AsTypeName { get; set; }
        public int? AsTypeDeclarationId { get; set; }
        public bool IsArray { get; set; }
        public string TypeHint { get; set; }

        public int ModuleDeclarationId { get; set; }
        public string ModuleName { get; set; }
        public string Folder { get; set; }

        public int ProjectDeclarationId { get; set; }
        public string ProjectName { get; set; }
        public string VBProjectId { get; set; }
    }
}
