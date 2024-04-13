This template uses aspnet 8 as the backend and a vue app as the front end.

To do a quick test, go into the `ClientUI` folder in command line and run

```sh
cd WebApp/ClientUI
npm install
npm run dev
```

Then in another command window start the `WebApp` in dotnet 

```sh
cd WebApp
dotnet run --launch-profile https
```

If it works you will be able to go to 
[https://localhost:7082](https://localhost:7082) and see a working page.
