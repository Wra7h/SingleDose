namespace SingleDose.Invokes
{
    internal class DInvoke
    {
        //Reference: https[:]//bohops.com/2022/04/02/unmanaged-code-execution-with-net-dynamic-pinvoke/
        public static string DynamicPInvokeBuilder = @"public static object DynamicPInvokeBuilder(Type type, string library, string method, ref Object[] args, Type[] paramTypes)
        {
            System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName(""{{ASSEMBLYNAME}}"");
            System.Reflection.Emit.AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, System.Reflection.Emit.AssemblyBuilderAccess.Run);
            System.Reflection.Emit.ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(""{{DYNAMICMODULE}}"");

            System.Reflection.Emit.MethodBuilder methodBuilder = moduleBuilder.DefinePInvokeMethod(method, library, System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.PinvokeImpl,
                                                                                                    System.Reflection.CallingConventions.Standard, type, paramTypes, CallingConvention.Winapi, CharSet.Auto);

            methodBuilder.SetImplementationFlags(methodBuilder.GetMethodImplementationFlags() | System.Reflection.MethodImplAttributes.PreserveSig);
            moduleBuilder.CreateGlobalFunctions();

            System.Reflection.MethodInfo dynamicMethod = moduleBuilder.GetMethod(method);
            object res = dynamicMethod.Invoke(null, args);
            return res;
        }

        {{INVOKE}}";
    }
}
