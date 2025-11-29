using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyORMLibrary
{
    public class ORMContext
    {
        private readonly string _connectionString;

        public ORMContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string GetTableName<T>()
        {
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? typeof(T).Name;
        }

        private Dictionary<string, PropertyInfo> GetPropertyMap<T>()
        {
            return typeof(T)
                .GetProperties()
                .Where(p => p.GetCustomAttribute<ColumnAttribute>() != null)
                .ToDictionary(
                    p => p.GetCustomAttribute<ColumnAttribute>().Name,
                    p => p);
        }

        public T Create<T>(T entity) where T : class
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var tableName = GetTableName<T>();
                var props = GetPropertyMap<T>();

                var sql = new StringBuilder();
                sql.Append($"INSERT INTO {tableName} (");
                sql.Append(string.Join(",", props.Keys));
                sql.Append(") VALUES (");
                sql.Append(string.Join(",", props.Keys.Select(k => $"@{k}")));
                sql.Append(")");

                var command = dataSource.CreateCommand(sql.ToString());

                foreach (var prop in props)
                {
                    command.Parameters.AddWithValue($"@{prop.Key}", prop.Value.GetValue(entity) ?? DBNull.Value);
                }

                command.ExecuteNonQuery();

                return entity;
            }
        }

        public T ReadById<T>(int id) where T : class, new()
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var tableName = GetTableName<T>();
                string sql = $"SELECT * FROM {tableName} WHERE hotel_id = @id";
                var command = dataSource.CreateCommand(sql);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return Map<T>(reader);
                }
            }
            return null;
        }

        public List<T> ReadByAll<T>() where T : class, new()
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var tableName = GetTableName<T>();
                string sql = $"SELECT * FROM {tableName}";
                var command = dataSource.CreateCommand(sql);
                var results = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        results.Add(Map<T>(reader));
                }

                return results;
            }
        }

        public void Update<T>(int id, T entity) where T : class
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var tableName = GetTableName<T>();
                var props = GetPropertyMap<T>();

                var sql = new StringBuilder();
 sql.Append($"UPDATE {tableName} SET ");
                sql.Append(string.Join(",", props.Keys.Select(k => $"{k}=@{k}")));
                sql.Append(" WHERE hotel_id=@id");

                var command = dataSource.CreateCommand(sql.ToString());

                foreach (var prop in props)
                {
                    command.Parameters.AddWithValue($"@{prop.Key}", prop.Value.GetValue(entity) ?? DBNull.Value);
                }
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void Delete<T>(int id) where T : class
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var tableName = GetTableName<T>();
                string sql = $"DELETE FROM {tableName} WHERE hotel_id=@id";
                var command = dataSource.CreateCommand(sql);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var sqlQuery = BuildSqlQuery(predicate, false);
            return ExecuteQueryMultiple<T>(sqlQuery);
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var sqlQuery = BuildSqlQuery(predicate, true);
            return ExecuteQuerySingle<T>(sqlQuery);
        }

        private string BuildSqlQuery<T>(Expression<Func<T, bool>> predicate, bool single)
        {
            var tableName = GetTableName<T>();
            var where = ParseExpression(predicate.Body, GetPropertyMap<T>());
            var limit = single ? "LIMIT 1" : "";
            return $"SELECT * FROM {tableName} WHERE {where} {limit}".Trim();
        }

        private string ParseExpression(Expression expression, Dictionary<string, PropertyInfo> propMap)
        {
            if (expression is BinaryExpression binary)
            {
                var left = ParseExpression(binary.Left, propMap);
                var right = ParseExpression(binary.Right, propMap);
                var op = GetSqlOperator(binary.NodeType);
                return $"({left} {op} {right})";
            }
            else if (expression is MemberExpression member)
            {
                if (member.Expression is ParameterExpression)
                {
                    var prop = propMap.FirstOrDefault(p => p.Value.Name == member.Member.Name);
                    return prop.Key;
                }
                var value = Expression.Lambda(member).Compile().DynamicInvoke();
                return FormatConstant(value);
            }
            else if (expression is ConstantExpression constant)
            {
                return FormatConstant(constant.Value);
            }
            else if (expression is ParameterExpression)
            {
                return string.Empty;
            }
            throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
        }

        private string GetSqlOperator(ExpressionType nodeType)
        {
            return nodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThan => "<",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.AndAlso => "AND",
                _ => throw new NotSupportedException($"Unsupported node type: {nodeType}")
            };
        }

        private string FormatConstant(object value)
        {
            return value switch
            {
                string s => $"'{s.Replace("'", "''")}'",
                null => "NULL",
                _ => value.ToString()
            };
        }

        private T Map<T>(NpgsqlDataReader reader) where T : class, new()
        {
            var obj = new T();
            var propMap = GetPropertyMap<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                if (propMap.TryGetValue(columnName, out var prop))
                {
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    if (value != null)
                    {
                        prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                    }
                }
            }

            return obj;
        }

        private T ExecuteQuerySingle<T>(string query) where T : class, new()
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var command = dataSource.CreateCommand(query);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return Map<T>(reader);
                }
            }
            return null;
        }

        private IEnumerable<T> ExecuteQueryMultiple<T>(string query) where T : class, new()
        {
            using (var dataSource = NpgsqlDataSource.Create(_connectionString))
            {
                var command = dataSource.CreateCommand(query);
                var results = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        results.Add(Map<T>(reader));
                }

                return results;
            }
        }
    }
}