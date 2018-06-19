using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SpiralWorks.Data.Ado.Extensions
{

    public static class DataExtensions
    {
        private static readonly BindingFlags _bindingflag = BindingFlags.Public | BindingFlags.Instance |
                                                            BindingFlags.IgnoreCase;
        public static List<T> ToEntityList<T>(this DataTable self)
            where T : new()
        {
            List<T> entityList = new List<T>();
            foreach (DataRow dr in self.Rows)
            {
                T entity = dr.ToEntity<T>();
                entityList.Add(entity);
            }

            return entityList;
        }
        public static List<T> ToEntityList<T>(this IDataReader self) where T : new()
        {
            List<T> entityList = new List<T>();
            T entity;
            var columnNames = new List<string>();
            for (int i = 0; i < self.FieldCount; i++)
                columnNames.Add(self.GetName(i));
            while (self.Read())
            {
                entity = new T();

                foreach (string column in columnNames)
                {
                    try
                    {
                        if (entity.GetType().GetProperty(column, _bindingflag) != null)
                        {
                            var pi = entity.GetType().GetProperty(column, _bindingflag);
                            if (self[column] != DBNull.Value)
                            {

                                if (pi.PropertyType.IsEnum)
                                {
                                    pi.SetValue(entity,
                                                Enum.Parse(pi.PropertyType, Convert.ToString(self[pi.Name]), true), null);
                                }
                                else
                                {
                                    pi.SetValue(entity, self[pi.Name], null);
                                }
                            }
                            else
                            {
                                pi.SetValue(entity, null, null);
                            }
                        }
                    }
                    catch
                    {
                        try
                        {
                            if (entity.GetType().GetProperty(column, _bindingflag) != null)
                            {
                                var pi = entity.GetType().GetProperty(column, _bindingflag);
                                if (self[pi.Name] != DBNull.Value)
                                {
                                    pi.SetValue(entity, self[pi.Name].DownCast(pi.PropertyType), null);
                                }
                                else
                                {
                                    pi.SetValue(entity, null, null);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }
                    }
                }

                entityList.Add(entity);
            }


            return entityList;
        }


        public static T ToEntity<T>(this DataRow self) where T : new()
        {
            T entity = new T();
            var columnNames = new List<string>();
            for (int i = 0; i < self.Table.Columns.Count; i++)
                columnNames.Add(self.Table.Columns[i].ColumnName);


            foreach (string column in columnNames)
            {
                try
                {
                    if (entity.GetType().GetProperty(column, _bindingflag) != null)
                    {
                        var pi = entity.GetType().GetProperty(column, _bindingflag);
                        if (self[column] != DBNull.Value)
                        {

                            if (pi.PropertyType.IsEnum)
                            {
                                pi.SetValue(entity, Enum.Parse(pi.PropertyType, Convert.ToString(self[pi.Name]), true),
                                            null);
                            }
                            else
                            {
                                pi.SetValue(entity, self[pi.Name], null);
                            }
                        }
                        else
                        {
                            pi.SetValue(entity, null, null);
                        }
                    }
                }
                catch
                {
                    try
                    {
                        if (entity.GetType().GetProperty(column, _bindingflag) != null)
                        {
                            var pi = entity.GetType().GetProperty(column, _bindingflag);
                            if (self[pi.Name] != DBNull.Value)
                            {
                                pi.SetValue(entity, self[pi.Name].DownCast(pi.PropertyType), null);
                            }
                            else
                            {
                                pi.SetValue(entity, null, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }

            return entity;
        }

        public static object DownCast(this object self, Type castTo)
        {
            object result;
            if (castTo == typeof(int) || castTo == typeof(int))
            {
                result = Convert.ToInt32(self);
            }
            else if (castTo == typeof(int?) || castTo == typeof(int?))
            {
                result = Convert.ToInt32(self);
            }
            else if (castTo == typeof(short))
            {
                result = Convert.ToInt16(self);
            }
            else if (castTo == typeof(long))
            {
                result = Convert.ToInt64(self);
            }
            else if (castTo == typeof(bool) || castTo == typeof(bool))
            {
                result = Convert.ToBoolean(self);
            }
            else if (castTo == typeof(decimal) || castTo == typeof(float))
            {
                result = Convert.ToDecimal(self);
            }

            else
            {
                result = Convert.ToString(self);
            }
            return result;
        }
        public static T ToEntity<T>(this object self)
            where T : new()
        {
            var entity = new T();

            var objtype = self.GetType();
            var objprops = objtype.GetProperties();
            var objpropdict = objprops.ToDictionary(pi => pi.Name);

            foreach (var pi in entity.GetType().GetProperties())
            {
                if (objpropdict.ContainsKey(pi.Name))
                {
                    if (objpropdict[pi.Name] != null)
                    {
                        var objpi = objpropdict[pi.Name];

                        pi.SetValue(
                            entity,
                            pi.PropertyType.IsEnum
                                ? Enum.Parse(pi.PropertyType, Convert.ToString(objpi.GetValue(self, null)), true)
                                : objpi.GetValue(self, null),
                            null);
                    }
                    else
                    {
                        pi.SetValue(entity, null, null);
                    }
                }
            }

            return entity;
        }


        public static List<T> ToEntityList<T>(this object self)
            where T : new()
        {
            var entityList = new List<T>();
            var objtype = self.GetType();
            if (objtype.IsArray)
            {
                var objitems = self as Array;
                entityList.AddRange(from object item in objitems select item.ToEntity<T>());
            }
            else
            {
                var entity = self.ToEntity<T>();
                entityList.Add(entity);
            }

            return entityList;
        }



        private static object ValueOrDbNull(this object self)
        {
            return self ?? DBNull.Value;
        }


        public static void CopyTo<T>(this object self, T to)
        {
            if (self != null)
            {
                foreach (var pi in to.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                {
                    pi.SetValue(to, self.GetType().GetProperty(pi.Name)?.GetValue(self, null) ?? null, null);
                }
            }

        }
    }
}
