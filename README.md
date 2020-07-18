# Twitch-Arduino
C# app connect to Twitch and sends commands to the Arduino plate
***
Depends:
   [TwitchLib](https://github.com/TwitchLib/TwitchLib)
***
App.config file:
```xml
<appSettings>
	<add key="twitchName" value=""/>
	<add key="twitchToken" value=""/>
	<add key="twitchChannel" value=""/>
	<add key="comparison" value="FullMessage"/>
	<add key="commandOn" value="!on,!enable"/>
	<add key="commandOff" value="!off,!disable"/>
	<add key="commandSwitch" value="!switch,!light"/>
	<add key="port" value="COM5"/>
</appSettings>
```