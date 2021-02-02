KRFTemplateApi

Access Token
	Configure token and app info on appsettings AppConfiguration
	Some modules are part of KRFCommon nuget
	On controller set data annotation on request -> [Authorize] for only auth users or [Authorize(Policies.Admin)] (include KRFCommon.Context; for policies) for only admin

Config DB (important)
	LocalDB database
		Add correct connection string for localdb -> [DIR TO PROJECT] DBFiles\KRFTemplateApiDB.mdf
		Set the assembly where project migrations are : MigrationAssembly

	Migrations --> Add-Migration [migration_name] -OutputDir "Migrations" -Project KRFTemplateApi.Infrastructure
	run update-database or run the project (auto migrations enabled)

	Alternative migrations (not tested)
			dotnet tool install --global dotnet-ef
			go to infrastructure project [KRFTemplateApi.Infrastructure]
			run:
			dotnet ef  migrations add [migration_name] --context Database.DBContext.[ContextMain] -o KRFTemplateApi.Infrastructure/Migrations	
Logging
	logs are fully enabled for dev env and warning only for prd
	logs appear on console
	request token and body are logged to the console in dev but not in prd unless appconfig LogExceptionOnPrd is set to true

-->>>>>> RUN The project -> use Kestrel profile to run microservice <<<<<<<<------


Projects 
	App
		Contains all services
		Injection of services made on Injection namespace classes
			-- inject proxies
			-- inject queries
			-- inject commands
			-- inject database context
			
		CQRS commands and querys logic
		Database querys EF
		Constants
		
	Domain
		This project must be free of dependencies
		Static classes and models for CQRS and Database
		
	Infrastructure
		Database Context
			-- main database context file
			-- Add configuration and seeder to configure the entity to main file
		Database Migrations
	
	WebApi
		Main application and web service
		Configuration of the app (appconfig.json)
		Configuration of services and call of service injection methods from KRFCommon and App project
		Controllers are setted here (always extend KRFController from KRFCommon)
		