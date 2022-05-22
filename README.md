# Embeddable web admin for the Quartz.NET scheduler

[Nuget package](https://www.nuget.org/packages/QuartzNetWebConsole/).

[An old blog post that's still relevant about what the project does](http://bugsquash.blogspot.com/2010/06/embeddable-quartznet-web-consoles.html).

To set this up in your ASP.NET Core (or any OWIN-compatible framework really), add to your Startup:

```
    app.UseOwin(m =>
    {
        m(Setup.Owin("/quartz/", () => Program.Scheduler));
        Setup.Logger = new MemoryLogger(100, "/quartz/");
    });
```

This repo also has a [reference sample app](SampleApp).