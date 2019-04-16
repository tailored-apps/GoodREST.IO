using GoodREST.Extensions.SwaggerExtension.Auxillary;
using GoodREST.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoodREST.Extensions.SwaggerExtension.Tests
{
    public class GetAllTypesDown
    {
        [Fact]
        public void Test()
        {
            Dictionary<Type, bool> types = new Dictionary<Type, bool>() {
                { typeof(GetMeMore), false },
                { typeof(TypeProperty), false },
                { typeof(SomeCollectionData), false },
                { typeof(GetMeMoreResponse), false },
                { typeof(ReturnType), false },
                { typeof(RetrurnedElements), false },
            };
            var type = typeof(GetMeMore);


            var result = type.GetTypeTree();
            
            foreach(var scannedType in result)
            {
                types[scannedType] = true;
            }
            Assert.All(types, x => Assert.True(x.Value, $" Type '{x.Key.Name}' not found"));
        }
        [Fact]
        public void TestParams()
        {
            Dictionary<Type, bool> types = new Dictionary<Type, bool>() {
                { typeof(ReturnType), false },
                { typeof(RetrurnedElements), false },
            };
            var type = typeof(ReturnType);


            var result = type.GetTypeTree();

            Assert.Equal(types.Count(), result.Count());

            foreach (var scannedType in result)
            {
                types[scannedType] = true;
            }
            Assert.All(types, x => Assert.True(x.Value, $" {x.Key.Name} not found"));
        }


        [Fact]
        public void TestStackOverflow()
        {
            Dictionary<Type, bool> types = new Dictionary<Type, bool>() {
                { typeof(A), false },
                { typeof(B), false }
            };
            var type = typeof(A);


            var result = type.GetTypeTree();

            foreach (var scannedType in result)
            {
                types[scannedType] = true;
            }
            Assert.All(types, x => Assert.True(x.Value, $" Type '{x.Key.Name}' not found"));
        }

    }
    public class GetMeMore : IHasResponse<GetMeMoreResponse>, ICorrelation
    {
        public int Id { get; set; }
        public string StringValiue { get; set; }
        public string CorrelationId { get; set; }
        public TypeProperty TypeProperty { get; set; }
        public ICollection<int> Ids { get; set; }
        public ICollection<SomeCollectionData> SomeCollectionData { get; set; }
    }
    public class GetMeMoreResponse : IResponse, ICorrelation
    {
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
        public string CorrelationId { get; set; }

        public ReturnType ReturnType { get; set; }
    }
    public class TypeProperty
    {
        public int Id { get; set; }
        public string StringValiue { get; set; }
    }
    public class ReturnType
    {
        public int Id { get; set; }
        public string StringValiue { get; set; }
        public ICollection<RetrurnedElements> RetrurnedElements { get; set; }

    }
    public class RetrurnedElements
    {
        public int ElementId { get; set; }
        public string StringValiue { get; set; }

    }
    public class SomeCollectionData
    {
        public int ElementId { get; set; }
        public string StringValiue { get; set; }

    }


    public class A
    {
        public B ClassB { get; set; }
    }
    public class B
    {
        public A ClassA { get; set; }
    }

}