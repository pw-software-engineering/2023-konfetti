# What is it?
This project was created, because the app is developed by three teams according to one documentation. In order to assure that everyone is following said documentation each team makes their own test suite which can be launched against API of any team.

# Launch
Simply run
```
dotnet run {API_ADDRESS} {ADMIN_EMAIL} {ADMIN_PASSWORD}
```
and wait for the results. 
- API_ADDRESS is path to API root.
- ADMIN_EMAIL is default admin email (as admins can't be registered via endpoints)
- ADMIN_PASSWORD is default admin password