﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using JsonAttr = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace PatreonClient.Internals
{
    public class Field
    {
        private Type Type { get; }
        private List<string> Fields { get; set; }

        private Field(Type type, List<string> fields)
        {
            Type = type;
            Fields = fields;
        }

        public static Field All<T>()
        {
            var type = typeof(T);
            var fields = type.GetProperties()
                             .Select(x => (JsonAttr) x.GetCustomAttribute(typeof(JsonAttr)))
                             .Select(x => x.Name)
                             .ToList();
            return new Field(type, fields);
        }

        public static Field Create<TAttribute>(Expression selector)
        {
            var fields = new List<string>();

            var lambda = (LambdaExpression) selector;
            if (!(lambda.Body is NewExpression ne)) throw new InvalidFieldSelectorException();

            foreach (var arg in ne.Arguments)
            {
                if (arg is MemberExpression member)
                {
                    var attr = member.Member.GetCustomAttributes().FirstOrDefault();

                    if (attr is JsonPropertyNameAttribute j)
                    {
                        fields.Add(j.Name);
                    }
                }
            }

            return new Field(typeof(TAttribute), fields);
        }

        public string ToString(string prefix)
        {
            if (Fields.Count <= 0) return "";

            var fieldName = Type.Name.ToLowerInvariant();
            return string.Concat(prefix, "fields%5B", fieldName, "%5D=", string.Join(',', Fields));
        }

        private class InvalidFieldSelectorException : Exception
        {
            private const string c_error =
                @"Use NewBody Expression to specify the list of fields to query from patreon api
Example:
x => new {
    x.FirstName,
    x.LastName,
    ...
};";

            public InvalidFieldSelectorException() : base(c_error) { }
        }
    }
}