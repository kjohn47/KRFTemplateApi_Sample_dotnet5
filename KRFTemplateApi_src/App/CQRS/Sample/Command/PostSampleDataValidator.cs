namespace KRFTemplateApi.App.CQRS.Sample.Command
{
    using System.Net;

    using FluentValidation;

    using KRFCommon.CQRS.Validator;

    using KRFTemplateApi.Domain.CQRS.Sample.Command;

    public class PostSampleDataValidator : KRFValidator<SampleCommandInput>, IKRFValidator<SampleCommandInput>
    {
        public PostSampleDataValidator() : base()
        {
            this.RuleFor( r => r.Min )
                .LessThan( r => r.Max )
                .WithErrorCode( GenerateErrorCodeWithHttpStatus( HttpStatusCode.BadRequest, "MINMAXERR" ) )
                .WithMessage( "Min Value must be smaller than Max Value" );


            this.RuleFor( r => r.Code )
                .NotNull()
                .WithErrorCode( GenerateErrorCodeWithHttpStatus( HttpStatusCode.BadRequest, "MISSINGCODEERR" ) )
                .WithMessage( "You must add a Code" )
                .NotEmpty()
                .WithErrorCode( GenerateErrorCodeWithHttpStatus( HttpStatusCode.OK, "EMPTYCODEERR" ) )
                .WithMessage( "Code Value cannot be empty" );

            this.RuleFor( r => r.Description )
                .NotNull()
                .WithErrorCode( GenerateErrorCodeWithHttpStatus( HttpStatusCode.BadRequest, "MISSINGDESCERR" ) )
                .WithMessage( "You must add a Description" )
                .NotEmpty()
                .WithErrorCode( GenerateErrorCodeWithHttpStatus( HttpStatusCode.OK, "EMPTYDESCERR" ) )
                .WithMessage( "Description Value cannot be empty" );
        }
    }
}
