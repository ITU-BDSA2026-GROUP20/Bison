## Practice for commit messages
#### Git messages must be set up the following way:

git commit -m "Refactor main to print CLI arguments.

Longer description...

Co-authored-by: Example <Example@itu.dk>"

## Temp To-dolist:

Need to implement the logic for getting posts based on a user and a page number, i was in the middle of doing this, but a page parameter has not yet been added. First add the page parameter in the serverside code and then update the Razor program client and service to handle the call.

Then i need to find the best way to handle how to give the page number as an argument to the service in the simplest way possible. Im assuming i just have to give it as an argument based on the field in the given page. But maybe there is a simpler way.

Then i need to think about the logical implementation of the path, since im considering doing the service / client to be more modular perhaps it can be coded through the service to automatically give it to the client inside the service so when you are doing function calls on the service you dont need to give a path which will be "baseurl/obs/name?page=2"

thank you for coming to my ted talk