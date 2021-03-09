$folder = "Assets/Fonts/"
$dlFile = $folder + "VT323-Regular.ttf"
$dlUrl = "https://github.com/phoikoi/VT323/raw/master/fonts/ttf/VT323-Regular.ttf"

if (!(Test-Path($folder)))
{
	Write-Host 'Create folder' $folder -ForegroundColor Yellow
	New-Item -ItemType directory -Force -Path $folder
}
if (Test-Path($dlFile)) 
{
    Write-Host 'Skipping file, already downloaded' -ForegroundColor Yellow
    return
}

Write-Host 'Download VT323 font from:' $dlUrl -ForegroundColor Yellow
Invoke-WebRequest -Uri $dlUrl -OutFile $dlFile