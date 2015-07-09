using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Database;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Database
{
    [TestFixture]
    public class DatabaseContextTests
    {
        private Type[] _types;

        [SetUp]
        public void Setup()
        {
            _types = typeof (DatabaseContext)
                .GetProperties()
                .Select(x => x.PropertyType)
                .Where(x => x.IsGenericType)
                .Where(x => x.GetGenericTypeDefinition() == typeof (DbSet<>))
                .Select(x => x.GetGenericArguments().Single())
                .ToArray();
        }

        [Test]
        public void ContextOnlyContainsBaseEntity()
        {
            var list = _types
                .Where(x => !x.IsSubclassOf(typeof (BaseEntity)))
                .ToArray();
            Assert.IsEmpty(list);
        }

        [Test]
        public void ContextContainsAllBaseEntity()
        {
            var actual = _types
                .Where(x => x.IsSubclassOf(typeof (BaseEntity)))
                .ToArray();
            var expected = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof (BaseEntity)))
                .Where(x => !x.IsAbstract)
                .Where(x => x != typeof(FakeEntity))
                .ToArray();
            foreach(var type in expected)
                Assert.Contains(type, actual);
        }

        [Test]
        public void ContextDoesNotContainArbitraryBaseEntity()
        {
            var list = _types
                .Where(x => x.IsSubclassOf(typeof (FakeEntity)))
                .ToArray();
            Assert.IsEmpty(list);
        }

        [Test]
        public void TogglEntitiesDoesNotHaveIdentityAttribute()
        {
            var sut = new DatabaseContext();
            var configuration = sut.Configuration;
            var internalContext = GetPrivateField(configuration, "_internalContext");
            var codeFirstModel = GetPublicProperty(internalContext, "CodeFirstModel");
            var cachedModelBuilder = GetPrivateProperty(codeFirstModel, "CachedModelBuilder");
            var configurations = GetPublicProperty(cachedModelBuilder, "Configurations");
            var modelConfiguration = GetPrivateField(configurations, "_modelConfiguration");
            var activeEntityConfigurations = GetPrivateProperty(modelConfiguration, "ActiveEntityConfigurations") as IList;
            var togglEntities = _types
                .Where(x => x.IsSubclassOf(typeof (TogglEntity)))
                .ToDictionary(x => x, x => false);
            foreach (var entityConfiguration in activeEntityConfigurations)
            {
                var clrType = GetPrivateProperty(entityConfiguration, "ClrType") as Type;
                var primitivePropertyConfigurations = GetPrivateProperty(entityConfiguration, "PrimitivePropertyConfigurations") as IDictionary;
                foreach (var kvp in primitivePropertyConfigurations)
                {
                    var property = GetPublicProperty(kvp, "Key");
                    var primitivePropertyConfiguration = GetPublicProperty(kvp, "Value");
                    var databaseGeneratedOption = GetPublicProperty(primitivePropertyConfiguration, "DatabaseGeneratedOption") as DatabaseGeneratedOption?;
                    if (property.ToString() == "id" && databaseGeneratedOption == DatabaseGeneratedOption.None)
                        togglEntities[clrType] = true;
                }
            }
            Assert.True(togglEntities.All(x => x.Value));
        }

        private object GetPublicField(object source, string name)
        {
            return source
                .GetType()
                .GetField(name)
                .GetValue(source);
        }

        private object GetPrivateField(object source, string name)
        {
            return source
                .GetType()
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(source);
        }

        private object GetPublicProperty(object source, string name)
        {
            return source
                .GetType()
                .GetProperty(name)
                .GetValue(source);
        }

        private object GetPrivateProperty(object source, string name)
        {
            return source
                .GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(source);
        }

        private class FakeEntity : BaseEntity
        { }
    }
}
