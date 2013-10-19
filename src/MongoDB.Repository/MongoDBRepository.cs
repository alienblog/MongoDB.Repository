using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    /// <summary>
    /// MongoDBRepository
    /// </summary>
    public class MongoDBRepository
    {
        static MongoDBRepository()
        {
            Contexts = new List<IRegistrationContext>();
        }
        static readonly List<IRegistrationContext> Contexts;

        /// <summary>
        /// register IMongoDBContext if not exists
        /// </summary>
        /// <param name="dbContext"></param>
        public static void RegisterMongoDBContext(IMongoDBContext dbContext)
        {
            if (Contexts.Exists(c => c.Code == dbContext.GetType().FullName))
                return;

            IRegistrationContext context = new RegistrationContext();
            context.RegisterDBContext(dbContext);
            Contexts.Add(context);
        }

        /// <summary>
        /// get MongoUrl of type which first found
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        internal static MongoUrl GetConfig(Type type)
        {
            var context = Contexts.SingleOrDefault(c => c.IsRegisterType(type));
            if (context == null) return null;
            return context.GetMongoUrl();
        }

        /// <summary>
        /// get MongoUrl of type which first found
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        internal static MongoUrl GetConfig<T>()
        {
            return GetConfig(typeof(T));
        }
        /// <summary>
        /// register collection type for IMongoDBContext
        /// </summary>
        /// <param name="dbContextType">IMongoDBContext</param>
        /// <param name="entityType">collection type</param>
        public static void RegisterType(Type dbContextType, Type entityType)
        {
            if (!Contexts.Exists(c => c.Code == dbContextType.FullName)) throw new MongoException("Unregisterd MongoDBContext");

            var context = Contexts.SingleOrDefault(c => c.Code == dbContextType.FullName);
            if (context == null) throw new MongoException("Unregisterd MongoDBContext");
            context.RegisterType(entityType);
        }
        /// <summary>
        /// register collection type for IMongoDBContext
        /// </summary>
        /// <typeparam name="TDBContextType">IMongoDBContext</typeparam>
        /// <typeparam name="TEntityType">collection type</typeparam>
        public static void RegisterType<TDBContextType, TEntityType>()
        {
            RegisterType(typeof(TDBContextType), typeof(TEntityType));
        }
        /// <summary>
        /// is register collection type
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        public static bool IsRegisterType(Type type)
        {
            return Contexts.Exists(c => c.IsRegisterType(type));
        }
        /// <summary>
        /// is register collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        public static bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }
        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <param name="type">collection type</param>
        public static void UnregisterType(Type type)
        {
            Contexts.ForEach(delegate(IRegistrationContext context)
            {
                context.UnregisterType(type);
            });
        }
        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        public static void UnregisterType<T>()
        {
            UnregisterType(typeof(T));
        }

        public static void UnregisterDBContext<TDBContext>() where TDBContext : IMongoDBContext
        {
            Contexts.RemoveAll(registeration => registeration.Code == typeof(TDBContext).FullName);
        }

        public static MongoUrl GetMongoUrl()
        {
            var context = Contexts.FirstOrDefault();
            if (context == null) throw new MongoException("Have No MongoDBContext");
            return context.GetMongoUrl();
        }
    }

}
