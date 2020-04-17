# Guess a Number - Microsoft Bot Framework Demo

Most of the README is straight from the core bot sample. Skip down to section "My Notes" for additional descriptions.

---

Built from: Bot Framework v4 core bot sample - see [dev.botframework.com](https://dev.botframework.com).

- Use [LUIS](https://www.luis.ai) to implement core AI capabilities
- Implement a multi-turn conversation using Waterfall dialogs
- Handle user interruptions for such things as `Help` or `Cancel`
- Prompt for and validate requests for information from the user

## Prerequisites

This sample **requires** prerequisites in order to run.

### Overview

This bot uses [LUIS](https://www.luis.ai), an AI based cognitive service, to implement language understanding.

### Install .NET Core CLI

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 2.1

  ```bash
  # determine dotnet version
  dotnet --version
  ```

### Create a LUIS Application to enable language understanding

The LUIS model for this example can be found under `CognitiveModels/Game.json` and the LUIS language model setup, training, and application configuration steps can be found [here](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-luis?view=azure-bot-service-4.0&tabs=cs).

Once you created the LUIS model, update `appsettings.json` with your `LuisAppId`, `LuisAPIKey` and `LuisAPIHostName`.

```json
  "LuisAppId": "Your LUIS App Id",
  "LuisAPIKey": "Your LUIS Subscription key here",
  "LuisAPIHostName": "Your LUIS App region here (i.e: westus.api.cognitive.microsoft.com)"
```

## To try this sample

- In a terminal, navigate to `MitchBarry.Bot.GuessANumber`

    ```bash
    # change into project folder
    cd MitchBarry.Bot.GuessANumber
    ```

- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Navigate to `MitchBarry.Bot.GuessANumber` folder
  - Select `MitchBarry.Bot.GuessANumber.csproj` file
  - Press `F5` to run the project

## Testing the bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.

- Install the Bot Framework Emulator version 4.5.0 or greater from [here](https://github.com/Microsoft/BotFramework-Emulator/releases)

### Connect to the bot using Bot Framework Emulator

- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages`

## Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Dialogs](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-dialog?view=azure-bot-service-4.0)
- [Gathering Input Using Prompts](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-prompts?view=azure-bot-service-4.0&tabs=csharp)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)

## My notes

* Since I built this straight from the Core Bot, it defaults to include LUIS 
 features related to Flight Bookings, etc. I started by adding my game to the same LUIS configuration - but 
 on the LUIS website itself - https://luis.ai (at time of writing, I used https://preview.luis.ai/). 
  * Since I added new _entities_ and a new _intent_, I needed to update my C# class which represented my LUIS
  application. To do this, I used the [BotFramework CLI](https://github.com/microsoft/botframework-cli). 
    * Note, before creating a C# class, I first exported my LUIS application to JSON and overwrote contents of 
    `FlightBooking.json` provided from the template itself and called it `Game.json`.
      * ![export](https://i.imgur.com/PSvJBfal.png)
    * It's worth calling out because you don't want to accidentally use their older LUISGen application. 
  * Updating with BF CLI looks like this (taken from help docs for generating new C# class [here](https://github.com/microsoft/botframework-cli/tree/master/packages/cli#bf-luisgeneratecs)):
    ```powershell
    npm i -g @microsoft/botframework-cli
    bf luis:generate:cs -i Game.json -o Game.cs --className MitchBarry.Bot.GuessANumber.CognitiveModels.Game --force
    ```
  * I noticed this actually broke the default extension methods provided in the Core Bot. Not a huge loss
  since I wasn't using the _flight booking_ features it mostly helped with, but in the [MainDialog.cs](/MitchBarry.Bot.GuessANumber/Dialogs/MainDialog.cs), 
  it was used extensively. Had to make some adjustments there accordingly. 

* I at one point wanted to go _backwards_ in my Waterfall step. The thought was, what if someone provided 
a Maximum number that was lower than the Minimum number? What if the user guessed the number wrong and I wanted 
to prompt to re-guess the number? 
  * I did some Stack Overflow digging and found that some older workarounds no longer worked in the v4 of Bot Framework. 
  * Do not go backwards with Waterfall steps! Instead, Replace the existing Dialog with a _new_ instance of the 
  Waterfall dialog itself, and pass along the current state. At each step, check if the state already has the value 
  it needs at that point. If so, continue to next step in dialog automatically. This works pretty well. 
