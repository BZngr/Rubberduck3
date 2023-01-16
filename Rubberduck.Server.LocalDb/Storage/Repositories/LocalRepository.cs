﻿using System.Data;
using System.Threading.Tasks;
using Dapper;
using Rubberduck.DataServer.Storage.Entities;
using Rubberduck.Server.LocalDb.Abstract;

namespace Rubberduck.DataServer.Storage.Repositories
{
    internal class LocalRepository : Repository<Local>
    {
        public LocalRepository(IDbConnection connection)
            : base(connection) { }

        protected override string Source { get; } = "[Locals]";
        protected override string[] ColumnNames { get; } = new[]
        {
            "DeclarationId",
            "IsAutoAssigned",
            "DeclaredValue",
        };

        public override async Task SaveAsync(Local entity)
        {
            if (entity.Id == default)
            {
                entity.Id = await Database.ExecuteAsync(InsertSql,
                    new
                    {
                        declarationId = entity.DeclarationId,
                        isAutoAssigned = entity.IsAutoAssigned,
                        declaredValue = entity.ValueExpression,
                    });
            }
            else
            {
                await Database.ExecuteAsync(UpdateSql,
                    new 
                    {
                        declarationId = entity.DeclarationId,
                        isAutoAssigned = entity.IsAutoAssigned,
                        declaredValue = entity.ValueExpression,
                        id = entity.Id,
                    });
            }
        }
    }
}
