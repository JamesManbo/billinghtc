using CachingLayer.Implementation;
using Castle.DynamicProxy;
using Global.Configs.SystemArgument;
using Global.Models.PagedList;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CachingLayer.Interceptor
{
    public class CacheInterceptor : IInterceptor
    {
        private static readonly MethodInfo handleAsyncMethodInfo
            = typeof(CacheInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo returnAsyncWithResult
            = typeof(CacheInterceptor).GetMethod("ReturnAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly ICacheHandler _cachHandler;

        public long Duration { private get; set; }

        public CacheInterceptor(ICacheHandler cachHandler)
        {
            _cachHandler = cachHandler;
        }

        public void Intercept(IInvocation invocation)
        {
            // Get the cache attribute info
            var cacheAttribute = invocation.Method.GetCustomAttribute<CacheAttribute>();
            if (cacheAttribute != null)
            {
                this.Duration = cacheAttribute.Duration > 0 ? cacheAttribute.Duration : 30;
            }
            else
            {
                // Invoke this method
                invocation.Proceed();
                return;
            }

            // Build the cache key.
            AppendCallInformation(invocation.Method, invocation.Arguments, out var cacheKeyStringBuilder);
            var cacheKey = cacheKeyStringBuilder.ToString();
            // Attempt to get cached data by the key.
            // Return either true if successful, false if failed
            if (_cachHandler.TryGet(out string cachedData, cacheKey))
            {
                var returnType = invocation.Method.ReturnType;
                var delegateType = GetDelegateType(invocation);
                object? cachedValue;

                if (delegateType == MethodType.AsyncFunction)
                {
                    var taskResultType = returnType.GetGenericArguments()[0];
                    if (taskResultType.GetGenericTypeDefinition() == typeof(IPagedList<>))
                    {
                        var typeArgurment = taskResultType.GetGenericArguments()[0];
                        var deserializeResultType = typeof(PagedList<>).MakeGenericType(typeArgurment);
                        cachedValue = JsonConvert.DeserializeObject(cachedData, deserializeResultType);
                    }
                    else
                    {
                        cachedValue = JsonConvert.DeserializeObject(cachedData, taskResultType);
                    }

                    var returnMethodInvkr = returnAsyncWithResult.MakeGenericMethod(taskResultType);
                    invocation.ReturnValue = returnMethodInvkr.Invoke(this, new[] { cachedValue });
                }
                else
                {
                    if (returnType.GetGenericTypeDefinition() == typeof(IPagedList<>))
                    {
                        var typeArgurment = returnType.GetGenericArguments()[0];
                        var deserializeResultType = typeof(PagedList<>).MakeGenericType(typeArgurment);
                        cachedValue = JsonConvert.DeserializeObject(cachedData, deserializeResultType);
                    }
                    else
                    {
                        cachedValue = JsonConvert.DeserializeObject(cachedData, returnType);
                    }
                    invocation.ReturnValue = cachedValue;
                }
            }
            else
            {
                // Invoke this method
                invocation.Proceed();
                var delegateType = GetDelegateType(invocation);

                // After this method returned result,
                // add the result into the cache memory
                if (delegateType == MethodType.AsyncFunction)
                {
                    if (invocation.ReturnValue.GetType().IsGenericType)
                    {
                        var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
                        var returnMethodInvkr = handleAsyncMethodInfo.MakeGenericMethod(resultType);
                        invocation.ReturnValue = returnMethodInvkr.Invoke(this, new[] { invocation.ReturnValue, cacheKey });
                    }
                }
                else
                {
                    _cachHandler.Set(cacheKey, invocation.ReturnValue, Duration);
                }
            }
        }

        private Task<T> ReturnAsyncWithResult<T>(T taskValue)
        {
            return Task<T>.FromResult(taskValue);
        }

        private async Task<T> HandleAsyncWithResult<T>(Task<T> task, string cacheKey)
        {
            await task.ConfigureAwait(false);
            var taskResult = task.Result;
            _cachHandler.Set(cacheKey, taskResult, Duration);

            return taskResult;
        }

        private MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }

        private void AppendCallInformation(MethodInfo methodInfo, object[] args, out StringBuilder stringBuilder)
        {
            stringBuilder = new StringBuilder();
            // Append type and method name.
            var declaringType = methodInfo.DeclaringType;
            CacheKeyFormatter.AppendTypeName(stringBuilder, declaringType);
            stringBuilder.Append('.');
            stringBuilder.Append(methodInfo.Name);

            // Append generic arguments.
            if (methodInfo.IsGenericMethod)
            {
                var genericArguments = methodInfo.GetGenericArguments();
                CacheKeyFormatter.AppendGenericArguments(stringBuilder, genericArguments);
            }

            // Append arguments.
            CacheKeyFormatter.AppendArguments(stringBuilder, args);
        }
    }
}
