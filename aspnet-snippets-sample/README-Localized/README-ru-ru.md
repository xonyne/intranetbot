# Пример фрагментов кода Microsoft Graph для ASP.NET 4.6
<a id="microsoft-graph-snippets-sample-for-aspnet-46" class="xliff"></a>

## Содержание
<a id="table-of-contents" class="xliff"></a>

* [Необходимые компоненты](#prerequisites)
* [Регистрация приложения](#register-the-application)
* [Сборка и запуск примера](#build-and-run-the-sample)
* [Полезный код](#code-of-note)
* [Вопросы и комментарии](#questions-and-comments)
* [Участие](#contributing)
* [Дополнительные ресурсы](#additional-resources)

В этом примере представлены фрагменты кода, использующие Microsoft Graph для отправки электронной почты, управления группами и выполнения других стандартных задач из приложения ASP.NET MVC. Для работы с данными, возвращаемыми Microsoft Graph, используется [клиентский пакет SDK .NET Microsoft Graph](https://github.com/microsoftgraph/msgraph-sdk-dotnet). 

Для проверки подлинности в этом примере используется библиотека [Microsoft Authentication Library (MSAL)](https://www.nuget.org/packages/Microsoft.Identity.Client/). В пакете SDK MSAL предусмотрены функции для работы с [конечной точкой Azure AD версии 2.0](https://azure.microsoft.com/en-us/documentation/articles/active-directory-appmodel-v2-overview), которая позволяет разработчикам создать единый поток кода для проверки подлинности как рабочих или учебных (Azure Active Directory), так и личных учетных записей Майкрософт.

Кроме того, в примере показано, как запрашивать маркеры пошагово. Эта функция поддерживается конечной точкой Azure AD версии 2.0. Пользователи предоставляют начальный набор разрешений при входе, но могут предоставить другие разрешения позже. Любой действительный пользователь может войти в это приложение, но администраторы могут позже предоставить разрешения, необходимые для определенных операций.

В примере используется [ПО промежуточного слоя ASP.NET OpenId Connect OWIN](https://www.nuget.org/packages/Microsoft.Owin.Security.OpenIdConnect/) для входа и при первом получении токена. В примере также используется специальное ПО промежуточного слоя Owin для обмена кода авторизации на токены доступа и обновления после входа. Специальное ПО промежуточного слоя вызывает MSAL для создания URI запроса на авторизацию и обрабатывает перенаправления. Дополнительные сведения о предоставлении дополнительных разрешений см. в статье [Интеграция идентификатора Майкрософт и Microsoft Graph в веб-приложении с помощью OpenID Connect](https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect-v2).

## Важное примечание о предварительной версии MSAL
<a id="important-note-about-the-msal-preview" class="xliff"></a>

Эту библиотеку можно использовать в рабочей среде. Для этой библиотеки мы предоставляем тот же уровень поддержки, что и для текущих библиотек рабочей среды. Мы можем внести изменения в API, формат внутреннего кэша и другие функциональные элементы, касающиеся этой предварительной версии библиотеки, которые вам потребуется принять вместе с улучшениями или исправлениями. Это может повлиять на ваше приложение. Например, в результате изменения формата кэша пользователям может потребоваться опять выполнить вход. При изменении API может потребоваться обновить код. Когда мы предоставим общедоступный выпуск, вам потребуется выполнить обновление до общедоступной версии в течение шести месяцев, так как приложения, написанные с использованием предварительной версии библиотеки, могут больше не работать.


## Необходимые условия
<a id="prerequisites" class="xliff"></a>

Для этого примера требуются следующие компоненты:  

  * [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads) 
  * [Учетная запись Майкрософт](https://www.outlook.com) или [учетная запись Office 365 для бизнеса](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account). Для выполнения административных операций требуется учетная запись администратора Office 365. Вы можете подписаться на [план Office 365 для разработчиков](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account), который включает ресурсы, необходимые для создания приложений.

## Регистрация приложения
<a id="register-the-application" class="xliff"></a>

1. Войдите на [портал регистрации приложений](https://apps.dev.microsoft.com/) с помощью личной, рабочей или учебной учетной записи.

2. Нажмите кнопку **Добавить приложение**.

3. Введите имя приложения и нажмите кнопку **Создать приложение**. 
    
   Откроется страница регистрации со свойствами приложения.

4. Скопируйте идентификатор приложения. Это уникальный идентификатор приложения. 

5. В разделе **Секреты приложения** нажмите кнопку **Создать новый пароль**. Скопируйте пароль из диалогового окна **Новый пароль создан**.

   Вам потребуется ввести скопированные код приложения и пароль в пример приложения. 

6. В разделе **Платформы** нажмите кнопку **Добавление платформы**.

7. Выберите пункт **Веб**.

8. Убедитесь, что установлен флажок **Разрешить неявный поток**, и введите универсальный код ресурса (URI) перенаправления *https://localhost:44300/*. 

   Параметр **Разрешить неявный поток** включает гибридный поток. Благодаря этому при проверке подлинности приложение может получить данные для входа (id_token) и артефакты (в данном случае — код авторизации), которые оно может использовать, чтобы получить маркер доступа.

9. Нажмите кнопку **Сохранить**.
 
 
## Сборка и запуск приложения
<a id="build-and-run-the-sample" class="xliff"></a>

1. Скачайте или клонируйте пример фрагментов кода Microsoft Graph для ASP.NET 4.6.

2. Откройте решение в Visual Studio.

3. В корневом каталоге файла Web.config замените заполнители **ida:AppId** и **ida:AppSecret** значениями, которые вы скопировали при регистрации приложения.

4. Нажмите клавишу F5 для сборки и запуска примера. При этом будут восстановлены зависимости пакета NuGet и откроется приложение.

   >Если при установке пакетов возникают ошибки, убедитесь, что локальный путь к решению не слишком длинный. Чтобы устранить эту проблему, переместите решение ближе к корню диска.

5. Войдите с помощью личной, рабочей или учебной учетной записи и предоставьте необходимые разрешения. 

6. Выберите категорию фрагментов, например "Пользователи", "Файлы" или "Почта". 

7. Выберите необходимую операцию. Обратите внимание на следующее:
  - Операции, для которых требуется аргумент (например, идентификатор), будут отключены до запуска фрагмента, который позволяет выбрать объект. 

  - Для некоторых фрагментов (помеченные *только для администраторов*) требуются платные разрешения, которые может предоставить только администратор. Для запуска этих фрагментов войдите в учетную запись администратора и воспользуйтесь ссылкой на вкладке *Области администрирования*, чтобы предоставить необходимые разрешения. Эта вкладка недоступна для пользователей, которые вошли с помощью личных учетных записей.
   
  - Если вы вошли с помощью личной учетной записи, фрагменты, которые не поддерживаются для учетных записей Майкрософт, будут отключены.
   
Сведения об ответе отображаются в нижней части страницы.

### Как пример влияет на данные учетной записи
<a id="how-the-sample-affects-your-account-data" class="xliff"></a>

Этот пример создает, обновляет и удаляет объекты и данные (например, пользователей или файлы). Используя его, **вы можете изменить или удалить объекты и данные**, а также оставить артефакты данных. 

Чтобы этого избежать, обновляйте и удаляйте только те объекты, которые созданы примером. 


## Полезный код
<a id="code-of-note" class="xliff"></a>

- [Startup.Auth.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/App_Start/Startup.Auth.cs). Выполняет проверку подлинности для текущего пользователя и инициализирует кэш маркеров примера.

- [SessionTokenCache.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/TokenStorage/SessionTokenCache.cs). Хранит информацию о маркере пользователя. Вы можете заменить его на собственный кэш маркеров. Дополнительные сведения см. в статье [Кэширование маркеров доступа в мультитенантном приложении](https://azure.microsoft.com/en-us/documentation/articles/guidance-multitenant-identity-token-cache/).

- [SampleAuthProvider.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Helpers/SampleAuthProvider.cs). Реализует локальный интерфейс IAuthProvider и получает маркер доступа с помощью метода **AcquireTokenSilentAsync**. Вы можете заменить его на собственного поставщика услуг авторизации. 

- [SDKHelper.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Helpers/SDKHelper.cs). Инициализирует класс **GraphServiceClient** из [клиентской библиотеки .NET Microsoft Graph](https://github.com/microsoftgraph/msgraph-sdk-dotnet), используемой для взаимодействия с Microsoft Graph.

- Указанные ниже контроллеры содержат методы, которые используют класс **GraphServiceClient** для создания и отправки вызовов в службу Microsoft Graph и обработки ответа.
  - [UsersController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/UsersController.cs) 
  - [MailController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/MailController.cs)
  - [EventsController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/EventsController.cs) 
  - [FilesController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/FilesController.cs)  
  - [GroupsController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/GroupsController.cs) 

- Указанные ниже представления содержат пользовательский интерфейс примера.  
  - [Users.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Users/Users.cshtml)  
  - [Mail.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Mail/Mail.cshtml)
  - [Events.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Events/Events.cshtml) 
  - [Files.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Files/Files.cshtml)  
  - [Groups.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Groups/Groups.cshtml)

- Указанные ниже файлы содержат представления по умолчанию и частичное представление, которые используются для анализа и отображения данных Microsoft Graph в виде общих объектов (в этом примере). 
  - [ResultsViewModel.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Models/ResultsViewModel.cs)
  - [_ResultsPartial.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Shared/_ResultsPartial.cshtml)  

- Файлы ниже содержат код, используемый для поддержки дополнительных разрешений. В этом примере при входе пользователям предлагается принять начальный набор разрешений и отдельно выбрать разрешения администратора. 
  - [AdminController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/AdminController.cs)
  - [OAuth2CodeRedeemerMiddleware.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Utils/OAuth2CodeRedeemerMiddleware.cs). Настраиваемое ПО промежуточного слоя, которое использует код авторизации для маркеров обновления и доступа после входа. Дополнительные сведения о внедрении дополнительного согласия см. по адресу: https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect-v2.

## Вопросы и комментарии
<a id="questions-and-comments" class="xliff"></a>

Мы будем рады узнать ваше мнение об этом примере. Вы можете отправлять нам вопросы и предложения на вкладке [Issues](https://github.com/microsoftgraph/aspnet-snippets-sample/issues) этого репозитория.

Ваше мнение важно для нас. Для связи с нами используйте сайт [Stack Overflow](http://stackoverflow.com/questions/tagged/microsoftgraph). Помечайте свои вопросы тегом [MicrosoftGraph].

## Участие
<a id="contributing" class="xliff"></a>

Если вы хотите добавить код в этот пример, просмотрите статью [CONTRIBUTING.md](CONTRIBUTING.md).

Этот проект соответствует [правилам поведения Майкрософт, касающимся обращения с открытым кодом](https://opensource.microsoft.com/codeofconduct/). Читайте дополнительные сведения в [разделе вопросов и ответов по правилам поведения](https://opensource.microsoft.com/codeofconduct/faq/) или отправляйте новые вопросы и замечания по адресу [opencode@microsoft.com](mailto:opencode@microsoft.com). 

## Дополнительные ресурсы
<a id="additional-resources" class="xliff"></a>

- [Другие примеры фрагментов кода Microsoft Graph](https://github.com/MicrosoftGraph?utf8=%E2%9C%93&query=snippets)
- [Общие сведения о Microsoft Graph](http://graph.microsoft.io)
- [Примеры кода приложений для Office](http://dev.office.com/code-samples)
- [Центр разработки для Office](http://dev.office.com/)

## Авторское право
<a id="copyright" class="xliff"></a>
(c) Корпорация Майкрософт (Microsoft Corporation), 2016. Все права защищены.
