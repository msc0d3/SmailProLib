# SmailProLib
SmailPro Library c#, easy to use
* create temp email from smailpro
* check mailbox ( return raw response )
* asynchronous method

## Installation
Download release and add SmailproLib.dll to you project references

## Usage

### To create new smailpro config : 
```
SmailProConfig config = new SmailProConfig();
```
set data for config :
```
config.username = SmailProConfig.randomUserName;
```
   OR
```
config.username = "username";
```
```
config.domain = SmailProConfig.randomDomain;
```
   OR
```
config.domain = "domain";
// domains list : { "cardgener.com", "ugener.com", "ychecker.com", "storegmail.com", "instasmail.com" }
```
### To create new smailpro session :
```
Smailpro smailpro = new Smailpro(config);
var isSuccessCreateTask = smailpro.CreateTask();
```
### To get result :
```
Task<SmailProResponse> taskCheckEmail = null;
taskCheckEmail = smailpro.GetResult();
if (taskCheckEmail.Result.Status == SmailProResponse.StatusCode.Success)
{
  Console.WriteLine($"[] Email : {smailpro.Auth.email} ===> {taskCheckEmail.Result.RawText}");
}
else
{
  Console.WriteLine("Unknow Error");
}
```
### when all is done
Using
``
taskCheckEmail.Dispose();
``
to finish mail session and clean temp data !
### Other example ( real time check email ... etc )
view this : [Example Program](https://github.com/msc0d3/SmailProLib/blob/main/SmailproExample/Program.cs)

### Credit : Copyright (c) 2021 Nguyen Dac Tai Pro

### Donate
if this project helpful you , you can donate me at :
## Bank
7778889992001 / MB BANK / NGUYEN DAC TAI
## Paypal : nguyendactaidn@gmail.com or [Link Paypal](https://www.paypal.com/paypalme/nguyendactai)
