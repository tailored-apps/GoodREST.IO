using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    [Route("/FooBar", HttpVerb.GET)]
    public class GetBar : IHasResponse<GetBarResponse>
    {

    }
}