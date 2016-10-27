using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MVP
{
    public static class Tools
    {
        public static string GetPropertyName(LambdaExpression propertyAccessExpression)
        {
            MemberExpression member = propertyAccessExpression.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException("propertyAccessExpression must be property access expression");
            }
            PropertyInfo propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new InvalidCastException("Invalid property access expression. Unable to extract property info from it");
            }
            return propertyInfo.Name;
        }

        public static string GetPropertyNameDelegate<T>(Expression<Func<T>> propertyExpression)
        {
            return GetPropertyName(propertyExpression);
        }
    }
}
