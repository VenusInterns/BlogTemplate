
# ASP.NET Core Blog Template
We created this blog template for Visual Studio to help users create a new, personal web application quickly and effortlessly.

----------------------

**Deprecated** Please use [Miniblog.Core](https://github.com/madskristensen/Miniblog.Core) as an example of an ASP.NET Core blog template.

----------------------

This is a blogging engine based on Razor Pages and ASP.NET.


__Live demo__: https://venusblog.azurewebsites.net/    

Username: _webinterns@microsoft.com_

Password: _Password.1_

### Introduction to the Blog Template
This blog template is meant to help developers building a blog or a different web app that includes any features from this template. In using our blog, developers will save time because they can use this template to work from and build off of. This blog template also provides a fresh look at building web apps from [ASP.NET](https://docs.microsoft.com/en-us/aspnet/core/) Core using the new [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/) architecture. If you're not aware of Razor Pages yet, you should take a look as it effectively streamlines building a web application by associating HTML pages with their code so rather than compartmentalizing a project by the model-view-controller pattern, the separation is made by each additional page. We have found this very easy to work with because it allows us to think of the project by the components that make it up rather than the type of each component.

We believe that a blog template can appeal to the broadest audience of developers while also providing an opportunity for us to show off a variety of unique and handy features. If we are already using the newest tools provided by the Visual Studio team at Microsoft, it felt appropriate to use the newest architecture (Razor Pages) for the project. With a blog, we could also appeal to those developers interested in building ecommerce, photo gallery, or a personal web site because all three alternatives are simply variations of a blog with authentication. This makes the basic structure of the template useful for a wider variety of developers than only those interested in building a blog.

A more detailed talk we did on writing the blog template with code reviews and demos is available [here](https://www.youtube.com/watch?v=H4KtEJnnakc&list=PL0M0zPgJ3HSftTAAHttA3JQU4vOjXFquF&index=1&t=1860s).

### Background
This template was designed to help Visual Studio users create new web applications fast and effortlessly. We decided on building a Blog Template because such a template would give developers (with varying levels of experience) the ability to create a simple project, and add as much complexity to it as they wanted. The additional features to the blog are what make it a useful tool for developers:
* [Entity Framework](https://docs.microsoft.com/en-us/aspnet/entity-framework) provides an environment that makes it easy to work with relational data. In our scenario, that data comes in the form of blog posts and comments for each post.
* The usage of [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/linq/) (Language Integrated Query) enables the developer to store (query) items from the blog into a variety of targets like databases, xml documents (currently in use), and in-memory objects without having to redesign how things are queried, but rather where they are stored. 
* The blog is built on Razor Pages from ASP.NET Core. Because of this, developers with some knowledge of ASP.NET Core can learn about the pros and cons of building with Razor Pages as opposed to the previously established MVC schema.
* The template includes a user authentication feature, done by implementing the new ASP.NET [Identity Library](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity) for Razor Pages. This was a simple tool to add that consisted of installing the NuGet package and creating a new project with the package and then transferring the previous project files into this new project with Identity. Although a hassle, moving the files from one project to the other was quite simple because both projects were built with Razor Pages.
* Customizing the theme is fast and flexible with the use of Bootstrap. Simply download a Bootstrap theme.min.css file and add it to the CSS folder in your project (wwwroot > css). You can find free or paid Bootstrap themes at websites such as bootswatch.com. You can delete our default theme file, journal-bootstrap.min.css, to remove the default theming. Run your project, and you'll see that the style of your blog has changed instantly. 

### Goals
We hope the above features make this a tool that developers can use to speed up the development process of their project while also serving an educational purpose for those who want to learn how to implement/work with those new items. In making this an educational tool, we hope that our blog provides effective examples of said features. The following are a set of goals that we attempted to achieve and we challenge you to accomplish on your way to building the best web applications:
	* Grow ASP.NET Core usage
	* Educate users about Razor Pages
	* Grow Visual Studio usage
	* Build a community around the template 

### Using the Template
* [Visual Studio](https://www.visualstudio.com/vs/)
* Make sure to install the following workloads to get started:
    * ASP.NET and web development
    * [Latest .NET Core cross-platform development](https://www.microsoft.com/net/)
    * [Latest .NET SDK](https://www.microsoft.com/en-us/download/details.aspx?id=19988)

### Creating an Instance of the Template (Your Own Blog)
There are two options for instantiating a template. You can use `dotnet new` included with the dotnet CLI and go `dotnet new -i c:\pathtothecloneofthisrepo`. However, the current version contains minor bugs that will be fixed soon. Alternatively, you'll need to get the [newest templating code](https://github.com/dotnet/templating) with the following steps.
Click the green "Clone or download" button. Copy the link in the dropdown that appears.
Open a command prompt and change directories to where you want to install the templating repo.
In the desired directory, enter the command:

    git clone [link you copied earlier]

This will pull all the dotnet.templating code and put it in a directory named "templating."
Now change to the templating directory and switch branches to "rel/2.0.0-servicing" by running:

    git checkout rel/2.0.0-servicing

Then run the command `setup`.
  * Note: If you get errors about not being able to run scripts, close your command window. Then open a powershell window as administrator and run the command `Set-ExecutionPolicy Unrestricted`.
  Close the powershell window, then open a new command prompt and go back to the templating directory and run setup again.

Once the setup runs correctly, you should be able to run the command `dotnet new3`. If you are just using the dotnet CLI, you can replace `dotnet new3` with `dotnet new` for the rest of the steps.
Install your blog template with the command:

    dotnet new3 -i [path to blog template source]

This path will be the root directory of your blog template repository.

Now you can create an instance of the template by running:

    dotnet new3 blog -o [directory you want to create the instance in] -n [name for the instance]

For example:

    dotnet new3 blog -o c:\temp\TestBlog\ -n "My Blog"
