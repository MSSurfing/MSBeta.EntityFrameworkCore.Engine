using System;
using System.IO;
using System.Text;

namespace EntityFramework.Engine
{
    public static class IDbContextExtensions
    {
        #region Utilities
        private static string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.Trim().Equals("go", StringComparison.OrdinalIgnoreCase))
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }
        #endregion

        #region Methods

        /// <summary>
        /// 执行Sql文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void ExecuteSqlFile(this IDbContext context, string path)
        {
            var statements = new System.Collections.Generic.List<string>();

            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                    statements.Add(statement);
            }

            foreach (string stmt in statements)
                context.ExecuteSqlCommand(stmt);
        }
        #endregion

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
