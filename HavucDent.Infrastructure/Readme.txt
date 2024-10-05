dotnet ef migrations add MigrationAdı --project HavucDent.Infrastructure --startup-project HavucDent.Web
dotnet ef database update --project HavucDent.Infrastructure --startup-project HavucDent.Web

MigrationAdı kısmını migration içeriği ne ise o yazılacak şekilde güncellenecek.