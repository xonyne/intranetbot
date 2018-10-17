# ASP.NET 4.6 用 Microsoft Graph スニペットのサンプル
<a id="microsoft-graph-snippets-sample-for-aspnet-46" class="xliff"></a>

## 目次
<a id="table-of-contents" class="xliff"></a>

* [前提条件](#prerequisites)
* [アプリケーションの登録](#register-the-application)
* [サンプルのビルドと実行](#build-and-run-the-sample)
* [ノートのコード](#code-of-note)
* [質問とコメント](#questions-and-comments)
* [投稿](#contributing)
* [その他のリソース](#additional-resources)

このサンプル プロジェクトには、ASP.NET MVC アプリ内からのメール送信、グループ管理、および他のアクティビティなどの一般的なタスクを実行するために Microsoft Graph を使用する、コード スニペットのリポジトリが用意されています。[Microsoft Graph .NET クライアント SDK](https://github.com/microsoftgraph/msgraph-sdk-dotnet) を使用して、Microsoft Graph が返すデータを操作します。 

サンプルでは認証に [Microsoft 認証ライブラリ (MSAL)](https://www.nuget.org/packages/Microsoft.Identity.Client/) を使用します。MSAL SDK には、[Azure AD v2 0 エンドポイント](https://azure.microsoft.com/en-us/documentation/articles/active-directory-appmodel-v2-overview)を操作するための機能が用意されており、開発者は職場または学校 (Azure Active Directory) アカウント、および個人用 (Microsoft) アカウントの両方に対する認証を処理する 1 つのコード フローを記述することができます。

またサンプルでは、トークンを段階的に要求する方法を示します。この方法は Azure AD v2.0 エンドポイントによってサポートされている機能です。ユーザーは、サインイン中にアクセス許可の適用範囲の最初のセットに同意することになりますが、後で他の適用範囲にも同意することができます。このサンプルの場合、すべての有効なユーザーがサインインできますが、管理者は後で特定の操作に必要な管理レベルの適用範囲に同意することができます。

サンプルでは、サインインと最初のトークン取得中に [ASP.NET OpenId Connect OWIN ミドルウェア](https://www.nuget.org/packages/Microsoft.Owin.Security.OpenIdConnect/) を使用します。またサンプルでは、カスタム Owin ミドルウェアも実装して、アクセスの認証コードを交換し、サインイン フローの外部のトークンを更新します。カスタム ミドルウェアは、MSAL を呼び出して承認要求 URI を作成して、リダイレクトを処理します。段階的な同意の詳細については、「[OpenID Connect を使用して、Microsoft Identity と Microsoft Graph を Web アプリケーションに統合する](https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect-v2)」を参照してください。

## MSAL プレビューに関する重要な注意事項
<a id="important-note-about-the-msal-preview" class="xliff"></a>

このライブラリは、運用環境での使用に適しています。 このライブラリに対しては、現在の運用ライブラリと同じ運用レベルのサポートを提供します。 プレビュー中にこのライブラリの API、内部キャッシュの形式、および他のメカニズムを変更する場合があります。これは、バグの修正や機能強化の際に実行する必要があります。 これは、アプリケーションに影響を与える場合があります。 例えば、キャッシュ形式を変更すると、再度サインインが要求されるなどの影響をユーザーに与えます。 API を変更すると、コードの更新が要求される場合があります。 一般提供リリースが実施されると、プレビュー バージョンを使って作成されたアプリケーションは動作しなくなるため、6 か月以内に一般提供バージョンに更新することが求められます。


## 前提条件
<a id="prerequisites" class="xliff"></a>

このサンプルを実行するには次のものが必要です:  

  * [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads) 
  * [Microsoft アカウント](https://www.outlook.com)または [Office 365 for Business アカウント](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)のいずれか。管理レベルの操作を実行するには、Office 365 の管理者アカウントが必要です。アプリの構築を開始するために必要なリソースを含む、[Office 365 Developer サブスクリプション](https://msdn.microsoft.com/en-us/office/office365/howto/setup-development-environment#bk_Office365Account)にサインアップできます。

## アプリケーションの登録
<a id="register-the-application" class="xliff"></a>

1. 個人用アカウント、あるいは職場または学校アカウントのいずれかを使用して、[アプリ登録ポータル](https://apps.dev.microsoft.com/)にサインインします。

2. **[アプリの追加]** を選択します。

3. アプリの名前を入力して、**[アプリケーションの作成]** を選択します。 
    
   登録ページが表示され、アプリのプロパティが一覧表示されます。

4. アプリケーション ID をコピーします。これは、アプリの一意識別子です。 

5. **[アプリケーション シークレット]** で、**[新しいパスワードを生成する]** を選びます。**[新しいパスワードを生成する]** ダイアログからパスワードをコピーします。

   サンプル アプリにコピーするアプリ ID とアプリ シークレットの値を入力する必要があります。 

6. **[プラットフォーム]** で、**[プラットフォームの追加]** を選択します。

7. **[Web]** を選択します。

8. **[暗黙的フローを許可する]** のチェック ボックスが選択されていることを確認して、リダイレクト URI として *https://localhost:44300/* を入力します。 

   **[暗黙的フローを許可する]** オプションにより、ハイブリッド フローが有効になります。認証時に、アクセス トークンを取得するためにアプリが使用できるサインイン情報 (id_token) と成果物 (この場合は、認証コード) の両方をアプリで受信できるようになります。

9. **[保存]** を選択します。
 
 
## サンプルの構築と実行
<a id="build-and-run-the-sample" class="xliff"></a>

1. ASP.NET 4.6 用 Microsoft Graph スニペットのサンプルをダウンロードするか、クローンを作成します。

2. Visual Studio でサンプル ソリューションを開きます。

3. ルート ディレクトリの Web.config ファイルで、**ida:AppId** と **ida:AppSecret** のプレースホルダ―の値をアプリの登録時にコピーした値と置き換えます。

4. F5 キーを押して、サンプルを構築して実行します。これにより、NuGet パッケージの依存関係が復元され、アプリが開きます。

   >パッケージのインストール中にエラーが発生した場合は、ソリューションを保存したローカル パスが長すぎたり深すぎたりしていないかご確認ください。ドライブのルート近くにソリューションを移動すると問題が解決する場合があります。

5. 個人用アカウント (MSA) あるいは職場または学校アカウントでサインインし、要求されたアクセス許可を付与します。 

6. ユーザー、ファイル、メールなどのスニペットのカテゴリを選択します。 

7. 実行する操作を選択します。以下の点に注意してください:
  - 引数 (ID など) を必要とする操作は、エンティティを選択することができるスニペットを実行するまで無効になっています。 

  - 一部のスニペット (*管理者のみ*としてマークされている) には、管理者だけが付与できる商用のアクセス許可の適用範囲が必要です。これらのスニペットを実行するには、管理者としてサインインして、*[管理者の適用範囲]* タブのリンクを使用して、管理レベルの適用範囲に同意する必要があります。このタブは、個人用アカウントでログインしているユーザーに対しては使用できません。
   
  - 個人用アカウントでログインした場合は、Microsoft アカウントでサポートされていないスニペットは無効になっています。
   
応答情報は、ページの下部に表示されます。

### サンプルによるアカウント データへの影響
<a id="how-the-sample-affects-your-account-data" class="xliff"></a>

このサンプルでは、エンティティとデータ (ユーザーまたはファイルなど) を作成、更新、および削除します。使用方法によっては、**実際のエンティティとデータを編集または削除して**、データの成果物をそのまま残す場合があります。 

実際のアカウント データを変更せずにサンプルを使用するには、必ずサンプルで作成されるエンティティ上でのみ操作の更新と削除を実行します。 


## ノートのコード
<a id="code-of-note" class="xliff"></a>

- [Startup.Auth.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/App_Start/Startup.Auth.cs).現在のユーザーを認証して、サンプルのトークン キャッシュを初期化します。

- [SessionTokenCache.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/TokenStorage/SessionTokenCache.cs).ユーザーのトークン情報を保存します。これを独自のカスタム トークン キャッシュと置き換えることができます。詳細については、「[マルチテナント アプリケーションのアクセス トークンのキャッシュ](https://azure.microsoft.com/en-us/documentation/articles/guidance-multitenant-identity-token-cache/)」を参照してください。

- [SampleAuthProvider.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Helpers/SampleAuthProvider.cs)。ローカルの IAuthProvider インターフェイスを実装して、**AcquireTokenSilentAsync** メソッドを使用してアクセス トークンを取得します。これを独自の承認プロバイダーと置き換えることができます。 

- [SDKHelper.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Helpers/SDKHelper.cs)。Microsoft Graph との対話に使用される [Microsoft Graph .NET クライアント ライブラリ](https://github.com/microsoftgraph/msgraph-sdk-dotnet)の **GraphServiceClient** を初期化します。

- 次のコントローラーには、呼び出しを構築して Microsoft Graph サービスに送信し、その応答を処理するために **GraphServiceClient** を使用するメソッドが含まれています。
  - [UsersController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/UsersController.cs) 
  - [MailController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/MailController.cs)
  - [EventsController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/EventsController.cs) 
  - [FilesController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/FilesController.cs)  
  - [GroupsController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/GroupsController.cs) 

- 次のビューにはサンプルの UI が含まれています。  
  - [Users.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Users/Users.cshtml)  
  - [Mail.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Mail/Mail.cshtml)
  - [Events.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Events/Events.cshtml) 
  - [Files.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Files/Files.cshtml)  
  - [Groups.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Groups/Groups.cshtml)

- 次のファイルには、汎用オブジェクトとして Microsoft Graph データを解析して表示する (このサンプルの目的用) ために使用されるビュー モデルと部分的なビューが含まれています。 
  - [ResultsViewModel.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Models/ResultsViewModel.cs)
  - [_ResultsPartial.cshtml](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Views/Shared/_ResultsPartial.cshtml)  

- 次のファイルには、段階的な同意をサポートするために使用されるコードが含まれています。このサンプルで、ユーザーはサインイン中にアクセス許可の初期セットへの同意を求められ、管理者アクセス許可への同意は別途求められます。 
  - [AdminController.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Controllers/AdminController.cs)
  - [OAuth2CodeRedeemerMiddleware.cs](/Graph-ASPNET-46-Snippets/Microsoft%20Graph%20ASPNET%20Snippets/Utils/OAuth2CodeRedeemerMiddleware.cs)。アクセスの認証コードを使い、サインイン フローの外部のトークンを更新するカスタム ミドルウェアです。段階的な同意の実装の詳細については、https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect-v2 を参照してください。

## 質問とコメント
<a id="questions-and-comments" class="xliff"></a>

このサンプルに関するフィードバックをお寄せください。質問や提案につきましては、このリポジトリの「[問題](https://github.com/microsoftgraph/aspnet-snippets-sample/issues)」セクションで送信できます。

お客様からのフィードバックを重視しています。[スタック オーバーフロー](http://stackoverflow.com/questions/tagged/microsoftgraph)でご連絡いただけます。ご質問には [MicrosoftGraph] のタグを付けてください。

## 投稿
<a id="contributing" class="xliff"></a>

このサンプルに投稿する場合は、[CONTRIBUTING.md](CONTRIBUTING.md) を参照してください。

このプロジェクトでは、[Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/) が採用されています。詳細については、「[規範に関する FAQ](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。または、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までにお問い合わせください。 

## 追加リソース
<a id="additional-resources" class="xliff"></a>

- [他の Microsoft Graph スニペットのサンプル](https://github.com/MicrosoftGraph?utf8=%E2%9C%93&query=snippets)
- [Microsoft Graph の概要](http://graph.microsoft.io)
- [Office 開発者向けコード サンプル](http://dev.office.com/code-samples)
- [Office デベロッパー センター](http://dev.office.com/)

## 著作権
<a id="copyright" class="xliff"></a>
Copyright (c) 2016 Microsoft. All rights reserved.
