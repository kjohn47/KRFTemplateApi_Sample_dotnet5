namespace KRFTemplateApi.App.CQRS.Sample.Command
{
    using System.Net;

    using FluentValidation;

    using KRFCommon.CQRS.Validator;

    using KRFTemplateApi.Domain.CQRS.Sample.Command;

    public class PostSampleDataValidator : KRFValidator<SampleCommandInput>, IKRFValidator<SampleCommandInput>
    {
        public PostSampleDataValidator()
            : base( HttpStatusCode.BadRequest, CascadeMode.Continue )
        {
            this.RuleFor( r => r.Min )
                .LessThan( r => r.Max )
                .WithErrorCode( "MINMAXERR" )
                .WithMessage( "Min Value must be smaller than Max Value" );

            this.RuleFor( r => r.Code )
                .NotNull()
                .WithErrorCode( "MISSINGCODEERR" )
                .WithMessage( "You must add a Code" )
                .NotEmpty()
                .WithErrorCode( "EMPTYCODEERR" )
                .WithMessage( "Code Value cannot be empty" );

            this.RuleFor( r => r.Description )
                .NotNull()
                .WithErrorCode( "MISSINGDESCERR" )
                .WithMessage( "You must add a Description" )
                .NotEmpty()
                .WithErrorCode( "EMPTYDESCERR" )
                .WithMessage( "Description Value cannot be empty" );
        }
    }
}
