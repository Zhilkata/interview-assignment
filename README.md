# Overview
This is my solution for part of a QA Assignment.

## How to set up?
Make sure you have .NET 8 installed on your local machine. Other dependencies are automatically handled, meaning that you only have to:
- Clone this repo locally.
- Run the ```dotnet test``` command.

Suggestion: utilize the full ```dotnet test --settings:.runsettings --logger "console;verbosity=detailed``` command.

## Why this tech stack?
I chose **Playwright** + **MSTest** because I'm not very familiar with the details of their usage, but have researched that they gain popularity and are maintained.
As such, I used the opportunity to set up a project following the documentation, and utilize the tools provided for tests based on the strategy I provided with a separate document.

## What's inside?
Main repo contains a showcase of multiple Playwright functionalities, such as:
  - **Event-driven** way of writing tests
  - Handling of **alerts** not part of the initial page context
  - **API Mocking** alongside UI interaction
  - General adherence to the **Page Object Model** pattern for locator and internal page logics
  - **GitHub Actions setup** for automated test execution against branch activity
  - **Local and CI/CD secrets** utilization
  - **Settings file** with customizable parameters
  - **Basic test report** export after each run

---

Repo contains several branches serving as comparison snapshots of different experiments and workflows. These being:
- **_codegen_** - the initial project state with tests directly copy-pasted from Playwright's recorder
- **_ai-improvement-suggestions_** - version of the current main branch "reimagined" by direct full-class rework from Claude Sonnet 4.5 Free
- **_incremental-ai-improvements_** - version of the current main branch with only a single TODO test reworked with help from Claude Sonnet 4.5 Free
- **_manual_** and **_github-actions-test_** left for history purposes

The idea is to show what happens in practice when utilizing different workflows, generate challenges and situations, and provide topics for discussion. 

# Structure & Design Reasoning
I'm familiar with POM and decided to use it as soon as possible. That's why the project's main content is divided in the Models and Tests directories.

## Models
While I researched more about the topic, I found out that it might've been appropriate to divide the WaitlistPage into multiple components, as in real-life scenarios, the waitlist part of it would be small compared to other components, which would result in a massive page model that's difficult to maintain and understand. For now though, it serves the purpose of containing locators, page construction, and initializing a dialogue handler that takes care of non-input alerts.

### What can be improved
- Divide the website page into multiple "component" page model object

## Tests
Playwright and MSTest provide a lot of options for structuring the tests, and I've utilized only the barebones components to provide my showcase.
My biggest challenges for sure were the alert handling and API mocking. There's a lot to learn about how Playwright manages events and what comes before/after what else. I admit that a lot of trial and error, and help from the AI, was required to get it to a consistently working state. I encountered deadlocks and asynchronous flakiness, lots of abstraction, but I learned a lot doing the solution.

### What can be improved
- More tests can be added, following the documented strategy and simulating other functionalities in the site. I decided to skip that to save time. If it was required, different functionalities would benefit from separate models and thus, from separate test classes.
- API and UI testing would rarely live in the same place due to how unmaintainable and off-putting it looks. I've included the mocking simply to simulate and show the possibilities; in the real world, I'd avoid that.

## Others
- The ``SampleTests.cs`` were part of the initial setup sanity testing the framework (I had installation problems that made me paranoid about it not breaking at some point). They also helped showcase the actions test success/failure reporting.
- The dotnet generated ``.gitignore`` is included in the repo because I find it a good practice that anyone cloning the repo can benefit from. Test runs generate a lot of garbage that has no place in the repo.
- The ``.runsettings`` were mentioned in several places throughout the Playwright and MSTest documentation. Thus, I took the sample configuration, removed what I didn't need and placed some common-use flags and settings inside. The biggest thing of note here is that I changed the Parallelization from Class to Method-level. In bigger, more complex projects, this is a recipe for disaster unless the isolation is perfect. But it cut a lot of time from my local tests, and is applicable in the current state, so I left it as that.

# Takeaway
I didn't expect Playwright to be so different, both from Selenium and its TypeScript version. The .NET version has specifics I had to trial-and-error and actively look guidance about.
At the same time, its performance is pretty good for what it does, and even though I am far away from creating the perfect framework architecture, I'm fairly confident I can expand it with more models and tests, and even get into deeper functionalities, like API mocking & real testing alongside the UI workflow, in a maintainable and performant way.
