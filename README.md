# law-firm-sales-campaign
專案在PAMO_TapPay File底下

#.NET Core 5.0 SDK 
需要安裝.NET Core 5.0 SDK
如果您已安裝 .NET Core，請使用 dotnet --info 命令來判斷您使用的 SDK。
dotnet publish -c Release 發佈後就可以進行DockerFile操作

#appsettings.json 
appsettings.json為專案設定檔，可自行修改抽換參數
#TapPay設定檔:
>>TapPayInfo
#簡訊設定檔:
>>TwilioInfo
#PAMO相關設定檔:
>>PamoInfo
#縮短網址設定檔:
>>Bitly

#NLog.config 
Log設定檔則是在NLog.config
可設定Log相關路徑等資訊如預設為
>>internalLogFile="c:\logs\internal-nlog.txt">

#以上，如有缺什麼資料或問題需要我協助處理，再不吝通知，謝謝!