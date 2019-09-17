﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Engine
{
    public static class IDbContextExtensions
    {
        #region ModelBuild Methods
        public static void ModelBuild(this IDbContext context)
        {
            context.ExecuteSqlCommand("SELECT 1");
        }

        public static void ModelBuild(this EfeObjectContext context)
        {
            context.ExecuteSqlCommand("SELECT 1");
        }
        #endregion
    }
}
