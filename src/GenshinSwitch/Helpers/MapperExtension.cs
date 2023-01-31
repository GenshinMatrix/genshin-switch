using AutoMapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace GenshinSwitch.Helpers;

public static class MapperExtension
{
    public static void CustomMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Action<TSource, TDestination> function)
    {
        expression.BeforeMap(function).ForAllMembers(cfg => cfg.Ignore());
    }

    public static IMappingExpression<TSource, TDestination> ForAllMembersStringClone<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
    {
        expression.ValueTransformers.Add<string>(value => (value.Clone() as string)!);
        return expression;
    }

    public static IMappingExpression<TSource, TDestination> IgnoreAllNotMappedAttribute<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
    {
        foreach (PropertyInfo prop in typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetCustomAttribute<NotMappedAttribute>() is NotMappedAttribute attr)
            {
                expression.ForMember(prop.Name, opt => opt.Ignore());
            }
        }
        return expression;
    }

    public static void Forget<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
    {
    }
}
