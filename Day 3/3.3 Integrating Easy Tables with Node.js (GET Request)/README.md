# 3.3 Integrating Easy Tables with Node.js (GET Request) 

### 6.3.1 Referencing Azure Mobile Services
At the earlier sections, we would have already added it to our Nuget Packages. If not

- For Visual Studio: Right-click your project, click Manage NuGet Packages, search for the `Microsoft.Azure.Mobile.Client` package, then click Install.
- For Xamarin Studio: Right-click your project, click Add > Add NuGet Packages, search for the `Microsoft.Azure.Mobile.Client` package, and then click Add Package.

#### `NOTE: Make sure to add it to all your solutions!`

If we want to use this SDK we add the following using statement
```Csharp
using Microsoft.WindowsAzure.MobileServices;
``` 

### Extra Learning Resources
* [Using App Service with Xamarin by Microsoft](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-dotnet-how-to-use-client-library/)
* [Using App Service with Xamarin by Xamarin - Outdated but good to understand](https://blog.xamarin.com/getting-started-azure-mobile-apps-easy-tables/)
* [ListView in Xamarin](https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/)