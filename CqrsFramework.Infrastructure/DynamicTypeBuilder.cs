using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CqrsFramework.Infrastructure
{
    internal static class DynamicTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new("DynamicCommands");

        public static TypeBuilder CreateType(string typeName)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(AssemblyName.Name);
            return moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
        }

        public static void AddProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType, bool isStrictCqrs = false)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // Sets Get Method
            var getMethodBuilder = typeBuilder.DefineMethod(
                "get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes
            );

            var getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getMethodBuilder);

            // If is not strict, we wil add an 'Set Method'

            if (!isStrictCqrs)
            {
                var setMethodBuilder = typeBuilder.DefineMethod(
                    "set_" + propertyName,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    null,
                    new[] { propertyType }
                );

                var setIl = setMethodBuilder.GetILGenerator();
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Ldarg_1);
                setIl.Emit(OpCodes.Stfld, fieldBuilder);
                setIl.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }
        }

        public static Type Build(this TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateType();
        }
    }
}