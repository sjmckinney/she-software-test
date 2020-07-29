# she-software-test

## Configuration and appsettings.json

To avoid commiting credentials to the repo I have add an ```appsettings.json.template``` file to the project.
This contains a skeleton appsettings file that each user will need to add the values that suit their needs.

The bare values required to get the test working (bar the actual password value) are set out below

```
{
  "driver": {
    "browser": "chrome"
  },

  "current_env": "dev",
  "url": "*******************************",

  "users": [
              {
                "Username": "StephenK",
                "Password": "********"
              }
            ]
}
```
I have adopted the ```appsettings.json``` approach for this demo but it is one of several ways to provide config information to the project.

I have not provided a meachanism to overwrite values provided by the config file with external environmental values but this is the approach I would take to allow the tests to be controlled by a CI/CD server.

I have added the latest driver binaries in the ```_drivers``` folder for convenience. I would not normally upload binary files to a repo as I feel these are best managed outside of source control.

## Framework

I have reaquainted myself with a BDD approach and Specflow for this demo whilst recycling some existing framework code for speed and convenience.

I have experimented somewhat and have given little consideration to running these tests in parallel or making more use of Dependency Injection.

I have split the workflow into two scenarios and shared context between the scenarios, which some may consider a heresy but it allowed me to experiment with navigating directly to one page in the second scenario. The use of REST in the app would allow some timesaving shortcuts in test setup and teardown, potentially allowing records to be created and deleted outside of the UI. 

## Assumptions

I provided more parameters than were asked for in the step that created the record to illustrate how I would tackle the requirement to create a variety of different records with varying parameters.

I assume that there are many common elements in the various pages and this would provide an opportunity to use inheritance when modelling the page objects.

I have assumed that the number of records in any test database would not cause paging and that any new records created would be visible on page 1.

I have used a proxy for todays date for the sample date in the record creation step. I would be tempted to provide an offset in days and or months from the current date time for date parameters to allow past and possible future records to be created in a maintenance free manner.

