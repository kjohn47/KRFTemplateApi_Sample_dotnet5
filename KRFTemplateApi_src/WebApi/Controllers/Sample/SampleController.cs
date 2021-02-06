namespace KRFTemplateApi.WebApi.Controllers.Sample
{
    using System.Net;
    using System.Threading.Tasks;

    using KRFCommon.Context;
    using KRFCommon.Controller;
    using KRFCommon.CQRS.Command;
    using KRFCommon.CQRS.Query;

    using KRFTemplateApi.Domain.CQRS.Sample.Command;
    using KRFTemplateApi.Domain.CQRS.Sample.Query;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route( "Sample/" )]
    public class SampleController : KRFController
    {
        [HttpGet( "ListAll" )]
        [ProducesResponseType( ( int ) HttpStatusCode.OK, Type = typeof( ListSampleOutput ) )]
        public async Task<IActionResult> ListAll(
                [FromServices] IQuery<ListSampleInput, ListSampleOutput> query,
                [FromQuery] string code )
        {
            return await this.ExecuteAsyncQuery( new ListSampleInput { Code = code }, query );
        }

        [HttpGet( "GetData" )]
        [ProducesResponseType( ( int ) HttpStatusCode.OK, Type = typeof( SampleOutput ) )]
        public async Task<IActionResult> GetData(
                [FromServices] IQuery<SampleInput, SampleOutput> query )
        {
            return await this.ExecuteAsyncQuery( new SampleInput(), query );
        }

        [Authorize]
        [HttpGet( "GetDataAuth" )]
        [ProducesResponseType( ( int ) HttpStatusCode.OK, Type = typeof( SampleOutput ) )]
        public async Task<IActionResult> GetDataAuth(
        [FromServices] IQuery<SampleInput, SampleOutput> query )
        {
            return await this.ExecuteAsyncQuery( new SampleInput(), query );
        }

        [Authorize( Policies.Admin )]
        [HttpGet( "GetDataAuthAdmin" )]
        [ProducesResponseType( ( int ) HttpStatusCode.OK, Type = typeof( SampleOutput ) )]
        public async Task<IActionResult> GetDataAuthAdmin(
        [FromServices] IQuery<SampleInput, SampleOutput> query )
        {
            return await this.ExecuteAsyncQuery( new SampleInput(), query );
        }

        [HttpPost]
        [ProducesResponseType( ( int ) HttpStatusCode.OK, Type = typeof( SampleCommandOutput ) )]
        public async Task<IActionResult> PostSampleData(
            [FromBody] SampleCommandInput request,
            [FromServices] ICommand<SampleCommandInput, SampleCommandOutput> command )
        {
            return await this.ExecuteAsyncCommand( request, command );
        }
    }
}
