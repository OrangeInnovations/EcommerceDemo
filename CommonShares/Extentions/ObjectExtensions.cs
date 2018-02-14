﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonShares.ExceptionHandlers;

namespace CommonShares.Extentions
{
    public static class ObjectExtensions
    {
        public static string ReportAllProperties<T>(this T instance) where T : class
        {
            try
            {
                if (instance == null)
                    return string.Empty;

                var strListType = typeof(List<string>);
                var strArrType = typeof(string[]);

                var arrayTypes = new[] { strListType, strArrType };
                var handledTypes = new[] { typeof(Int32), typeof(String), typeof(bool), typeof(DateTime), typeof(double), typeof(decimal), strListType, strArrType };

                var validProperties = instance.GetType()
                                              .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                              .Where(prop => handledTypes.Contains(prop.PropertyType))
                                              .Where(prop => prop.GetValue(instance, null) != null)
                                              .ToList();

                var format = string.Format("{{0,-{0}}} : {{1}}", validProperties.Count > 0 ? validProperties.Max(prp => prp.Name.Length) : 10);

                return string.Join(
                         Environment.NewLine,
                         validProperties.Select(prop => string.Format(format,
                                                                      prop.Name,
                                                                      (arrayTypes.Contains(prop.PropertyType) ? string.Join(", ", (IEnumerable<string>)prop.GetValue(instance, null))
                                                                                                              : prop.GetValue(instance, null)))));
            }
            catch (Exception exception)
            {
                return string.Format("ReportAllProperties fails because of exception = ({0})", ExceptionUtils.FormatException(exception));
            }

        }
    }
}
