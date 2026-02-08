# Definition of Done
The following should be done per change:

- [ ] Run **Api.sln** solution in **Development** configuration and download the `swagger.json` to the **Api.Client\swagger.json**.  If the file is different refresh the **Api.Client**, update the **Package Version** semantic version based on the change and also the version in the **main.yml** to reflect that.
- [ ] If any events were changed in the **App.Events** project update the **Package Version** semantic version based on the change and also the version in the **main.yml** to reflect that.
- [ ] All unit tests are passing.
- [ ] Validate code coverage is above 70%.