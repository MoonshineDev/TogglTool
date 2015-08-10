using NUnit.Framework;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
        public void Ctor_InitializeWithSpecifiedConnectionString()
        {
            var expected = "Server=.;Database=foobar;Trusted_Connection=True;";
            var sut = new DatabaseContext(expected);
            var actual = sut.Database.Connection.ConnectionString;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Ctor_InitializeWithSettingsByDefault()
        {
            var expected = TogglTool.Api.Properties.Settings.Default.ConnectionString;
            var sut = new DatabaseContext();
            var actual = sut.Database.Connection.ConnectionString;
            Assert.AreEqual(expected, actual);
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
                .Where(x => !x.Equals(Assembly.GetExecutingAssembly()))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof (BaseEntity)))
                .Where(x => !x.IsAbstract)
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
            // Avoid initializing the whole DbContext.
            var sut = (DatabaseFakeContext)FormatterServices.GetUninitializedObject(typeof (DatabaseFakeContext));
            var modelBuilder = sut.TriggerOnModelCreating();
            var modelConfiguration = GetPrivateField(modelBuilder, "_modelConfiguration");
            var activeEntityConfigurations = GetPrivateProperty(modelConfiguration, "ActiveEntityConfigurations") as IList;
            var togglEntities = _types
                .Where(x => x.IsSubclassOf(typeof(TogglEntity)))
                .ToDictionary(x => x, x => false);
            Assert.IsNotNull(activeEntityConfigurations);
            foreach (var entityConfiguration in activeEntityConfigurations)
            {
                var clrType = GetPrivateProperty(entityConfiguration, "ClrType") as Type;
                var primitivePropertyConfigurations = GetPrivateProperty(entityConfiguration, "PrimitivePropertyConfigurations") as IDictionary;
                Assert.IsNotNull(clrType);
                Assert.IsNotNull(primitivePropertyConfigurations);
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

        private static object GetPrivateField(object source, string name)
        {
            var field = source.GetType()
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field);
            return field.GetValue(source);
        }

        private static object GetPublicProperty(object source, string name)
        {
            var property = source.GetType()
                .GetProperty(name);
            Assert.IsNotNull(property);
            return property.GetValue(source);
        }

        private static object GetPrivateProperty(object source, string name)
        {
            var property = source.GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(property);
            return property.GetValue(source);
        }

        private class FakeEntity : BaseEntity
        { }

        private class DatabaseFakeContext : DatabaseContext
        {
            public DbModelBuilder TriggerOnModelCreating()
            {
                var modelBuilder = new DbModelBuilder();
                OnModelCreating(modelBuilder);
                return modelBuilder;
            }
        }
    }
}
