﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EFCustomAnnotations
{
    public static class PropertyHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(
            Expression<Func<T, TValue>> selector) {
            Expression body = selector;
            if (body is LambdaExpression) {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType) {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
