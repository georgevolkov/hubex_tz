- устанавливаем postgresql последней версии
- в проекте открываем терминал
- заходим в папку Hubex.Module.Adm и запускаем dotnet ef database update --startup-project ../Hubex.Web --context AdmDbContext
- заходим в папку Hubex.Module.Work и запускаем dotnet ef database update --startup-project ../Hubex.Web --context WorkDbContext
- Запускаем проект в сваггере дергаем нужные методы. 

(Тестовые данные заполнил только для Adm контекста)