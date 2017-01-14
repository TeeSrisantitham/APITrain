using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheckList.API.Repository
{
    public static class SqlHelper
    {
        public static List<T> FillResultToObject<T>(List<Dictionary<string, object>> results) where T : new()
        {
            List<T> modelList = new List<T>();

            T model = new T();
            if (model.GetType().IsClass)
            {
                foreach (var result in results)
                {

                    foreach (var prop in model.GetType().GetProperties())
                    {
                        if (result.ContainsKey(prop.Name))
                        {
                            Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            try
                            {
                                object safeValue = result[prop.Name] == DBNull.Value ? null : Convert.ChangeType(result[prop.Name], t);
                                prop.SetValue(model, safeValue);
                            }
                            catch { }
                        }
                    }
                    modelList.Add(model);
                    model = new T();
                }
            }
            else
            {
                foreach (var result in results)
                {
                    var value = result.FirstOrDefault().Value;
                    object safeValue = value == DBNull.Value ? null : Convert.ChangeType(value, model.GetType());
                    modelList.Add((T)safeValue);
                }
            }

            return modelList;
        }

        public static List<T> ExecuteCommandAsType<T>(SqlCommand cmd, List<Dictionary<string, object>> rawResult = null) where T : new()
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            var reader = cmd.ExecuteReader();
            var columns = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i));

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < columns.Count; i++)
                    row.Add(columns[i], reader[columns[i]]);
                results.Add(row);
            }

            reader.Close();
            if (rawResult != null)
                rawResult = results;
            return FillResultToObject<T>(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="output_type">is type for out put can be class or primitive.it's need ordering follow the result form cmd</param>
        /// <returns></returns>
        public static object[] ExecuteCommandAsTypeWithMultileTable(SqlCommand cmd, Type[] output_type, List<Dictionary<string, object>> rawResult = null)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            var reader = cmd.ExecuteReader();

            List<object> objResult = new List<object>();
            var method = typeof(SqlHelper).GetMethod("FillResultToObject", BindingFlags.Public | BindingFlags.Static);

            //foreach (Type type in output_type)
            int j = 0;
            do
            {
                results = new List<Dictionary<string, object>>();
                var columns = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                    columns.Add(reader.GetName(i));

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < columns.Count; i++)
                        row.Add(columns[i], reader[columns[i]]);
                    results.Add(row);
                    if (rawResult != null)
                        rawResult.Add(row);
                }

                var genericMethod = method.MakeGenericMethod(output_type[j]);
                objResult.Add(genericMethod.Invoke(null, new[] { results }));
                j++;
            } while (reader.NextResult());
            return objResult.ToArray();
        }
    }
}
