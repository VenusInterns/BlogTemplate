# Welcome to the New Blog Template for ASP.NET Developers
#### By Juliet Daniel, Lucas Isaza, and Uma Lakshminarayan

This blog template is a tool to help developers build a blog or similar web applications. Developers will save time by building off of this template. This blog template also serves as an example of building web apps from [ASP.NET](https://docs.microsoft.com/en-us/aspnet/core/) Core using the new [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/) architecture. Razor Pages effectively streamlines building a web application by associating HTML pages with C# code, rather than compartmentalizing a project into the Model-View-Controller pattern.

We believe that a blog template appeals to a broad audience of developers while also showcasing a variety of unique and handy features. The basic structure of the template is useful for developers interested in building an application beyond blogs, such as an ecommerce, photo gallery, or personal web site. All three alternatives are simply variations of a blog with authentication.

You can find our more detailed talk on writing the blog template with code reviews and demos [here](https://www.youtube.com/watch?v=H4KtEJnnakc&list=PL0M0zPgJ3HSftTAAHttA3JQU4vOjXFquF&index=1&t=1860s). You can also access our live demo at https://venusblog.azurewebsites.net/.

### Background
This template was designed to help Visual Studio users create new web applications fast and effortlessly. The various features built in the template make it a useful tool for developers: 

* Data is currently stored using XML files. This was an early design decision made to allow users on other blogs to move their data to this template smoothly. The usage of [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/linq/) (Language Integrated Query) enables the developer to query items from the blog from a variety of sources such as databases, XML documents (currently in use), and in-memory objects without having to redesign or learn how elements are queried from a specific source.

![datastoreimplementation](https://user-images.githubusercontent.com/15066077/29988129-82f8b9fa-8f20-11e7-9f65-dc2c6fa3d8b5.png)

* The blog is built on Razor Pages from ASP.NET Core. The image below showcases the organization of the file structure that Razor Pages uses. Each view contains a corresponding Model in a C# file. Adding another Razor Page to your project is as simple as adding a new item to the Pages folder and choosing the Razor Page with model type.

![filestructure](https://user-images.githubusercontent.com/15066077/29988450-4480ab2c-8f22-11e7-9e9c-689ed834be0e.png)

* The template includes a user authentication feature, implemented using the new ASP.NET [Identity Library](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity). This tool allows the owner of the blog to be the single user registered and in control of the blog. Identity also provided us with a tested and secure way to create and protect user profiles. We were able to use this library to implement login, registration, password recovery, and other user management features. To enable identity we simply included it in the startup file and added the corresponding pages (with their models). 

![configureservices](https://user-images.githubusercontent.com/15707311/29847724-06f342ee-8cd2-11e7-8497-abc836b59269.PNG) 

![identityfiles](https://user-images.githubusercontent.com/15066077/29988871-5b485376-8f24-11e7-8a19-dd15d6d5f7f0.png)

* Customizing the theme is fast and flexible with the use of Bootstrap. Simply download a Bootstrap theme `.min.css` file and add it to the CSS folder in your project (wwwroot > css). You can find free or paid Bootstrap themes at websites such as [bootswatch.com](https://bootswatch.com/). You can delete our default theme file, `journal-bootstrap.min.css`, to remove the default theming. Run your project, and you'll see that the style of your blog has changed instantly.

![bootstrapfiles](https://user-images.githubusercontent.com/15066077/29989021-54cd895c-8f25-11e7-9a4a-a32d9551dff6.png)

* [Entity Framework](https://docs.microsoft.com/en-us/aspnet/entity-framework) provides an environment that makes it easy to work with relational data. In our scenario, that data comes in the form of blog posts and comments for each post.

### Using the Template
* [Visual Studio](https://www.visualstudio.com/vs/)
* Make sure to install the following workloads to get started:
    * ASP.NET and web development
    * [Latest .NET Core cross-platform development](https://www.microsoft.com/net/)
    * [Latest .NET SDK](https://www.microsoft.com/en-us/download/details.aspx?id=19988)

### Creating an Instance of the Template (Your Own Blog)
First, you'll need to get the [newest templating code](https://github.com/dotnet/templating).
Switch the Branch to "rel/2.0.0-servicing." Then click the green "Clone or download" button. Copy the link in the dropdown that appears.
Open a command prompt and change directories to where you want to install the templating repo.
In the desired directory, enter the command:

    git clone [link you copied earlier]

This will pull all the dotnet.templating code and put it in a directory named "templating."
Now change to the templating directory and switch branches to "rel/2.0.0-servicing." Then run the command "setup."
  * Note: If you get errors about not being able to run scripts, close your command window. Then open a powershell window and run the command "Set-ExecutionPolicy Unrestricted."
  Close the powershell window, then open a new command prompt and go back to the templating directory and run setup again.

Once the setup runs correctly, you should be able to run the command "dotnet new3."
Install your blog template with the command:

    dotnet new3 -i [path to blog template source]

This path will be the root directory of your blog template repository.

Now you can create an instance of the template by running:

    dotnet new3 blog -o [directory you want to create the instance in] -n [name for the instance]

For example:

    dotnet new3 blog -o c:\temp\TestBlog\ -n "My Blog"
    

Three months ago, we walked into the Visual Studio Web Tools team and were met with a warm welcome from our manager and mentors. Our task was to create a template of a web application as a pilot for a set of templates showcasing new functionalities in Razor Pages. We decided on building a blog template because of our familiarity with writing and reading blogs, and because we felt it could be relatable within the developer community. Along with researching topics in web development, we had fun playing with different blog engines to help us brainstorm features for our project. In that first week, we all acted as program managers and prioritized features. Every three weeks we rotated between the PM and developer roles, with one of us acting as PM and the other two as developers. 

"My favorite part of the internship was being able to interact with people from all over Visual Studio to get their feedback on our Blog Template or to ask them questions about specific technologies" -Lucas





Through developing this we've learned a lot about web development and Razor Pages and we hope that our project encourages developers to build more web applications with Microsoft's technologies and have fun doing so.
