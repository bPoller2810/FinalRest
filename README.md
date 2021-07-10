
# FinalRest

FinalRest removes the need to write all the RestAPI Client code over and over again. Just use a declarative way to create your requests and client. A simple Builder style allows for inheritance and individual behaviour for each individual request, without the need of much code.

---

## FinalRest is available on NuGet 
![Nuget](https://img.shields.io/nuget/v/FinalRest.core?logo=NuGet)

## HowTo

### The request
To start a FinalRestRequestBuilder needs to be created:
```c#
var request = new FinalRestRequestBuilder()
```
\
The basic building blocks are fairly simple:
```c#
.SetRoute("api/")
.SetMethod(ERestMethod.GET)
.AddHeader("some-header", "the-header-value")
.Copy()
```
\
To handle Pre- and Post request handlers (for authentication as example) there are the needed blocks too
Find this handler in the sample here: [CustomJwtAuthHandler.cs](/sample/FinalRest.sample.console/Handlers/CustomJwtAuthHandler.cs)
```c#
.AddPreRequestHandler<CustomJwtAuthHandler>() 
.AddPostRequestHandler<CustomJwtAuthHandler>()
```
\
To invoke actions on certain responses you can use the behaviours. All behaviours exist as normal callbacks and as async callbacks. ResponseBehaviours let you access the Response. Result Behaviour just give you the HttpStatusCode.
```c#
.AddAsyncResponseBehaviour(          HttpStatusCode.OK,                   (status)       => Task.Factory.StartNew(() => Debug.WriteLine(status.ToString())))
.AddResponseBehaviour(               HttpStatusCode.OK,                   (status)       => Debug.WriteLine($"Sync: {status}"))
.AddAsyncResultBehaviour<ApiResult>( HttpStatusCode.Unauthorized,         (status, data) => Nav.HandleAuthFailAsync($"damn {status} => {data.Info.Version}"))
.AddResultBehaviour<ApiResult>(      HttpStatusCode.InternalServerError,  (status, data) => Dialog.ShowErrorDialog(data.Info.Seed))
```
\ 
Now you can copy and build more out of this defined builder, or build it to supply this to a client
```c#
.Build();
```

---

### The client 
The client itself also is in the form of a Builder. The Generic Argument will be the Type of wich the InvocationKeys is. (Usually a enum is the intended way, but you can just use any type):
```c#
var client = new FinalRestClientBuilder<ERequests>()
```
\
For now choosing the HttpClient is optional, but later more implementations will be possible. (HttpClient will be always the Default)
```c#
.UseHttpClient()
```
\
The mandatory base url to the api needs to be supplied:
```c#
.SetBaseUrl("https://randomuser.me/")
```
\
In case you need to verify the SSL certificate yourself you can add this handler if you need:
```c# 
.UseCustomCertificateValidation(ValidateCert)
```
\
Now its time to add all your requests to this client:
```c#
.AddRequest(randomUsers, ERequests.RandomUsers)
```
\
Build and enjoy your RinalRestClient:
```c#
.Build();
```

---
### Use the client
To invoke a request we just need 1 method with a few parameters:
- The generic type is your expected result type on success
- The Enum is your key that you gave the request when you added it to your client
- The Body is null on this request.
- The following parameters are the url parameters
```c#
var users = await client.InvokeAsync<ApiResult>(ERequests.RandomUsers, null, "inc=id,gender,name,email", "results=10");
```
## Contributions

This Project is open for Contributions. Please file a Issue before you start working on something
If you have any questions or looking for a good first issue, feel free to raise a issue aswell

